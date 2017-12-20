var DetailSimulation = kendo.observable({
    tabStripDetailSimulationId: "tabStripDetailSimulation",
    spanNotificationId: "spanNotification",
    iconSimulationStateId: "iconSimulationState",
    btnSendToCompaniesId: "btnSendToCompanies",
    btnSendToSEDEId: "btnSendToSEDE",
    
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

        this.updateButtons();
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
        if (this.simulationStateId === Constants.simulationState.SendToCompany) {
            html = "<div id='circleSuccess' class='pull-right'></div>";
        }
        if (this.simulationStateId === Constants.simulationState.Deleted) {
            html = "<div id='circleDeleted' class='pull-right'></div>";
        }

        $("#" + this.iconSimulationStateId).html(html);
    },

    createKendoWidgets: function () {
        this.createTabStripSimulation();
    },

    createTabStripSimulation: function () {
        var tabStrip = $("#" + this.tabStripDetailSimulationId).kendoTabStrip().data("kendoTabStrip");
        tabStrip.append({
            text: "SERVICIO de PREVENCIÓN AJENO",
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

                    $("#" + DetailSimulation.btnSendToSEDEId).removeAttr("disabled");
                    $("#" + DetailSimulation.btnSendToSEDEId).prop("disabled", true);
                }
                if (data.result.Status === Constants.resultStatus.Error) {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            },
            error: function () {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    },

    updateButtons: function () {
        if (GeneralData.userRoleId === Constants.role.Super || GeneralData.userRoleId === Constants.role.PreveaPersonal) {
            $("#" + this.btnSendToSEDEId).removeAttr("disabled");
            $("#" + this.btnSendToSEDEId).prop("disabled", true);
            $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + this.btnSendToCompaniesId).prop("disabled", true);

            return;
        }

        if (this.simulationStateId === Constants.simulationState.SendToCompany) {            
            $("#" + this.btnSendToSEDEId).removeAttr("disabled");
            $("#" + this.btnSendToSEDEId).prop("disabled", false);
            $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + this.btnSendToCompaniesId).prop("disabled", false);

            return;
        }

        if (this.simulationStateId === Constants.simulationState.Validated) {
            $("#" + this.btnSendToSEDEId).removeAttr("disabled");
            $("#" + this.btnSendToSEDEId).prop("disabled", true);
            $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + this.btnSendToCompaniesId).prop("disabled", false);

            return;
        }
    },

    updateButtonsFromSimulationServices: function () {
        if (GeneralData.userRoleId === Constants.role.Super ||
            GeneralData.userRoleId === Constants.role.PreveaPersonal) {
            $("#" + this.btnSendToSEDEId).removeAttr("disabled");
            $("#" + this.btnSendToSEDEId).prop("disabled", true);
            $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + this.btnSendToCompaniesId).prop("disabled", true);
        } else {
            switch (DetailSimulation.simulationStateId) {
                case Constants.simulationState.ValidationPending:
                    $("#" + this.btnSendToSEDEId).removeAttr("disabled");
                    $("#" + this.btnSendToSEDEId).prop("disabled", false);
                    $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
                    $("#" + this.btnSendToCompaniesId).prop("disabled", true);
                    break;
                case Constants.simulationState.Modificated:
                case Constants.simulationState.Validated:                    
                    $("#" + this.btnSendToSEDEId).removeAttr("disabled");
                    $("#" + this.btnSendToSEDEId).prop("disabled", true);
                    $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
                    $("#" + this.btnSendToCompaniesId).prop("disabled", false);
                    break;
                case Constants.simulationState.SendToCompany:
                    $("#" + this.btnSendToSEDEId).removeAttr("disabled");
                    $("#" + this.btnSendToSEDEId).prop("disabled", true);
                    $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
                    $("#" + this.btnSendToCompaniesId).prop("disabled", true);
                    break;
            }
        }
    }
});