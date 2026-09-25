package com.smartlife.pocmodulith.autorizacion;

import java.time.LocalTime;

public class ServicioAutorizacion {

    public boolean puedeConsultarCuentaDeCobro(Usuario usuario, CuentaDeCobro cuenta) {
        if (!usuario.getRol().equals("PROPIETARIO") && !usuario.getRol().equals("ADMINISTRADOR")) {
            System.out.println("[RBAC] RECHAZADO: el rol '" + usuario.getRol() + "' no tiene permiso general para consultar cuentas de cobro.");
            return false;
        }
        System.out.println("[RBAC] Permitido: el rol '" + usuario.getRol() + "' tiene acceso general a cuentas de cobro.");

        if (usuario.getRol().equals("PROPIETARIO") && !usuario.getViviendaId().equals(cuenta.getViviendaId())) {
            System.out.println("[ReBAC] RECHAZADO: la cuenta de cobro pertenece a la vivienda '" + cuenta.getViviendaId() + "', pero el usuario pertenece a la vivienda '" + usuario.getViviendaId() + "'.");
            return false;
        }
        System.out.println("[ReBAC] Permitido: la cuenta de cobro pertenece a la vivienda del usuario.");

        return true;
    }

    public boolean puedeRegistrarIngresoVisita(Usuario usuario, LocalTime horaActual, LocalTime inicioTurno, LocalTime finTurno) {
        if (!usuario.getRol().equals("VIGILANTE")) {
            System.out.println("[RBAC] RECHAZADO: el rol '" + usuario.getRol() + "' no tiene permiso para registrar ingresos de visitantes.");
            return false;
        }
        System.out.println("[RBAC] Permitido: el rol 'VIGILANTE' tiene acceso general a registrar visitas.");

        if (horaActual.isBefore(inicioTurno) || horaActual.isAfter(finTurno)) {
            System.out.println("[ABAC] RECHAZADO: la hora actual (" + horaActual + ") esta fuera del turno asignado (" + inicioTurno + " a " + finTurno + ").");
            return false;
        }
        System.out.println("[ABAC] Permitido: la hora actual esta dentro del turno asignado.");

        return true;
    }
}