# Prueba de Concepto — ADR-0001

## Objetivo

Validar si un usuario puede autenticarse con sus credenciales de acceso y genere un token temporal que permita el acceso al sistema del usuario.

## Métricas y resultados

Se registró un usuario de prueba mediante Firebase Authentication (createUserWithEmailAndPassword) y se inició sesión (signInWithEmailAndPassword), obteniendo un token JWT válido. Con ese token, la petición GET /api/perfil devolvió 200 OK junto con el uid y el email del usuario. Se probaron dos casos de rechazo en Postman: una petición sin el header Authorization devolvió 401 Unauthorized, y la misma petición con un carácter alterado dentro del token también devolvió 401 Unauthorized. Al cerrar sesión desde el cliente, el backend ejecutó RevokeRefreshTokensAsync sobre el uid, y el mismo token que antes daba 200 OK, probado de nuevo sin generar uno nuevo, pasó a devolver 401 Unauthorized. Durante la implementación se identificaron y corrigieron tres fallas reales: la validación JWT no comprobaba revocación (se agregó una verificación explícita con VerifyIdTokenAsync(token, checkRevoked: true) dentro de OnTokenValidated), faltaba la política de CORS para que el cliente, servido en otro puerto, pudiera llamar al backend, y la ruta de logout invocada desde el cliente no coincidía con la ruta real expuesta por el controlador.

## Conclusiones

La PoC valida los cinco criterios de autenticación exigidos: registro de usuario, emisión de un token válido que autoriza el acceso, rechazo de peticiones sin token, rechazo de tokens alterados, y revocación efectiva del token al cerrar sesión. Este último punto no funcionaba en la implementación inicial: revocar el refresh token en Firebase no invalida por sí solo un token ya emitido, ya que la validación estándar del middleware solo revisa firma, emisor y expiración, sin consultar el estado de revocación. Fue necesario agregar una verificación adicional contra Firebase en cada petición para que una sesión cerrada quedara realmente sin acceso, en vez de seguir siendo válida hasta su expiración natural.
