var HistoricMedicalExaminationController = kendo.observable({

    companyId: null,

    init: function (companyId) {
        this.companyId = companyId;
    },

    goToHistoric: function() {
        var params = {
            url: "/HistoricMedicalExamination/HistoricMedicalExamination",
            data: {}
        };
        GeneralData.goToActionController(params);
    }
});