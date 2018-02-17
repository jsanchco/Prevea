var DetailSimulation = kendo.observable({
    tabStripDetailSimulationId: "tabStripDetailSimulation",
    spanNotificationId: "spanNotification",
    iconSimulationStateId: "iconSimulationState",
    btnSendToCompaniesId: "btnSendToCompanies",
    btnValidateId: "btnValidate",
    
    // Fields
    simulationId: null,
    simulationStateId: null,
    selectTabId: null,

    init: function (simulationId, simulationStateId, selectTabId) {
        this.simulationId = simulationId;
        this.simulationStateId = simulationStateId;
        this.selectTabId = selectTabId;

        this.createKendoWidgets();

        this.updateButtonsFromSimulationServices(false);
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

        if (GeneralData.userRoleId === Constants.role.Super ||
            GeneralData.userRoleId === Constants.role.PreveaPersonal) {
            $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + this.btnSendToCompaniesId).prop("disabled", true);

            $("#" + this.btnValidateId).show();
        } else {
            $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + this.btnSendToCompaniesId).prop("disabled", false);

            $("#" + this.btnValidateId).hide();
        }
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

    goToDetailSimulation: function (selectTabId) {
        var params = {
            url: "/CommercialTool/Simulations/DetailSimulation",
            data: {
                simulationId: this.simulationId,
                selectTabId: selectTabId
            }
        };
        GeneralData.goToActionController(params);
    },

    goToSendToCompanies: function () {
        $.ajax({
            url: "/Simulations/SendToCompanies",
            data: {
                simulationId: this.simulationId
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                switch (data.resultSimulation.Status) {
                    case Constants.resultStatus.Ok:
                        DetailSimulation.simulationStateId = Constants.simulationState.SendToCompany;
                        DetailSimulation.updateButtonsFromSimulationServices(false);
                        GeneralData.showNotification(Constants.ok, "", "success");

                        break;
                    case Constants.resultStatus.Warning:
                        DetailSimulation.simulationStateId = Constants.simulationState.SendToCompany;
                        DetailSimulation.updateButtonsFromSimulationServices(false);
                        GeneralData.showNotification(data.resultSimulation.Message, "", "warning");

                        break;
                    case Constants.resultStatus.Error:
                        GeneralData.showNotification(Constants.ko, "", "error");
                        break;
                }
            },
            error: function () {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
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
    },

    goToValidate: function() {
        $.ajax({
            url: "/Simulations/SendNotificationValidateToUser",
            data: {
                simulationId: this.simulationId
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result.Status === Constants.resultStatus.Ok) {
                    GeneralData.showNotification(Constants.ok, "", "success");

                    DetailSimulation.simulationStateId = data.result.Object.SimulationStateId;
                    DetailSimulation.createIconSimulationState();
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

    updateButtonsFromSimulationServices: function (fromServices) {
        if (GeneralData.userRoleId === Constants.role.Super ||
            GeneralData.userRoleId === Constants.role.PreveaPersonal) {

            if (fromServices === true) {
                DetailSimulation.simulationStateId = Constants.simulationState.Modificated;
            }
           
            $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + this.btnSendToCompaniesId).prop("disabled", true);

            switch (DetailSimulation.simulationStateId) {
                case Constants.simulationState.ValidationPending:
                case Constants.simulationState.Modificated:
                    $("#" + this.btnValidateId).removeAttr("disabled");
                    $("#" + this.btnValidateId).prop("disabled", false);
                    break;                
                case Constants.simulationState.Validated:
                case Constants.simulationState.SendToCompany:
                    $("#" + this.btnValidateId).removeAttr("disabled");
                    $("#" + this.btnValidateId).prop("disabled", true);
                    break;
            }
        } else {

            if (fromServices === true) {
                DetailSimulation.simulationStateId = Constants.simulationState.ValidationPending;
            }
            
            $("#" + this.btnValidateId).removeAttr("disabled");
            $("#" + this.btnValidateId).prop("disabled", true);

            switch (DetailSimulation.simulationStateId) {
                case Constants.simulationState.ValidationPending:
                    $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
                    $("#" + this.btnSendToCompaniesId).prop("disabled", true);
                    break;
                case Constants.simulationState.Modificated:
                case Constants.simulationState.Validated:                    
                    $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
                    $("#" + this.btnSendToCompaniesId).prop("disabled", false);
                    break;
                case Constants.simulationState.SendToCompany:
                    $("#" + this.btnSendToCompaniesId).removeAttr("disabled");
                    $("#" + this.btnSendToCompaniesId).prop("disabled", true);
                    break;
            }
        }

        DetailSimulation.createIconSimulationState();
    }
});