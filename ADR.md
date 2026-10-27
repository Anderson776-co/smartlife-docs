# Escenarios ADR

## Índice

- **adr-0001** — Seleccionar el proveedor de identidad (identity provider)
- **adr-0002** — Seleccionar servicio de almacenamiento en la nube para archivos multimedia
- **adr-0003** — Seleccionar estrategia para el control de transiciones de estado
- **adr-0004** — Seleccionar la pasarela de pagos (Payment gateway)
- **adr-0005** — Seleccionar estrategia para la recepción de  confirmación de pagos
- **adr-0006** — Seleccionar estrategia de reintentos en fallos de pago
- **adr-0007** — Selección de servicios externos de notificación (Notification gateway)
- **adr-0008** — Selección de estrategia para auditar operaciones criticas
- **adr-0009** — Seleccionar estrategia de autorización y control de acceso a recursos
- **adr-0010** — Seleccionar un estilo arquitectonico para estructurar los componentes del sistema
- **adr-0011** — Seleccionar patron de arquitectura para el backend del sistema
- **adr-0012** — Seleccionar patron de arquitectura para el frontend del sistema
- **adr-0013** — Selección del modeo de procesamiento de la aplicación
- **adr-0014** — Seleccionar estrategia de diseño de software para el funcionamiento de una aplicación sin conectividad de red
- **adr-0015** — Seleccionar herramienta de base de datos local para funcionamiento offline
- **adr-0016** — Seleccionar protocolos de servicio de comunicación
- **adr-0017** — Seleccionar estrategia de desarrollo para la experiencia de usuario multiplataforma
- **adr-0018** — Seleccionar framework del backend
- **adr-0019** — Selección del modelo de base de datos
- **adr-0020** — Seleccionar el motor de base de datos relacional
- **adr-0021** — Seleccionar framework del frontend
- **adr-0022** — Seleccionar plataforma de despliegue del frontend
- **adr-0023** — Seleccionar proveedor de nube
- **adr-0024** — Seleccionar Web Application Firewall
- **adr-0025** — Seleccionar mecanismo de gestión de parámetros de configuración
- **adr-0026** — Seleccionar mecanismo de generación y validación de códigos QR
- **adr-0027** — Seleccionar gestor de mensajes de aplicacion
- **adr-0028** — Seleccionar estrategia de agendamiento de reservas de zonax comunes
- **adr-0029** — Seleccionar mecanismo de gestión de secretos (Key Vault)
- **adr-0030** — Seleccionar mecanismo de instrumentación y observabilidad
- **adr-0031** — Seleccionar estrategia de resiliencia ante fallos en el sistema
- **adr-0032** — Seleccionar estrategia de ejecución de tareas en segundo plano de la aplicación
- **adr-0033** — Seleccionar mecanismo de API Gateway
- **adr-0034** — Seleccionar mecanismo de caché
- **adr-0035** — Seleccionar mecanismo de catálogo de notificaciones

---

## adr-0001 — Seleccionar el proveedor de identidad (identity provider)

**Estado:** Propuesta

### Contexto

Smartlife requiere autenticar usuarios previo a la asignación de reglas de autorización sobre operaciones e información gestionada.

### Escenarios de calidad y funcionalidades significativas referenciados

- ESC-CAL-SEG-0003
- ESC-CAL-SEG-0006

### Decisión

B. Firebase authentication: Se selecciono Firebase como proveedor de identidad de Smartlife debido a sus caracteristicas que benefician arquitectonicamente una escalabilidad a bajo costo, facilidad de despliegue gracias al respaldo que le proporciona google como su proveedor y facilidad de integración dentro del ecosistema de negocio del sistema de las unidades residenciales. Frente al resto de alternativas, en primera instancia se descarto el proveedor Auth0 ya que esta a diferencia de Firebase no se presenta como una alternativa optima a nivel de costo-beneficio ya que aunque inicialmente ambas ofrecen un plan gratuito inicial, Firebase lo proporciona para un limite inicial de 50.000 usuarios activos mensuales, mientras que Auth0 dispone de un limite inciial de 25.000 usuarios activos mensuales. Adicionalmente a nivel de escalabilidad y soporte de mas usuarios, un sistema para unidades residenciales tiene la caracteristica de poder crecer de manera sostenida de acuerdo al número y tamaño de las unidades residenciales, por lo que a nivel de optimización de costos firebase es una mejor alternativa ya que proporciona un cobro por usuario activo mensual al limite inicial que baja gradualmente según el crecimiento de usuarios que inicien sesión mensualmente en la plaicación, mientras que firebase mantiene un crecimiento sostenido en sus diferentes planes por cada 500 usuarios activos mensuales, comenzando desde los 35 USD para los primeros 500 hasta los 2100 USD para mas de 30.000 usuarios, lo que genera un sobrecosto en licenciamientos para un proveedor de identidad. Adicionalmente firebase ofrece una mayor facilidad de despliegue para su propio ecosistema de servicios de cloud de google, sin introducir complejidad operativa en la configuración de parametros de autenticación, proporciona una alternativa más rapida de migración de usuarios a otro proveedor de identidad diferente, ya que permite realizar una migración masiva de usuarios junto con sus hashes de contraseña, sin obligar al usuario a cambiar su contraseña luego de la migración, mientras que con Auth0 implica una migración masiva dependiente de cambios de contraseña. Frente a Amazon Cognito ofrece un limite gratuito inicial de solamente 10.000 usuarios mensuales activos, lo que en un ambiente de crecimiento de usuarios supone un aumento más temprano de los costos aunque mantenga la misma dinamica da disminución progresiva por cada nuevo usuario activo mensual que supere el limite gratuito. Adicionalmente firebase destaca sobre AWS cogntio por ser un proveedor de identidad pensado para optimizar la experiencia y velocidad en la integración, ya que Cognito requiere una configuración inicial para el grupi de usuarios (user pools), clientes de aplicación, comportamiento de tokens y las interacciones de identidades con servicios AWS, lo que complejiza la integración de este servicio dentro de Smartlife. Frente a keycloak al ser un proveedor de identidad open source que requiere ser autohospeado, se asumen responsabilidades como la configuración desde cero de los parametros de seguridad, configuraciones de despliegue y elementos de soporte de su servicio como planes de actualización, observabilidad de eventos y estrategias para garantizar una alta disponibilidad, lo que supone un aumento de la carga operativa y costes de mano de obra, lo que a largo plazo a nivel de costo-beneficio, adoptar firebase como proveedor de identidad, permitirá que se adopten mas servicios de su ecosistema que no solo facilitará su integración, sino que optimizará su despliegue en nube y la carga operativa para recursos de soporte como la autenticación de un sistema será menor. Frente a el proveedor de identidad Microsoft Entra ID, se eligio Firebase authentication debido a que permite cibrir necesidades de autenticación para los diferentes roles del sistema: vigilantes, administrador, residente y propietario que requieren ingresar al sistema sin requierir directamente capacidades empresariales para el control de identidad de los usuarios y sin requierir implementación externa como lo es Microsoft entra external ID para permitir la autenticación de aquellos que no tengan ningun tipo de relaacion laboral con la urbanización. Adicionalmente aunque este servicio de entra external ID proporcione el mismo umbral limite de plan gratuito de 50.000 usuario activos mensuales, cuando se sobrepase mantiene un cobro sostenido de 0,03 dolares por cada nuevo usuario activo al mes. Frente a la opcion de autenticación OTP, que es una alternativa de desarrollo propio para la autenticación, y se prioriza la adopción de Firebase authentication como proveedor de identidad principalmente a sus facilidades de integración y despliegue, lo que no solo permite que se puedan usar plataformas existentes expertas en el tema de procesos de autenticación, sino que permite un acceso a diferentes capas de autenticación por medio de proveedores federados, autenticación multifactor y servicios de inciio de sesió passwordless mediante auntenticación por SMS o enlaces de correo electronico.

### Opciones consideradas

- A. Auth0: Plataforma de gestión de identidad y acceso (IAM) orientada a desarolladores. Permite implementar autenticación, autorización, SSO (Single Sign-On), MFA (Autenticación multifactor) y gestión de usuarios mediante SDK y APIs. Utiliza estandares como OAuth 2.0 y OpenID Connect.
- B. Amazon Cognito: Servicio de identidad de AWS para aplicaciones web y móviles. Permite registrar y autenticar usuarios mediange User Pools y, mediante Identity Pools, para proporcionar credenciales temporales para acceder a recursos de AWS.
- C. Firebase Authentication: Servicio de autenticación de Firebase/Google que facilita la implementación de inicio de sesión mediante correo y contraseña, teléfono y proveedores como Google, Apple, Facebook o Github. Cuenta con SDK para web, Android, IOS y otras plataformas.
- D. Keycloak: Plataforma open source de gestión de identidad y acceso que puede ser instalada y administrada por la propia organización. Soporta SSO, OAth 2.0, OpenID Connect, SAML y diferentes mecanismos de autorización.
- E. Microft EntraID: Es un servicio de gestión de identidades y acceso (IAM) basado en la nube de Microsoft que permite administrar usuarios, grupos, aplicaciones, dispositivos y recursos desde un punto centralizado. Sus principales características incluyen autenticación multifactor (MFA), inicio de sesión único (SSO), restablecimiento de contraseñas, control de acceso basado en roles (RBAC), gestión de identidades y aplicaciones, y Acceso Condicional, que permite aplicar políticas de acceso según factores como el usuario, dispositivo, ubicación o nivel de riesgo.
- F. Autenticación OTP: (One-Time Password) es un mecanismo que utiliza códigos de un solo uso para verificar la identidad de un usuario, de manera que cada código solo puede utilizarse una vez y, en modalidades como TOTP, tiene además una vigencia limitada basada en el tiempo. Los OTP pueden generarse mediante aplicaciones, dispositivos físicos o enviarse por canales externos, y permiten demostrar que el usuario posee el dispositivo o autenticador asociado a su cuenta.

### Consecuencias positivas

- Reduce la complejidad de implementación: Propociona SDK, servicios de backend y componentes preperados para autenticación mediante correo/contraseña, vinculo de correo, por número de telefono y proveedores federados (google, apple).
- Facilita la implementación de controles de acceso basados en identidad: Proporciona identidicadores únicos y permite incorporar custom claims para establecer reglas de seguridad del control de acceso.
- Reduce la responsabilidad de seguridad sobre credenciales: Firebase administra elementos sensibles de la autenticación, usa protocolos HTTPS para los datos en transito y cifrado para datos almacenados en reposo.
- Manejo adecuado de las sesiones mediante tokens: Establecen mecanismos de identificación mediante tokens con duraciones de 1 hora.

### Consecuencias negativas

- Aumento de la dependencia del ecosistema Google/Firebase.
- Limites y cuotas periodicas sobre operaciones: Firebase tiene limites operativos sobre la creación de cuentas, recuperación de contraseña, correos de verificación y solicitudes de autenticación.
- Requiere del cambio a Identity Platform: Firebase Authrntication básico no incluye todas las capacidades avanzadas de autenticación al estar orientada para B2C y requiere de integrarse con Identity Platform para conectar proveedores corporativos mediante protocolos SAML y OpenID Connect, autenticación multifactor y multitenancy.

---

## adr-0002 — Seleccionar servicio de almacenamiento en la nube para archivos multimedia

**Estado:** Propuesta

### Contexto

Smartlife requiere gestionar el acceso a datos no estructurados (imágenes, videos y documentos) adjuntos en la generación de comunicados, reportes de incidentes y PQRS.

### Escenarios de calidad y funcionalidades significativas referenciados

- HU78 - Reporte de incidente
- HU83 - Radicación de PQRS
- HU81 - Envío de comunicado informativo a usuarios

### Decisión

B. Cloudflare R2: Se seleccionó Cloudflare R2 como servicio de almacenamiento de objetos no estructurados. Cloudflare es un proveedor que permite almacenar y servir contenido a través de Internet sin cobros por transferencia de datos de salida (egress), independientemente de la clase de almacenamiento. De acuerdo con las necesidades del proyecto, que contempla la descarga constante u ocasional de contenido multimedia, Cloudflare R2 resulta una alternativa atractiva para estos fines.

Frente a Google Cloud Storage, Cloudflare R2 se presenta como un servicio moderno de almacenamiento de objetos diseñado sin comisiones de salida. Esto lo diferencia de Google Cloud Storage, que es un servicio tradicional de almacenamiento en la nube orientado a empresas e integrado con herramientas avanzadas de análisis de datos y aprendizaje automático, pero que aplica cobros asociados al volumen de descarga de archivos. Adicionalmente, Cloudflare R2 cuenta con una tarifa de almacenamiento plana y uniforme a nivel mundial de 0,015 dólares por GB al mes para el nivel Standard, lo que contribuye a la optimización de costos en tecnologías de uso sencillo, como el almacenamiento y la descarga de archivos e imágenes generados desde el mismo sistema de la unidad. En contraste, los precios de Google Cloud Storage varían según la región, con valores que rondan entre 0,020 y 0,026 dólares por GB al mes, dependiendo del tipo de ubicación y la modalidad de uso.

Frente a OCI Object Storage, Cloudflare R2 también destaca por no aplicar tarifas de salida para los archivos. Aunque OCI Object Storage ofrece tarifas bajas de almacenamiento y transferencia, incluyendo una asignación de hasta 10 TB de transferencia mensual a nivel mundial, Cloudflare mantiene como ventaja la ausencia de cobros por transferencia de salida.

Frente a Backblaze B2, aunque ambas herramientas favorecen una buena relación costo-beneficio para el almacenamiento masivo de datos, Cloudflare R2 continúa diferenciándose por la ausencia de cobros por transferencia de datos de salida. Adicionalmente, Cloudflare R2 dispone de dos niveles de acceso, Standard e Infrequent Access, que permiten gestionar de manera diferenciada los datos que requieren acceso regular o frecuente y aquellos archivos que se consultan con menor frecuencia. En contraste, Backblaze B2 utiliza un único nivel de almacenamiento de alta frecuencia (Single-Tier o Always-Hot).

Finalmente frente a AWS S3 se escogió Cloudflare R2 debido a que ofrece almacenamiento de objetos compatible con el mismo S3 por un menor precio de almacenamiento al mes, donde mientras S3 cobre 0,023 USD por GB al mes por los primeros 50TB, cloudflare R2 cuenta con un plan gratuito de 10GB de almacenamiento, y un cobro de 0,015 USD luego de sobrepasar este limite para almacenamiento estandar y 0,01 USD para almacenamiento de acceso poco frecuente. Adicionalmente es un servicio que cubre las necesidades de almacenamiento y con un umbral gratuito para almacenamientos estandar (1 millon de solicitudes para operaciones de clase A - Escritura, modificación y listado de datos y 10 millones de solicitudes para operaciones de clase B - Lecturas) y permite las salidad de objetos de información de manera gratuita.

En conclusión, Cloudflare R2 se considera una alternativa adecuada para el proyecto debido principalmente a la ausencia de costos de egress, sus tarifas de almacenamiento competitivas y la disponibilidad de diferentes niveles de acceso, características que se ajustan a las necesidades de almacenamiento y descarga de contenido multimedia del sistema.

### Opciones consideradas

- A. Backblaze B2: Servicio de almacenamiento de objetos orientado a ofrecer costos predecibles y bajos. Es compatible con la API de Amazon S3, permite almacenar y distribuir archivos mediante API y navegador, y ofrece integración con numerosas herramientas de backup, multimedia y nube. Actualmente incluye transferencia de salida gratuita hasta 3 veces el almacenamiento mensual promedio.
- B. Cloudflare R2: Servicio de almacenamiento de objetos compatible con S3, diseñado para almacenar y servir grandes cantidades de datos. Su principal ventaja es que no cobra tarifas de salida (egress), lo que resulta atractivo para aplicaciones que entregan frecuentemente imágenes, videos y otros archivos. Además, se integra directamente con Cloudflare Workers y su red global.
- C. Google Cloud Storage: Srvicio administrado y escalable basado en Buckets y objetos. Cuenta con diferentes clases de almacenamiento (Standard, Nearline, Coldline y Archive), para adaptar costos al patrón de acceso e integraciones con Pub/Sub, auditoria y contoles de acceso.
- D. OCI Object Storage: Servicio de almacenamiento de objetos en la nube de Oracle para el almacenamiento de datos no estructurados (imagenes y videos). Destaca por su consistencia, redundancia, cifrado, politicas IAM, versionado, replicación y distribución en diferentes niveles de almacenamiento.
- E. Amazon S3: Servicio de almacenamiento de objetos en la nube orientado a ofrecer alta escalabilidad, durabilidad y disponibilidad. Permite almacenar y recuperar archivos mediante API, organizarlos en buckets y aplicar controles de acceso y cifrado. Además, ofrece diferentes clases de almacenamiento para optimizar costos según la frecuencia de acceso y permite pagar únicamente por los recursos utilizados.

### Consecuencias positivas

- Sin tarifas de salida: Elimina el costo por transferir o descargar tus datos fuera de la nube, evitando que el crecimiento en las consultas de archivos incremente los costos operativos del sistema a medida que aumenta el número de usuarios. Esto se validó mediante una prueba de concepto, en la que múltiples operaciones de subida y descarga de archivos no generaron ningún cargo en el panel de facturación.
- Compatible con Amazon S3: Permite integrar el almacenamiento usando SDKs y herramientas ya estandarizadas en la industria, sin depender de una librería propietaria del proveedor, lo que facilita reemplazar o combinar el servicio con otras soluciones compatibles con S3 en el futuro si la arquitectura lo requiere.
- Precios bajos: El costo por gigabyte almacenado y por operación de lectura y escritura es más bajo frente a la competencia, reduciendo el costo total de propiedad de la arquitectura a medida que crece el volumen de archivos generados por comunicados, incidentes y PQRS.
- Integración nativa: Conexión con el ecosistema Cloudflare, que permite servir los archivos a través de su capa de caché para reducir la latencia percibida por el usuario, sin necesidad de implementar ni mantener un CDN adicional. En pruebas realizadas, la descarga directa al bucket sin esta capa de caché tomó entre 524 ms y 1617 ms según el archivo, evidenciando el margen de mejora que ofrece esta integración para los adjuntos consultados con mayor frecuencia.

### Consecuencias negativas

- Menor cantidad de regiones: A diferencia de AWS S3 o Google Cloud Storage, que permiten elegir una región específica de almacenamiento, en R2 la ubicación del bucket se maneja como "automática" (auto), con solo unas pocas zonas de referencia disponibles (por ejemplo Eastern North America, Western North America, Europa o Asia-Pacífico). Esto limita el control fino sobre dónde residen físicamente los datos en comparación con proveedores que ofrecen selección explícita de región.
- Latencia sin cache: Al acceder a los objetos directamente contra el endpoint del bucket, sin pasar por la red de Cloudflare, el tiempo de respuesta depende únicamente de la distancia al centro de datos y no se beneficia de ningún punto de presencia cercano al usuario. En la prueba de concepto realizada, la descarga directa al bucket sin esta capa de caché tomó entre 524 ms y 1617 ms según el archivo, confirmando que esta variación es real y relevante para el sistema.
- Compatibilidad limitada con S3: Cloudflare documenta oficialmente que R2 no implementa todas las operaciones de la API de S3; por ejemplo, no soporta ciertas operaciones de ACL de bucket u objeto, ni todos los algoritmos de checksum que sí soporta S3. En la prueba de concepto se evidenció esta limitación de forma directa: R2 no soporta el método de firmado por streaming (Streaming SigV4) que el SDK de AWS utiliza por defecto en las operaciones de subida, siendo necesario deshabilitarlo explícitamente para lograr una carga exitosa de archivos.
- Dependencia del ecosistema: Funcionalidades adicionales como el procesamiento de imágenes al vuelo, reglas de acceso más granulares o la personalización del comportamiento de entrega de archivos solo están disponibles integrando R2 con Cloudflare Workers, lo que añade una curva de aprendizaje adicional y acopla parte de la arquitectura al ecosistema de un único proveedor, en lugar de depender únicamente del estándar S3.
- Restricciones operativas en sus herramientas de migración: R2 impone un límite de tamaño por operación de subida simple (5 GiB por objeto), por lo que archivos más grandes requieren subida multiparte, y las herramientas de migración masiva dependen de qué tan completa sea la compatibilidad de R2 con la API de S3 del proveedor de origen, lo que puede generar limitaciones al migrar grandes volúmenes de datos desde otros servicios.

---

## adr-0003 — Seleccionar estrategia para el control de transiciones de estado

**Estado:** Propuesta

### Contexto

SmartLife requiere controlar las transiciones de estado válidas de las entidades del sistema que siguen un ciclo de vida definido, como pagos, reservas e incidentes.

### Escenarios de calidad y funcionalidades significativas referenciados

- ESC-CAL-FIA-0005, ESC-CAL-FIA-0007, ESC-CAL-CPA-0006

### Decisión

A. Máquina de estados explícita: Se selecciona el patrón de Máquina de Estados como estrategia para controlar el ciclo de vida de las entidades del sistema que pasan por estados definidos (un pago, una reserva, un incidente). En lugar de dispersar la validación de "qué transición es válida" en condicionales repartidos por distintos servicios, se centraliza explícitamente el conjunto de estados posibles y los eventos que disparan cada transición válida entre ellos, rechazando de forma estructural cualquier cambio de estado no contemplado por ejemplo, que un pago pase directamente de "fallido" a "aplicado" sin pasar por "aprobado". Al estar el backend ya construido en Spring Boot, la implementación concreta de esta estrategia se apoya de forma natural en las herramientas que el propio ecosistema Spring ofrece para este patrón, de la misma manera en que ya se apoyaron en Spring Modulith para la modularidad y en Spring Cloud Config para la configuración externa.

Frente a la validación manual dispersa (opción B), se descarta porque depende de que cada desarrollador recuerde escribir correctamente la validación de transición en cada punto del código donde se modifica el estado de una entidad, sin ninguna garantía estructural que impida un error humano un riesgo real en un equipo de varias personas trabajando en paralelo sobre distintos módulos del mismo Monolito.

Frente al patrón State orientado a objetos clásico (opción C), aunque encapsula mejor el comportamiento por estado que un simple enum con condicionales, sigue exigiendo que el propio equipo diseñe y mantenga a mano toda la lógica de transición dentro de las clases de estado, sin una definición declarativa centralizada ni una verificación automática de que todas las transiciones estén cubiertas.

Frente a un motor de flujo de trabajo completo (BPMN, opción D), se descarta por sobreingeniería: este tipo de herramientas está pensado para procesos de negocio grandes, con pasos de aprobación humana, temporizadores y ramificaciones complejas, mientras que los ciclos de vida identificados en SmartLife (un pago, una reserva) son acotados a pocos estados, sin justificar la complejidad operativa adicional que exigiría desplegar y mantener un motor de este tipo.

