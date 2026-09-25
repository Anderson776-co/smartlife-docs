# Prueba de Concepto — ADR-0009

## Objetivo

Validar que el modelo híbrido RBAC + ABAC + ReBAC resuelve simultáneamente los tres tipos de restricción de acceso que exige SmartLife para control por rol, restricción contextual (horario) y validación de pertenencia sobre el recurso, confirmando que ninguna capa por sí sola cubre los cuatro escenarios de seguridad identificados en el ADR (ESC-CAL-SEG-0010, 0014, 0016).

## Métricas y resultados

Se implementó un servicio de autorización con las tres capas aplicadas en cadena, reutilizando el proyecto Spring Boot ya configurado en las PoC anteriores. Se ejecutaron 4 casos de prueba con JUnit:

Un propietario intentando consultar la cuenta de cobro de otra vivienda pasó el filtro RBAC (rol válido) pero fue rechazado por ReBAC ("la cuenta de cobro pertenece a la vivienda 'vivienda-B', pero el usuario pertenece a la vivienda 'vivienda-A'").
Un propietario consultando su propia cuenta pasó tanto RBAC como ReBAC, resultando en acceso permitido.
Un vigilante registrando el ingreso de un visitante fuera de su horario de turno pasó RBAC (rol válido) pero fue rechazado por ABAC ("la hora actual (23:30) esta fuera del turno asignado (06:00 a 14:00)").
Un residente intentando consultar una cuenta de cobro fue rechazado directamente por RBAC ("el rol 'RESIDENTE' no tiene permiso general para consultar cuentas de cobro"), sin llegar siquiera a evaluar las capas posteriores.

Los 4 casos arrojaron "Tests run: 4, Failures: 0, Errors: 0, Skipped: 0" y "BUILD SUCCESS".

## Conclusiones

La PoC valida el criterio central de la decisión: el modelo híbrido resuelve correctamente los tres tipos de restricción que exige el negocio, con cada capa interviniendo exactamente donde le corresponde y sin redundancia. RBAC actúa como primer filtro general por rol, ABAC evalúa condiciones de contexto cuando el rol lo amerita, y ReBAC valida la pertenencia específica del recurso solo para quien ya superó el filtro de rol. El caso 4 confirma además que RBAC funciona como una salida temprana eficiente: cuando el rol ya descarta el acceso, el sistema ni siquiera necesita evaluar las capas de atributos o relaciones, evitando trabajo computacional innecesario. Queda fuera del alcance de esta PoC la persistencia real de usuarios y relaciones de propiedad en base de datos (aquí se usaron objetos en memoria), así como la integración de estas reglas con un framework de seguridad real como Spring Security, que sería el paso natural para llevar esta lógica a producción dentro de SmartLife.
