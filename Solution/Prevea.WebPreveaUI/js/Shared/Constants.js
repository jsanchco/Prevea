var Constants = kendo.observable({

    ok: "Operación realizada correctamente",
    ko: "Error en la operación",

    widthProfilePhoto: 115,
    heigthProfilePhoto: 115,

    colorError: "#CC2424",
    colorWarning: "#F89406",
    colorSuccess: "#00A651",

    typeChange: {
        add: 1,
        remove: 2
    },

    resultStatus: {
        Ok: 0,
        Error: 1,
        Warning: 2
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
        Modified: 2,
        Validated: 3,
        SendToCompany: 4,
        Deleted: 5
    },

    workCenterState: {
        Alta: 1,
        Baja: 2
    },

    contractualDocumentType: {
        OfferSPA: 1, 
        OfferGES: 2, 
        OfferFOR: 3, 
        ContractSPA: 4, 
        ContractGES: 5, 
        ContractFOR: 6, 
        Annex: 7, 
        UnSubscribeContract: 8, 
        Firmed: 9
    },

    requestMedicalExaminationState: {
        Pending: 1,
        Validated: 2,
        Deleted: 3,
        Blocked: 4
    },

    requestMedicalExaminationEmployeeState: {
        Pending: 1,
        Validated: 2        
    },

    medicalExaminationState: {
        Pending: 1,
        InProcess: 2,
        Finished: 3
    },

    inputTemplateType: {
        Input: 1,
        Single: 2,
        Multiple: 3,
        DateTime: 4,
        TextArea: 5
    }
});