En conclusión, la Máquina de Estados explícita se considera la estrategia más adecuada para SmartLife, ya que hace estructuralmente imposible una transición de estado inválida, sin la sobreingeniería de un motor de flujo de trabajo completo ni la fragilidad de depender de que cada desarrollador valide manualmente cada transición.

### Opciones consideradas

- A. Máquina de estados explícita: Patrón de diseño que define de forma centralizada y declarativa los estados posibles de una entidad y los eventos que disparan cada transición válida entre ellos, rechazando cualquier transición no contemplada.
- B. Validación manual dispersa: Un enum con los estados posibles, validado mediante condicionales (if/switch) escritos a mano en cada servicio que modifica el estado de la entidad, sin un mecanismo centralizado que garantice la validez de la transición.
- C. Patrón State orientado a objetos (GoF): Encapsula el comportamiento específico de cada estado en una clase separada, delegando en cada una la decisión de a qué estado se puede transicionar, sin una definición declarativa centralizada de todo el flujo.
- D. Motor de flujo de trabajo completo (BPMN): Herramienta especializada en modelar procesos de negocio complejos, con soporte para pasos de aprobación humana, temporizadores y ramificaciones, más allá de una simple transición de estados.

### Consecuencias positivas

- Transiciones inválidas estructuralmente imposibles: El propio patrón rechaza cualquier cambio de estado no definido, sin depender de que un desarrollador recuerde validarlo manualmente en cada punto del código.
- Lógica de transición centralizada y auditable: Todas las reglas de "qué estado puede pasar a cuál" quedan en un solo lugar, facilitando su revisión frente a tenerlas dispersas en distintos servicios del Monolito.
- Coherencia con eventos de dominio del Monolito Modular: Cada transición de estado puede dispararse como un evento de dominio, alineándose con la comunicación entre módulos ya establecida en el ADR-0018 (Spring Modulith).
- Base natural para el Audit Log: Cada transición registrada es exactamente el tipo de evento que ya se documenta en el Audit Log append-only del ADR-0008(quién cambió el estado, de cuál a cuál, y cuándo).
- Refuerza la idempotencia frente a operaciones duplicadas: Al no existir una transición válida definida desde un estado hacia sí mismo, una orden repetida (por un reintento del backoff exponencial del ADR-0006, o un doble clic del usuario) no ejecuta ninguna acción adicional, sustentando directamente el escenario ESC-CAL-FIA-0007 ("clic repetido sobre 'pagar' → se procesa una única solicitud") sin depender exclusivamente de la idempotency key externa que ya ofrece Wompi en el ADR-0004.

### Consecuencias negativas

- Curva de diseño inicial: Exige modelar explícitamente todos los estados y transiciones válidas antes de implementar, un esfuerzo de diseño que no existiría con validaciones manuales improvisadas sobre la marcha.
- Rigidez ante cambios de flujo: Agregar un nuevo estado o una nueva transición exige modificar la definición formal de la máquina de estados, en lugar de simplemente agregar un if adicional en un punto del código.
- Curva de aprendizaje para el equipo: El equipo debe familiarizarse con el patrón y con las convenciones propias de su implementación dentro del ecosistema Spring, si no tienen experiencia previa con este tipo de herramienta.

---

## adr-0004 — Seleccionar la pasarela de pagos (Payment gateway)

**Estado:** Propuesta

### Contexto

Smartlife requiere procesar pagos en línea entre el sistema de las unidades residenciales y las entidades financieras.

### Escenarios de calidad y funcionalidades significativas referenciados

- ESC-CAL-ESC-0001
- HU55 - Procesar transacción de pago.

### Decisión

A. Wompi: Se selecciona Wompi como proveedor de pasarela de pagos debido a su presencia en Colombia gracias a su respaldo del ecosistema Bancolombia, sobre el ofrecimiento de servicios de los medios de pago flexibles y frecuentadas por la población colombiana comi tarjetas de débito y crédito de Visa, Mastercar y American Express, PSE, Botón Bancolombia, Nequi, Daviplata, y efectivo a traves de corresponsales bancarios de bancolombia. Con respecto a las otras alternativas una en las que se destaca la decision es por el tema de costos de comisión por uso. Frente al resto wompi destaca por tener un porcentaje menor con respecto a las otras alternativas, mientras wompi posee una comisión por transacción exitosa del 2,65% + $700 COP + IVA y recepción del dinero al seguiente dia habil en su Plan Avanzado Agregador y la posibilidad de negociar comisiones por transacción exitorsa con Bancolombia en su plan anvanzado gateway, Mercado pago posee una comisión por transacción exitosa modificacle según el momento de recepción del dinero - (Inmediatamente 3,29% + $800 + IVA, 7 días 2,99% + $800 + IVA, 14 días 2,79% + $800 + IVA), PayU establece una comisión por transacción aplicable a tarjetaas, PSE, billeteras digitales y QR del 3,29% + $300 + IVA por transacción exitosa, Bold establece comisiones desde el 2,89% hasta el $3,99 con un cobro fijo de $900 y Stripe usa un modelo internacional de tarifas que para un negocio que empezará aqui en Colombia no será beneficioso. Adicionalmente al igual que las otras alternativas posee certificaciones de seguridad PCI DSS Nivel 1, tokenización de datos bancarios sensibles y desconexión total del sistema sobre la gestión de estos datos, inicialmente se plantea el uso de wompi por su facilidad de integración por web checkouts al igual que las otras alternativas. Sin embargo, influye el dominio sobre el ecosistema de los pagos en linea de Colombia gracias al respaldo de Bancolombia, donde mezclando estos dos factores permite ofrecer un servicio de pasarela de pagos para las unidades residenciales con costos competitivos y diversidad de medios de pago propios del ecosistema financiero Colombiano. Cabe recalcar que Wompi como payment gateway cuenta con una arquitectura basada en eventos para informar en el momento en que haya ocurrido algo importante con el procesamiento del pago usandpo webhook.

### Opciones consideradas

- A. Wompi: Pasarela de pagos nativa en Colombia orientada a desarrolladores. Ofrece integración simplificada mediante widgets e APIs REST, soporte nativo para Nequi, PSE, tarjetas de crédito/débito y corresponsales bancarios, delegando la responsabilidad de cumplimiento PCI-DSS Nivel 1. Es ideal para startups, e-commerce locales y que buscan integración rápida y baja tasa de abandono en pagos digitales dentro de Colombia.
- B. Mercado Pago: Pasarela regional de gran alcance, espaldada por el ecosistema Mercado Libre. Es ideal para tiendas B2C (Business to Consumer) y comercio conversacional. Ofrece soporte multimoneda, financiación (cuotas) y diversidad de medios de pago locales (tarjetas, PSE, efecty). Incluye un motor propio de prevención de fraude basado en aprendizaje automático, aunque con comisiones ligeramente superiores en recaudos recurrentes.
- C. PayU: Pasarela de pagos con trayectoria e infraestructura en America Latina. Es ideal para empresas de volumen mediano y grande. su alta cobertura regional (operando en Colombia, México, Argentina, Brasil, Chile, Perú, etc.) y su amplia disponibilidad de medios de pago, permitiendo procesar tarjetas locales e internacionales, transferencias (PSE), billeteras digitales y efectivo en puntos de recaudo (Efecty, Baloto, etc.). Cuenta con un módulo antifraude avanzado (PayU Anti-Fraud) y tarifas de comisión negociables por volumen de ventas.
- D. ePayco: Plataforma colombiana versátil orientada a recaudo, suscripciones y modelos de netocio complejos que incluye soporte para soluciones tipo marketplace (split o divisiones de pagos entre múltiples vendedores), pagos recurrentes y recepción de giros/efectivo. Ofrece múltiples modelidades de integración (desde enlaces directos hasta contenedores para CMS) y comisiones competitivas para PyMES.
- E. Bold: Pasarela con enfoque omnicanal (venta presencial + e-commerce) diseñada para aquellos comercios en crecimiento. Permite la integración de datáfonos físicos inalambricos sin menusalidades fijas, que complemementan con una pasarela digital para cobrar vía PSE, Nequi y tarjetas debito/credito, facilitando las ventas fisicas y virtuales desde un único panel administrativo.
- F. Stripe: Plataforma estándar global en procesamiento de pagos digiltales e infraestrcutura financiera la cual esta orientada a desarrolladores, ofrece una API robusta, herramientas avanzadas de prevención de fraude (Stripe Radar) y un ecosistema completo para cobros internacionales y suscripciones (Stripe Billing). Es la opción predilecta para SaaS globales, empresas multinacionales y startups con clientes en el exterior.

### Consecuencias positivas

- Facilita la integración con el sistema: Wompi ofrece tres modalidades de integración: Widget, Checkout Web y API REST para permitir disminuir el esfuerzo inicial de desarrollo y adaptar la experencia de pago a la arquitectura del sistema.
- Ofrece varios medios de pago colombianos: Wompi permitr aceptar tarjetas Visa, Mastercard y American Express, PSE, Nequi, Botón bancolombia y medios de pago presenciales por corresponsales bancarios, lo que aumenta las alternativas disponibles al propietario sin necesidad de integrar las entidades financieras requeridas.
- Permite recibir notificaciones mediante webhook: Proporciona un mecanismo de webhooks para notificar al sistema cuando cmabia el estadi de una transacción.
- Dispone de mecanismos con idempotencia: La API de Wompi cuenta con mecanismos de idempotencia para evitar operaciones duplicadas mediante una llave única generada con expiración de 24 horas.

### Consecuencias negativas

- Introduce dependencia de un proveedor externo: Aumenta la dependencia de la disponibilidad de uno de los componentes criticos del sistema a la disponibilidad de un componente externo.
- Existe un costo por transacción: Cada transacción que se procese exitosamente tendra un costo sobre el valor a pagar, lo que introduce un aumento del monto a pagar en las unidades residenciales.
- Dependencia de las condiciones comerciales de Wompi: El negocio debe tener relacionadas estar vinculado de manera activa con una cuenta bancaria de Bancolombia para negociar las tarifas acordadas con Bancolombia.
- El dinero de la transacción no es depositado inmediatamente: En el plan Avanzado wompi indica que el dinero se recibe al siguiente día hábil y en el plan gateway se divide segun su medio de pago: Boton bancolombia de forma inmediata, tarjetas y nequi al siguiente dia hábil y PSE toma unas cuantas horas o al siguiente día hábil.

---

## adr-0005 — Seleccionar estrategia para la recepción de  confirmación de pagos

**Estado:** Propuesta

### Contexto

Smartlife requiere confirmar el pago procesado para garantizar la completitud de la transacción y su aplicación a las cuentas de cobro correspondientes.

### Escenarios de calidad y funcionalidades significativas referenciados

- ESC-CAL-FIA-0005
- Relacionado: HU57-Aplicar pago a una cuenta de cobro, HU60-Actualizar estado de cuenta de cobro

### Decisión

D. Combinación Webhook + Reconciliación periódica de respaldo: Se selecciona el Event-Driven Webhook Ingestion Handler (opción D: Combinación Webhook + Reconciliación periódica de respaldo) como mecanismo de confirmación de pagos para SmartLife, dado que el requisito de negocio exige notificar "de inmediato" al usuario sobre el resultado de su transacción.

Comparando los tres mecanismos base en términos de latencia: el Scheduled Polling / Batch Sync (opción A) introduce una latencia mínima igual al intervalo del cron job, por ejemplo, con una consulta cada 5 minutos, un pago aprobado a los 10 segundos de iniciado el ciclo tardaría hasta 4 minutos y 50 segundos en reflejarse, muy por encima de una notificación "inmediata". El Long Polling desde el backend (opción C) reduciría en teoría la frecuencia de solicitudes vacías frente al Scheduled Polling, pero Wompi no ofrece soporte nativo para este patrón en su API pública, por lo que implementarlo exigiría simular el comportamiento con reintentos cortos y espaciados, sin ninguna garantía real de reducción de carga frente a un polling programado bien calibrado, además de agregar complejidad de implementación sin un beneficio comprobado.

Frente a ambas, los webhooks notifican en el orden de milisegundos a pocos segundos desde que la pasarela confirma la transacción, siendo el único de los tres mecanismos que cumple el requisito de inmediatez sin depender de un intervalo fijo ni de una acción del usuario. Para mitigar la debilidad reconocida de los webhooks (pérdida de entrega ante caídas de red del lado del receptor), se complementa esta decisión con un job de reconciliación periódico de baja frecuencia (equivalente al patrón de la opción A) que actúa exclusivamente como mecanismo de respaldo ante webhooks no recibidos, y no como vía principal de confirmación. Esta combinación no mezcla dos tecnologías compitiendo por resolver la misma pregunta, sino un mecanismo principal y un respaldo explícitamente subordinado para cubrir una debilidad reconocida del primero, análogo al caso de Resilience4j como complemento del aislamiento de fallos entre módulos.

En conclusión, la combinación de webhooks como vía principal y un job de reconciliación de baja frecuencia como respaldo se considera la estrategia más adecuada para SmartLife, ya que cumple el requisito de inmediatez exigido por el negocio sin sacrificar la confiabilidad ante fallos puntuales de entrega, resolviendo así tanto la velocidad de notificación como la garantía de que ningún pago aprobado quede sin reflejarse en el sistema.

### Opciones consideradas

- A. Scheduled Polling / Batch Sync (Cron Jobs): El backend de SmartLife consulta periódicamente la API REST de Wompi (GET /transactions/{id}) para verificar el estado de las transacciones pendientes, mediante un proceso programado a intervalos fijos.
- B. Event-Driven Webhook Ingestion Handler (Webhooks del proveedor): Wompi notifica de forma activa al backend de SmartLife mediante un evento HTTP POST apenas cambia el estado de una transacción, sin que SmartLife tenga que preguntar.
- C. Long Polling desde el backend: El backend de SmartLife abre una solicitud hacia la API de Wompi y la mantiene abierta esperando una respuesta hasta que hay una actualización de estado o se cumple un tiempo límite, repitiendo el ciclo inmediatamente después. Reduce la frecuencia de solicitudes vacías frente al Scheduled Polling, pero Wompi no ofrece soporte nativo para este patrón en su API pública, por lo que implementarlo implicaría simular el comportamiento con reintentos cortos y espaciados, sin ninguna garantía real de reducción de carga frente a un polling programado bien calibrado.
- D. Combinación Webhook + Reconciliación periódica de respaldo: Usa el mecanismo B (Webhooks) como vía principal de confirmación, complementado con el mecanismo A (Scheduled Polling de baja frecuencia) actuando exclusivamente como respaldo ante la pérdida de un webhook por caída de red del lado del receptor.

### Consecuencias positivas

- Confirmación casi inmediata: El usuario recibe confirmación de su pago en el orden de milisegundos a pocos segundos, cumpliendo directamente el requisito de negocio de notificación inmediata.
- Menor carga de cómputo en el servidor: Frente a un mecanismo de sondeo periódico masivo sobre la totalidad de transacciones pendientes, el sistema solo procesa un evento cuando efectivamente ocurre.
- Respaldo ante fallos puntuales de entrega: El job de reconciliación complementario reduce a un mínimo el riesgo de transacciones aprobadas que nunca se reflejan en el sistema, cubriendo la debilidad reconocida de los webhooks ante caídas de red.
- Independiente del comportamiento del usuario: No depende de que el usuario mantenga una sesión activa ni vuelva a abrir la aplicación para completar la confirmación del pago.
- Escalable sin aumentar carga sobre la pasarela: Permite escalar el volumen de transacciones sin necesidad de aumentar la frecuencia de consultas activas contra la API de Wompi.

### Consecuencias negativas

- Depende de la disponibilidad del endpoint receptor: Una caída de la infraestructura receptora de webhooks durante el envío implica pérdida de la notificación si no existe reconciliación posterior.
- Requiere validación de autenticidad: Exige implementar validación de firma del webhook para prevenir notificaciones de pago falsificadas (OWASP A08).
- Exige diseño idempotente: El endpoint receptor debe diseñarse de forma idempotente, dado que Wompi puede reintentar el envío del mismo webhook más de una vez.
- Dependencia con el mecanismo de tareas programadas: El job de reconciliación de respaldo depende de contar con un mecanismo de ejecución de tareas programadas ya definido dentro del backend (Spring Scheduler o equivalente).
- Superficie de ataque ampliada: Al exponer un endpoint público para recibir los webhooks, se amplía la superficie de ataque del sistema, requiriendo controles adicionales de validación de origen y límite de tasa.

### Notas

- Preguntar si se necesita este

---

## adr-0006 — Seleccionar estrategia de reintentos en fallos de pago

**Estado:** Propuesta

### Contexto

Smartlife requiere procesar pagos que pueden presentar fallos por problemas de red o indisponibilidad de la pasarela.

### Escenarios de calidad y funcionalidades significativas referenciados

- ESC-CAL-FIA-0001
- ESC-CAL-FIA-0002

### Decisión

D. Reintento con Backoff exponencial + Jitter:Se selecciona la estrategia de Reintento con Backoff exponencial + Jitter (opción D), basándose en la Best Practice del Well-Architected Framework en su pilar de fiabilidad, para el control y límite de las llamadas de reintento. Se define una política concreta de 5 intentos máximo, con intervalos base de 1s, 2s, 4s, 8s y 16s (sumando hasta 31 segundos de espera acumulada en el peor caso), a los que se aplica una variación aleatoria (jitter) de hasta ±20% sobre cada intervalo.

Frente a no tener reintento automático (opción A), esta alternativa implica que el 100% de los fallos transitorios de red requerirían intervención manual del usuario, sin ninguna recuperación automática, trasladando al propietario la responsabilidad de detectar el fallo y volver a intentar el pago por su cuenta.

Frente al reintento inmediato (opción B), esta alternativa reintentaría sin espera alguna (0 segundos entre intentos), lo que ante una caída del proveedor generaría una ráfaga de solicitudes simultáneas de todos los usuarios afectados en el mismo instante, saturando tanto al proveedor como al propio sistema en el peor momento posible, justo cuando ya está degradado.

Frente al backoff exponencial sin jitter (opción C), esta alternativa sí espacía los intentos en la misma secuencia de 1s, 2s, 4s, 8s, 16s, pero al no tener variación aleatoria, todos los clientes que fallaron en el mismo instante inicial reintentarían exactamente en los mismos segundos 1, 3, 7, 15 y 31, generando picos de carga sincronizados sobre el proveedor en cada reintento. La estrategia D evita ese efecto añadiendo el jitter, distribuyendo esos mismos reintentos en una ventana de tiempo en vez de en un instante único.

En conclusión, el Backoff exponencial + Jitter se considera la alternativa más adecuada para SmartLife, ya que reduce la sobrecarga sobre el proveedor y el propio sistema durante fallos transitorios, evita la sincronización de reintentos entre múltiples usuarios afectados simultáneamente, y mejora la probabilidad de recuperación exitosa sin intervención manual, siguiendo la práctica recomendada por el Well-Architected Framework para el manejo de fallos de interacción entre sistemas.

### Opciones consideradas

- A. Sin reintento automatico: Ante un fallo, la operación termina inmediatamente y el usuario debe intentar nuevamente.
- B. Reintento inmediato: La misma operación se vuelve a ejecutar inmediatamente después de un fallo, normalmente con un número máximo muy pequeño de intentos.
- C. Reintento con Backoff exponencial: El tiempo entre intentos aumenta progresivamente, por ejemplo 1 s → 2 s → 4 s → 8 s. Normalmente se establece un máximo de intentos y/o un tiempo máximo.
- D. Reintento con Backoff exponencial + jitter: Combina el backoff exponencial con una variación aleatoria (jitter) aplicada al tiempo de espera de cada reintento.

### Consecuencias positivas

- Reduce la sobrecarga sobre el proveedor y los recursos del sistema: ya que el backoff exponencial incrementa el tiempo de espera entre intentos para evitar el recibimiento continuo de solicitudes mientras el proveedor se recupera.
- Reduce el riesgo de ráfagas de reintentos simultáneos gracias al jitter: distribuyendo la carga de reintentos en una ventana de tiempo en vez de concentrarla en instantes exactos.
- Mejora la recuperación ante fallos transitorios sin requerir intervención manual del usuario en la mayoría de los casos.
- Sigue una práctica recomendada y documentada (Well-Architected Framework), reduciendo el riesgo de reinventar una política de reintentos mal calibrada.

### Consecuencias negativas

- Aumenta la latencia percibida por el usuario entre el primer fallo y la confirmación final: pudiendo llegar hasta 31 segundos de espera acumulada en el peor caso.
- Existe riesgo de que el usuario perciba la operación como bloqueada o congelada si la interfaz no comunica claramente que el sistema está reintentando en segundo plano.
- No garantiza la recuperación de errores permanentes: los reintentos no aseguran que, dentro del número de intentos definido, el fallo desaparezca y el proveedor recupere su disponibilidad.
- Riesgo de generar operaciones duplicadas si la operación no es idempotente, lo que exige que esta estrategia dependa directamente de la idempotency key ya definida en el ADR de la pasarela de pagos para evitar cobros repetidos sobre una misma transacción.

### Notas

- https://docs.aws.amazon.com/wellarchitected/latest/framework/rel_mitigate_interaction_failure_limit_retries.html

---

## adr-0007 — Selección de servicios externos de notificación (Notification gateway)

**Estado:** Propuesta

### Contexto

Smartlife requiere notificar a los usuarios sobre comunicados, cambios de estado en cuentas de cobro, PQRS, incidentes y confirmaciones de teléfonos móviles.

### Escenarios de calidad y funcionalidades significativas referenciados

- HU81-Cambio de estado y cierre de incidente
- HU73-Aprobación o rechazo de reservas
- HU60-Actualizar estado de cuenta de cobro
- HU56-Generar cuenta de cobro
- HU65 - Generar factura de pago aplicado
- ESC-CAL-INT-0001

### Decisión

F. Resend: Se selecciona Resend (opción F) como servicio de notificación para SmartLife. Resend ofrece una API moderna orientada a desarrolladores, con buena entregabilidad y un plan gratuito de 3.000 correos/mes (100 correos/día), suficiente para el volumen inicial de comunicaciones transaccionales del proyecto (cambios de estado de cuentas de cobro, aprobación de reservas, generación de facturas), y un plan de pago desde $20 USD/mes para 50.000 correos si el proyecto requiere escalar.

Frente a Twilio + SendGrid (opción A), esta alternativa requiere contratar dos productos por separado: SendGrid cobra desde $19.95 USD/mes (hasta $89.95 USD/mes en el plan Pro) solo por el canal de correo, un costo de entrada más alto que el plan gratuito de Resend, sin que el proyecto necesite todavía el canal SMS que aportaría Twilio.

