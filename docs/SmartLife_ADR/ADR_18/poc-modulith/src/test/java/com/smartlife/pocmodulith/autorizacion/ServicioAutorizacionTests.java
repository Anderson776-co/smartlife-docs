package com.smartlife.pocmodulith.autorizacion;

import org.junit.jupiter.api.Test;
import java.time.LocalTime;
import static org.junit.jupiter.api.Assertions.*;

class ServicioAutorizacionTests {

    private final ServicioAutorizacion servicio = new ServicioAutorizacion();

    @Test
    void propietarioNoPuedeVerCuentaDeOtraVivienda() {
        System.out.println("\n=== CASO 1: Propietario intentando ver cuenta de OTRA vivienda (ReBAC) ===");
        Usuario propietario = new Usuario("u1", "PROPIETARIO", "vivienda-A");
        CuentaDeCobro cuentaDeOtro = new CuentaDeCobro("c1", "vivienda-B", 150000);

        boolean resultado = servicio.puedeConsultarCuentaDeCobro(propietario, cuentaDeOtro);

        assertFalse(resultado, "El propietario NO deberia poder ver la cuenta de otra vivienda");
        System.out.println("RESULTADO FINAL: " + (resultado ? "PERMITIDO (INCORRECTO)" : "RECHAZADO (CORRECTO)"));
    }

    @Test
    void propietarioSiPuedeVerSuPropiaCuenta() {
        System.out.println("\n=== CASO 2: Propietario viendo SU PROPIA cuenta (debe permitirse) ===");
        Usuario propietario = new Usuario("u2", "PROPIETARIO", "vivienda-A");
        CuentaDeCobro cuentaPropia = new CuentaDeCobro("c2", "vivienda-A", 150000);

        boolean resultado = servicio.puedeConsultarCuentaDeCobro(propietario, cuentaPropia);

        assertTrue(resultado, "El propietario SI deberia poder ver su propia cuenta");
        System.out.println("RESULTADO FINAL: " + (resultado ? "PERMITIDO (CORRECTO)" : "RECHAZADO (INCORRECTO)"));
    }

    @Test
    void vigilanteNoPuedeRegistrarFueraDeTurno() {
        System.out.println("\n=== CASO 3: Vigilante registrando visita FUERA de su turno (ABAC) ===");
        Usuario vigilante = new Usuario("u3", "VIGILANTE", null);
        LocalTime horaActual = LocalTime.of(23, 30);
        LocalTime inicioTurno = LocalTime.of(6, 0);
        LocalTime finTurno = LocalTime.of(14, 0);

        boolean resultado = servicio.puedeRegistrarIngresoVisita(vigilante, horaActual, inicioTurno, finTurno);

        assertFalse(resultado, "El vigilante NO deberia poder registrar fuera de su turno");
        System.out.println("RESULTADO FINAL: " + (resultado ? "PERMITIDO (INCORRECTO)" : "RECHAZADO (CORRECTO)"));
    }

    @Test
    void residenteNoPuedeConsultarCuentasDeCobro() {
        System.out.println("\n=== CASO 4: Residente intentando consultar cuenta de cobro (RBAC) ===");
        Usuario residente = new Usuario("u4", "RESIDENTE", "vivienda-A");
        CuentaDeCobro cuenta = new CuentaDeCobro("c4", "vivienda-A", 150000);

        boolean resultado = servicio.puedeConsultarCuentaDeCobro(residente, cuenta);

        assertFalse(resultado, "El residente NO deberia tener permiso general para consultar cuentas de cobro");
        System.out.println("RESULTADO FINAL: " + (resultado ? "PERMITIDO (INCORRECTO)" : "RECHAZADO (CORRECTO)"));
    }
}