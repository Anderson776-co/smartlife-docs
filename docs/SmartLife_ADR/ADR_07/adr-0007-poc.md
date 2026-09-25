# Prueba de Concepto — ADR-0007

## Objetivo

Validar el envio real de correos electronicos transaccionales para los comunicados de SmartLife,

## Métricas y resultados

El envio se realizo mediante la API REST de Resend (POST /emails) desde una aplicacion de consola en .NET 8, obteniendo un ID de mensaje valido en la respuesta. El correo se recibio de forma real en la bandeja de entrada de Gmail del destinatario, y el panel de Resend (seccion Emails) registro el envio con estado Delivered. Se identifico una limitacion real: sin un dominio propio verificado, el envio solo es posible desde el remitente compartido onboarding@resend.dev hacia el mismo correo con el que se creo la cuenta de Resend; el intento de enviar a un destinatario distinto responde con error 403.

## Conclusiones

Resend demuestra un flujo de envio de correo funcional de extremo a extremo (API, entrega real, panel de seguimiento), pero no alcanza a validar el escenario real de SmartLife de enviar un comunicado a multiples residentes distintos, ya que eso requeriria verificar un dominio propio. Se recomienda dejar registrada esta sustitucion frente a la decision original de OneSignal en el ADR-6, aclarando que el motivo fue un bloqueo externo de verificacion de dominio (ajeno a OneSignal como herramienta) y no una limitacion tecnica de la plataforma en si.