Frente a Bird (opción B), esta alternativa tiene, según datos de contratos reales reportados por la plataforma Vendr, un costo medio anual de $38.146 USD entre sus clientes, una estructura orientada a comunicaciones empresariales de gran escala que excede ampliamente el volumen inicial de una plataforma de unidades residenciales.

Frente a OneSignal (opción C), aunque ofrece un plan gratuito multicanal (push, correo, SMS/RCS, in-app) con buena segmentación de audiencias, su canal de correo depende de la verificación de un dominio propio ante el proveedor de envío, un requisito que actualmente no se ha podido completar (la universidad aún no figura en la lista de dominios elegibles), bloqueando la validación práctica de ese canal. Resend no presenta esa misma restricción para comenzar a operar el canal de correo de forma inmediata.

Frente a Novu (opción D), siendo open source, puede autohospedarse sin costo de licencia, pero exige mantenimiento propio del motor de orquestación, contradiciendo la restricción de Cloud Native ya establecida para el proyecto; su alternativa administrada (Novu Cloud) cuesta $30 USD/mes por apenas 30.000 eventos, con un plan gratuito limitado a 10.000 eventos/mes y solo 24 horas de retención de actividad, insuficiente para un entorno de producción.

Frente a la API Google / Firebase Cloud Messaging (opción E), aunque es gratuita, cubre exclusivamente el canal push y no resuelve el canal de correo, que es el canal principal de comunicaciones transaccionales que SmartLife necesita cubrir primero (confirmaciones de cuentas de cobro, cambios de estado).

En conclusión, Resend se considera la alternativa más adecuada para SmartLife en esta etapa, ya que resuelve el canal de correo de forma inmediata y sin bloqueos de verificación de dominio, con un plan gratuito suficiente para el volumen inicial del proyecto y una API simple orientada a desarrolladores, sin incurrir en la complejidad de gestionar múltiples proveedores o de autohospedar un motor de notificaciones.

### Opciones consideradas

- A. Twilio + SendGrid: Twilio proporciona APIs de comunicación para SMS y otros canales, mientras SendGrid se especializa en correo electrónico transaccional y de marketing. Requiere contratar y gestionar dos productos por separado.
- B. Bird: Plataforma multicanal unificada que permite gestionar email, SMS, WhatsApp, RCS y notificaciones push desde una misma infraestructura, orientada a comunicaciones a escala empresarial.
- C. OneSignal: Plataforma de comunicación multicanal (push, correo, SMS/RCS, in-app) con segmentación nativa de audiencias. Su canal de correo requiere verificación de dominio propio ante el proveedor de envío subyacente.
- D. Novu: Plataforma de orquestación de notificaciones open source, con API unificada para múltiples canales. Autohospedada exige mantenimiento propio; su versión administrada tiene un plan gratuito limitado a 24 horas de retención.
- E. API Google (Firebase Cloud Messaging): Servicio gratuito de Google para notificaciones push. Cubre exclusivamente ese canal, sin correo, SMS ni in-app.
- F. Resend: Plataforma de correo electrónico transaccional orientada a desarrolladores, con API moderna y buena entregabilidad. Plan gratuito de 3.000 correos/mes (100/día) y plan de pago desde $20 USD/mes para 50.000 correos. Cubre exclusivamente el canal de correo electrónico.

### Consecuencias positivas

- Sin bloqueo de verificación de dominio: A diferencia de OneSignal, cuyo canal de correo dependía de que la universidad apareciera en la lista de dominios elegibles del proveedor subyacente, Resend permite comenzar a enviar correos transaccionales de inmediato, sin ese requisito previo pendiente de aprobación externa.
- API simple orientada a desarrolladores: Ofrece una integración directa mediante SDK y documentación clara, reduciendo el esfuerzo de implementación frente a soluciones multicanal más complejas de configurar.
- Costo predecible y suficiente para el volumen inicial: El plan gratuito de 3.000 correos/mes (100/día) cubre las necesidades actuales de comunicaciones transaccionales del proyecto (cambios de estado de cuentas de cobro, facturas, aprobación de reservas), y el plan de pago desde $20 USD/mes para 50.000 correos ofrece una ruta de escalamiento clara si el volumen crece.
- Buena entregabilidad reportada: Al estar orientada específicamente a correos transaccionales, evita los problemas de entregabilidad que suelen afectar a plataformas de envío masivo o de marketing, relevante para comunicaciones críticas como confirmaciones de pago o cambios de estado.
- Sin carga operativa de mantenimiento: Al ser un servicio administrado, no exige operar ni actualizar infraestructura propia, a diferencia de la alternativa self-hosted de Novu.

### Consecuencias negativas

- Cobertura limitada a un solo canal: A diferencia de OneSignal, Resend cubre exclusivamente correo electrónico, dejando sin resolver push, SMS/RCS e in-app messaging, que quedarían pendientes de un proveedor adicional si el negocio los llega a requerir más adelante.
- Sin segmentación nativa de audiencias: OneSignal ofrecía segmentación por rol, torre, bloque o apartamento de fábrica; con Resend, esa lógica de dirigir comunicados a grupos específicos de usuarios tendría que construirse manualmente en la capa de aplicación, aumentando el esfuerzo de desarrollo.
- Dependencia de un proveedor externo adicional: Introduce otra dependencia de disponibilidad de un tercero para la entrega de comunicaciones críticas, que debe monitorearse como parte de la estrategia de resiliencia general del sistema, de forma similar al riesgo ya reconocido con Wompi y con la decisión original de OneSignal.
- Reintroduce fragmentación si se necesita multicanal: Si en el futuro el proyecto necesita cubrir push o SMS, tendría que sumar un segundo proveedor especializado, reintroduciendo exactamente el problema de gestionar múltiples plataformas que la decisión original de OneSignal buscaba evitar desde el principio.

---

## adr-0008 — Selección de estrategia para auditar operaciones criticas

**Estado:** Propuesta

### Contexto

Smartlife requiere gestionar el registro, consulta y almacenamiento de manera cronológica sobre accioes, eventos y cambios en la información critica de la copropiedad.

### Escenarios de calidad y funcionalidades significativas referenciados

- ESC-CAL-CPA-0006
- ESC-CAL-CPA-0001
- ESC-CAL-CPA-0004

### Decisión

B. Audit log append-only + Audit trail: Se selecciona Audit log append-only + Audit trail (opción B) porque satisface la necesidad de registrar y consultar el estado de las operaciones críticas con menor complejidad de implementación que las demás alternativas. Comparando el volumen de datos generado: Event Sourcing completo + CQRS (opción A) requiere almacenar cada cambio de estado como un evento inmutable individual para una operación como el pago de una cuota, esto implica guardar por separado los eventos de "pago iniciado", "pago autorizado" y "pago aplicado" en vez de un único registro final, multiplicando por al menos 3 el número de registros por transacción frente al Audit log, que almacena directamente el resultado final con su metadata (quién, cuándo, qué cambió).

Frente a la opción C (ambos combinados), esta alternativa hereda ese mismo problema de volumen del Event Sourcing, sumándole la complejidad adicional de mantener sincronizados dos modelos de datos en paralelo.

Frente a la Auditoría a nivel de base de datos (opción D), aunque este mecanismo tiene la ventaja de no requerir lógica adicional en la capa de aplicación al capturar automáticamente cada sentencia SQL ejecutada, resulta insuficiente para las necesidades de SmartLife porque queda limitado a lo que ocurre directamente en el motor de base de datos, sin capturar contexto de negocio propio de la operación ,por ejemplo, ESC-CAL-CPA-0006 exige registrar explícitamente el monto aplicado, la cuenta actualizada y el saldo restante de un pago, información que un trigger genérico a nivel de fila no puede inferir por sí solo sin ser enriquecido manualmente, perdiendo así parte de la trazabilidad semántica que si ofrece de forma nativa un Audit log diseñado a nivel de aplicación.

Frente a ambas, el Audit log append-only permite reconstruir el estado actual y su historial de auditoría con un único registro por operación relevante, facilitando la escalabilidad horizontal y reduciendo el uso de memoria y almacenamiento frente a las otras tres opciones.

En conclusión, el Audit log append-only se considera la alternativa más adecuada para SmartLife, ya que ofrece la trazabilidad semántica que exigen los escenarios de auditoría del negocio, con un volumen de datos considerablemente menor que Event Sourcing completo, y sin la limitación de contexto que tendría delegar la auditoría únicamente al motor de base de datos.

### Opciones consideradas

- A. Event sourcing completo + CQRS (Command Query Responsibility Segregation): Almacena cada cambio del dominio como un evento inmmutable y utiliza modelos separados para escritura y lectura. A partir del estado actual puede reconstruirse a partir de la reproducción de los eventos.
- B. Audit log append-only + Audit trail: Mantiene un registro independiente de las acciones relevantes realizadas sobre el sistema, indicando qé ocurrió, cuándo y quién lo realizó.
- C. Ambos: Combinación de las dos alternativas anteriores de manera que se pueda tener un enfoque hibrido de auditabilidad de las operaciones criticas del sistema, donde en una parte se conserve la secuencia de eventos y el el resto se registren acciones relevantes realizadas por los usuarios.
- D. Auditoria a nivel de base de datos: D. Auditoría a nivel de base de datos (pgAudit / triggers): Mecanismo de auditoría implementado directamente en el motor de base de datos, mediante extensiones nativas como pgAudit para PostgreSQL o triggers AFTER INSERT/UPDATE/DELETE que registran automáticamente cada operación sobre las tablas críticas en una tabla de bitácora paralela. No requiere lógica adicional en la capa de aplicación, ya que el propio motor intercepta y registra cada sentencia SQL ejecutada, pero limita la auditoría a las operaciones que pasan directamente por la base de datos, sin capturar contexto de negocio adicional (como el motivo de un cambio o metadatos propios de la operación) a menos que se enriquezca explícitamente el registro.

### Consecuencias positivas

- Trazabilidad: permite reconstruir las acciones realizadas sobre una operación e identificar quién la hizo y cuándo.
- Mayor capacidad de auditoría: facilita revisar las operaciones y determinar si fueron realizadas correctamente.
- Detección de actividades sospechosas: permite identificar accesos no autorizados, fallos de autenticación y modificaciones inusuales.
- Investigación ante incidentes: proporciona evidencia del antes, durante y después de un incidente no controlado.
- Responsabilidad y no repudio sobre las acciones: permite el asociamiento directo con usuarios, fechas, operaciones y resultados.
- Menor volumen de almacenamiento que Event Sourcing completo, al guardar un único registro por operación relevante en lugar de cada cambio de estado intermedio.

### Consecuencias negativas

- Mayor consumo de almacenamiento: registrar de manera cronológica las acciones realizadas por usuarios o procesos dentro de un sistema genera un volumen considerable de datos, aunque menor que el de Event Sourcing.
- Costo de infraestructura: requiere almacenamiento centralizado, herramientas de análisis, monitoreo y mecanismos de protección contra alteraciones.
- Riesgo de exposición de información sensible si el log no se protege adecuadamente con controles de acceso propios.
- Riesgo de sufrir modificaciones que alteren o borren la evidencia inequívoca de quién y cuándo se realizó una operación, si no se implementa como verdaderamente append-only a nivel de permisos.
- Generación de sobrecostos y riesgos en la gestión de históricos, tanto en su retención como en su eliminación temprana.
- A diferencia de la Auditoría a nivel de base de datos, requiere que la propia capa de aplicación implemente y mantenga explícitamente la lógica de registro, en lugar de heredarla automáticamente del motor.

### Notas

- https://www.servicenow.com/es/products/governance-risk-and-compliance/what-is-an-audit-log.html

---

## adr-0009 — Seleccionar estrategia de autorización y control de acceso a recursos

**Estado:** Propuesta

### Contexto

Smartlife requiere autorizar el acceso de los usuarios a los recursos, información y acciones que correspondan a su rol.

### Escenarios de calidad y funcionalidades significativas referenciados

- ESC-CAL-SEG-0010, ESC-CAL-SEG-0014, ESC-CAL-SEG-0016

### Decisión

C. Modelo RBAC + ABAC + ReBAC: Se selecciona el modelo híbrido RBAC + ABAC + ReBAC (opción C) porque ninguna de las otras dos opciones cubre por sí sola los tres tipos de restricción que el sistema necesita aplicar simultáneamente.

Frente al modelo RBAC + ReBAC (opción A), esta alternativa permite dividir funcionalidades por rol y limitar el acceso a los recursos directamente relacionados con el usuario por ejemplo, que un propietario solo vea las cuentas de cobro de su propia vivienda, pero no tiene forma de aplicar restricciones basadas en el contexto de la operación, como impedir que un vigilante registre el ingreso de una visita fuera del horario de su turno o desde una ubicación distinta a la portería asignada

Frente al modelo ABAC puro (opción B), esta alternativa resuelve justamente ese tipo de restricción contextual, pero al prescindir del componente relacional, no distingue entre "un propietario cualquiera" y "el propietario dueño de esa vivienda específica", obligando a modelar cada relación de propiedad como un atributo adicional y volviendo la política de acceso más difícil de mantener a medida que crece el número de usuarios y viviendas.

El modelo híbrido evita ambos problemas: RBAC actúa como primer filtro, otorgando o negando el acceso general según el rol; ABAC aplica después las condiciones de contexto (horario, ubicación, estado de la operación); y ReBAC valida por último que el recurso solicitado pertenezca efectivamente al usuario que lo pide. Esta separación en capas, aunque añade complejidad de implementación frente a las otras dos alternativas, evita tener que crear roles adicionales cada vez que surge una nueva restricción específica, ya que la mayoría de esos casos quedan cubiertos por las capas de atributos y relaciones sin tocar la definición de roles.

En conclusión, el modelo RBAC + ABAC + ReBAC se considera la alternativa más adecuada para SmartLife, ya que es el único capaz de resolver simultáneamente el control por rol, las restricciones contextuales y la validación de pertenencia sobre el recurso que exigen los escenarios ESC-CAL-SEG-0010, ESC-CAL-SEG-0014 y ESC-CAL-SEG-0016, sin sacrificar mantenibilidad a medida que crece el sistema.

### Opciones consideradas

- A. Modelo RBAC Tradicional (Role-Based Access Control) + ReBAC (Relatioship-Based Access Control): Metodo de control de acceso que define que puede hacer los usuarios dentro de un sistema a partir de la asignación previa de uno o más roles que le otrogan diferentes permisos dentro de la plataforma alineado con las relaciones que tiene cada usuario sobre los recursos a los que se le fue habilotado a su rol.
- B. Modelo ABAC (Attribute-Based Access Control): Control de acceso basado alternativos que permite gestionar el acceso a un sistema de manera más granular que el RBAC, basandose en los atributos del usuario, de la acción que realiza o propiedades del mundo real cmo la hora actual del día o la ubicación fisica desde la que accede.
- C. Modelo RBAC (Role-Based Access Control Puro) + ABAC (Attribute-Based Access Control) + ReBAC (Relationship-Based Access Control): Modelo hibrido que combina el control basado en roles con la flexibilidad contextual basado en atributos, de manera que RBAC actue como la primera capa para validar los permisos generales sobre los cuales un rol tiene acceso, mientras que ABAC interviene como segunda capa para evaluar aquellas restricciones a nivel de atributos y propiedades del contexto, y establecer controles de acceso por relaciones que tiene un usuario, sus funcioanlidades de rol y a sus relaciones que puede acceder.

### Consecuencias positivas

- Mayor granularidad y precisión de autorización, al combinar tres niveles de control complementarios en lugar de uno solo.
- Aplicabilidad del principio de mínimo privilegio establecido por las prácticas de seguridad del Well-Architected Framework.
- Mejor representación de las dinámicas del dominio del negocio, distinguiendo entre "cualquier propietario" y "el propietario dueño de ese recurso específico".
- Mayor flexibilidad en futuros cambios respecto a la implementación de nuevos factores de control de acceso para los usuarios.
- Evita la creación de nuevos roles redundantes para garantizar restricciones específicas, al resolver la mayoría de esos casos mediante las capas de atributos y relaciones.

### Consecuencias negativas

- Mayor complejidad de implementación: se establece la necesidad de representar los roles y, además, los atributos y relaciones sobre los que cada usuario puede actuar.
- Dificultad en la auditoría: la autorización ya no depende solo del rol, lo que añade complejidad al determinar quién ejecutó una operación, además de tener que identificar sus relaciones y atributos asociados.
- Mayor complejidad en el diseño de políticas ante nuevas inclusiones de roles o funcionalidades.
- Dependencia de la consistencia de las relaciones: si los datos de relación (por ejemplo, propietario-vivienda) están desactualizados o mal sincronizados, las decisiones de acceso pueden ser incorrectas.
- Mayor esfuerzo de pruebas de control de autorización y sus posibles combinaciones con respecto a sus relaciones y atributos.
- Mayor costo operativo: requiere de un servicio de autorización especializado en roles, restricción por atributos, relaciones y sincronización de permisos.

---

## adr-0010 — Seleccionar un estilo arquitectonico para estructurar los componentes del sistema

**Estado:** Propuesta

### Contexto

Smartlife requiere modularizar los componentes del sistema en dominios con fronteras claras.

### Escenarios de calidad y funcionalidades significativas referenciados

- No aplica

### Decisión

A. Arquitectura monolítica modular: Se optó por un estilo arquitectónico monolítico modular debido a que combina la simplicidad operativa de un monolito tradicional con la clara delimitación de contextos de dominio, similar a la separación por servicios individuales de una arquitectura de servicios distribuidos, pero dentro de un mismo proceso desplegable.

La justificación de esta decisión parte de las necesidades actuales del sistema para la unidad residencial, garantizando el cumplimiento de sus reglas de negocio sin introducir una complejidad operativa innecesaria. Aunque una arquitectura de servicios distribuidos contribuye directamente a la disponibilidad, la tolerancia a fallos, la seguridad reinforced en el control de acceso y el escalado independiente de cada servicio (atributos de calidad prioritarios para SmartLife), no resulta justificable como arquitectura inicial.

Adoptar servicios distribuidos en esta etapa prematura redistribuye y amplifica la complejidad del sistema, exigiendo un nivel de madurez técnica que el equipo tendría que asumir desde el inicio: orquestación de contenedores a gran escala, descubrimiento de servicios, manejo de consistencia eventual entre bases de datos distribuidas, observabilidad distribuida y coordinación de despliegues independientes. Todo esto representaría un costo económico, tecnológico y de mantenibilidad elevado por la cantidad de herramientas requeridas para gestionar la comunicación entre servicios.

Intentar resolver problemas de crecimiento masivo desde el día uno introduciría una sobreingeniería desalineada de la realidad del proyecto. La arquitectura debe evolucionar junto con el sistema de forma natural y sin fricciones: ni quedarse atrás ni precipitarse. En las circunstancias iniciales de operación, un sistema de gestión residencial no amerita un escalado independiente por módulos ni ciclos de vida de despliegue diferenciados.

Por tanto, la arquitectura monolítica modular representa el punto medio ideal: evita el antipatrón de un monolito tradicional altamente acoplado y permite conservar límites claros entre contextos delimitados sin incurrir en el costo operativo de un sistema distribuido en red. De esta manera, el equipo de desarrollo puede enfocar sus esfuerzos directamente en las funcionalidades clave del *core* del negocio y en generar valor para los usuarios finales, en lugar de desgastarse en la gestión de infraestructura técnica compleja.

### Opciones consideradas

- A. Arquitectura monolitica modular: Modelo de diseño de software donde la aplicación es una única unidad desplegable que se divide en modulos de negocio bien definidos.
- B. Arquitectura de servicios distribuidos: Modelo de diseño de software donde las funciones de una aplicación se dividen en componentes independientes que operan en múltiples nodos conectados por una red.

### Consecuencias positivas

- Desarrollo más sencillo: Concentrar la aplicación en una sola únidad permite realizar ejecuciones dentro de un mismo proceso local y depurar el flujo completo del sistema mediate puntos de interrupción unificados.
- Menores costos de infraestructura: Esta arquitectura minimiza el gasto financiero en servicios en la nube (OpEx) y reduce la carga de trabajo operativa de administración de TI. Al desplegarse como una sola unidad, evita la necesidad de configurar, mantener y pagar herramientas complejas como orquestadores de clústeres (Kubernetes), sistemas de descubrimiento de servicios, gateways de enrutamiento o motores de mensajería distribuida de manera conjunta, concentrando la memoria y el procesamiento en menos servidores físicos o virtuales.
- Implementación más sencilla: La publicación de cambios en producción se vuelve un proceso lineal, directo y de bajo riesgo operativo. En lugar de coordinar múltiples lanzamientos con dependencias de versiones entre servicios independientes, los pipelines de Integración y Despliegue Continuo (CI/CD) solo deben compilar y empaquetar un único artefacto ejecutable. Esto acorta los tiempos de despliegue y permite revertir cambios (rollback) de forma inmediata en caso de fallo.
- Mejor rendimiento: La comunicación entre los diferentes módulos del sistema ocurre directamente en la memoria del servidor a través de llamadas de métodos nativos, tomando nanosegundos en ejecutarse. Esto elimina la latencia de red, evita el consumo excesivo de CPU derivado de la serialización y deserialización de datos (como JSON) y erradica los fallos de conexión o tiempos de espera (timeouts) inherentes a las arquitecturas distribuidas en red.

### Consecuencias negativas

- Despliegues a gran escala: Aunque el código esté separado en módulos con limites muy claros en el editor, al compilar y empaquetar la aplicación genera un solo artefacto desplegable que ralentiza el despliegue de cambios en una de las partes del sistema por posibles errores en algún modulo de la aplicación, y su eventual crecimiento pipelines de CI/CD tardan más tiempo en ser ejecutadas y entregadas.
- Recursos compartidos: Los modulos suelen compartir una misma base de datos e infraestructura, lo que puede generar "Acoplamient oculto" a nivel de base de datos en caso de aplicar incorrectamente reglas de independencia modular, compiten cada uno por los recursos fisicos del servidor y un unico punto de falla puede generar caidas en cadena en el resto de la aplicación.
- Limitaciones de escala: Toda la aplicación escala de forma conjunta lo que puede limitar procesos de escalado según la demanda individual de los diferentes modulos del dominio de la aplicación.

---

## adr-0011 — Seleccionar patron de arquitectura para el backend del sistema

### Contexto

Smartlife requiere desacoplar la lógica de negocio de los aspectos técnicos y tecnologías externas en el backend.

### Escenarios de calidad y funcionalidades significativas referenciados

- No aplica

### Decisión

B. Arquitectura hexagonal: Se selecciona Arquitectura hexagonal (Puertos y Adaptadores) como patrón de arquitectura para el backend de SmartLife. Este patrón aísla la lógica de negocio en un núcleo de aplicación independiente, que se comunica con el exterior exclusivamente mediante puertos (interfaces), mientras que los adaptadores concretos implementan esos puertos hacia cada tecnología externa que el sistema ya requiere integrar: la base de datos relacional, el proveedor de identidad (Firebase), la pasarela de pagos (Wompi), el servicio de notificación (Resend) y el almacenamiento de archivos multimedia. Esta separación explícita entre puertos y adaptadores responde directamente a la necesidad planteada en el contexto de desacoplar la lógica de negocio de los aspectos técnicos y tecnologías externas.

