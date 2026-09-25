# Prueba de Concepto — ADR-0027

## Objetivo

Validar que Strapi permite crear, editar y exponer mensajes de aplicación mediante una API pública, sin costo, y que los cambios de contenido se reflejan de inmediato sin necesidad de redesplegar el backend.

## Métricas y resultados

Se instaló Strapi Community Edition (versión 5.54.0) de forma local usando Node v20.20.2 y base de datos SQLite, sin ningún costo de licenciamiento ni infraestructura adicional (Plan: Community, Database: sqlite). Se creó una colección de contenido "Mensaje" con dos campos (codigo y texto), y se registró un mensaje de prueba con codigo "pago.registrado.exitoso" y texto "Tu pago se registró satisfactoriamente". Tras habilitar los permisos de lectura pública (find, findOne) sobre esa colección, se consultó exitosamente el endpoint GET /api/mensajes, obteniendo una respuesta JSON válida con el contenido completo del mensaje. Se editó el texto del mensaje directamente desde el panel administrativo (cambiándolo a "Tu pago fue registrado con éxito") sin modificar ningún archivo de código ni redesplegar el servicio, y al volver a consultar el mismo endpoint, el cambio se reflejó de inmediato, confirmado por la actualización del campo updatedAt. Se midió el tiempo de respuesta de la API mediante las herramientas de desarrollador del navegador (pestaña Network), obteniendo un tiempo de 24 ms para la solicitud a /api/mensajes en un entorno local.

## Conclusiones

La PoC valida los criterios centrales de la decisión: Strapi expone un panel administrativo funcional para gestionar mensajes sin intervención de desarrollo, ofrece una API REST pública consultable en tiempo real, y permite actualizar contenido sin redesplegar el backend, exactamente como argumenta el ADR frente a la opción descartada de Spring MessageSource (que sí exige modificar código y redesplegar ante cualquier cambio de texto). La latencia medida (24 ms) es baja en el entorno local de prueba, pero corresponde a Strapi y el consultor corriendo en la misma máquina; en el despliegue real de SmartLife, con Strapi y el backend de Spring Boot en servidores distintos, esa latencia sería mayor, confirmando la consecuencia negativa ya reconocida en el ADR de que la solución "introduce latencia... salvo que se implemente una capa de caché". Queda fuera del alcance de esta prueba medir el comportamiento bajo múltiples solicitudes concurrentes y validar el escenario de indisponibilidad de Strapi (punto de falla adicional ya reconocido en el ADR), que podría abordarse en una prueba posterior si se considera necesario.
