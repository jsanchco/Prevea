var Constants = kendo.observable({

    ok: "Operación realizada correctamente",
    ko: "Error en la operación",

    colorError: "#CC2424",
    colorWarning: "#F89406",
    colorSuccess: "#00A651",

    resultStatus: {
        Ok: 0,
        Error: 1
    },

    role: {
        Super: 1,
        Admin: 2,
        Library: 3,
        ContactPerson: 4,
        Employee: 5,
        Agency: 6,
        Doctor: 7,
        Manager: 8,
        PreveaPersonal: 9,
        PreveaCommercial: 10,
        ExternalPersonal: 11
    },

    simulationState: {
        ValidationPending: 1,
        Modificated: 2,
        Validated: 3,
        SedToCompany: 4
    }

});