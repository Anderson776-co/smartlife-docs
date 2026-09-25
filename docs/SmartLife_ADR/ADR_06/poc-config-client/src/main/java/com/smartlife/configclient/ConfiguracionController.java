package com.smartlife.configclient;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.cloud.context.config.annotation.RefreshScope;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RefreshScope
public class ConfiguracionController {

    @Value("${limite.reintentos.pago}")
    private String limiteReintentosPago;

    @GetMapping("/config")
    public String verConfiguracion() {
        return "Limite de reintentos de pago actual: " + limiteReintentosPago;
    }
}