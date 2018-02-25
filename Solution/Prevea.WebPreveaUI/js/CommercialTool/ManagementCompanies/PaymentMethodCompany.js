var PaymentMethodCompany = kendo.observable({

    companyId: null,
    btnValidateId: "btnValidatePaymentMethodCompany",
    MonthsSplitPaymentId: "MonthsSplitPayment",
    SplitPaymentId: "SplitPayment",

    init: function (id) {
        this.companyId = id;

        this.setUpPage();
    },

    setUpPage: function() {
        if (GeneralData.userRoleId === Constants.role.ContactPerson) {
            $("#" + PaymentMethodCompany.btnValidateId).removeAttr("disabled");
            $("#" + PaymentMethodCompany.btnValidateId).prop("disabled", true);
        }
    },

    onClickValidate: function () {
        var splitPayment = $("#" + this.SplitPaymentId).kendoMultiSelect().data("kendoMultiSelect");
        var monthsSplitPayment = splitPayment.value().join(",");
        $("#" + this.MonthsSplitPaymentId).val(monthsSplitPayment);
    }
});