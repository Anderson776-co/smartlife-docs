package com.smartlife.pocmodulith;

import org.junit.jupiter.api.Test;
import org.springframework.modulith.core.ApplicationModules;

class PocModulithApplicationTests {

    @Test
    void verificarLimitesDeModulos() {
        ApplicationModules modulos = ApplicationModules.of(PocModulithApplication.class);
        modulos.verify();
    }
}