Frente a la Arquitectura en capas (opción A), se descarta porque, aunque separa responsabilidades en capas de presentación, lógica de negocio y acceso a datos, su regla de dependencia habitual permite que la capa de negocio dependa directamente de la capa de acceso a datos, lo que en la práctica suele acoplar el dominio a detalles técnicos concretos (por ejemplo, a un ORM o a un cliente HTTP específico de un proveedor externo), en lugar de depender únicamente de abstracciones.

Frente a Clean Architecture (opción C), no se descarta por incompatibilidad, ya que comparte el mismo principio de inversión de dependencias hacia el dominio, sino porque introduce una mayor cantidad de capas concéntricas explícitas (entidades, casos de uso, adaptadores de interfaz, frameworks y drivers) que, para el tamaño actual del proyecto, agregan una formalidad adicional sin un beneficio claro frente al modelo, más simple de comunicar y aplicar, de puertos y adaptadores.

Frente a Onion Architecture (opción D), se descarta por una razón similar: al ser conceptualmente muy cercana a la arquitectura hexagonal (dominio en el centro, dependencias dirigidas hacia el núcleo), no aporta una ventaja adicional relevante, mientras que la terminología de puertos y adaptadores de la opción B resulta más directa para modelar explícitamente cada integración externa del sistema (identidad, pagos, notificaciones, almacenamiento) como un adaptador independiente y sustituible.

En conclusión, la arquitectura hexagonal se considera la alternativa más adecuada para SmartLife, ya que aísla el núcleo de negocio de los detalles técnicos y de las múltiples tecnologías externas ya seleccionadas para el proyecto, sin incurrir en la formalidad adicional de Clean Architecture ni perder la claridad conceptual frente a Onion Architecture.

### Opciones consideradas

- A. Arquitectura en capas:Organiza el sistema en capas lógicas con responsabilidades específicas, estableciendo reglas sobre cómo pueden depender unas de otras. Una estructura típica contiene presentación, lógica de negocio y acceso a datos.
- B. Arquitectura hexagonal: Organiza la aplicación alrededor de un núcleo de aplicación aislado, que se comunica con el exterior mediante puertos; los adaptadores conectan esos puertos con tecnologías concretas como bases de datos, interfaces de usuario o servicios externos.
- C. Clean Architecture: Organiza el código colocando las reglas de negocio y el modelo de aplicación en el centro, mientras que las dependencias de infraestructura y detalles externos apuntan hacia ese núcleo. Su objetivo es evitar que el dominio dependa de tecnologías externas.
- D. Onion Architecture: Organiza el sistema mediante capas concéntricas, colocando el dominio en el centro y las preocupaciones externas en las capas que lo rodean. Las dependencias deben dirigirse hacia el núcleo.

### Consecuencias positivas

- Bajo acoplamiento con tecnologías externas: cada integración externa (base de datos, proveedor de identidad, pasarela de pagos, servicio de notificación, almacenamiento) se implementa como un adaptador independiente, permitiendo sustituir uno de estos componentes sin modificar la lógica de negocio.
- Mayor facilidad de pruebas: al depender únicamente de puertos (interfaces), la lógica de negocio puede probarse de forma aislada reemplazando los adaptadores reales por implementaciones de prueba (mocks/stubs), sin requerir una base de datos o servicios externos disponibles.
- Coherencia con la arquitectura monolítica modular ya adoptada: el patrón de puertos y adaptadores puede aplicarse de forma consistente dentro de cada módulo del monolito, reforzando los límites de dominio definidos entre los distintos contextos del sistema.
- Facilidad para evolucionar integraciones: si en el futuro se requiere cambiar de proveedor de pagos, de notificación o de identidad, el cambio queda contenido en el adaptador correspondiente, sin propagarse hacia el núcleo de la aplicación.

### Consecuencias negativas

- Mayor cantidad de código de intermediación: definir puertos (interfaces) y adaptadores para cada integración externa incrementa la cantidad de clases e interfaces del proyecto frente a acceder directamente a la tecnología concreta desde la lógica de negocio.
- Curva de aprendizaje adicional: exige que el equipo comprenda y aplique consistentemente la separación entre el núcleo de dominio y los adaptadores, lo que puede generar errores de diseño si no se respeta la dirección de las dependencias.
- Riesgo de sobreabstracción: si se definen puertos para integraciones que no tienen una necesidad real de ser sustituidas, se introduce una capa de indirección adicional sin un beneficio proporcional.

---

## adr-0012 — Seleccionar patron de arquitectura para el frontend del sistema

### Contexto

Smartlife requiere mantener la evolución y mantenibilidad de los componentes con los que interactúa el usuario.

### Escenarios de calidad y funcionalidades significativas referenciados

- No aplica

### Decisión

E. Feature-Based + Component-Based (Híbrido): Se selecciona la combinación de Feature-Based con Component-Based como patrón de arquitectura para el frontend de SmartLife. Los módulos por feature (residentes, reservas, cuentas de cobro, incidentes, visitas) delimitan fronteras claras entre los distintos dominios de negocio del sistema, de forma consistente con los límites de módulo ya definidos para el backend en la arquitectura monolítica modular (adr-0010); dentro de cada módulo, los componentes reutilizables e independientes propios del enfoque Component-Based resuelven la composición visual concreta de cada funcionalidad. Esta combinación responde directamente al contexto planteado: garantizar la mantenibilidad y evolución de los componentes con los que interactúa el usuario, tanto a nivel de dominio (feature) como a nivel de interfaz (componente).

Frente a Component-Based de forma aislada (opción A), se descarta porque, al ser inherente a cualquier framework basado en componentes como Angular, no resuelve por sí solo cómo agrupar y delimitar los distintos dominios de negocio de la aplicación a medida que el número de componentes crece, lo que puede derivar en una estructura de carpetas sin fronteras claras entre funcionalidades.

Frente a Atomic Design de forma aislada (opción C), se descarta como criterio principal de organización porque su jerarquía de átomos, moléculas y organismos prioriza la reutilización de piezas visuales pequeñas sobre la delimitación por dominio de negocio, lo que dificulta ubicar rápidamente todo el código relacionado con una funcionalidad específica (por ejemplo, todo lo relativo a reservas) cuando varios integrantes del equipo trabajan en paralelo sobre distintas funcionalidades.

Frente a la separación estricta Smart/Dumb Components (opción D), no se descarta como práctica complementaria dentro de cada componente, pero se descarta como criterio principal de organización del proyecto, ya que resuelve la responsabilidad interna de un componente (presentación vs. estado) y no la forma en que se agrupan los componentes según el dominio de negocio al que pertenecen.

En conclusión, la combinación Feature-Based + Component-Based se considera la alternativa más adecuada para SmartLife, ya que delimita fronteras claras entre los dominios de negocio del frontend, de forma coherente con la modularidad ya adoptada en el backend, sin renunciar a la reutilización y el bajo acoplamiento que aporta la composición basada en componentes.

### Opciones consideradas

- A. Component-Based: Organiza el frontend como un árbol de componentes reutilizables e independientes, cada uno encapsulando su propia plantilla, lógica y estilos, sin imponer una agrupación adicional según el dominio o funcionalidad de negocio a la que pertenecen.
- B. Feature-Based (Feature Modules): Organiza el código agrupando por funcionalidad de negocio (feature), de forma que cada módulo contiene los componentes, servicios y rutas relacionados exclusivamente con esa funcionalidad, delimitando fronteras claras entre los distintos dominios de la aplicación.
- C. Atomic Design: Organiza los componentes en una jerarquía de niveles según su grado de composición visual (átomos, moléculas, organismos, plantillas y páginas), priorizando la reutilización de piezas de interfaz pequeñas sobre la agrupación por dominio de negocio.
- D. Arquitectura por capas de responsabilidad (Smart/Dumb Components): Separa los componentes en dos tipos según su responsabilidad: componentes de presentación (dumb), encargados únicamente del renderizado a partir de datos recibidos, y componentes contenedores (smart), encargados de gestionar el estado y la lógica de negocio.
- E. Feature-Based + Component-Based (Híbrido): Combina ambos enfoques, utilizando módulos por feature (opción B) como unidad principal de organización del dominio, y dentro de cada uno de ellos, componentes reutilizables e independientes (opción A) como unidad de composición de la interfaz.

### Consecuencias positivas

- Fronteras claras entre dominios de negocio: cada feature module agrupa los componentes, servicios y rutas de una funcionalidad específica, facilitando ubicar y modificar el código relacionado con un dominio concreto (reservas, cuentas de cobro, incidentes) sin explorar el resto de la aplicación.
- Trabajo en paralelo sin conflictos: al delimitar cada funcionalidad en su propio módulo, distintos integrantes del equipo pueden desarrollar features diferentes de forma simultánea con un menor riesgo de conflictos sobre los mismos archivos.
- Coherencia con la modularidad del backend: la organización por feature en el frontend refleja los mismos límites de dominio ya establecidos entre los módulos del backend, facilitando que un mismo desarrollador reconozca la correspondencia entre ambos.
- Reutilización de componentes: los componentes definidos dentro de cada módulo, y aquellos compartidos entre varios módulos, pueden reutilizarse en distintas pantallas sin duplicar lógica de presentación.

### Consecuencias negativas

- Mayor esfuerzo de diseño inicial: exige decidir, antes de comenzar a implementar, cuáles son los límites de cada feature module y qué componentes deben ser compartidos entre varios módulos en lugar de pertenecer a uno solo.
- Riesgo de componentes mal ubicados: un componente que en realidad es de uso transversal puede quedar inicialmente definido dentro de un único feature module, obligando a moverlo posteriormente a una carpeta compartida cuando otro módulo también lo necesite.
- Curva de aprendizaje sobre la convención del equipo: al no ser una estructura impuesta automáticamente por el framework, el equipo debe conocer y respetar consistentemente el criterio de qué pertenece a un feature module y qué pertenece a los componentes compartidos.

---

## adr-0013 — Selección del modeo de procesamiento de la aplicación

**Estado:** Propuesta

### Contexto

Smartlife requiere concurrir el procesamiento de peticiones entre los diferentes componentes del sistema.

### Decisión

C. Ambas: Se opto por elegir un enfoque hibrido ya que la comunicación entre módulos, con servicios externos o actualizaciones de estado en la ejecución de una operación no debe generar bloqueos sobre los hilos de petición del sistema y a su vez el sistema debe garantizar que los usuarios esperen respuesta del sistema para traer información que no requiere de que el sistema reaccione en tiempo real ante estimulo dados que le permitan al sistema generar acciones sobre la plataforma. Un ejemplo de ello es cuando un usuario intenta realizar una reserva o un pago en linea. Inicialmente estas operaciones son sincronicas pero una vez generen el registro de un evento determiando, como “Pago procesado” o “Reserva Solicitada” el sistema debe reaccionar de manera inmediata ante estos estimulos para garantizar la consistencia de la aplicación y reflejar los resultados correspondientes en pantalla sin que el usuario tenga que esperar por bloqueos o reiniciar la aplicación. Mientras que operaciones que involucren operaciones de una sola dirección donde el usuario realiza una petición, como operaciones de envío o consulta, donde el sistema no debe reaccionar a ningún tipo de estimulo y devuelve el resultado de la operación mientras el usuario espera. Aqui se involucran todos los registros, actualizaciones y consultas descritas para el sistema.

### Opciones consideradas

- A. Aplicación reactiva: Sistemas diseñados para responder de manera asincrona y en tiempo real, previniendo bloqueos a componentes del sistema en la interfaz del usuario.
- B. Aplicación estándar: Sistema que sigue un enfoque secuencial donde al momento de mandar una solicitud, se le asigna un espacio de respuesta hasta que la transacción sea exitosa.
- C. Ambas: Modelo hibrido de aplicación donde todas aquellas operaciones convencionales y transaccionales se ejecutan en un bloque de modelo estándar y aquellas que implican una alta cocurrencia usar procesamientos asincronos y no bloquantes.

### Consecuencias positivas

- Responsividad: El procesamiento asíncrono y no bloqueante permite que una operación no bloquee recursos de petición del sistema. Esto puede mejorar la capacidad de respuesta bajo concurrencia.
- Resiliencia: Los módulos pueden comunicarse mediante mensajes y delegar tareas, permitiendo aplicar reintentos, aislamiento y manejo independiente de fallos.
- Concurrencia: Los modelos reactivos son adecuados para múltiples operaciones simultáneas, especialmente cuando existen operaciones I/O-bound.
- Acoplamiento: La mensajería asíncrona reduce el acoplamiento temporal entre módulos.
- Mantenimiento: Los módulos proporcionan límites claros y la comunicación por mensajes puede reducir dependencias directas.

### Consecuencias negativas

- Mayor complejidad arquitectonica.
- Mayor complejidad para el desarrollo sobre el conocimiento mixto de ambos paradigmas de aplicación.
- Mayor complejidad en las pruebas y de puración de las operaciones asincronas en la sección reactiva.
- Introduce una mayor dificultad en los procesos de diseño arquitectonico de la aplicación.
- La sección reactiva no garantiza al 100% el cumplimiento del rendimiento de las operaciones debidoa la presencia de agentes externos de los recursos y diseños de la aplicación.

---

## adr-0014 — Seleccionar estrategia de diseño de software para el funcionamiento de una aplicación sin conectividad de red

### Contexto

Smartlife requiere disponibilizar el componente de visitas ante fallos de conectividad de red.

### Escenarios de calidad y funcionalidades significativas referenciados

- Si puede aplicar para un escenario de confiabilidad

### Decisión

A. Offline-First + Local-First: Se opta por el enfoque Offline-First, complementado con el paradigma Local-First (prioridad local), para garantizar la continuidad operativa de la aplicación ante fallos de conectividad. Esta combinación permite realizar tanto consultas como escrituras de manera local, almacenando las operaciones realizadas por el vigilante y sincronizándolas posteriormente con el servidor cuando se restablezca la conexión. Esto responde directamente a las necesidades del control de acceso en las porterías de las unidades residenciales, donde se requiere mantener la operación incluso sin conexión a Internet.

Frente a Cache First / Cache-Aside, estas estrategias permiten atender consultas utilizando información previamente almacenada en caché, pero están orientadas principalmente al acceso y recuperación de datos. Por sí solas, no proporcionan un mecanismo para persistir y sincronizar las operaciones de escritura realizadas durante la desconexión.

Por su parte, Network First prioriza la obtención de información actualizada desde el servidor y utiliza la caché como mecanismo de respaldo cuando la red no está disponible. Sin embargo, tampoco define por sí sola un mecanismo para almacenar y sincronizar las escrituras generadas durante el período offline.

Finalmente, LWW (Last Write Wins) es una estrategia para resolver conflictos durante la sincronización, determinando qué versión de un dato debe conservarse cuando existen modificaciones concurrentes. Por tanto, puede complementar una solución Offline-First/Local-First, pero no resuelve por sí misma la continuidad operativa ante la pérdida de conectividad.

### Opciones consideradas

- A. Offline first + Local-first: Enfoque de diseño y desarrollo de software donde las aplicaciones se construyen para funcionar de manera total o parcial sin necesidad de conexión a internet y los datos se guardan de manera local y realizar replicaciones de ellos en segundo plano hacia el servidor de almacenamiento online.
- B. Patron Repository + Cache (Cache-Aside/Cache-First): Estrategia de diseño de software donde el sistema busca información en la memoria (cache) antes de intentar conectarse a una fuente externa o servidor remoto.
- C. Network-first: Es una estrategia de cache utilizada en aplicaciones web progresivas (PWA). Su objetivo es priorizar la información más actualizada, intentando obtenerela primero desde la red y usando la cache solamente si la red falla.
- D. Patron LWW (Last Write-Win): Es una estrategia para resolver actualizaciones concurrentes donde se conserva la que tenga marca de tiempo o versión más reciente, y se descarta la otra

### Consecuencias positivas

- Continuidad operativa: La aplicación puede continuar realizando funciones criticas (visitas) sin conexión, evitando que caidas de coneción a internet detenga los procesos del control de acceso de las personas externas a la unidad residencial.
- Persistencia de escrituras offline: Las operaciones de escritura pueden guardarse primero en el almacenamiento local y ponerse en cola para sincronizarse para reducir el riesgo de perder registros generados durante la desconexión.
- Menor dependencia a la red: Las consultas pueden realizarse directamente sobre la base de datos local, evitando esperar respuestas del servidor para operaciones que ya tienen información disponible localmente.
- Tolerancia a conexiones intermitentes: El sistema puede continuar funcionando durante interrupciones temporales y sincronizar posteriormente los cambios cuando se restablezca la conectividad.

### Consecuencias negativas

- Mayor complejidad arquitectonica: No existe una única fuente de datos: se debe gestionar una fuente local, una fuente de red y mecanismos que mantienen ambas sincronizadas.
- Resolución de conflictos: Dos dispositivos pueden modificar el mismo dato mientras estan sincronizados y provocar conflictos sobre la escritura y sincronización.
- Mayor almacenamiento local: La aplicación debe mantener en el dispositivo una cantidad de información sufuciente para continuar operando offline.
- Información potencialmente desactualizada: La información almacenada localmente puede quedar atrasada respecto al servidor mientras permanece el dispositivo desconectado.

---

## adr-0015 — Seleccionar herramienta de base de datos local para funcionamiento offline

### Contexto

Smartlife requiere persistir la información de las transacciones de control de ingresos en una base de datos local relacional.

### Decisión

C. RxDB: Se selecciona base de datos local debido a que etsa orientada al desarrollo de aplicaciones Local-First que permite almacenar y consultar información directamente desde el dispositivo del usuario. Su elección permite soportar el enfoque Offline first definido para las operaciones de portería requieren que el vigilante pueda registrar localmente y posteriormente sincronizarse con el servidor cuando se reestablezcla la conexión. Adicionalmente proporciona funcionalidades de consultas, indices, reactividad y mecanismos de replicación, lo que reduce la carga lógica que tendría que desarollarse directamente sobre una API de almacenamiento como IndexedDB.

Frente a IndexedDB proporciona una capa de abstracción de mayor nivel que facilita la gestión de documentos, consultas, esquemas y sincronización, evitando implementar desde cero funcionalidades necesarias para el funcionamiento Local-First. Por otra parte, aunque SQLite ofece un modelo relacional y lenguaje SQL de consulta, su utilización requiere de mecanismos adicionales como WebAssembly y OPFS, aumentando la complejidad de la solución.

Por estas características, RxDB se considera adecuado para la aplicación, ya que permite disponer de una base de datos local persistente orientada a aplicaciones web, facilita la implementación del funcionamiento offline y proporciona mecanismos que pueden integrarse con la estrategia de sincronización entre el dispositivo del vigilante y el servidor.

### Opciones consideradas

- A. SQLite: Es un sistema de gestión de bases bases de datos relacional, autónomo y sin servidor que almacena toda la información dentro de un único archivo en el disco.
- B. IndexedDB: Es una API para almacenar grandes cantidades de datos estructurados. La API es asincronica y se puede usar tanto desde el codigo front-end de la aplicación como desde el código de trabajo del servicio.
- C. RxDB: Es una base de datos NoSQL para JavaScript que se ejecuta directamente en tu aplicación con un duseño que prioriza la operación local, ofrece consultas con latencia cero incluso sin conexión y se sincroniza a la perfección con cualquier backend.

### Consecuencias positivas

- Persistencia de información sin conexión: Permite la continuidad operativa del control de acceso de la unidad, incluyendo lecturas y escrituras de manera local.
- Sincronización con el servidor: RxDB incorpora mecanismos de replicación que permiten sincronizar los datos locales con un backend y realizar seguimiento de los cambios. Además, puede integrarse con diferentes tipos de backend.
- Resolucipon de conflictos: Cuenta con mecanismos de detección y resolución de conflictos entre modificaciones realizadas localmente y en el servidor. También permite definir un conflictHandler personalizado según las reglas del sistema.
- Menor dependencia de la red para operaciones locales: Las consultas realizadas sobre los datos locales no requieren una solicitud al servidor, lo que puede mejorar la capacidad de respuesta de la interfaz, especialmente en escenarios con conectividad intermitente.

### Consecuencias negativas

- Mayor complejidad arquitectónica: Incorporar una base de datos local y sincronización implica gestionar estados offline/online, replicación, errores de sincronización y conflictos. Esto aumenta la complejidad respecto a una aplicación que depende exclusivamente del servidor.
- Gestión de conflictos: Aunque RxDB proporciona mecanismos para resolver conflictos, el sistema debe definir qué hacer cuando, por ejemplo, dos dispositivos modifican el mismo registro mientras están desconectados. Una estrategia incorrecta puede provocar que una modificación válida sea descartada.
- Persistencia limitada del almacenamiento del navegador: Los datos almacenados localmente en el navegador no deben considerarse permanentes. El navegador o el usuario pueden eliminar el almacenamiento local, por lo que los datos importantes deben poder recuperarse nuevamente desde el servidor.
- Limitaciones de almacenamiento y rendimiento: El rendimiento depende del mecanismo de almacenamiento utilizado y de las características del dispositivo. En navegadores, IndexedDB puede presentar limitaciones frente a otras alternativas como OPFS para determinadas operaciones de alto volumen.
- Necesidad de controlar la cantidad de datos sincronizados: No resulta conveniente replicar toda la información del sistema en cada dispositivo. RxDB recomienda mantener acotado el conjunto de datos local y sincronizar únicamente la información necesaria, además de implementar limpieza de datos cuando corresponda.

---

## adr-0016 — Seleccionar protocolos de servicio de comunicación

**Estado:** Propuesta

### Contexto

SmartLife requiere comunicar el backend con el frontend web y la aplicación de portería para el intercambio de información de operaciones.

### Decisión

E. REST + WebSocket (Híbrido): Se selecciona una combinación de REST, para las operaciones convencionales de solicitud-respuesta sobre recursos (consultas, registros y actualizaciones), junto con WebSocket, mediante el soporte nativo que ofrece Spring Boot para STOMP sobre WebSocket, destinado exclusivamente a la difusión de eventos en tiempo real hacia los clientes conectados (actualización de estado de pagos, aprobación o rechazo de reservas, cambio de estado de un incidente). Esta combinación resuelve directamente el modelo de procesamiento híbrido definido en adr-0012: las operaciones síncronas convencionales se mantienen bajo el modelo REST ya adoptado de forma transversal en el proyecto para todas las integraciones externas (Firebase, Wompi, Resend, Strapi), mientras que el canal WebSocket asume únicamente la notificación push de los eventos que sí requieren reacción inmediata en pantalla, evitando que el cliente deba recurrir a consultas periódicas (polling) para enterarse de dichos cambios.

