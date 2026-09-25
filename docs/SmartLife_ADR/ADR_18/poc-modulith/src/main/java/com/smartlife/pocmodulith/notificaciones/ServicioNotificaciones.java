package com.smartlife.pocmodulith.notificaciones;

import org.springframework.stereotype.Service;
import com.smartlife.pocmodulith.pagos.internal.CalculadoraInternaPagos;

@Service
public class ServicioNotificaciones {

    public void notificarComision(double monto) {
        CalculadoraInternaPagos calculadora = new CalculadoraInternaPagos();
        double comision = calculadora.calcularComision(monto);
        System.out.println("La comision calculada es: " + comision);
    }
}