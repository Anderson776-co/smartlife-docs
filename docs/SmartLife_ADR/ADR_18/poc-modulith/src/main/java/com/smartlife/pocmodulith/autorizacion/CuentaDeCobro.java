package com.smartlife.pocmodulith.autorizacion;

public class CuentaDeCobro {
    private String id;
    private String viviendaId;
    private double monto;

    public CuentaDeCobro(String id, String viviendaId, double monto) {
        this.id = id;
        this.viviendaId = viviendaId;
        this.monto = monto;
    }

    public String getId() { return id; }
    public String getViviendaId() { return viviendaId; }
    public double getMonto() { return monto; }
}