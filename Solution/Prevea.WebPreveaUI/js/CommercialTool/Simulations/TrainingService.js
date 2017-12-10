var TrainingService = kendo.observable({

    textAreaObservationsId: "textAreaObservationsTrainingService",
    btnValidateId: "btnValidateTrainingService",

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
            $("#" + TrainingService.btnValidateId).removeAttr("disabled");
            $("#" + TrainingService.btnValidateId).prop("disabled", false);
        });
    },

    goToTrainingService: function () {
        var params = {
            url: "/CommercialTool/Simulations/DetailSimulation",
            data: {
                simulationId: this.simulationId,
                selectTabId: 2
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

        TrainingService.goToTrainingService();
    }

});