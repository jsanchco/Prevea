var DetailRiskEvaluation = kendo.observable({

    riskEvaluationId: null,
    cnaeId: null,
    workStationId: null,
    selectTabId: null,
    notification: null,

    spanNotificationId: "spanNotification",
    tabStripDetailRiskEvaluationId: "tabStripDetailRiskEvaluation",
    
    init: function (riskEvaluationId, cnaeId, workStationId, selectTabId, notification) {
        kendo.culture("es-ES");

        this.riskEvaluationId = riskEvaluationId;
        this.cnaeId = cnaeId;
        this.workStationId = workStationId;
        this.selectTabId = selectTabId;
        this.notification = notification;
        
        this.createTabStripDetailRiskEvaluation();
    },
    
    createTabStripDetailRiskEvaluation: function () {
        if (this.notification) {
            GeneralData.showNotification(Constants.ok, "", "success");
        } else {
            $("#" + this.spanNotificationId).hide();
        }

        var tabStrip = $("#" + this.tabStripDetailRiskEvaluationId).kendoTabStrip().data("kendoTabStrip");
        tabStrip.append({
            text: "RIESGOS DETECTADOS",
            contentUrl: kendo.format("/Tecniques/RiskDetected?riskEvaluationId={0}", this.riskEvaluationId)
        });

        $("#" + this.tabStripDetailRiskEvaluationId).kendoTabStrip({
            scrollable: false
        });
 
        tabStrip = $("#" + this.tabStripDetailRiskEvaluationId).data("kendoTabStrip");
        tabStrip.select(this.selectTabId);
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
                cnaeId: DetailRiskEvaluation.cnaeId,
                workStationId: DetailRiskEvaluation.workStationId,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    }
});