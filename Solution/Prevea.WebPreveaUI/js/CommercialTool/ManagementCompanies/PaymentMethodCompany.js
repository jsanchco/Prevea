var PaymentMethodCompany = kendo.observable({

    companyId: null,
    cmbSplitPaymentId: "cmbSplitPayment",
    cmbModePaymentId: "cmbModePayment",
    cmbModePaymentMedicalExaminationId: "cmbModePaymentMedicalExamination",
    textBoxEntityNameId: "textBoxEntityName",
    textBoxAccountNumberId: "textBoxAccountNumber",
    btnValidateId: "btnValidatePaymentMethodCompany",
    MonthsSplitPaymentId: "MonthsSplitPayment",
    SplitPaymentId: "SplitPayment",

    init: function (id) {
        this.companyId = id;

        this.setUpPage();
    },

    setUpPage: function() {
        if (GeneralData.userRoleId === Constants.role.ContactPerson) {
            $("#" + PaymentMethodCompany.cmbSplitPaymentId).removeAttr("disabled");
            $("#" + PaymentMethodCompany.cmbSplitPaymentId).prop("disabled", true);
            $("#" + PaymentMethodCompany.cmbModePaymentId).removeAttr("disabled");
            $("#" + PaymentMethodCompany.cmbModePaymentId).prop("disabled", true);
            $("#" + PaymentMethodCompany.cmbModePaymentMedicalExaminationId).removeAttr("disabled");
            $("#" + PaymentMethodCompany.cmbModePaymentMedicalExaminationId).prop("disabled", true);
            $("#" + PaymentMethodCompany.textBoxEntityNameId).removeAttr("disabled");
            $("#" + PaymentMethodCompany.textBoxEntityNameId).prop("disabled", true);
            $("#" + PaymentMethodCompany.textBoxAccountNumberId).removeAttr("disabled");
            $("#" + PaymentMethodCompany.textBoxAccountNumberId).prop("disabled", true);
            $("#" + PaymentMethodCompany.btnValidateId).removeAttr("disabled");
            $("#" + PaymentMethodCompany.btnValidateId).prop("disabled", true);
        }
    },

    onClickValidate: function () {
        var splitPayment = $("#" + this.cmbSplitPaymentId).data("kendoMultiSelect");
        var monthsSplitPayment = splitPayment.value().join(",");
        $("#" + this.MonthsSplitPaymentId).val(monthsSplitPayment);
    }
});