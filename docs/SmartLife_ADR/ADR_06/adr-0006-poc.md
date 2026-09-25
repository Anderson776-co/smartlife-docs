# Prueba de Concepto — ADR-0006

## Objetivo

Validar que la estrategia de Reintento con Backoff exponencial + Jitter respeta los tiempos de espera definidos (1s, 2s, 4s, 8s, 16s con variación de ±20%), evita reintentos sincronizados mediante la aleatoriedad del jitter, y respeta el límite máximo de 5 intentos antes de declarar un fallo permanente.

## Métricas y resultados

Se implementó en Node.js una función que simula el llamado a la pasarela de pagos (Wompi), aplicando la política de backoff exponencial con jitter de ±20% sobre cada intervalo base. Se ejecutaron dos escenarios: (1) Éxito en el último intento: los intentos 1 a 4 fallaron con esperas de 805 ms, 2136 ms, 4529 ms y 7323 ms respectivamente (sobre bases de 1000, 2000, 4000 y 8000 ms), y el intento 5 tuvo éxito a los 14.825 ms desde el inicio del proceso. (2) Fallo permanente: los 5 intentos fallaron consecutivamente, con esperas de 1009 ms, 2080 ms, 3366 ms y 9255 ms, declarando el fallo permanente a los 15.745 ms desde el inicio, notificando al usuario en lugar de continuar reintentando indefinidamente.

## Conclusiones

La PoC valida los criterios centrales de la decisión: cada intervalo de espera varió realmente dentro del rango de ±20% sobre su valor base (confirmado en ambas direcciones, por ejemplo, el intento 4 del primer escenario esperó 7323 ms en lugar de 8000 ms, y el intento 3 del segundo escenario esperó 3366 ms en lugar de 4000 ms), demostrando que el jitter efectivamente introduce aleatoriedad real y no un valor fijo, lo cual evita que múltiples usuarios afectados por la misma caída del proveedor reintenten en el mismo instante exacto. El sistema respetó el límite máximo de 5 intentos en ambos escenarios, deteniéndose correctamente y notificando el fallo permanente en el segundo caso en lugar de reintentar indefinidamente. Los tiempos totales observados (14.8 s y 15.7 s) se mantuvieron por debajo del máximo teórico de 31 segundos mencionado en el ADR, dentro del comportamiento esperado dado el rango de variación del jitter. Queda fuera del alcance de esta prueba validar el comportamiento bajo múltiples solicitudes concurrentes reales (para confirmar que efectivamente se evitan picos de carga sincronizados sobre el proveedor a escala), así como la integración real con la idempotency key de Wompi para prevenir cobros duplicados ante un reintento exitoso después de un fallo de red que sí llegó a procesarse del lado del proveedor.
