# Prueba de Concepto — ADR-0002

## Objetivo

Validar que Cloudflare R2 permite subir, almacenar y descargar archivos multimedia mediante la API de S3, sin generar cobros por transferencia de salida (egress).

## Métricas y resultados

Se subieron 4 archivos de prueba, cada uno con su clase de almacenamiento asignada. La latencia de descarga directa al bucket, sin capa de cache, vario entre 524 ms y 1617 ms segun el archivo. El panel de Billing de Cloudflare confirmo $0.00 en uso facturable despues de las subidas y descargas, validando la ausencia de cobro por egress. Se detecto una incompatibilidad real con el SDK de AWS S3: R2 no soporta el firmado por streaming (STREAMING-AWS4-HMAC-SHA256-PAYLOAD) que el SDK usa por defecto, siendo necesario deshabilitarlo (DisablePayloadSigning) para lograr la subida.

## Conclusiones

La PoC valida los tres criterios centrales de la decision: compatibilidad con la API de S3, disponibilidad de los dos niveles de acceso, y ausencia de cobro por egress. Quedan fuera del alcance de esta prueba el limite de tamano en migraciones masivas y la comparacion de precios contra otros proveedores. La variacion de latencia sin cache confirma la debilidad ya reconocida en el ADR y sugiere complementar con un dominio conectado a la red de Cloudflare para los archivos mas consultados.
