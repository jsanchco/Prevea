var DocumentsMedicalExamination = kendo.observable({

    id: null,

    init: function (id) {
        kendo.culture("es-ES");

        this.id = id;

        this.setUpPage();
        this.createKendoWidgets();
    },

    setUpPage: function () {

    },

    createKendoWidgets: function () {

    },

    goToDetailCompany: function () {
        var params = {
            url: "/Companies/DetailCompany",
            data: {
                id: this.id,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    }
});