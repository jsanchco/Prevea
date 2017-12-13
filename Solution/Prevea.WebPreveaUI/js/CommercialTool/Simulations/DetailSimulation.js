var DetailSimulation = kendo.observable({
    tabStripDetailSimulationId: "tabStripDetailSimulation",
    spanNotificationId: "spanNotification",
    iconSimulationStateId: "iconSimulationState",
    btnSendToCompaniesId: "btnSendToCompanies",
    btnValidatedBySEDEId: "btnValidatedBySEDE",
    
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

        if (GeneralData.userRoleId === Constants.role.PreveaCommercial &&
            this.simulationStateId === Constants.simulationState.Modificated) {
            $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + this.btnSendToCompaniesId).prop("disabled", false);
        } else {
            $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + this.btnSendToCompaniesId).prop("disabled", true);
        }

        if (GeneralData.userRoleId === Constants.role.PreveaCommercial) {
            $("#" + this.btnValidatedBySEDEId).show();
        } else {
            $("#" + this.btnValidatedBySEDEId).hide();
        }

        if (this.simulationStateId === Constants.simulationState.Validated ||
            this.simulationStateId === Constants.simulationState.SedToCompany) {
            $("#" + this.btnValidatedBySEDEId).removeAttr("disabled");
            $("#" + this.btnValidatedBySEDEId).prop("disabled", true);
        } else {
            $("#" + this.btnValidatedBySEDEId).removeAttr("disabled");
            $("#" + this.btnValidatedBySEDEId).prop("disabled", false);
        }
    },

    createIconSimulationState: function () {
        var html = "";
        if (this.simulationStateId === Constants.simulationState.ValidationPending) {
            html = "<div id='circleError' class='pull-right'></div>";
        }
        if (this.simulationStateId === Constants.simulationState.Modificated) {
            html = "<div id='circleWarning' class='pull-right'></div>";
        }
        if (this.simulationStateId === Constants.simulationState.Validated) {
            html = "<div id='circleSuccess' class='pull-right'></div>";
        }
        if (this.simulationStateId === Constants.simulationState.SedToCompany) {
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
    },

    goToSendToSEDE: function () {
        $.ajax({
            url: "/Simulations/SendToSEDE",
            data: {
                simulationId: DetailSimulation.simulationId
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result.Status === Constants.resultStatus.Ok) {
                    GeneralData.showNotification("Enviada Notificación a SEDE", "", "success");
                }
                if (data.result.Status === Constants.resultStatus.Error) {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            },
            error: function () {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    }

});