var CollectiveProtectionEquipments = kendo.observable({

    riskEvaluationId: null,

    editorIndividualProtectionEquipmentsId: "editorCollectiveProtectionEquipments",

    init: function (riskEvaluationId) {
        kendo.culture("es-ES");

        this.riskEvaluationId = riskEvaluationId;

        this.createKendoWidgets();
    },

    createKendoWidgets: function () {
        $("#" + this.editorCollectiveProtectionEquipmentsId).kendoEditor({
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