var DetailSimulation = kendo.observable({
    tabStripDetailSimulationId: "tabStripDetailSimulation",
    spanNotificationId: "spanNotification",
    iconSimulationStateId: "iconSimulationState",
    
    // Fields
    simulationId: null,
    simulationStateId: null,
    selectTabId: null,

    init: function (simulationId, simulationStateId, selectTabId) {
        this.simulationId = simulationId;
        this.simulationStateId = simulationStateId;
        this.selectTabId = selectTabId;

        this.createIconSimulationState();
        this.createKendoWidgets();
    },

    createIconSimulationState: function () {
        var html = "";
        if (this.simulationStateId === 1) {
            html = "<div id='circleError' class='pull-right'></div>";
        }
        if (this.simulationStateId === 2) {
            html = "<div id='circleWarning' class='pull-right'></div>";
        }
        if (this.simulationStateId === 3) {
            html = "<div id='circleSuccess' class='pull-right'></div>";
        }

        $("#" + this.iconSimulationStateId).html(html);
    },

    createKendoWidgets: function () {
        this.createTabStripSimulation();
    },

    createTabStripSimulation: function () {
        var tabStrip = $("#" + this.tabStripDetailSimulationId).kendoTabStrip().data("kendoTabStrip");
        tabStrip.append({
            text: "SERVICIO de PREVENCIÓN AGENA",
            contentUrl: kendo.format("/CommercialTool/Simulations/ForeignPreventionService?simulationId={0}", this.simulationId)
        });
        tabStrip.append({
            text: "GESTORÍA",
            contentUrl: kendo.format("/CommercialTool/Simulations/AgencyService?simulationId={0}", this.simulationId)
        });
        tabStrip.append({
            text: "FORMACIÓN",
            contentUrl: kendo.format("/CommercialTool/Simulations/TrainingService?simulationId={0}", this.simulationId)
        });

        tabStrip = $("#" + this.tabStripDetailSimulationId).data("kendoTabStrip");

        tabStrip.bind("activate",
            function() {
            });

        tabStrip.select(this.selectTabId);
    },

    goToSimulations: function () {
        var params = {
            url: "/CommercialTool/Simulations/Simulations",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToDetailSimulation: function () {
        var params = {
            url: "/CommercialTool/Simulations/DetailSimulation",
            data: {
                simulationId: this.simulationId,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    }

});