var OfferReport = kendo.observable({
  
    companyId: null,

    init: function (companyId) {
        this.companyId = companyId;
    },

    goToOfferView: function () {
        var params = {
            url: "/Companies/OfferView",
            data: {
                companyId: OfferReport.companyId
            }
        };
        GeneralData.goToActionController(params);
    }
});