Frente a GraphQL (opción B), se descarta como protocolo principal porque su ventaja central, permitir al cliente especificar exactamente los campos que necesita, resuelve un problema de sobre-fetching o sub-fetching de datos que no se ha identificado como una necesidad real del proyecto: las pantallas de SmartLife consumen estructuras de datos conocidas y relativamente estables, por lo que adoptar un esquema tipado, resolvers y una capa de resolución adicional introduciría una complejidad de desarrollo y de curva de aprendizaje no justificada frente al beneficio obtenido, además de no resolver por sí solo la necesidad de actualizaciones en tiempo real hacia el cliente sin una capa adicional de suscripciones.

Frente a gRPC (opción C), se descarta porque está orientado principalmente a comunicación de alto rendimiento entre servicios (service-to-service) dentro de arquitecturas distribuidas, un escenario que no aplica a SmartLife al tratarse de un monolito modular (adr-0010). Adicionalmente, su consumo directo desde el navegador no cuenta con soporte nativo, ya que requiere una capa de traducción adicional (grpc-web) hacia un proxy que reconvierta las llamadas a HTTP/1.1, lo que introduce infraestructura y complejidad operativa adicional que no se justifica para clientes web y móviles como los de este proyecto.

Frente a WebSocket como protocolo único (opción D), se descarta usarlo de forma exclusiva porque exigiría reimplementar sobre un canal persistente funcionalidades que REST ya resuelve de forma estándar para las operaciones convencionales: semántica de métodos HTTP, independencia de estado (statelessness) entre peticiones, cacheo HTTP y compatibilidad directa con herramientas ya utilizadas en el proyecto para probar y documentar endpoints (Postman). Usar WebSocket para el 100% de la comunicación supondría un sobrecosto de diseño para operaciones que no requieren ningún tipo de push desde el servidor.

En conclusión, la combinación REST + WebSocket se considera la alternativa más adecuada para SmartLife, ya que mantiene la consistencia con el protocolo REST ya utilizado de forma transversal en el resto de integraciones del sistema, aprovecha el soporte nativo y maduro de Spring Boot para ambos protocolos sin exigir librerías o infraestructura adicional, y resuelve directamente, sin sobreingeniería, la necesidad de actualizaciones inmediatas identificada en el modelo de procesamiento híbrido definido en adr-0012.

### Opciones consideradas

- A. REST: Estilo arquitectónico para servicios web basado en operaciones estándar de HTTP (GET, POST, PUT, DELETE) sobre recursos identificados mediante URIs, con intercambio de datos habitualmente en formato JSON. Es un modelo sin estado (stateless) en el que cada petición contiene toda la información necesaria para ser procesada.
- B. GraphQL: Lenguaje de consulta para APIs que permite al cliente especificar exactamente los campos y relaciones que necesita en una única petición, resuelto mediante un esquema tipado y resolvers en el servidor, evitando el sobre-fetching o sub-fetching de datos típico de REST.
- C. gRPC: Framework de comunicación de alto rendimiento desarrollado por Google, basado en HTTP/2 y en la serialización binaria mediante Protocol Buffers, orientado principalmente a comunicación entre servicios (service-to-service) de baja latencia, con soporte para streaming bidireccional.
- D. WebSocket: Protocolo que establece un canal de comunicación full-duplex y persistente entre cliente y servidor sobre una única conexión TCP, permitiendo que el servidor envíe información al cliente sin que este la solicite explícitamente (server push).
- E. REST + WebSocket (Híbrido): Combinación de ambos protocolos, empleando REST para las operaciones convencionales de solicitud-respuesta y WebSocket exclusivamente para la difusión de eventos en tiempo real hacia los clientes conectados." "* Consistencia con el ecosistema ya integrado: mantiene REST como protocolo principal, igual que los servicios externos ya integrados al proyecto (Firebase, Wompi, Resend, Strapi), reduciendo la curva de aprendizaje y la heterogeneidad de estilos de comunicación dentro del sistema.

### Consecuencias positivas

- Consistencia con el ecosistema ya integrado: mantiene REST como protocolo principal, igual que los servicios externos ya integrados al proyecto (Firebase, Wompi, Resend, Strapi), reduciendo la curva de aprendizaje y la heterogeneidad de estilos de comunicación dentro del sistema.
- Soporte nativo en Spring Boot: Spring Boot ofrece soporte de primera clase tanto para APIs REST (Spring Web) como para WebSocket con STOMP (Spring WebSocket), sin requerir librerías ni infraestructura adicional de terceros.
- Actualizaciones inmediatas sin sobrecarga de red: el canal WebSocket evita recurrir a polling para reflejar eventos de dominio en tiempo real, reduciendo peticiones innecesarias al servidor frente a consultar periódicamente el estado de una operación.
- Amplia compatibilidad de clientes: tanto REST como WebSocket cuentan con soporte nativo en los navegadores modernos, sin necesidad de proxies o capas de traducción adicionales, a diferencia de gRPC en el navegador.
- Herramientas de prueba y documentación maduras: al mantener REST como protocolo principal, se conserva la posibilidad de usar herramientas ya utilizadas en el proyecto, como Postman, para probar y documentar los endpoints.

### Consecuencias negativas

- Dos protocolos a mantener: la coexistencia de REST y WebSocket implica configurar, asegurar (autenticación) y monitorear dos mecanismos de comunicación distintos, en lugar de uno solo.
- Gestión adicional de conexiones persistentes: WebSocket exige mantener conexiones abiertas por cliente, lo que implica gestionar reconexiones, tiempos de espera y un consumo de recursos del servidor distinto al modelo stateless de REST.
- Complejidad en la autenticación del canal persistente: el canal WebSocket requiere su propio mecanismo de autenticación/autorización sobre la conexión (por ejemplo, mediante token en el handshake), en lugar de heredar directamente el mismo esquema aplicado a cada petición REST.
- No aprovechable en el vigilante durante la desconexión: en la aplicación del vigilante, el canal WebSocket pierde su propósito durante los periodos sin conectividad, por lo que las actualizaciones en tiempo real solo aplican mientras existe conexión, y la sincronización posterior sigue dependiendo de la reconciliación ya definida para el funcionamiento offline (RxDB).

---

## adr-0017 — Seleccionar estrategia de desarrollo para la experiencia de usuario multiplataforma

**Estado:** Propuesta

### Contexto

Smartlife requiere adaptar el sistema a dispositivos móviles y de escritorio, garantizando el mismo nivel de rendimiento y funcionalidades.

### Escenarios de calidad y funcionalidades significativas referenciados

- ESC-CAL-POR-0001

### Decisión

A. Progressive Web App (PWA) Mobile-First con App Shell: Se selecciona una estrategia de Progressive Web App como solución de experiencia multiplataforma para SmartLife. Un único código fuente responsive resuelve la adaptación de pantalla, y sus capacidades adicionales Service Worker, instalación en pantalla de inicio y funcionamiento offline dejan de ser opcionales al confirmarse que el sistema necesita que el vigilante pueda seguir consultando autorizaciones de visita y registrando ingresos aunque no haya conectividad en portería, un requisito crítico que no existía cuando se evaluó originalmente esta decisión.

Frente al desarrollo multiplataforma nativo (opción B), se descarta porque exige mantener una base de código adicional, separada de la web, junto con el proceso de publicación y actualización en las tiendas de aplicaciones, un costo de mantenimiento no justificado por un requisito de negocio que exija presencia nativa en App Store o Play Store, más allá del componente offline que la PWA ya resuelve.

Frente a Responsive Web Design tradicional (opción C), que había sido la decisión previa, se revierte porque, si bien resuelve la adaptación visual básica mediante hojas de estilo, no ofrece capacidades de instalación ni de trabajo sin conexión, ahora identificadas como indispensables para que el vigilante pueda seguir operando el módulo de visitas durante una caída de conectividad, sin las cuales esa operación crítica quedaría completamente bloqueada.

En conclusión, la PWA Mobile-First con App Shell se considera nuevamente la alternativa más adecuada para SmartLife, ya que resuelve tanto la adaptación multiplataforma exigida por el contexto original como el requisito crítico de continuidad operativa en portería sin conexión, siendo además la base técnica indispensable (Service Worker) sobre la que se construyen las decisiones ya tomadas de almacenamiento local, cola de sincronización y Background Sync.

### Opciones consideradas

- A. Progressive Web App (PWA) Mobile-First con App Shell
- Aplicación web mejorada progresivamente, instalable y con capacidades de funcionamiento offline mediante Service Workers, que ofrece la experiencia de móvil y escritorio con un único código fuente y sin publicación en tiendas de aplicaciones.
- B. Desarrollo Multiplataforma Nativo
- Aplicaciones compiladas a código nativo para iOS y Android, con acceso completo a las APIs de hardware del dispositivo y el mejor rendimiento posible en interacciones complejas. Exige mantener una base de código separada de la versión web y gestionar el proceso de publicación y actualización en las tiendas de aplicaciones (App Store y Play Store).
- C. Responsive Web Design (RWD Tradicional) X
- Sitio web adaptable mediante hojas de estilo CSS según el tamaño de pantalla. Permite visualizar el contenido en cualquier dispositivo, pero no ofrece capacidades de instalación en pantalla de inicio, notificaciones push nativas ni soporte para trabajo sin conexión (offline).

### Consecuencias positivas

- Continuidad operativa en portería sin conexión: El vigilante puede seguir consultando autorizaciones y registrando ingresos de visitantes aunque se pierda la conectividad, resolviendo el requisito crítico identificado con el equipo.
- Base técnica para el resto de la estrategia offline: El Service Worker habilita directamente las decisiones ya tomadas de almacenamiento local (IndexedDB), cola de acciones y sincronización en segundo plano (Background Sync).
- Instalación sin tiendas de aplicaciones: Los usuarios pueden instalar la aplicación en pantalla de inicio sin pasar por el proceso de revisión de App Store o Play Store.
- Único código fuente: Mantiene una sola base de código para toda la experiencia multiplataforma, evitando el costo de bases de código separadas que exigiría el desarrollo nativo.

### Consecuencias negativas

- Complejidad técnica adicional: Requiere configurar Service Workers, Web App Manifest y estrategias de cacheo, un esfuerzo de desarrollo mayor que el que exigiría Responsive Web Design puro.
- Soporte desigual entre navegadores: Persisten las limitaciones conocidas de iOS/Safari para funcionalidades de instalación, notificaciones push y Background Sync, que exigen mantener mecanismos de respaldo (como el evento online) para no dejar a esos usuarios sin cobertura.
- Curva de aprendizaje para el equipo: Exige conocimientos específicos de Service Workers y del ciclo de vida de una aplicación instalable que no eran necesarios bajo Responsive Web Design.
- Segunda reversión de esta decisión: Es la segunda vez que se cambia esta estrategia (de PWA a RWD y de vuelta a PWA), lo cual debe documentarse claramente como evidencia de que el requisito de trabajo offline no estaba identificado en la primera evaluación, para que quede trazable ante el profesor por qué la decisión cambió con el tiempo.

---

## adr-0018 — Seleccionar framework del backend

**Estado:** Propuesta

### Contexto

SmartLife requiere desarrollar el backend bajo arquitectura de puertos y adaptadores, con soporte de concurrencia, tipado fuerte y compatibilidad con MassTransit y Polly.

### Decisión

B. Spring Boot (Java): Se selecciona Spring Boot (Java, opción B) como framework de desarrollo del backend de SmartLife. Spring Boot ofrece un desempeño y una madurez arquitectónica sólidos para arquitecturas orientadas a puertos y adaptadores, con soporte especialmente robusto para el estilo de Modulith (monolito modular) que el proyecto ya adoptó como arquitectura: el propio ecosistema Spring cuenta con Spring Modulith, un framework diseñado específicamente para estructurar aplicaciones monolíticas en módulos internos bien delimitados, con comunicación entre ellos mediante eventos de dominio, verificación de límites de módulo en tiempo de compilación, y documentación arquitectónica generada automáticamente capacidades pensadas exactamente para el "comunicador entre módulos" que exige un Modulith, y que no tienen un equivalente igual de maduro y respaldado oficialmente en el ecosistema .NET.

Frente a ASP.NET Core (opción A), aunque .NET ofrece tipado fuerte y buen desempeño bajo concurrencia, no cuenta con un framework equivalente a Spring Modulith diseñado específicamente para la comunicación interna entre módulos de un monolito modular; construir esa capa desde cero o adaptarla sobre MassTransit exigiría más esfuerzo de diseño propio que adoptar una herramienta ya pensada para ese propósito exacto dentro del ecosistema Java.

Frente a NestJS (opción C), se descarta por la misma razón ya identificada: su modelo de concurrencia de un solo hilo (event loop) resulta menos adecuado para operaciones con alta carga transaccional como el procesamiento simultáneo de pagos (ESC-CAL-ESC-0001), frente al modelo de hilos que sí ofrecen tanto Java como .NET.

Frente a Django (opción D), se descarta principalmente por tipado: Python es de tipado dinámico, lo que incrementa el riesgo de errores en tiempo de ejecución sobre operaciones financieras críticas que Java, con tipado fuerte y estático, detecta en tiempo de compilación antes de llegar a producción.

En conclusión, Spring Boot se considera la alternativa más adecuada para SmartLife, ya que además de mantener tipado fuerte y buen desempeño bajo concurrencia, ofrece con Spring Modulith una herramienta oficial y madura específicamente diseñada para resolver la comunicación entre módulos de una arquitectura de monolito modular, que es justamente el estilo arquitectónico que el proyecto ya adoptó.

### Opciones consideradas

