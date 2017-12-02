var PaymentMethodCompany = kendo.observable({

    companyId: null,
    btnValidateId: "btnValidate",
    MonthsSplitPaymentId: "MonthsSplitPayment",
    SplitPaymentId: "SplitPayment",

    init: function (id) {
        this.companyId = id;
    },

    onClickValidate: function () {
        var splitPayment = $("#" + this.SplitPaymentId).kendoMultiSelect().data("kendoMultiSelect");
        var monthsSplitPayment = splitPayment.value().join(",");
        $("#" + this.MonthsSplitPaymentId).val(monthsSplitPayment);
    }
});