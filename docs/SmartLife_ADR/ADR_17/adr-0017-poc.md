# Prueba de Concepto — ADR-0017

## Objetivo

Validar que una Progressive Web App con Service Worker permite que la aplicación cachee su App Shell y continúe funcionando correctamente ante la pérdida total de conectividad a internet, requisito crítico para que el vigilante pueda seguir operando el módulo de visitas durante un corte de conexión en portería.

## Métricas y resultados

Se construyó una PWA mínima (index.html, sw.js, manifest.json) servida localmente mediante http-server en Node.js. Al cargar la página, el Service Worker se registró correctamente (confirmado en consola: "[SW] Instalando y cacheando App Shell..." y "Service Worker registrado correctamente"), almacenando en Cache Storage (bucket smartlife-poc-v1) un total de 3 entradas: /, /index.html y /manifest.json. Al simular la pérdida de conexión mediante la opción "Offline" en las herramientas de desarrollador del navegador y recargar la página, esta cargó exitosamente mostrando su contenido completo, con el estado dinámico confirmando correctamente "SIN CONEXIÓN (funcionando en modo offline)" en lugar del error estándar de "sin conexión a internet". La pestaña Network confirmó a nivel técnico que las peticiones a 127.0.0.1 y manifest.json retornaron Status 200 con la columna "Fulfilled by" indicando "(ServiceWorker)", evidenciando que ninguna de esas solicitudes intentó salir a la red: fueron interceptadas y respondidas directamente por el Service Worker desde la caché local.

## Conclusiones

La PoC valida el criterio central de la decisión: el Service Worker efectivamente permite que la PWA continúe funcionando y respondiendo peticiones incluso con conectividad completamente nula, confirmando la viabilidad técnica del requisito crítico que motivó revertir la estrategia de Responsive Web Design de vuelta a PWA (que el vigilante pueda seguir consultando autorizaciones y registrando ingresos durante una caída de conectividad en portería). Esta prueba valida específicamente la capa de App Shell/caché estática; queda fuera de su alcance la validación de las capas adicionales que dependen de esta base técnica de almacenamiento local de datos (IndexedDB), cola de acciones offline y sincronización en segundo plano (Background Sync) ,que corresponden al ADR-0011 y requieren una PoC independiente y complementaria a esta.