- A. ASP.NET Core (C#): Framework de código abierto de Microsoft, con tipado estático y buen desempeño bajo concurrencia mediante Tasks. No cuenta con un framework oficial equivalente a Spring Modulith para estructurar y comunicar módulos dentro de un monolito.
- B. Spring Boot (Java): Framework maduro del ecosistema Java, con tipado estático, modelo de concurrencia basado en hilos, y el respaldo de Spring Modulith, un framework oficial diseñado específicamente para construir monolitos modulares con comunicación entre módulos vía eventos de dominio.
- C. NestJS (Node.js/TypeScript): Framework backend sobre Node.js con arquitectura modular inspirada en Angular. Su modelo de concurrencia de un solo hilo es menos adecuado para cargas transaccionales intensivas en cómputo.
- D. Django (Python): Framework backend de alto nivel con baterías incluidas. Tipado dinámico, lo que incrementa el riesgo de errores en tiempo de ejecución sobre operaciones financieras críticas.

### Consecuencias positivas

- Comunicador de módulos resuelto de forma oficial: Spring Modulith ofrece soporte maduro y respaldado oficialmente para la comunicación entre módulos de un monolito modular mediante eventos de dominio, resolviendo directamente la necesidad identificada como "comunicador entre módulos".
- Tipado estático para reducir errores financieros: Java detecta errores de tipos en tiempo de compilación, reduciendo el riesgo de fallos en operaciones financieras críticas frente a lenguajes de tipado dinámico.
- Concurrencia adecuada para carga transaccional: El modelo de hilos de Java sostiene los escenarios de alta carga transaccional, como el procesamiento simultáneo de pagos, mejor que un modelo de un solo hilo.
- Disciplina arquitectónica verificable: Spring Modulith permite verificar los límites entre módulos en tiempo de compilación, ayudando a mantener la separación del Modulith a medida que el proyecto crece.
- Ecosistema maduro y documentado: Cuenta con amplia documentación oficial y comunidad activa, especialmente robusta en aplicaciones empresariales.

### Consecuencias negativas

- Reescritura de decisiones previas: Obliga a reevaluar y reemplazar MassTransit y Polly, ambas librerías del ecosistema .NET, por sus equivalentes en Java, exigiendo revisar y volver a justificar esos dos ADRs.
- Curva de aprendizaje del ecosistema Java: El equipo debe adquirir o reforzar conocimiento específico de Java y Spring si no cuenta con experiencia previa.
- Mayor rigidez estructural: Spring Boot y Spring Modulith imponen convenciones más opinionadas que ASP.NET Core, reduciendo la flexibilidad de organización del código.
- Runtime adicional en el entorno de hosting: Requiere una JVM desplegada en Render, en lugar del runtime más liviano de .NET.

---

## adr-0019 — Selección del modelo de base de datos

**Estado:** Propuesta

### Contexto

Smartlife requiere almacenar la información según sus características estructurales, consistencia y completitud de persistencia.

### Decisión

B. Bases de datos relacionales (SQL): Se seleccionó el modelo de bases de datos relacionales (SQL) para SmartLife, debido a las características estructurales y a la necesidad de integridad de la información gestionada dentro de la unidad residencial. Las bases de datos relacionales permiten almacenar los datos mediante filas y columnas con una estructura previamente definida, lo que garantiza una gestión íntegra, consistente, aislada y completa de las transacciones realizadas en el sistema, tal como lo exige la naturaleza de entidades estrechamente relacionadas entre sí como vivienda, propietario, cuenta de cobro y pago.

Frente a las bases de datos no relacionales (NoSQL), el modelo relacional resulta más adecuado porque sí satisface la necesidad de mantener la integridad de los datos, garantizando la consistencia y precisión de todas las operaciones en todo momento; si se genera un error, este se controla dentro de la transacción en lugar de propagarse de forma descontrolada a las operaciones relacionadas. Adicionalmente, el modelo relacional impone reglas estrictas de validación, evita duplicados y preserva las relaciones entre estructuras de datos mediante llaves foráneas y restricciones de unicidad, manteniendo la coherencia de la información de forma constante. Las bases de datos no relacionales, si bien favorecen la escalabilidad horizontal y ofrecen altos rendimientos de procesamiento al no requerir una estructura previa, no garantizan consistencia inmediata, lo que puede romper la integridad referencial entre la información y comprometer la exactitud de las consultas financieras que la unidad residencial requiere; esto es crítico en escenarios como ESC-CAL-FIA-0005 y ESC-CAL-FIA-0007, donde el sistema debe aplicar correctamente el monto de un pago recuperado a su cuenta de cobro correspondiente y procesar una única solicitud de pago sin duplicarla, algo que un modelo NoSQL sin transacciones ACID nativas no puede asegurar sin reimplementar esas garantías a nivel de aplicación.

Frente a la opción de combinar ambos modelos (Ambas), se descarta por representar una complejidad innecesaria para las necesidades actuales del proyecto. Mantener dos motores de persistencia distintos uno relacional para las transacciones y otro no relacional para otro tipo de datos implicaría duplicar la infraestructura, los mecanismos de respaldo y sincronización, y el conocimiento operativo requerido por el equipo, sin que exista dentro de las historias de usuario y los escenarios de calidad relevados un volumen significativo de información semiestructurada o no estructurada que justifique dicha combinación; prácticamente la totalidad de las entidades del sistema (usuarios, viviendas, cuentas de cobro, visitas, reservas, parqueaderos, incidentes, PQRS) son datos estructurados con relaciones explícitas entre sí, por lo que introducir un segundo motor de base de datos añadiría sobrecarga arquitectónica sin un beneficio claro sobre los atributos de calidad priorizados.

En conclusión, el modelo relacional (SQL) se considera la alternativa más adecuada para el proyecto, ya que sus mecanismos nativos de integridad referencial, validación estricta y consistencia transaccional se ajustan directamente a los atributos de calidad con mayor peso identificados en la priorización del sistema Confiabilidad (0.228), Seguridad (0.222) y Capacidad para ser auditado (0.144) y a la naturaleza altamente relacional de la información que SmartLife debe gestionar.

### Opciones consideradas

- A. Bases de datos no relacionales (NoSQL): Servicio de almacenamiento de información que no semiestructurada o no estructurada que permita el guardado de datos sin definir una estructura previa, favorecer la escalabilidad horizontal de datos y poseer altos rendimientos de procesamiento de datos.
- B Base de datos relacionales (SQL): Servicio de almacenamiento de datos estructurados a traves de filas y columnas que permitan garantizar la gestión integra, consistente, aisladas y completas de las persistencias de las transacciones realizadas en el sistema.
- C. Ambas: Combinción de sistemas SQL y NoSQL en la misma solución para almacenar todos la información de los objetos del sistema según su naturaleza estructural, relacional, consistencia e integridad.

### Consecuencias positivas

- Integridad referencial garantizada: Se asegura la consistencia entre entidades estrechamente relacionadas como usuarios, viviendas, cuentas de cobro y pagos, evitando registros huérfanos o inconsistentes que podrían generarse si dos operaciones concurrentes modificaran datos relacionados sin control transaccional.
- Transacciones financieras protegidas: Las garantías ACID aseguran que los pagos y su aplicación a las cuentas de cobro sean atómicos, de modo que no se dupliquen ni se pierdan ante un fallo a mitad de la operación, algo crítico para escenarios como ESC-CAL-FIA-0005.
- Validación de reglas de negocio simplificada: Las restricciones nativas del modelo relacional (llaves foráneas, unicidad, checks) reducen la necesidad de reimplementar esas mismas validaciones manualmente en la capa de aplicación.
- Trazabilidad facilitada: La estructura relacional permite construir históricos consistentes y consultables mediante joins entre tablas, apoyando directamente los requisitos de auditoría del proyecto.
- Menor complejidad operativa: Al mantener un único motor de persistencia en lugar de administrar y sincronizar dos tecnologías distintas, se reduce la carga operativa del equipo frente a la opción de combinar SQL y NoSQL.

### Consecuencias negativas

- Escalado horizontal más limitado: A diferencia de un modelo no relacional, escalar horizontalmente exige particionamiento, réplicas o sharding a medida que crece el volumen de datos, en lugar de la escalabilidad más directa que ofrecería NoSQL.
- Esquema rígido ante cambios: Cualquier modificación estructural, como agregar o cambiar una entidad, requiere migraciones planificadas, reduciendo la flexibilidad frente a cambios frecuentes de requisitos del negocio.
- Rendimiento inferior en lectura masiva: Para operaciones de lectura de gran volumen o datos semiestructurados, el rendimiento es menor al que ofrecería un motor NoSQL optimizado específicamente para ese propósito.
- Sensibilidad a cuellos de botella de concurrencia: Las escrituras simultáneas sobre las mismas filas o tablas exigen un diseño cuidadoso de índices y bloqueos para evitar degradación del rendimiento bajo carga alta.
- Dependencia inicial de escalado vertical: El crecimiento del sistema depende en gran medida de aumentar los recursos del propio servidor de base de datos, antes de recurrir a estrategias de escalado horizontal más complejas.

---

## adr-0020 — Seleccionar el motor de base de datos relacional

**Estado:** Propuesta

### Contexto

Smartlife requiere estructurar la información de usuarios, viviendas, pagos, visitas, cuentas, autorizaciones, reservas, parqueaderos e incidentes con relaciones explícitas.

### Decisión

A. PostgreSQL: Se seleccionó PostgreSQL como motor de base de datos relacional para SmartLife. PostgreSQL es un motor de código abierto que no impone costos de licenciamiento ni límites de volumen de almacenamiento, y que incorpora de fábrica control de concurrencia multiversión (MVCC), separación de esquemas con permisos independientes por institución, seguridad a nivel de fila (Row-Level Security) y soporte para tablas de solo inserción (Append-Only). De acuerdo con las necesidades del proyecto, que exige integridad transaccional sobre operaciones financieras, aislamiento de datos entre urbanizaciones y trazabilidad auditable de cada cambio, PostgreSQL resulta la alternativa más adecuada para estos fines.

Frente a MySQL, PostgreSQL se presenta como el motor con mayor rigurosidad en el aislamiento estructural de los datos. Mientras MySQL no distingue de forma nativa entre esquema y base de datos lo que dificulta separar lógicamente la información de cada urbanización dentro de una misma instancia, PostgreSQL sí ofrece esquemas independientes con permisos propios. Adicionalmente, MySQL carece de soporte nativo para seguridad granular a nivel de fila, algo indispensable para escenarios como ESC-CAL-SEG-0010 y ESC-CAL-SEG-0014, donde un propietario no debe poder consultar ni modificar cuentas de cobro que no le pertenecen; en PostgreSQL esta regla se aplica directamente en el motor mediante Row-Level Security, en lugar de depender únicamente de la capa de aplicación.

Frente a SQL Server, PostgreSQL destaca por no imponer límites de almacenamiento en su versión gratuita. SQL Server ofrece capacidades de esquemas y seguridad a nivel de fila equivalentes a las de PostgreSQL, pero su edición sin costo restringe cada base de datos a 10 GB, un umbral que SmartLife superaría con rapidez: solo el módulo de cuentas de cobro está proyectado en la matriz de escalabilidad para crecer de 1.000.000 a 2.000.000 de registros (ESC-CAL-ESC-0006) manteniendo los tiempos de consulta establecidos, sin contar los históricos de pagos, visitas, incidentes y PQRS que también deben conservarse sin degradar el rendimiento. Escalar a una edición de pago de SQL Server para levantar ese límite excede el presupuesto disponible para el proyecto.

Frente a Oracle Database, PostgreSQL se diferencia por no requerir costos de licenciamiento corporativo ni la complejidad operativa asociada a su administración. Oracle Database ofrece un rendimiento sobresaliente e incluso capacidades avanzadas similares a las de PostgreSQL en materia de integridad y trazabilidad, pero su modelo de costos y la necesidad de personal especializado para su mantenimiento no son viables para el presupuesto ni para el equipo reducido con el que cuenta el proyecto.

En conclusión, PostgreSQL se considera la alternativa más adecuada para el proyecto debido principalmente a la ausencia de costos de licenciamiento y de límites de volumen de datos, sus mecanismos nativos de aislamiento y seguridad a nivel de fila, y su capacidad comprobada para sostener los requisitos de confiabilidad, seguridad y escalabilidad que exige el manejo de información financiera y estructurada dentro de SmartLife.

### Opciones consideradas

- A. PostgreSQL: Motor relacional de código abierto. Trae de fábrica el mecanismo para atender solicitudes en orden de llegada bajo concurrencia, esquemas con permisos propios por institución, seguridad a nivel de fila y permisos para tablas inmutables (Append-Only). Sin costo de licencia ni límite de volumen de datos.
- B. MySQL: Motor relacional de código abierto, sin costo de licencia. Su limitación principal radica en el aislamiento: no distingue de forma nativa entre esquema y base de datos, y carece de soporte nativo para seguridad granular a nivel de fila.
- C. SQL Server: Motor relacional comercial con capacidades avanzadas de esquemas y seguridad a nivel de fila. Su edición gratuita impone un límite rígido de 10 GB por base de datos, y pasar a ediciones de pago excede el presupuesto del proyecto.
- D. Oracle Database: Motor relacional comercial de alto rendimiento. Se descarta debido a sus costos de licenciamiento corporativo y a la alta complejidad de administración que requiere.

### Consecuencias positivas

- Sin costos de licenciamiento ni límites de volumen: Al ser de código abierto, PostgreSQL permite el crecimiento del sistema sin restricciones económicas ni topes de almacenamiento que obliguen a migrar de plan a medida que aumente el volumen de datos.
- Seguridad reforzada a nivel de fila: El soporte nativo de Row-Level Security permite aplicar directamente en el motor las reglas de autorización por rol y por propietario, sin depender únicamente de la capa de aplicación para impedir que un usuario acceda a información que no le pertenece.
- Aislamiento lógico entre urbanizaciones: El manejo de esquemas independientes con permisos propios facilita separar la información de cada urbanización dentro de una misma instancia, algo que MySQL no resuelve de forma nativa.
- Concurrencia sin bloqueos innecesarios: El control de concurrencia multiversión (MVCC) permite atender lecturas y escrituras simultáneas sosteniendo los tiempos de respuesta exigidos por la matriz de transacciones bajo alta concurrencia.
- Auditoría facilitada mediante tablas inmutables: El soporte de tablas de solo inserción (Append-Only) permite implementar históricos inmutables, como el requerido en el registro de aplicaciones de pago.
- Comunidad activa y validación práctica: Cuenta con una comunidad extensa y documentación amplia que reduce la dependencia de soporte comercial, y ya fue validado mediante una prueba de concepto con datasets simulados de gran volumen, reduciendo el riesgo de adoptar una tecnología no probada.

### Consecuencias negativas

- Administración a cargo del equipo: Al no contar con soporte comercial oficial, el equipo asume directamente las tareas de configuración y mantenimiento del motor, sin un proveedor que las resuelva por contrato.
- Tuning de rendimiento propio: El ajuste de índices, particionamiento y configuración de memoria recae en el equipo del proyecto, exigiendo conocimiento técnico adicional a medida que crece el volumen de datos.
- Funcionalidades avanzadas sin integración de fábrica: Capacidades como monitoreo avanzado, alta disponibilidad o replicación, que en motores comerciales vienen integradas, deben configurarse o complementarse con herramientas externas.
- Soporte dependiente de la comunidad: Sin un proveedor comercial detrás, la resolución de incidentes críticos depende de la comunidad o de documentación pública, sin garantías contractuales de tiempos de respuesta (SLA).
- Riesgo de portabilidad limitada: Una eventual migración futura hacia otro motor relacional puede implicar reescritura de funciones o extensiones específicas de PostgreSQL que no son directamente portables a otros sistemas.

### Notas

- https://aws.amazon.com/es/compare/the-difference-between-mysql-vs-postgresql/

---

## adr-0021 — Seleccionar framework del frontend

**Estado:** Propuesta

### Contexto

SmartLife requiere desarrollar el frontend garantizando componentización mantenible, estructura para trabajo en paralelo y buena accesibilidad/indexación.

### Escenarios de calidad y funcionalidades significativas referenciados

- ESC-CAL-POR-0001

### Decisión

B. Angular: Se selecciona Angular como framework de implementación del frontend de SmartLife. Angular es un framework opinionado, con una estructura de proyecto, convenciones de módulos e inyección de dependencias definida de fábrica, lo cual favorece la componentización mantenible exigida por el contexto y reduce la variabilidad de criterios entre los distintos integrantes del equipo al momento de estructurar el código especialmente valioso en un proyecto donde varias personas contribuyen en paralelo. Al renderizar HTML real, Angular también favorece la accesibilidad y el posicionamiento en buscadores (SEO), relevante al tratarse de una plataforma de acceso público vía navegador.

Frente a React (opción A), se descarta como opción principal porque, si bien ofrece una curva de aprendizaje más suave y mayor flexibilidad, carece de la estructura opinionada que sí trae Angular de fábrica, lo que exige mayor disciplina propia del equipo para mantener consistencia en la organización del código a medida que crecen los módulos del sistema. Con el cambio de estrategia de PWA a Responsive Web Design, además, deja de pesar la ventaja que React tenía por su ecosistema de herramientas PWA (Workbox), ya que ese criterio ya no aplica al contexto actual.

Frente a Flutter Web (opción C), se descarta como framework principal porque su renderizado por defecto no genera HTML semántico, lo que afecta la accesibilidad y el posicionamiento en buscadores frente a Angular y React, un compromiso que no se justifica dado que SmartLife es una plataforma de acceso público vía navegador.

En conclusión, Angular se considera la alternativa más adecuada para SmartLife, ya que impone una estructura de proyecto consistente que favorece la componentización mantenible entre varios desarrolladores trabajando en paralelo, satisface el requisito de accesibilidad/SEO por HTML real, y no exige el compromiso de accesibilidad que sí tendría Flutter Web.

### Opciones consideradas

- A. React: Biblioteca de componentes con curva de aprendizaje suave y ecosistema amplio. Renderiza HTML real, favoreciendo accesibilidad y SEO, pero requiere mayor disciplina propia del equipo para mantener una estructura de proyecto consistente, al no ser un framework opinionado.
- B. Angular: Framework opinionado, con estructura de proyecto, módulos e inyección de dependencias definidos de fábrica. Renderiza HTML real, igual de favorable para accesibilidad y SEO que React, favoreciendo la consistencia del código entre varios desarrolladores.
- C. Flutter Web: Permite construir el frontend con el mismo lenguaje (Dart) y motor de renderizado que Flutter usa para móvil y escritorio. Su renderizado por defecto no genera HTML semántico, lo que afecta accesibilidad y posicionamiento en buscadores frente a las otras dos opciones.

### Consecuencias positivas

- Estructura de proyecto consistente: Las convenciones definidas de fábrica (módulos, inyección de dependencias, servicios) favorecen la componentización mantenible entre varios integrantes trabajando en paralelo.
- Accesibilidad y SEO por HTML real: Al renderizar HTML real, favorece la accesibilidad para lectores de pantalla y el posicionamiento en buscadores.
- Menos decisiones de librerías adicionales: Al ser un framework completo, reduce la necesidad de estandarizar librerías propias para enrutamiento, formularios o gestión de estado.
- Respaldo y documentación sólidos: Cuenta con amplia documentación oficial y comunidad activa, respaldado directamente por Google.
- Coherencia con el backend: Comparte con Spring Boot una filosofía similarmente opinionada y estructurada en ambos extremos del stack.

### Consecuencias negativas

- Curva de aprendizaje más alta: Exige familiarizarse con TypeScript estricto, decoradores, módulos y el ciclo de vida propio del framework, frente a la simplicidad de React.
- Sin reutilización hacia nativo: No comparte código con un eventual desarrollo nativo de móvil o escritorio, a diferencia de Flutter Web.
- Menor flexibilidad arquitectónica: Su estructura más rígida ofrece menos libertad para decisiones de arquitectura de frontend no convencionales.
- Pérdida de sinergia con Vercel: Vercel mantiene su compatibilidad de referencia principal con React/Next.js, por lo que con Angular se pierde esa ventaja específica de integración, aunque sigue funcionando correctamente.

---

## adr-0022 — Seleccionar plataforma de despliegue del frontend

**Estado:** Propuesta

### Contexto

SmartLife requiere desplegar el frontend de la PWA garantizando disponibilidad, bajos tiempos de carga y CI/CD sencillo de operar.

### Decisión

A. Vercel: Se selecciona Vercel como plataforma de despliegue del frontend de SmartLife. Vercel ofrece CDN global integrada, despliegues automáticos por cada push a la rama principal y previews automáticos por cada Pull Request, sin necesidad de configurar manualmente la infraestructura de entrega de contenido ni el pipeline de integración continua, lo cual reduce significativamente la carga operativa para un equipo reducido como el del proyecto.

Frente a Cloudflare Pages (opción B), aunque Cloudflare ya es un proveedor conocido del equipo por la decisión previa de usar Cloudflare R2 para almacenamiento de objetos, y Cloudflare Pages ofrece también CI/CD automático por Git y CDN integrada de forma muy similar a Vercel, se descarta como opción principal porque su integración de referencia está más orientada a frameworks como Next.js, Astro o sitios estáticos generales, mientras que Vercel mantiene compatibilidad de referencia directa con el ecosistema React que ya seleccionaron para la PWA, al ser desarrollado por el mismo equipo detrás de Next.js. Aun así, Cloudflare Pages queda como la alternativa más cercana si en algún momento quisieran consolidar todo el frontend y el almacenamiento de objetos bajo un mismo proveedor (Cloudflare), ya que reutilizarían la cuenta y la facturación que ya manejan con R2.

Frente a Azure Storage Account (Static Website Hosting) + Azure CDN (opción C), se descarta porque, a diferencia de Vercel y Cloudflare Pages, Storage Account no incluye CI/CD de fábrica: el equipo tendría que mantener manualmente un script o pipeline propio (por ejemplo, en GitHub Actions) que compile el proyecto y suba los archivos resultantes al contenedor $web en cada cambio, además de configurar por separado Azure CDN o Azure Front Door para lograr una distribución de baja latencia equivalente a la que Vercel y Cloudflare Pages ya traen integrada sin esfuerzo adicional. Aunque esta alternativa tiene la ventaja de mantener el despliegue del frontend dentro del mismo ecosistema Azure ya usado para el backend (AKS) y el Key Vault, el esfuerzo de configuración y mantenimiento de ese script propio no se justifica frente al beneficio marginal de centralizar toda la infraestructura en un solo proveedor, dado el tamaño y el tiempo disponible del equipo.

En conclusión, Vercel se considera la alternativa más adecuada para el despliegue del frontend de SmartLife, ya que resuelve de forma automática la distribución global de contenido y el flujo de integración continua con el menor esfuerzo operativo posible para el equipo, manteniendo además la mejor compatibilidad con React frente a las otras dos opciones evaluadas.

### Opciones consideradas

- A. Vercel: Plataforma de despliegue de frontend con CDN global integrada, CI/CD automático por cada push a Git y previews automáticos por Pull Request. Compatibilidad de referencia con proyectos React/Next.js. Plan gratuito (Hobby) sin costo por ancho de banda hasta un uso razonable.
- B. Cloudflare Pages: Plataforma de despliegue de frontend del mismo proveedor ya elegido para almacenamiento de objetos (Cloudflare R2), con CDN global integrada y CI/CD automático por Git de forma similar a Vercel. Su integración de referencia está más orientada a Next.js, Astro y sitios estáticos generales que específicamente a React puro.
- C. Azure Storage Account (Static Website Hosting) + Azure CDN: Servicio de almacenamiento de objetos de Azure con función nativa para servir sitios estáticos, complementado con Azure CDN o Front Door para distribución de baja latencia. No incluye CI/CD de fábrica: requiere mantener un script o pipeline propio (por ejemplo, en GitHub Actions) para compilar y desplegar el proyecto en cada cambio.

### Consecuencias positivas

- CI/CD sin configuración manual: Se despliega automáticamente desde el repositorio Git, sin necesidad de escribir ni mantener manifiestos ni scripts de despliegue propios.
- CDN incluida sin esfuerzo adicional: La distribución global reduce los tiempos de carga percibidos por el usuario sin importar su ubicación.
- Previews automáticos por Pull Request: Facilitan la revisión de cambios antes de fusionarlos a la rama principal.
- Compatibilidad de referencia con React: Al ser desarrollado por el mismo equipo detrás de Next.js, mantiene la mejor integración posible con el framework ya elegido para la PWA.
- Costo cero para el volumen actual: El plan gratuito cubre el tráfico actual del proyecto sin costo adicional de egress, a diferencia de Storage Account.

### Consecuencias negativas

- Proveedor externo adicional: Se suma fuera tanto del ecosistema Azure como del ecosistema Cloudflare ya usados en otras partes del proyecto.
- Menor control sobre la infraestructura: Ofrece menos personalización granular frente a desplegar dentro de la propia nube ya contratada.
- Dependencia de un tercero más: Su disponibilidad y condiciones comerciales deben monitorearse igual que las demás dependencias externas del sistema.
- Aislado de una eventual consolidación futura: Si el proyecto migrara por completo dentro de un mismo ecosistema de nube, este sería el único componente transversal que quedaría fuera de esa unificación.

---

## adr-0023 — Seleccionar proveedor de nube

**Estado:** Propuesta

### Contexto

SmartLife requiere alojar los servicios transversales de infraestructura (observabilidad, seguridad perimetral, gestión de parámetros) en un proveedor de nube centralizado.

### Decisión

B. Google Cloud Platform (GCP): Se opto por escoger GCP como proveedor de alojamiento en la nube ya que como un hyperscaler proporciona todo un abanico de componentes de stack tecnológico que le permite usar y facilitar procesos de integración y despliegue de sistemas con herramientas soportadas por GCP al igual que lo ofrece Microsoft Azure y AWS. Sin embargo frente a las otras alternativas de hyperscaler, inicialmente GCP ofrece un plan inicial gratuito de $300 dolares en un periodo de 90 días, mientras que AWS ofrece un credito de $200 en 6 meses y Azure el mismo precio por 30 días. Adicionalmente ofrece un modelo de pago por uso ajustable a la demanda de consumo real de la evitando sobredimensionar el costo de la infraestructura del servidor ya que ofrece planes de pago dividida por instancias, trabajos y trabajos retrasados que ofrecen una capa gratuita en cada una y un cobro por consumo. Azure establece un cobro por consumo dependiente de los recursos de servicio usado (CPU virtual + GB de RAM usadas) donde según la demanda de consumo se tendrá que pagar más o menos de manera mensual por la operación de la aplicación. AWS si bien define pago por consumo. Con respecto a AWS, gcp es una alternativa económicamente viable lata el dominio ya que al ser un negocio con picos de concurrecia mensuales y predecibles de la aplicación, sin mantener la capacidad máxima del servidor de nube que genere sobrecostos. Si bien AWS genera esta misma estrategia, tiene en cuenta costos granulares por cada uno de sus servicios de su abanico tecnológico, lo que hace más difícil el cálculo total de su costo total e incluso puede generar gastos altos para sistemas que implementen muchos de sus componentes a cobrar de manera individual. Lo cual para una unidad residencial con concurrencia mensual y definida impacta sobre su economía y sostenimiento. Frente a Render, este al ser un PaaS si bien es una alternativa de despliegue que prioriza la simplicidad de despliegue y operacion, GCP proporciona mayor flexibilidad al sistema ya que permite construir arquitecturas mucho más complejas dentro del mismo entorno y stack tecnológico, de manera que cualque arquitectura de aplicación inicial puede evolucionar sobre el mismo entorno de software.

### Opciones consideradas

- A. Amazon Web Services (AWS): Plataforma de nube líder del mercado, con CloudWatch, Secrets Manager y WAF nativos. Exige configuración inicial detallada de IAM y espacios de nombres de métricas antes de poder operar sus servicios de observabilidad y seguridad.
- B. Google Cloud Platform (GCP): Plataforma de nube con Cloud Monitoring (150 MB de logs/mes gratis, métricas básicas sin costo), Cloud Logging y Secret Manager (6 versiones de secretos activas gratis/mes), con niveles gratuitos amplios para proyectos de bajo volumen.
- C. Microsoft Azure: Plataforma de computación en la nube creada por Microsoft para construir, probar, desplegar y administrar aplicaciones y servicios a través de su red global de centros de datos.
- D. Servicio PaaS de despliegue (Render): Intentar resolver observabilidad y gestión de secretos exclusivamente con las capacidades nativas y limitadas de los proveedores ya contratados para otras funciones.

### Consecuencias positivas

- Nivel gratuito suficiente: Cloud Monitoring (150 MB de logs/mes) y Secret Manager (6 versiones activas/mes) cubren el volumen inicial del proyecto sin costo.
- Servicios transversales centralizados: Unifica bajo un mismo proveedor la observabilidad, la gestión de secretos y la gestión de parámetros, simplificando credenciales y facturación.
- Curva de configuración más simple: Requiere menos configuración inicial que AWS, adecuado para un equipo sin experiencia extensa en administración de nube.

### Consecuencias negativas

- Proveedor adicional en el ecosistema: Se suma a Render, Vercel y Cloudflare, cada uno con su propia consola y modelo de facturación a monitorear.
- Curva de aprendizaje de una consola nueva: El equipo debe familiarizarse con las herramientas específicas de GCP si no tiene experiencia previa.
- Sin cargas de trabajo pesadas alojadas allí: Ninguna parte crítica del backend corre directamente en GCP, limitando su rol a servicios transversales de soporte.

---

## adr-0024 — Seleccionar Web Application Firewall

**Estado:** Propuesta

### Contexto

SmartLife requiere filtrar el tráfico contra ataques comunes (inyección SQL, XSS, fuerza bruta) antes de que llegue a la aplicación.

### Decisión

B. Cloudflare WAF: Se selecciona Cloudflare WAF (opción B) como capa de protección perimetral para SmartLife. Aplicando el criterio de reutilizar infraestructura ya contratada, Cloudflare WAF opera sobre la misma cuenta ya usada para Cloudflare R2, sin costo adicional en su plan gratuito, el cual incluye el conjunto de reglas gestionadas contra el OWASP Top 10 (inyección, cross-site scripting, cross-site request forgery), exactamente los vectores de ataque ya identificados como riesgo en las tácticas arquitectónicas del proyecto para historias como HU52, HU78 y HU83.

Frente a un WAF nativo de un proveedor de nube distinto al que aloja el backend o el frontend (opción A), se descarta porque exigiría enrutar todo el tráfico de la aplicación a través de ese proveedor adicional exclusivamente para inspección de tráfico, duplicando el número de saltos de red que atraviesa cada solicitud y complicando la topología DNS del sistema sin un beneficio de protección adicional real frente a Cloudflare WAF.

Frente a no implementar ningún WAF y depender exclusivamente de la validación de entradas a nivel de aplicación (opción C), se descarta porque, aunque el sistema ya sanitiza entradas en el propio código del backend, dejar esa validación como única línea de defensa expone al sistema a que un error de implementación puntual (una expresión regular mal construida, un caso límite no contemplado) se convierta directamente en una vulnerabilidad explotable, sin ninguna capa perimetral que actúe como respaldo.

En conclusión, Cloudflare WAF se considera la alternativa más adecuada para SmartLife, ya que aprovecha infraestructura ya contratada, cubre sin costo adicional los vectores de ataque ya identificados como riesgo del proyecto (OWASP Top 10), y filtra el tráfico malicioso antes de que llegue al backend en Render.

### Opciones consideradas

- A. WAF nativo de un proveedor de nube distinto al backend/frontend: Servicio de firewall de aplicaciones web administrado, que exigiría enrutar el tráfico a través de un proveedor de nube que actualmente no aloja ninguna carga de trabajo del sistema, duplicando saltos de red.
- B. Cloudflare WAF: WAF del mismo proveedor ya usado para almacenamiento de objetos (R2), con reglas gestionadas contra el OWASP Top 10 y plan gratuito con protección básica.
- C. Ningún WAF (validación únicamente a nivel de aplicación): Depender exclusivamente de la sanitización y validación de entradas implementada en el propio código del backend, sin una capa de filtrado perimetral independiente.

### Consecuencias positivas

- Reutilización de infraestructura existente: Opera sobre la misma cuenta ya activa para Cloudflare R2, sin introducir un proveedor adicional exclusivamente para este propósito.
- Cobertura de los ataques más comunes sin costo: El plan gratuito incluye reglas gestionadas contra el OWASP Top 10, cubriendo los vectores de riesgo ya identificados en el proyecto.
- Filtrado antes de llegar al backend: Reduce la carga de validación que debe asumir la propia aplicación en Render, al bloquear tráfico malicioso en el perímetro.

### Consecuencias negativas

- Configuración de DNS adicional: Requiere enrutar el dominio de la aplicación a través de Cloudflare, una coordinación adicional con las plataformas de despliegue.
- Reglas avanzadas de pago: La protección más granular contra ataques sofisticados puede requerir el plan de pago si el proyecto necesita mayor cobertura.
- No reemplaza la validación de aplicación: Sigue siendo necesaria la sanitización de entradas a nivel de código como segunda capa de defensa.

---

## adr-0025 — Seleccionar mecanismo de gestión de parámetros de configuración

**Estado:** Propuesta

### Contexto

SmartLife requiere externalizar los valores de configuración de negocio para ajustarlos sin modificar ni redesplegar el código.

### Decisión

B. Spring Cloud Config: Se selecciona Spring Cloud Config (opción B) como mecanismo de gestión de parámetros para SmartLife. Aplicando el criterio de mantener coherencia con el stack de backend ya adoptado, Spring Cloud Config se integra de forma nativa al haberse elegido Spring Boot como framework, centralizando la configuración en un repositorio Git versionado (el mismo que el equipo ya usa para el código fuente) y permitiendo que los servicios recarguen los valores actualizados mediante el endpoint /actuator/refresh, sin necesidad de reiniciar ni redesplegar el servicio completo.

Frente a un servicio de gestión de parámetros administrado por un proveedor de nube (opción A), se descarta porque introduciría un servicio adicional, con su propio SDK y su propia curva de configuración, exclusivamente para resolver algo que Spring Cloud Config ya resuelve dentro del mismo ecosistema de backend, sin necesidad de credenciales ni llamadas de red adicionales hacia un proveedor externo.

Frente a mantener los parámetros embebidos en archivos de configuración estáticos por entorno como application.yml (opción C), se descarta porque cualquier ajuste a un valor (como cambiar el límite de reintentos de pago de 5 a 3) exigiría modificar el archivo, reconstruir el artefacto y volver a desplegar el servicio completo en Render, un ciclo de varios minutos para un cambio que debería tomar segundos, contradiciendo directamente el objetivo de agilidad que motiva este ADR.

En conclusión, Spring Cloud Config se considera la alternativa más adecuada para SmartLife, ya que resuelve la externalización de parámetros sin introducir infraestructura fuera del ecosistema del backend, y permite actualizar valores de configuración en caliente sin necesidad de redesplegar el servicio.

### Opciones consideradas

- A. Servicio de gestión de parámetros administrado por un proveedor de nube: Servicio externo para almacenar y consultar parámetros de configuración de aplicación, gestionado por un proveedor de nube, con su propio SDK y modelo de autenticación.
- B. Spring Cloud Config: Módulo del ecosistema Spring para centralizar configuración externa en un repositorio Git, con recarga de valores mediante el endpoint /actuator/refresh sin necesidad de redesplegar el servicio.
- C. Archivos de configuración estáticos por entorno (application.yml): Configuración embebida directamente en el artefacto desplegado, requiriendo reconstrucción y redespliegue completo para cualquier cambio de valor.

### Consecuencias positivas

- Integración nativa con el backend: Se integra directamente con Spring Boot, sin introducir un servicio externo adicional ni credenciales de terceros.
- Actualización sin redespliegue: Permite cambiar parámetros de negocio en caliente, sin necesidad de reiniciar ni redesplegar el servicio.
- Cambios versionados y auditables: Los ajustes de configuración quedan registrados en Git, con historial de quién modificó cada valor y cuándo.

### Consecuencias negativas

- Servidor de configuración adicional: Exige desplegar y mantener un Config Server dentro del backend.
- No cubre secretos sensibles: No está diseñado para almacenar información sensible, que debe resolverse con un mecanismo de gestión de secretos independiente.

---

## adr-0026 — Seleccionar mecanismo de generación y validación de códigos QR

**Estado:** Propuesta

### Contexto

SmartLife requiere validar un código QR asociado a cada autorización de visita cuando se de el ingreso del visitante en portería.

### Decisión

A. Librería embebida en el backend (ZXing): Se selecciona ZXing (opción A) como librería de generación y lectura de códigos QR, embebida directamente en el backend de Spring Boot. Aplicando el criterio de priorizar tecnología open source ya adoptada en decisiones previas del proyecto, ZXing es una librería madura de Java (con más de una década de desarrollo activo y soporte oficial de Google) que genera y decodifica códigos QR dentro del propio proceso del backend en milisegundos, sin ninguna llamada de red de por medio. El contenido del QR se limita a un identificador único (UUID) de la autorización de visita, que en portería se decodifica localmente y se contrasta directamente contra la tabla de autorizaciones en PostgreSQL, cerrando el ciclo completo de validación sin depender de ningún servicio externo.

Frente a un servicio externo de generación de QR en la nube (opción B), se descarta porque introduciría una llamada de red saliente por cada autorización de visita generada una operación que hoy toma microsegundos localmente pasaría a depender de la latencia y disponibilidad de un tercero, además de una dependencia externa adicional y potencialmente un costo por operación para una funcionalidad que no requiere ningún procesamiento ni dato que no esté ya disponible en el propio backend.

En conclusión, ZXing se considera la alternativa más adecuada para SmartLife, ya que resuelve la generación y validación de códigos QR sin costo, sin latencia de red y sin dependencia de un proveedor externo, aprovechando una librería madura y ampliamente adoptada del ecosistema Java.

### Opciones consideradas

- A. Librería embebida (ZXing): Librería open source de Java para generar y leer códigos QR localmente dentro del propio proceso del backend, con soporte oficial de Google, sin dependencias externas de red.
- B. Servicio externo de generación de QR en la nube: Servicio de terceros que genera el código QR mediante una API REST, introduciendo latencia de red y una dependencia externa adicional para una operación de bajo costo computacional.

### Consecuencias positivas

- Sin costo ni dependencia externa: No requiere pagar ni depender de la disponibilidad de un proveedor de terceros.
- Generación instantánea: Genera y valida en microsegundos, sin ninguna llamada de red de por medio.
- Integración directa con el stack ya elegido: Al ser una librería estándar de Java, se integra sin fricción con Spring Boot.

### Consecuencias negativas

- Mejoras futuras atadas al proyecto open source: Cualquier avance en formatos o corrección de errores depende del ciclo de actualización de la propia librería.
- Cambios de formato requieren redespliegue: Al estar la lógica embebida en el backend, modificar el formato del QR exige actualizar y volver a desplegar el servicio.

---

## adr-0027 — Seleccionar gestor de mensajes de aplicacion

**Estado:** Propuesta

### Contexto

SmartLife requiere estandarizar los mensajes dirigidos a usuarios y a desarrolladores, evitando que queden dispersos e inconsistentes en el código.

### Decisión

C. Strapi (Headless CMS): Se selecciona Strapi como gestor de mensajes de aplicación para SmartLife. Aplicando el criterio de permitir ajustes sin intervención directa de desarrollo, Strapi es una plataforma open source que expone un panel administrativo desde el cual se puede crear, editar y organizar los textos de los mensajes de usuario y técnicos, sin necesidad de que un desarrollador modifique código ni redespliegue el backend cada vez que se requiera ajustar la redacción de un mensaje algo que sí exigiría la opción A (Spring MessageSource), donde cualquier cambio de texto obliga a modificar el archivo .properties y volver a desplegar el servicio en Render.

Frente a Spring MessageSource + ResourceBundles (opción A), se descarta como mecanismo principal porque, aunque resuelve la centralización de mensajes dentro del backend, mantiene el ajuste de textos acoplado al ciclo de despliegue del código: cualquier persona del equipo administrativo que quiera corregir la redacción de un mensaje de cara al usuario tendría que pasar por un desarrollador, una modificación de código y un nuevo despliegue, un proceso desproporcionado para un cambio que en esencia es solo de texto.

Frente a mensajes codificados directamente en el código (opción B), se descarta porque dificulta mantener consistencia entre módulos desarrollados por distintos integrantes del equipo, y cualquier cambio de redacción exige buscar y modificar el texto en cada punto disperso del código donde aparece.

Strapi, al exponer una API REST/GraphQL propia para consultar el contenido gestionado, permite que el backend de SmartLife obtenga los textos de los mensajes en tiempo de ejecución consultando esa API, en lugar de tenerlos embebidos en el propio artefacto desplegado, desacoplando así la gestión de contenido textual del ciclo de vida del código de la aplicación.

En conclusión, Strapi se considera la alternativa más adecuada para SmartLife, ya que permite gestionar y actualizar los mensajes de usuario y técnicos sin intervención de desarrollo ni redespliegue del backend, a diferencia de las otras dos alternativas evaluadas, que mantienen esa dependencia.

### Opciones consideradas

- A. Spring MessageSource + ResourceBundles: Mecanismo nativo de Spring Boot para centralizar mensajes en archivos .properties, referenciados por código en lugar de texto embebido. Cualquier cambio de texto exige modificar el archivo y redesplegar el servicio.
- B. Mensajes codificados directamente en el código (hardcoded strings): Cada mensaje se escribe como una cadena de texto literal dentro del controlador o servicio que lo genera, sin centralización ni reutilización.
- C. Strapi (Headless CMS): Plataforma open source de gestión de contenido con panel administrativo, que permite administrar los textos de los mensajes sin que un desarrollador modifique código ni redespliegue el backend, exponiendo el contenido mediante una API propia.

### Consecuencias positivas

- Edición sin intervención de desarrollo: Cualquier integrante del equipo con acceso al panel puede ajustar la redacción de un mensaje sin tocar código ni pasar por un nuevo despliegue.
- Desacoplado del ciclo de despliegue del backend: Los cambios de contenido se reflejan de inmediato al consultarse por API, sin depender del proceso de build y despliegue del servicio en Render.
- Consistencia centralizada: Todos los mensajes de usuario y técnicos quedan organizados en un único panel, evitando variaciones de tono entre los distintos módulos del sistema.
- Preparado para escalar a otros idiomas: Strapi soporta de forma nativa contenido multi-idioma si en el futuro surge ese requisito, sin rediseñar el mecanismo desde cero.

### Consecuencias negativas

- Nueva dependencia de infraestructura: Introduce un servicio adicional (Strapi) que debe desplegarse, mantenerse y monitorearse, a diferencia de la opción A, que no exige infraestructura extra al ser nativa de Spring Boot.
- Llamada de red adicional por mensaje: El backend debe consultar la API de Strapi para obtener el contenido de los mensajes, introduciendo latencia y una dependencia de disponibilidad que no existiría con archivos locales, salvo que se implemente una capa de caché.
- Curva de aprendizaje de una herramienta nueva: El equipo debe familiarizarse con la administración y configuración de Strapi, una plataforma que no formaba parte del stack previamente adoptado.
- Punto de falla adicional: Si Strapi queda indisponible, el sistema podría quedarse sin poder resolver los textos de los mensajes a mostrar, a menos que se implemente un mecanismo de respaldo con valores por defecto embebidos.

---

## adr-0028 — Seleccionar estrategia de agendamiento de reservas de zonax comunes

**Estado:** Propuesta

### Contexto

Smartlife requiere reservar zonas comunes conservando las reglas de negocio de validación.

### Decisión

B. Desarrollo propio de backend + Librería de visualización de frontend: Se optó por desarrollar el agendamiento de reservas de zonas comunes dentro de la unidad residencial, debido a que las reglas de negocio planteadas requieren un nivel de personalización que no se encontró resuelto de manera adecuada en los servicios externos evaluados. El flujo propuesto permite que un residente o propietario consulte las zonas comunes y su disponibilidad, seleccione un horario de inicio y fin, y que el sistema valide las condiciones necesarias para realizar la reserva, como la disponibilidad del recurso, la ausencia de reservas que se superpongan, el estado de apertura de la zona y la duración máxima permitida. Adicionalmente, se requiere que un administrador pueda programar periodos de mantenimiento, así como gestionar cancelaciones y notificar a los usuarios mediante correo electrónico, indicando el motivo de la cancelación y los horarios disponibles.

Si bien los proveedores externos ofrecen funcionalidades de agendamiento y gestión de recursos, su adopción requeriría incorporar mecanismos de integración y sincronización para mantener la consistencia entre el sistema externo y SmartLife. Esto incrementaría la complejidad de la solución y trasladaría parte de la gestión de las reservas a un componente externo, cuando las principales operaciones dependen de reglas de negocio propias de la unidad residencial. Por esta razón, se decidió implementar el módulo de reservas dentro del Modulith de SmartLife, manteniendo la lógica de negocio y la información de las reservas bajo el control de la aplicación.

### Opciones consideradas

- A. Proveedor externo de agendamiento: Integrar una plataforma especializada de reservas, como Cal.com, para delegar la gestión del agendamiento, disponibilidad y calendario a un servicio externo.
- B. Desarrollo propio de backend + Librería de visualización de frontend: Consiste en implementar la lógica y gestión de las reservas dentro del backend de SmartLife, utilizando la base de datos relacional seleccionada para su persistencia y una librería frontend para representar y seleccionar fechas y horarios.

### Consecuencias positivas

- Control sobre las reglas de negocio: Permite implementar directamente las restricciones de las zonas comunes, disponibilidad, duración de las reservas, horarios permitidos y prevención de solapamientos.
- Integración con el dominio existente: Puede utilizar directamente los usuarios, roles, viviendas y mecanismos de autorización de SmartLife, evitando adaptar estos conceptos a un sistema externo.
- Fuente única de verdad: Las reservas permanecen almacenadas en PostgreSQL junto con el resto de la información de SmartLife, evitando sincronización entre sistemas.
- Mayor capacidad de evolución: permite modificar el comportamiento del módulo según futuras necesidades de la unidad residencial sin depender de las capacidades o cambios de un proveedor externo.
- Consistencia transaccional: las operaciones relacionadas con las reservas pueden gestionarse dentro de la misma aplicación y base de datos.

### Consecuencias negativas

- Mayor esfuerzo de desarrollo: el equipo debe implementar la lógica de disponibilidad, creación, modificación, cancelación y validación de reservas
- Responsabilidad de mantenimiento: SmartLife deberá mantener y evolucionar el módulo, incluyendo corrección de errores y nuevas funcionalidades..
- Desarrollo de funcionalidades de calendario: Aunque la libreria para calendario facilite la visualizaciín desde su interfaz requiere de procesos de integración con el backend y adaptar el componente a las necesidades del negocio.
- Posible incremento del alcance: si posteriormente se requieren funcionalidades avanzadas de calendarios, como sincronización con calendarios externos o recurrencias complejas, deberán desarrollarse o integrarse adicionalmente.

---

## adr-0029 — Seleccionar mecanismo de gestión de secretos (Key Vault)

**Estado:** Propuesta

### Contexto

SmartLife requiere proteger las credenciales y valores sensibles del sistema sin exponerlos en el código fuente ni en configuraciones versionadas.

### Decisión

B. Google Secret Manager: Se selecciona Google Secret Manager como mecanismo de gestión de secretos para SmartLife. Aplicando el criterio de reutilizar infraestructura ya contratada, Secret Manager opera sobre la misma cuenta de Google Cloud Platform ya seleccionada para los servicios transversales de observabilidad (Cloud Monitoring, Cloud Logging), sin necesidad de dar de alta un proveedor adicional exclusivamente para este propósito. Su nivel gratuito cubre las primeras 6 versiones de secretos activas por mes y cobra $0.03 USD por cada 10.000 operaciones de acceso adicionales, un umbral que el volumen actual del proyecto no alcanzaría a superar.

Frente a AWS Secrets Manager (opción A), se descarta porque introduciría un proveedor de nube adicional AWS exclusivamente para gestionar secretos, sin que ninguna otra carga de trabajo del sistema corra sobre esa plataforma, sumando una consola, un modelo de facturación y un esquema de credenciales más que el equipo tendría que monitorear, además de que AWS Secrets Manager no ofrece nivel gratuito permanente (cobra $0.40 USD por secreto al mes desde el primer secreto almacenado).

Frente a HashiCorp Vault autohospedado (opción C), se descarta porque, siendo open source, exige que el propio equipo despliegue, configure y mantenga la infraestructura del servidor de Vault —incluyendo su propia estrategia de alta disponibilidad, respaldo y parcheo de seguridad, una carga operativa desproporcionada frente al tamaño del equipo y que contradice el criterio de minimizar la infraestructura propia que se ha mantenido en decisiones similares (como Render en lugar de un clúster propio).

Frente a mantener los secretos como variables de entorno simples sin un gestor dedicado (opción D), se descarta porque no ofrece versionado del historial de cambios, ni un registro de auditoría de quién accedió a cada secreto y cuándo, ni rotación de credenciales gestionada; un secreto comprometido o rotado incorrectamente quedaría sin ningún rastro trazable, contradiciendo los requisitos de auditabilidad que el propio proyecto prioriza como uno de sus atributos de calidad de mayor peso.

En conclusión, Google Secret Manager se considera la alternativa más adecuada para SmartLife, ya que aprovecha la cuenta de GCP ya contratada para otros servicios transversales, ofrece un nivel gratuito suficiente para el volumen actual del proyecto, y resuelve versionado y auditoría de accesos de forma nativa, sin la carga operativa que exigiría autohospedar HashiCorp Vault.

### Opciones consideradas

- A. AWS Secrets Manager: Servicio administrado de gestión de secretos de Amazon Web Services, con rotación automática de credenciales e integración nativa con otros servicios de AWS. No ofrece nivel gratuito permanente; cobra desde el primer secreto almacenado.
- B. Google Secret Manager: Servicio administrado de Google Cloud Platform para almacenar y gestionar el acceso a claves de API, contraseñas, certificados y otros datos sensibles, con versionado y registro de auditoría de accesos incluidos. Nivel gratuito de 6 versiones de secretos activas por mes.
- C. HashiCorp Vault (autohospedado): Herramienta open source de gestión de secretos, con cifrado en tránsito y en reposo, políticas de acceso granulares y generación dinámica de credenciales. Requiere ser desplegado, configurado y mantenido por el propio equipo.
- D. Variables de entorno sin gestor dedicado: Almacenar los secretos directamente como variables de entorno en la plataforma de despliegue (Render), sin ningún servicio intermedio de gestión, versionado ni auditoría.

### Consecuencias positivas

- Reutilización de infraestructura existente: Se confirmó que Secret Manager opera sobre la misma cuenta de GCP ya usada para otros servicios transversales, sin introducir un proveedor de nube adicional para monitorear ni facturar por separado.
- Versionado real ante rotación de credenciales: Se comprobó que crear una nueva versión de un secreto no sobrescribe ni elimina las versiones anteriores; ambas quedan disponibles simultáneamente con su propia fecha de creación, permitiendo conservar el historial completo de un secreto rotado.
- Registro de auditoría automático y sin configuración: Se confirmó que cada operación sobre un secreto (creación, nuevas versiones) queda registrada automáticamente con fecha y hora exacta en la pestaña de Registros, sin que el equipo tuviera que activar ni configurar manualmente ningún sistema de logging adicional.
- Sin infraestructura propia que mantener: Todo el proceso de creación, versionado y consulta se realizó desde la consola web administrada por Google, sin necesidad de desplegar, parchear ni operar ningún servidor propio.

### Consecuencias negativas

- Requiere activar facturación antes de poder usarse: Se confirmó en la práctica que habilitar la Secret Manager API exige tener la facturación del proyecto activada con una tarjeta de verificación, incluso para operar completamente dentro del nivel gratuito sin ningún cargo real.
- Dependencia adicional de un proveedor externo: Introduce otra dependencia de disponibilidad de GCP para operaciones críticas del sistema, similar al riesgo ya reconocido con Cloud Monitoring y Cloud Logging.
- Curva de configuración de permisos IAM: Esta PoC se realizó con permisos de administrador completo sobre el proyecto; requerirá configurar correctamente los roles de Google Cloud IAM para que solo los servicios autorizados de SmartLife puedan leer cada secreto, algo que exige planificación adicional frente a simplemente definir una variable de entorno.
- Costo variable ante crecimiento: Si el proyecto supera el nivel gratuito de versiones de secretos activas por mes, empieza a generar un costo por operación de acceso, aunque bajo, que debe monitorearse a medida que crezca el número de secretos gestionados.

---

## adr-0030 — Seleccionar mecanismo de instrumentación y observabilidad

**Estado:** Propuesta

### Contexto

Smartlife requiere capturar métricas, trazas y registros de actividad del backend del sistema.

### Escenarios de calidad y funcionalidades significativas referenciados

- ESC-CAL-REN-0001, ESC-CAL-CPA-0006, ESC-CAL-DIS-0007

### Decisión

A. OpenTelemetry + Google Cloud Managed Service for Prometheus: Se selecciona esta combinación como mecanismo de instrumentación y observabilidad para SmartLife. OpenTelemetry actúa como la capa de captura, un estándar abierto y neutral de proveedor que recoge métricas y trazas directamente desde Spring Boot Actuator (ya presente en el proyecto desde el ADR-0025), sin acoplar el código de negocio a librerías propietarias en el punto de instrumentación. Esos datos se envían hacia Google Cloud Managed Service for Prometheus, un servicio administrado por Google que ofrece la misma compatibilidad y lenguaje de consultas (PromQL) que un Prometheus tradicional, pero sin que el equipo tenga que instalar, desplegar ni mantener ningún servidor propio. Al estar construido sobre el mismo backend que Cloud Monitoring, los datos de métricas se visualizan directamente dentro de esa misma consola, mientras que las trazas capturadas por OpenTelemetry se exportan hacia Cloud Trace y los registros de actividad continúan gestionándose con Cloud Logging (ADR-0023) cubriendo así los tres pilares completos de observabilidad (logs, métricas y trazas) dentro de una única cuenta y consola de GCP, la misma ya usada para Secret Manager (ADR-0029).

Frente a Prometheus + Grafana autohospedados (opción B), se descarta porque exigiría que el propio equipo despliegue, configure y mantenga tanto el servidor de Prometheus como Grafana como infraestructura adicional, sumando una segunda consola distinta a la de GCP que el equipo tendría que monitorear por separado; además, un Prometheus autohospedado no incluye por defecto la retención de datos de largo plazo que sí ofrece la versión administrada de Google sin costo adicional (24 meses).

Frente a Elastic Stack / ELK (opción C), se descarta porque está orientado principalmente a la gestión y búsqueda de logs en grandes volúmenes de texto, no a métricas numéricas de series de tiempo, que es la necesidad central identificada; adoptarlo exigiría además una infraestructura considerablemente más pesada para un beneficio que Managed Service for Prometheus resuelve de forma más simple para este caso de uso.

Frente a un SaaS de observabilidad de pago como Datadog o New Relic (opción D), se descarta porque introduce un costo mensual recurrente por volumen de datos ingeridos, considerablemente mayor al costo mínimo por muestra que cobra Managed Service for Prometheus para el volumen actual del proyecto.

En conclusión, OpenTelemetry + Google Cloud Managed Service for Prometheus se considera la alternativa más adecuada para SmartLife, ya que captura métricas y trazas mediante un estándar abierto sin acoplar el código a un proveedor específico en el punto de instrumentación, evita mantener infraestructura propia de observabilidad, y centraliza los tres pilares de observabilidad dentro de la misma cuenta de GCP ya usada para los demás servicios transversales del proyecto.

### Opciones consideradas

- A.OpenTelemetry + Google Cloud Managed Service for Prometheus: Estándar abierto de captura de métricas y trazas (OpenTelemetry), combinado con la versión administrada de Prometheus de Google Cloud, integrada de forma nativa con Cloud Monitoring y Cloud Trace, sin necesidad de instalar ni mantener un servidor propio.
- B. Prometheus + Grafana autohospedados: Prometheus y Grafana desplegados y mantenidos directamente por el equipo, como servidores propios independientes de GCP.
- C. Elastic Stack (ELK): Conjunto de herramientas open source orientado principalmente a la gestión y búsqueda de logs en grandes volúmenes, con capacidades de métricas menos especializadas.
- D. SaaS de observabilidad de pago (Datadog / New Relic): Plataforma comercial de monitoreo integral, con costo mensual recurrente según volumen de datos ingeridos.

### Consecuencias positivas

- Sin infraestructura propia que mantener: Google administra por completo el almacenamiento y la escalabilidad del servicio, sin que el equipo despliegue ni parchee ningún servidor.
- Consola centralizada con los tres pilares de observabilidad: Logs (Cloud Logging), métricas (Managed Service for Prometheus dentro de Cloud Monitoring) y trazas (Cloud Trace) conviven en la misma cuenta y consola de GCP ya usada para Secret Manager.
- Retención de datos de largo plazo sin costo adicional: Conserva los datos de métricas durante 24 meses sin configuración ni pago adicional, algo que un Prometheus autohospedado no ofrece de fábrica.
- Sin acoplamiento del código a un SDK propietario: OpenTelemetry mantiene la instrumentación neutral en el punto de captura, aunque el destino final sea un servicio de Google.
- Compatible con el ecosistema estándar de Prometheus: Al usar el mismo lenguaje de consultas (PromQL), conserva la posibilidad de conectar Grafana más adelante si el proyecto lo llegara a necesitar, sin perder portabilidad.
- Costo mínimo y predecible: $0.06 USD por cada millón de muestras ingeridas, una tarifa prácticamente insignificante para el volumen de pruebas de un proyecto estudiantil.

### Consecuencias negativas

- Sin nivel gratuito real: A diferencia de Secret Manager o Cloud Logging, cobra desde la primera muestra ingerida, aunque el monto sea mínimo para el volumen actual del proyecto.
- Dependencia adicional de GCP: Aunque la captura (OpenTelemetry) es neutral, el almacenamiento y la visualización final quedan atados a la infraestructura de Google, similar a la dependencia ya reconocida con Cloud Monitoring y Secret Manager.
- Curva de configuración del exportador: Configurar correctamente el flujo desde Spring Boot Actuator hacia Managed Service for Prometheus exige una fase inicial de prueba y ajuste.

---

## adr-0031 — Seleccionar estrategia de resiliencia ante fallos en el sistema

**Estado:** Propuesta

### Contexto

Smartlife requiere disponer de patrones de diseño orientados a mantener la resiliencia de la aplicación sin comprometer todo el sistema.

### Decisión

A. Circuit breaker + Autoscaling + Bulkhead: Se propone manejar una estrategia de resiliencia enfocada en circuit breaker junto con procesos de autoscaling y el patron bulkhead ya que aborda de manera integrar los principales escenarios de fallo y sobrecarga de la aplicación, de manera que una vez detectado una anomalia desde los procesos de monitoreo proactivo de las herrmainetas de telemetria e intrumentalización, se puedan tomar acciones que limiten la propagación de fallos entre componentes (modulos), procesos de autoscaling transversales a la aplicación y aislamiento de los recursos por modulos, de manera que ante la presencia de fallos se limite el número de conexiones de cada modulo para evitar que se agoten los recursos para la aplicación entera y junto con el enfoque circuit breaker permite que ante fallas repetidas que interrumpen temporalmente las llamadas hacia algún componente del sistema, el flujo de peticiones hacia el componente se abre de manera que se evite el consumo de conexiones del sistema en peticiones que esten fallando de manera constante. Las otras alternativas no responde al requerimiento del asilamiento de fallos, ya que si bien inicialmente se tiene una arquitectura de modulos en un todo, es importante que ante la degradación del servicio (fallos) de un modulo determinado o integraciones con servicios externos, no el resto de operaciones del sistema, de manera que se evite que se sigan consumiendo recursos del sistema en primer plano sobre procesos con fallos repetidos.

### Opciones consideradas

- A. Circuit breaker + Autosacaling + Bulkhead: Contener fallos, adaptar los recursos a la demanda y aislar componentes, evitando que la sobrecarga o falla de un módulo afecte al resto de la aplicación.
- B. Health checks + timeout: Permite detectar oportunamente la indisponibilidad o degradación de los componentes y limitar el tiempo de espera de las operaciones. De esta manera, la aplicación puede evitar bloqueos prolongados y reducir el impacto de fallos en sus dependencias
- C. Queue-Based Load Leveling: Introduce una cola para desacoplar la recepción de solicitudes del procesamiento y evitar sobrecargar un componente.
- D. Graceful degradation: Mantiene el sistema operativo reduciendo temporalmente algunas capacidades no esenciales.

### Consecuencias positivas

- Aislamiento de fallos: un módulo con problemas puede ser limitado de manera aislada mediante Bulkhead, reduciendo la posibilidad de que consuma los recursos necesarios para los demás módulos.
- Prevención de fallos en cascada: Circuit Breaker evita continuar enviando solicitudes a componentes o servicios que presentan fallos repetitivos.
- Mayor disponibilidad: las funcionalidades que no dependen del componente afectado pueden continuar operando.
- Mejor gestión de picos de demanda: Autoscaling permite aumentar o reducir los recursos según la carga de la aplicación.
- Recuperación controlada: Circuit Breaker permite detectar posteriormente si la dependencia se ha recuperado y restablecer progresivamente las solicitudes.
- Uso eficiente de recursos: se evita mantener conexiones y recursos ocupados por operaciones que están fallando continuamente.

### Consecuencias negativas

- Riesgo de configuración inadecuada: límites demasiado bajos pueden generar rechazos innecesarios, mientras que límites demasiado altos pueden permitir que un módulo consuma excesivos recursos.
- Posibles interrupciones temporales: Circuit Breaker puede rechazar solicitudes mientras una dependencia está fallando, aunque esto evita un consumo innecesario de recursos.
- Mayor consumo de infraestructura: el Autoscaling puede incrementar temporalmente los costos cuando aumenta la demanda.
- Necesidad de observabilidad: se requiere monitorear métricas y eventos para determinar si los mecanismos están actuando correctamente y ajustar sus parámetros.

---

## adr-0032 — Seleccionar estrategia de ejecución de tareas en segundo plano de la aplicación

**Estado:** Propuesta

### Contexto

Smartlife requiere tener una estrategia de ejecución de tareas en segundo plano que no requieren que eñ usuario se quede bloqueado esperando una respuesa del sistema.

### Decisión

A. Worker + Transactional outbox BD: Se selecciona la estrategia Worker + Transactional Outbox con base de datos debido a que permite ejecutar tareas en segundo plano de forma asíncrona, garantizando la persistencia de los eventos y tareas pendientes. Al almacenar el evento Outbox dentro de la misma transacción que la operación de negocio, se evita que un cambio confirmado quede sin su evento correspondiente, y el worker puede procesarlo posteriormente mediante reintentos ante fallos. Frente al bus de eventos en memoria, esta alternativa ofrece mayor confiabilidad al no perder las tareas ante reinicios de la aplicación; mientras que, frente al uso de un message broker, permite alcanzar estas características sin incorporar infraestructura adicional, reduciendo la complejidad operativa y manteniendo una solución adecuada para el volumen de procesamiento esperado por la aplicación. Adicionalmente con su implemntacion de schedule permite generar eventos programados que permitan generar tareas en segundo plano, como la generacion mensual de cuotas administrativas para la unidad residencial.

### Opciones consideradas

- A. Worker + Transactional outbox BD + Schedule: Las tareas programadas son iniciadas mediante un Scheduler, que determina cuándo deben ejecutarse y puede generar los registros correspondientes en una tabla Outbox de la base de datos. Los eventos asociados a operaciones de negocio se almacenan en la Outbox dentro de la misma transacción que modifica la información de negocio, garantizando su persistencia y consistencia. Posteriormente, un Worker consulta los registros pendientes, los procesa y actualiza su estado, permitiendo implementar mecanismos de recuperación y reintento ante fallos. Esta alternativa proporciona ejecución asíncrona, persistencia y confiabilidad, aprovechando la base de datos existente sin requerir un sistema de mensajería externo.
- B. Worker + message broker: Las tareas se envían a un broker de mensajes como RabbitMQ o Kafka, donde quedan almacenadas hasta que uno o varios workers las consumen y procesan. El broker permite desacoplar productores y consumidores, distribuir la carga y gestionar grandes volúmenes de mensajes, ofreciendo mecanismos como reintentos, confirmaciones y escalabilidad horizontal.
- C. Bus de eventos en memoria: Los módulos publican eventos en un bus que los distribuye directamente a los consumidores dentro de la misma instancia de la aplicación. Los handlers pueden ejecutarse de forma asíncrona mediante un thread pool, permitiendo realizar tareas en segundo plano con baja latencia y sin infraestructura adicional.

### Consecuencias positivas

- Mayor confiabilidad: los eventos quedan persistidos en PostgreSQL y no se pierden simplemente porque la aplicación se reinicie.
- Consistencia: el evento y la operación de negocio pueden confirmarse dentro de una misma transacción.
- Recuperación ante fallos: el worker puede reintentar tareas que quedaron pendientes o fallaron durante su procesamiento.
- Desacoplamiento: la operación principal no necesita esperar a que termine la tarea en segundo plano.
- Escalabilidad: pueden utilizarse varios workers para procesar tareas concurrentemente.

### Consecuencias negativas

- Mayor complejidad de implementación: requiere gestionar la tabla Outbox, workers, estados, reintentos e idempotencia.
- Carga adicional sobre PostgreSQL: las tareas pendientes implican operaciones adicionales de lectura, escritura y actualización en la base de datos.
- Escalabilidad limitada frente a un broker especializado: PostgreSQL puede convertirse en un cuello de botella si aumenta considerablemente el volumen de eventos.
- Mantenimiento de la Outbox: es necesario implementar mecanismos para limpiar o archivar eventos procesados y evitar que la tabla crezca indefinidamente.

---

## adr-0033 — Seleccionar mecanismo de API Gateway

**Estado:** Propuesta

### Contexto

Smartlife requiere un punto de entrada único que centralice la autenticación, el control de tráfico y la publicación de la API del backend del sistema.

### Decisión

A. Google Cloud API Gateway: Se selecciona este servicio como punto de entrada de la API de SmartLife. Es un servicio administrado de Google que se configura a partir de una especificación OpenAPI, valida credenciales (claves de API o tokens JWT, incluidos los emitidos por Firebase) y aplica cuotas de consumo antes de reenviar la petición al backend, sin que el equipo despliegue ni mantenga ningún servidor. Se opera dentro de la misma cuenta de GCP que ya usan los demás servicios transversales del proyecto.

Frente a Kong Gateway (opción B), se descarta porque, aunque es de código abierto y muy completo, exigiría desplegar y mantener el gateway y su base de datos de configuración como infraestructura propia, con un costo operativo desproporcionado para el tamaño del proyecto.

Frente a Spring Cloud Gateway (opción C), se descarta porque correría como una aplicación adicional del equipo, que habría que desarrollar, desplegar y escalar, y no aporta un servicio administrado.

Frente a Azure API Management (opción D), se descarta porque pertenece a otro proveedor de nube distinto al ya adoptado, lo que obligaría a gestionar una segunda cuenta y consola, y sus niveles con más funciones tienen un costo mensual fijo.

Frente a Apache APISIX (opción E), se descarta porque, aunque es de código abierto y ofrece plugins de autenticación y control de tráfico, exigiría desplegar y mantener el gateway junto con su almacén de configuración (etcd) como infraestructura propia, un costo operativo desproporcionado para el tamaño del proyecto frente a un servicio administrado.

En conclusión, Google Cloud API Gateway es la alternativa más adecuada para SmartLife porque entrega autenticación, cuotas y publicación de la API como servicio administrado, dentro de la cuenta de GCP ya usada, con un nivel gratuito suficiente para el volumen del proyecto.

### Opciones consideradas

- A. Google Cloud API Gateway: Gateway administrado de Google Cloud, configurado mediante OpenAPI, con validación de credenciales y cuotas, sin servidores propios.
- B. Kong Gateway: Gateway de código abierto con amplio ecosistema de plugins, desplegado y mantenido por el equipo.
- C. Spring Cloud Gateway: Gateway basado en Spring, ejecutado como una aplicación Java adicional del propio proyecto.
- D. Azure API Management: Servicio administrado de gestión de APIs de Microsoft Azure, con costo fijo mensual en los niveles con más funciones.
- E. Apache APISIX: Gateway de código abierto de la Apache Software Foundation (licencia Apache 2.0), con configuración dinámica y plugins de autenticación y limitación de tráfico, desplegado y mantenido por el equipo junto con su almacén de configuración..

### Consecuencias positivas

- Sin infraestructura propia que mantener: Google administra la disponibilidad y la escalabilidad del gateway, y el equipo solo mantiene la especificación OpenAPI.
- Punto de entrada único: Los clientes consumen una sola URL estable, y el backend puede cambiar de ubicación o evolucionar sin afectarlos.
- Autenticación y cuotas antes del backend: Valida claves de API o tokens JWT y aplica límites de consumo, de modo que el tráfico no autorizado o excesivo no llega a la aplicación.
- Integración con la cuenta de GCP existente: Comparte consola, facturación e identidad con los demás servicios transversales ya adoptados.
- Nivel gratuito: Incluye un volumen mensual de llamadas sin costo, suficiente para las pruebas y la demostración del proyecto, y luego cobra por millón de llamadas.

### Consecuencias negativas

- Configuración basada en OpenAPI 2.0: La definición del gateway debe mantenerse sincronizada con la API real, y exige trabajo manual cuando esta cambia.
- Funciones limitadas frente a gateways especializados: Ofrece pocas capacidades de transformación de peticiones o respuestas y no incluye caché ni WAF propios.
- Dependencia adicional de GCP: El gateway queda atado al proveedor, y migrarlo implicaría reconstruir la configuración en otra herramienta.
- Latencia adicional: Introduce un salto de red extra entre el cliente y el backend en cada petición.

---

## adr-0034 — Seleccionar mecanismo de caché

**Estado:** Propuesta

### Contexto

Smartlife requiere almacenar temporalmente los datos de consulta frecuente y poco cambiantes del backend del sistema, de forma compartida entre sus instancias.

### Decisión

A. Redis: Se selecciona Redis como mecanismo de caché de SmartLife. Es un almacén de datos en memoria de código abierto, con una comunidad amplia y madura, que soporta expiración automática de claves (TTL) y estructuras de datos variadas. Se integra con Spring Boot mediante Spring Data Redis y la abstracción de caché de Spring (@Cacheable), por lo que la lógica de negocio no se acopla a Redis en cada punto de uso. Al ser un servicio externo a la aplicación, la caché se comparte entre instancias y sobrevive a los reinicios del backend.

Frente a Memcached (opción B), se descarta porque ofrece únicamente pares clave-valor simples, sin persistencia opcional ni estructuras de datos adicionales, y su ecosistema de integración con Spring es menos directo.

Frente a Caffeine (opción C), se descarta porque es una caché local dentro de la JVM: cada instancia del backend tendría su propia copia de los datos, con riesgo de inconsistencias entre instancias, y el contenido se perdería en cada reinicio.

Frente a Hazelcast (opción D), se descarta porque su modelo de grid de datos distribuido es más pesado de configurar y operar que lo que requiere el proyecto, y cuenta con una comunidad y una oferta de recursos gratuitos más reducidas que Redis.

En conclusión, Redis es la alternativa más adecuada para SmartLife porque combina una caché compartida y con expiración automática, una integración casi transparente con Spring Boot y el respaldo de una comunidad de código abierto muy amplia.

### Opciones consideradas

- A. Redis: Almacén en memoria de código abierto, compartido entre instancias, con expiración por TTL y estructuras de datos variadas.
- B. Memcached: Caché distribuida en memoria, de código abierto, limitada a pares clave-valor simples.
- C. Caffeine: Caché local en memoria dentro de la JVM de cada instancia, sin servidor externo.
- D. Hazelcast: Plataforma de datos distribuidos en memoria con enfoque de grid, más amplia y compleja que una caché simple.

### Consecuencias positivas

- Caché compartida entre instancias: Todas las copias del backend leen y escriben en el mismo almacén, lo que evita datos desactualizados distintos en cada instancia.
- Expiración automática de datos: El TTL por clave permite que la información caduque sola y se vuelva a consultar, sin lógica de limpieza propia.
- Integración nativa con Spring Boot: La anotación @Cacheable y Spring Data Redis permiten cachear sin acoplar el código de negocio a la tecnología.
- Código abierto con comunidad amplia: Abundante documentación, ejemplos y soporte, y puede ejecutarse localmente con Docker para desarrollo y pruebas.
- Menor carga sobre la base de datos: Las lecturas repetidas se atienden desde memoria, lo que reduce la latencia y deja la base de datos libre para operaciones transaccionales.

### Consecuencias negativas

- Componente adicional que operar: Es un servicio más que desplegar, monitorear y mantener disponible, y el backend debe tolerar su caída.
- Riesgo de datos desactualizados: Si los datos de origen cambian antes de que expire el TTL, los clientes pueden ver información antigua hasta que se invalide la clave.
- Memoria limitada en planes gratuitos: Las ofertas gratuitas de Redis administrado tienen poca capacidad, por lo que hay que decidir con cuidado qué se almacena.
- Decisión de qué cachear y por cuánto tiempo: Exige definir y ajustar estrategias de invalidación y TTL, que si se configuran mal generan inconsistencias.

---

## adr-0035 — Seleccionar mecanismo de catálogo de notificaciones

**Estado:** Propuesta

### Contexto

Smartlife requiere administrar de forma centralizada el contenido de las notificaciones enviadas a los usuarios, sin depender de cambios en el código del backend del sistema.

### Decisión

A. Strapi: Se selecciona Strapi como gestor del catálogo de notificaciones. Es un CMS headless de código abierto (licencia MIT en su edición comunitaria) que permite modelar un tipo de contenido «Notificación» con sus campos (tipo, canal, asunto, cuerpo con variables, idioma) y editarlo desde un panel de administración sin tocar el código. El backend consulta el catálogo mediante la API REST que Strapi genera automáticamente y completa las variables al momento de enviar. Se utiliza la misma instancia de Strapi ya adoptada para el catálogo de mensajes de la aplicación, con un tipo de contenido adicional, lo que evita levantar y mantener una segunda herramienta.

Frente a archivos de propiedades con Spring MessageSource (opción B), se descarta porque el contenido queda dentro del repositorio y requiere recompilar y redesplegar para cambiar un solo texto, justo la dependencia de desarrollo que se busca eliminar.

Frente a Directus (opción C), se descarta porque su licencia (BSL 1.1) impone restricciones que lo alejan del criterio de priorizar software verdaderamente de código abierto, y no aporta una ventaja que justifique mantener una herramienta distinta a la ya elegida.

Frente a Contentful (opción D), se descarta porque es un servicio propietario cuyo nivel gratuito es limitado y que genera dependencia de un proveedor externo, sin ventaja sobre una alternativa de código abierto autohospedable.

En conclusión, Strapi es la alternativa más adecuada para SmartLife porque permite editar el contenido de las notificaciones sin intervención de desarrollo ni redespliegue, es de código abierto con una comunidad amplia, y reutiliza la instancia ya adoptada para los mensajes de la aplicación.

### Opciones consideradas

- A. Strapi: CMS headless de código abierto (MIT), con panel de administración y API REST generada automáticamente.
- B. Spring MessageSource: Mecanismo de Spring que carga textos desde archivos de propiedades empaquetados con la aplicación.
- C. Directus: Plataforma headless sobre bases de datos SQL, con licencia BSL 1.1 y panel de administración.
- D. Contentful: Plataforma SaaS propietaria de gestión de contenido, con nivel gratuito limitado.

### Consecuencias positivas

- Edición sin despliegues: El contenido de las notificaciones se cambia desde el panel de administración y queda disponible de inmediato para el backend.
- Una sola herramienta de contenido: Reutilizar la instancia existente evita una segunda infraestructura y un segundo panel que aprender y mantener.
- Contenido estructurado y multilenguaje: Cada notificación se modela con campos definidos y soporta varios idiomas mediante el plugin de internacionalización.
- Código abierto y autohospedable: La licencia MIT y la comunidad activa dan libertad de uso y abundante documentación, sin costo por licencia.
- Separación de responsabilidades: El catálogo define qué se dice y el backend define cuándo y a quién se envía, lo que mantiene limpio el código de negocio.

### Consecuencias negativas

- Servicio adicional que mantener: Strapi requiere su propio despliegue y base de datos, y si no está disponible el backend no puede obtener el contenido a menos que lo respalde con caché.
- Renderizado de variables a cargo del backend: Strapi solo almacena las plantillas, por lo que el reemplazo de variables debe implementarse en el código.
- Planes gratuitos con limitaciones: Los servicios de hospedaje gratuitos suelen suspender la instancia por inactividad, lo que puede añadir latencia en la primera consulta.
- Mantenimiento de versiones: Las actualizaciones mayores de Strapi pueden requerir migración de los tipos de contenido y ajustes en la integración.

---
