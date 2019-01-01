var DetailRiskEvaluation = kendo.observable({

    riskEvaluationId: null,
    cnaeId: null,
    workStationId: null,

    tabStripDetailRiskEvaluationId: "tabStripDetailRiskEvaluation",
    
    init: function (riskEvaluationId, cnaeId, workStationId) {
        kendo.culture("es-ES");

        this.riskEvaluationId = riskEvaluationId;
        this.cnaeId = cnaeId;
        this.workStationId = workStationId;
        
        this.createTabStripDetailRiskEvaluation();
    },
    
    createTabStripDetailRiskEvaluation: function () {
        var tabStrip = $("#" + this.tabStripDetailRiskEvaluationId).kendoTabStrip().data("kendoTabStrip");
        tabStrip.append({
            text: "RIESGOS DETECTADOS",
            contentUrl: kendo.format("/Tecniques/RiskDetected?riskEvaluationId={0}", this.riskEvaluationId)
        });

        $("#" + this.tabStripDetailRiskEvaluationId).kendoTabStrip({
            scrollable: false
        });
 
        tabStrip = $("#" + this.tabStripDetailRiskEvaluationId).data("kendoTabStrip");
        tabStrip.select(0);
    },

    goToWorkStations: function () {
        var params = {
            url: "/Tecniques/WorkStations",
            data: {
                cnaeSelected: this.cnaeId
            }
        };
        GeneralData.goToActionController(params);
    },

    goToRiskEvaluation: function () {
        var params = {
            url: "/Tecniques/RiskEvaluation",
            data: {
                cnaeId: this.cnaeId,
                workStationId: this.workStationId
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDetailRiskEvaluation: function (id) {
        var params = {
            url: "/Tecniques/DetailRiskEvaluation",
            data: {
                riskEvaluationId: id,
                cnaeId: RiskEvaluation.cnaeId,
                workStationId: RiskEvaluation.workStationId
            }
        };
        GeneralData.goToActionController(params);
    }
});