const CACHE_NAME = 'smartlife-poc-v1';
const ARCHIVOS_A_CACHEAR = [
  './',
  './index.html',
  './manifest.json'
];

self.addEventListener('install', (event) => {
  console.log('[SW] Instalando y cacheando App Shell...');
  event.waitUntil(
    caches.open(CACHE_NAME).then((cache) => {
      return cache.addAll(ARCHIVOS_A_CACHEAR);
    })
  );
});

self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.match(event.request).then((respuestaCacheada) => {
      return respuestaCacheada || fetch(event.request);
    })
  );
});
