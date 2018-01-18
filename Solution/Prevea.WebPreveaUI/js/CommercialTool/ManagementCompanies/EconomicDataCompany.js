var EconomicDataCompany = kendo.observable({

    companyId: null,

    init: function (id) {
        kendo.culture("es-ES");

        this.companyId = id;

        this.setKendoUIWidgets();
    },

    setKendoUIWidgets: function () {
        
    }

});