var RiskDetected = kendo.observable({

    riskEvaluationId: null,

    editorRiskDetectedId: "editorRiskDetected",

    init: function (riskEvaluationId) {
        kendo.culture("es-ES");

        this.riskEvaluationId = riskEvaluationId;

        //this.createKendoWidgets();
    },

    createKendoWidgets: function () {
        $("#" + this.editorRiskDetectedId).kendoEditor({
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