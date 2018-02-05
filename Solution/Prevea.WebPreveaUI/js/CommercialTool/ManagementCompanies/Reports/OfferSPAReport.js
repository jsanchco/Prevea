var OfferSPAReport = kendo.observable({

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
                contractualDocumentId: OfferSPAReport.contractualDocumentId,
                isPartialView: true
            }
        };
        GeneralData.goToActionController(params);
    }
});