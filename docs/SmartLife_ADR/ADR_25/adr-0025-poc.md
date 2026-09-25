# Prueba de Concepto — ADR-0025

## Objetivo

Validar que Spring Cloud Config permite externalizar valores de configuración de negocio (como el límite de reintentos de pago) en un repositorio Git, y que los servicios pueden recargar esos valores actualizados mediante el endpoint /actuator/refresh, sin necesidad de reiniciar ni redesplegar el servicio.

## Métricas y resultados

Se implementaron dos proyectos Spring Boot: un Config Server (puerto 8888) apuntando a un repositorio Git local con el archivo smartlife-app.properties (conteniendo limite.reintentos.pago=5), y un cliente (config-client, puerto 8081, spring.application.name=smartlife-app) que consulta ese valor mediante @Value y lo expone en un endpoint /config. Al consultar directamente http://localhost:8888/smartlife-app/default, el servidor devolvió correctamente el valor: "limite.reintentos.pago":"5". Al arrancar el cliente, este se conectó exitosamente al servidor (confirmado en logs: "Fetching config from server at: http://localhost:8888" y "Located environment: name=smartlife-app"), y el endpoint /config mostró "Límite de reintentos de pago actual: 5".

Se modificó el valor en el repositorio (de 5 a 3) y se confirmó el cambio con un commit de Git. Sin reiniciar el proceso del cliente en ningún momento, se envió una petición POST al endpoint /actuator/refresh, obteniendo respuesta StatusCode: 200 con el contenido ["config.client.version","limite.reintentos.pago"], confirmando que Spring detectó el cambio en esa propiedad específica. Al recargar el endpoint /config inmediatamente después, este mostró el nuevo valor: "Límite de reintentos de pago actual: 3", sin que el proceso Java del cliente se hubiera detenido en ningún instante de la prueba.

## Conclusiones

La PoC valida el criterio central de la decisión, citado textualmente en el ADR: "permite que los servicios recarguen los valores actualizados mediante el endpoint /actuator/refresh, sin necesidad de reiniciar ni redesplegar el servicio completo". Se confirmó de extremo a extremo que un cambio de configuración de negocio (el límite de reintentos de pago) puede aplicarse en caliente sobre un servicio en ejecución, resolviendo directamente el problema que motivó descartar la opción de archivos de configuración estáticos (application.yml), donde cualquier ajuste exigiría reconstruir el artefacto y redesplegar el servicio completo en Render — un ciclo de varios minutos para un cambio que, como demostró esta prueba, puede tomar segundos. Queda fuera del alcance de esta PoC la integración con un repositorio Git remoto real (aquí se usó uno local), el manejo de credenciales para repositorios privados, y la anotación @RefreshScope aplicada a componentes más complejos que un simple controlador REST (por ejemplo, servicios con lógica de negocio dependiente del parámetro), que sería el siguiente paso natural para llevar esta validación más cerca del uso real en SmartLife.
