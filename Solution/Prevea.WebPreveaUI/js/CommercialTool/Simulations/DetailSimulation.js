var DetailSimulation = kendo.observable({
    tabStripDetailSimulationId: "tabStripDetailSimulation",
    spanNotificationId: "spanNotification",

    // Fields
    simulationId: null,
    selectTabId: null,

    init: function (simulationId, selectTabId) {
        this.simulationId = simulationId;
        this.selectTabId = selectTabId;

        this.createKendoWidgets();
    },

    createKendoWidgets: function () {
        this.createTabStripSimulation();
    },

    createTabStripSimulation: function () {
        var tabStrip = $("#" + this.tabStripDetailSimulationId).kendoTabStrip().data("kendoTabStrip");
        tabStrip.append({
            text: "DATOS GENERALES",
            //contentUrl: kendo.format("/Company/GeneralDataCompany?simulationId={0}", this.simulationId)
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