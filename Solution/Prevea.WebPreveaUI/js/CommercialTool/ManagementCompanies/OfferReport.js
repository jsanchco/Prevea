var OfferReport = kendo.observable({

    companyId: null,
    contractualDocumentId: null,

    init: function (companyId, contractualDocumentId) {
        this.companyId = companyId;
        this.contractualDocumentId = contractualDocumentId;
    },

    goToOfferView: function () {
        var params = {
            url: "/Companies/OfferView",
            data: {
                contractualDocumentId: this.contractualDocumentId
            }
        };
        GeneralData.goToActionController(params);
    }
});