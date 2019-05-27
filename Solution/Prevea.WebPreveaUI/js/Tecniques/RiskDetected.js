var RiskDetected = kendo.observable({

    riskEvaluationId: null,

    init: function (riskEvaluationId) {
        kendo.culture("es-ES");

        this.riskEvaluationId = riskEvaluationId;
    }
});