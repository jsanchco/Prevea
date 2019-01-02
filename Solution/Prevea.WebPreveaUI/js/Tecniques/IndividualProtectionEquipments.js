var IndividualProtectionEquipments = kendo.observable({

    riskEvaluationId: null,

    editorIndividualProtectionEquipmentsId: "editorIndividualProtectionEquipments",

    init: function (riskEvaluationId) {
        kendo.culture("es-ES");

        this.riskEvaluationId = riskEvaluationId;

        this.createKendoWidgets();
    },

    createKendoWidgets: function () {
        $("#" + this.editorIndividualProtectionEquipmentsId).kendoEditor({
            resizable: {
                content: true
            },
            tools: [
                {
                }
            ]
        });
    }
});