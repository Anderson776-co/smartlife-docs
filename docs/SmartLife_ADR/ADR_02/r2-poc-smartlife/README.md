# PoC ADR-5 — Cloudflare R2 (SmartLife)

PoC en .NET 8 para verificar si Cloudflare R2 cumple lo que dice el ADR-5:
sin cobro de egress, tarifa plana, compatibilidad con la API de S3, dos
niveles de acceso y el riesgo de latencia sin caché.

## 1. Crear el bucket y el token en Cloudflare

1. Entra a `dashboard.cloudflare.com` > **R2 Object Storage** > **Create bucket**.
   Nómbralo, por ejemplo, `smartlife-multimedia-poc`.
2. Ve a **Manage R2 API Tokens** > **Create API Token** con permisos
   `Object Read & Write` sobre ese bucket.
3. Cloudflare te muestra el **Access Key ID**, el **Secret Access Key**
   y tu **Account ID**. Guárdalos, no se vuelven a mostrar.

## 2. (Opcional) Conectar un dominio público con caché

Si quieres comparar la latencia "sin caché" vs. "con la red de Cloudflare",
conecta un dominio propio al bucket desde **R2 > tu bucket > Settings >
Public Access > Connect Domain**. Si te lo saltas, la PoC solo mide la
latencia directa al bucket.

## 3. Configurar variables de entorno

```bash
export R2_ACCOUNT_ID="tu-account-id"
export R2_ACCESS_KEY="tu-access-key"
export R2_SECRET_KEY="tu-secret-key"
export R2_BUCKET="smartlife-multimedia-poc"
export R2_PUBLIC_DOMAIN="archivos.tu-dominio.me"   # opcional
```

En Windows (PowerShell) usa `setx` o `$env:R2_ACCOUNT_ID = "..."`.

## 4. Preparar los archivos de prueba

Crea una carpeta `muestras/` junto al proyecto con 4 archivos reales
(una imagen y tres PDF pequeños sirven):

```
muestras/incidente.jpg
muestras/pqrs.pdf
muestras/comunicado.pdf
muestras/factura.pdf
```

## 5. Ejecutar

```bash
dotnet restore
dotnet run
```

La consola imprime, para cada caso de uso (HU78, HU83, HU81, HU65):
la confirmación de subida con su clase de almacenamiento, la URL
firmada de descarga y la latencia medida.

## 6. Verificación manual (no automatizable por API)

Al final la consola te recuerda revisar en el dashboard:

- **Billing** de Cloudflare, para confirmar que no se cobró egress.
- El límite de tamaño por objeto de R2 (5 GiB en subida simple).
- Si la diferencia de latencia con/sin caché es aceptable para tu caso
  (adjuntos de incidentes y PQRS que se consultan poco después de subidos).

Con esas verificaciones ya tienes evidencia real para cerrar el punto
7 del plan original: confirmar si el ADR-5 queda validado tal cual o
si necesita un ajuste (por ejemplo, forzar caché para los adjuntos que
más se consultan).
