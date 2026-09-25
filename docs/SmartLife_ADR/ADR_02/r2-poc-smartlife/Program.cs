using System.Diagnostics;
using Amazon.S3;
using Amazon.S3.Model;

// ============================================================
// PoC ADR-5: Seleccionar servicio de almacenamiento en la nube
// para archivos multimedia -> Cloudflare R2
//
// Valida los 5 criterios definidos en la PoC:
//  1) Compatibilidad con la API de S3
//  2) Niveles de acceso (Standard / Infrequent Access)
//  3) Ausencia de cobro por egress (se verifica manualmente
//     en el dashboard de Cloudflare, no vía API)
//  4) Latencia sin caché vs. con la red de Cloudflare
//  5) Límite de tamaño por operación de subida
// ============================================================

var accountId = ObtenerVariable("R2_ACCOUNT_ID");
var accessKey = ObtenerVariable("R2_ACCESS_KEY");
var secretKey = ObtenerVariable("R2_SECRET_KEY");
var bucketName = Environment.GetEnvironmentVariable("R2_BUCKET") ?? "smartlife-multimedia-poc";
// Opcional: dominio público conectado al bucket en Cloudflare (con caché activada)
// Si no lo configuras, el paso de comparación de caché simplemente se omite.
var publicDomain = Environment.GetEnvironmentVariable("R2_PUBLIC_DOMAIN");

var config = new AmazonS3Config
{
    ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
    ForcePathStyle = true,
    AuthenticationRegion = "auto" // R2 no usa regiones AWS reales
};

using var s3Client = new AmazonS3Client(accessKey, secretKey, config);

// Los 4 casos de uso reales que motivan el ADR-5
var casosDeUso = new[]
{
    new CasoDeUso("incidentes/HU78-adjunto-incidente.jpg", "muestras/incidente.jpg", "STANDARD"),
    new CasoDeUso("pqrs/HU83-documento-pqrs.pdf", "muestras/pqrs.pdf", "STANDARD"),
    new CasoDeUso("comunicados/HU81-comunicado.pdf", "muestras/comunicado.pdf", "STANDARD_IA"),
    new CasoDeUso("facturas/HU65-factura.pdf", "muestras/factura.pdf", "STANDARD"),
};

Console.WriteLine("=== PoC Cloudflare R2 - SmartLife (ADR-5) ===\n");

foreach (var caso in casosDeUso)
{
    if (!File.Exists(caso.RutaLocal))
    {
        Console.WriteLine($"[AVISO] No se encontró {caso.RutaLocal}. Coloca un archivo de prueba ahí antes de correr la PoC.");
        continue;
    }

    await SubirArchivoAsync(caso);
    var urlFirmada = GenerarUrlFirmada(caso.Key, TimeSpan.FromMinutes(15));
    Console.WriteLine($"URL firmada ({caso.Key}): {urlFirmada}");

    var latenciaDirecta = await MedirLatenciaDescargaAsync(urlFirmada);
    Console.WriteLine($"Latencia directa al bucket (sin caché): {latenciaDirecta} ms");

    if (!string.IsNullOrWhiteSpace(publicDomain))
    {
        var urlPublica = $"https://{publicDomain}/{caso.Key}";
        var latenciaCache = await MedirLatenciaDescargaAsync(urlPublica);
        Console.WriteLine($"Latencia vía dominio público con caché de Cloudflare: {latenciaCache} ms");
    }

    Console.WriteLine();
}

Console.WriteLine("=== Verificación manual pendiente ===");
Console.WriteLine("1) Entra al dashboard de Cloudflare > R2 > Billing y confirma que no aparece ningún cargo por egress.");
Console.WriteLine("2) Revisa el tamaño de los archivos subidos frente al límite de subida simple de R2 (5 GiB por objeto) para validar el criterio de migración.");
Console.WriteLine("3) Compara la latencia directa vs. con caché impresa arriba contra el umbral aceptable para adjuntos de incidentes/PQRS.");

// ---------- Funciones ----------

async Task SubirArchivoAsync(CasoDeUso caso)
{
        var request = new PutObjectRequest
    {
        BucketName = bucketName,
        Key = caso.Key,
        FilePath = caso.RutaLocal,
        StorageClass = new S3StorageClass(caso.ClaseAlmacenamiento),
        DisablePayloadSigning = true
    };
    await s3Client.PutObjectAsync(request);
    Console.WriteLine($"Subido: {caso.Key} (clase: {caso.ClaseAlmacenamiento})");
}

string GenerarUrlFirmada(string key, TimeSpan vigencia)
{
    var request = new GetPreSignedUrlRequest
    {
        BucketName = bucketName,
        Key = key,
        Expires = DateTime.UtcNow.Add(vigencia)
    };

    return s3Client.GetPreSignedURL(request);
}

async Task<long> MedirLatenciaDescargaAsync(string url)
{
    using var http = new HttpClient();
    var cronometro = Stopwatch.StartNew();

    var respuesta = await http.GetAsync(url);
    respuesta.EnsureSuccessStatusCode();
    await respuesta.Content.ReadAsByteArrayAsync();

    cronometro.Stop();
    return cronometro.ElapsedMilliseconds;
}

string ObtenerVariable(string nombre)
{
    return Environment.GetEnvironmentVariable(nombre)
        ?? throw new InvalidOperationException($"Falta configurar la variable de entorno {nombre}.");
}

record CasoDeUso(string Key, string RutaLocal, string ClaseAlmacenamiento);
