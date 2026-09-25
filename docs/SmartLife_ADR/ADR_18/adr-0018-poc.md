# Prueba de Concepto — ADR-0018

## Objetivo

Validar dos capacidades específicas mencionadas en el ADR como razones para elegir Spring Boot (Java) sobre ASP.NET Core, NestJS y Django: (1) que Spring Modulith detecta y bloquea en tiempo de test una violación real de los límites entre módulos de un monolito modular, y (2) que el tipado estático de Java detecta errores de tipo en tiempo de compilación, antes de llegar a producción.

## Métricas y resultados

Sobre un proyecto Spring Boot con Spring Modulith y dos módulos de prueba (pagos y notificaciones), se creó una clase interna dentro de un subpaquete internal del módulo pagos, accedida indebidamente desde el módulo notificaciones. Al ejecutar mvn test, la verificación ApplicationModules.verify() falló correctamente con "Tests run: 1, Failures: 0, Errors: 1", reportando explícitamente: "Module 'notificaciones' depends on non-exposed type com.smartlife.pocmodulith.pagos.internal.CalculadoraInternaPagos within module 'pagos'!", señalando el archivo y la línea exactos de la violación (ServicioNotificaciones.java:10 y :11), y terminando en BUILD FAILURE.

Adicionalmente, se forzó un error de tipo pasando un String como parámetro a un método que esperaba un double (representando un monto de pago). Al ejecutar mvn compile, el compilador de Java rechazó el código antes de cualquier ejecución, con el mensaje "incompatible types: java.lang.String cannot be converted to double", también terminando en BUILD FAILURE. Tras esta prueba, el archivo se revirtió a su estado correcto, confirmándose mediante una nueva ejecución de mvn test que el proyecto vuelve a compilar sin errores de tipo y que la violación de módulos sigue detectándose consistentemente por Spring Modulith.

## Conclusiones

La PoC valida dos criterios puntuales y verificables mencionados explícitamente en el ADR-0018. Primero, la frase "verificación de límites de módulo en tiempo de compilación" y la consecuencia positiva "Disciplina arquitectónica verificable": se confirmó que Spring Modulith efectivamente detecta y bloquea el acceso indebido entre módulos internos del sistema mediante una prueba automatizada, ejecutable en cualquier pipeline de CI/CD, resolviendo la necesidad de "comunicador entre módulos" que motivó descartar ASP.NET Core (sin herramienta equivalente). Segundo, la consecuencia positiva "Tipado estático para reducir errores financieros": se confirmó con evidencia concreta que Java detecta errores de tipo en tiempo de compilación, en lugar de en tiempo de ejecución, respaldando el argumento usado para descartar Django (tipado dinámico).

Durante el proceso se identificó un matiz técnico relevante para la implementación real de SmartLife: Spring Modulith solo protege como "no expuesto" aquello ubicado en subpaquetes explícitamente nombrados como internal, no cualquier clase que simplemente no esté fuera del paquete raíz del módulo — un primer intento con la clase interna ubicada directamente en el paquete raíz de pagos no fue detectado como violación. Esta lección debe comunicarse al equipo para que los módulos reales del proyecto (pagos, reservas, notificaciones, PQRS) sigan esta convención de subpaquetes internal desde el diseño.

Quedan fuera del alcance de esta PoC las demás afirmaciones del ADR no demostrables mediante una prueba técnica puntual: la comparación de concurrencia bajo carga transaccional (modelo de hilos vs. event loop) frente a NestJS, la madurez del ecosistema y comunidad, y las consecuencias negativas relacionadas con la reescritura de MassTransit/Polly o la curva de aprendizaje del equipo.
