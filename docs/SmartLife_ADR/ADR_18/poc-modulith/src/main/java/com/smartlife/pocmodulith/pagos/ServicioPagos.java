package com.smartlife.pocmodulith.pagos;

import org.springframework.stereotype.Service;
import com.smartlife.pocmodulith.pagos.internal.CalculadoraInternaPagos;

@Service
public class ServicioPagos {

    private final CalculadoraInternaPagos calculadora = new CalculadoraInternaPagos();

    public double procesarPago(double monto) {
        double comision = calculadora.calcularComision(monto);
        return monto + comision;
    }
}