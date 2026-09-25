package com.smartlife.pocmodulith.autorizacion;

public class Usuario {
    private String id;
    private String rol;
    private String viviendaId;

    public Usuario(String id, String rol, String viviendaId) {
        this.id = id;
        this.rol = rol;
        this.viviendaId = viviendaId;
    }

    public String getId() { return id; }
    public String getRol() { return rol; }
    public String getViviendaId() { return viviendaId; }
}