const { createRxDatabase } = require("rxdb");
const { getRxStorageMemory } = require("rxdb/plugins/storage-memory");

async function manejarConflictoVisitas(i, context) {
  console.log("\n[conflictHandler] Conflicto detectado para el documento: " + i.newDocumentState.id + " (contexto: " + context + ")");
  console.log("  Estado que intentaba escribir este cliente (fork): " + i.newDocumentState.estado + " - " + i.newDocumentState.observaciones);
  console.log("  Estado real actual en el servidor (master): " + i.realMasterState.estado + " - " + i.realMasterState.observaciones);

  if (i.realMasterState.estado === "ingresado") {
    console.log("  REGLA DE NEGOCIO: un ingreso ya confirmado en el servidor no puede revertirse por una edicion desactualizada.");
    console.log("  RESOLUCION: se conserva el estado del servidor (ingresado).");
    return { isEqual: false, documentData: i.realMasterState };
  }

  console.log("  RESOLUCION: el servidor no tenia un ingreso confirmado, se acepta el cambio local.");
  return { isEqual: false, documentData: i.newDocumentState };
}

async function main() {
  console.log("=== INICIANDO PoC DE RxDB (ADR-0012) ===\n");

  const db = await createRxDatabase({
    name: "smartlife_visitas_db",
    storage: getRxStorageMemory(),
    multiInstance: false
  });

  const schema = {
    version: 0,
    primaryKey: "id",
    type: "object",
    properties: {
      id: { type: "string", maxLength: 100 },
      nombre: { type: "string" },
      estado: { type: "string" },
      observaciones: { type: "string" },
      updatedAt: { type: "number" }
    },
    required: ["id", "nombre", "estado"]
  };

  await db.addCollections({
    visitas: {
      schema: schema,
      conflictHandler: manejarConflictoVisitas
    }
  });

  const visitas = db.collections.visitas;

  console.log("--- Paso 1: Persistencia y reactividad ---");
  visitas.find().$.subscribe(function (docs) {
    console.log("[REACTIVO] La coleccion local ahora tiene " + docs.length + " documento(s)");
  });

  const estadoOriginal = {
    id: "visita-1",
    nombre: "Juan Perez",
    estado: "pendiente",
    observaciones: "",
    updatedAt: Date.now()
  };

  await visitas.insert(estadoOriginal);
  await new Promise(function (r) { setTimeout(r, 300); });

  console.log("\n--- Paso 2: Simulando dos vigilantes editando el mismo registro sin conexion ---");
  const cambioVigilanteA = Object.assign({}, estadoOriginal, {
    estado: "ingresado",
    observaciones: "Ingreso autorizado por porteria A",
    updatedAt: Date.now() + 1000
  });
  const cambioVigilanteB = Object.assign({}, estadoOriginal, {
    estado: "rechazado",
    observaciones: "Visitante no aparece en la lista, B lo rechaza",
    updatedAt: Date.now() + 1000
  });

  console.log("Vigilante A (offline) cambia el estado a: " + cambioVigilanteA.estado);
  console.log("Vigilante B (offline) cambia el estado a: " + cambioVigilanteB.estado);

  console.log("\n--- Paso 3: Vigilante A recupera conexion y envia su cambio (sin conflicto) ---");
  const resultadoA = await manejarConflictoVisitas({
    newDocumentState: cambioVigilanteA,
    realMasterState: estadoOriginal,
    assumedMasterState: estadoOriginal
  }, "push-vigilante-A");
  console.log("Servidor actualizado a: " + resultadoA.documentData.estado + " - " + resultadoA.documentData.observaciones);
  const servidorActual = resultadoA.documentData;

  console.log("\n--- Paso 4: Vigilante B recupera conexion y envia su cambio (CONFLICTO real) ---");
  const resultadoB = await manejarConflictoVisitas({
    newDocumentState: cambioVigilanteB,
    realMasterState: servidorActual,
    assumedMasterState: estadoOriginal
  }, "push-vigilante-B");

  console.log("\n=== RESULTADO FINAL EN EL SERVIDOR ===");
  console.log("Estado: " + resultadoB.documentData.estado);
  console.log("Observaciones: " + resultadoB.documentData.observaciones);

  await db.remove();
  console.log("\n=== PoC FINALIZADA ===");
}

main().catch(function (err) {
  console.error("ERROR:", err);
});
