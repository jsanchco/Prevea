﻿var AgencyService = kendo.observable({

    textTotalId: "textTotalAgencyService",
    textAreaObservationsId: "textAreaObservationsAgencyService",
    btnValidateId: "btnValidateAgencyService",

    simulationId: null,
    numberEmployees: null,

    init: function (id, numberEmployees) {
        this.simulationId = id;
        this.numberEmployees = numberEmployees;

        this.setUpWidgets();

        this.blockFields();
    },

    setUpWidgets: function() {
        $("#" + this.btnValidateId).removeAttr("disabled");
        $("#" + this.btnValidateId).prop("disabled", true);

        $("#" + this.textTotalId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: function() {
                AgencyService.updateButtonsOnChange();
            }
        });

        $("#" + this.textAreaObservationsId).change(function () {
            AgencyService.updateButtonsOnChange();
        });
    },

    goToAgencyService: function () {
        DetailSimulation.goToDetailSimulation(1);
    },

    updateButtonsOnChange: function () {
        $("#" + AgencyService.btnValidateId).removeAttr("disabled");
        $("#" + AgencyService.btnValidateId).prop("disabled", false);

        DetailSimulation.updateButtonsFromSimulationServices(true);
    },

    onSuccessUpdate: function (data) {
        if (data.Status === 0) {
            GeneralData.showNotification(Constants.ok, "", "success");
        }
        if (data.Status === 1) {
            GeneralData.showNotification(Constants.ko, "", "error");
        }

        AgencyService.goToAgencyService();
    },

    blockFields: function () {
        if (DetailSimulation.simulationStateId === Constants.simulationState.SendToCompany) {
            var numerictextbox = $("#" + AgencyService.textTotalId).data("kendoNumericTextBox");
            numerictextbox.enable(false);

            $("#" + AgencyService.textAreaObservationsId).removeAttr("disabled");
            $("#" + AgencyService.textAreaObservationsId).prop("disabled", true);

            $("#" + AgencyService.btnValidateId).removeAttr("disabled");
            $("#" + AgencyService.btnValidateId).prop("disabled", true);
        }
    }
});