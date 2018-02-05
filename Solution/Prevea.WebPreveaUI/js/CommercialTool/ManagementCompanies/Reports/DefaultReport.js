var DefaultReport = kendo.observable({

    companyId: null,
    contractualDocumentId: null,

    init: function (companyId, contractualDocumentId) {
        this.companyId = companyId;
        this.contractualDocumentId = contractualDocumentId;
    },

    goToDefaultView: function () {
        var params = {
            url: "/Companies/DefaultView",
            data: {
                contractualDocumentId: this.contractualDocumentId
            }
        };
        GeneralData.goToActionController(params);
    }
});