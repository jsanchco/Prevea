var AgencyService = kendo.observable({

    textAreaObservationsId: "textAreaObservationsAgencyService",
    btnValidateId: "btnValidateAgencyService",

    simulationId: null,
    numberEmployees: null,

    init: function (id, numberEmployees) {
        this.simulationId = id;
        this.numberEmployees = numberEmployees;

        this.setUpWidgets();
    },

    setUpWidgets: function() {
        $("#" + this.btnValidateId).removeAttr("disabled");
        $("#" + this.btnValidateId).prop("disabled", true);

        $("#" + this.textAreaObservationsId).change(function () {
            $("#" + AgencyService.btnValidateId).removeAttr("disabled");
            $("#" + AgencyService.btnValidateId).prop("disabled", false);
        });
    },

    goToAgencyService: function () {
        var params = {
            url: "/CommercialTool/Simulations/DetailSimulation",
            data: {
                simulationId: this.simulationId,
                selectTabId: 1
            }
        };
        GeneralData.goToActionController(params);
    },

    onSuccessUpdate: function (data) {
        if (data.Status === 0) {
            GeneralData.showNotification(Constants.ok, "", "success");
        }
        if (data.Status === 1) {
            GeneralData.showNotification(Constants.ko, "", "error");
        }

        AgencyService.goToAgencyService();
    }

});