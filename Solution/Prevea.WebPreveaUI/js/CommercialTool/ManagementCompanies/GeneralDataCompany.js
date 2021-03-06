﻿var GeneralDataCompany = kendo.observable({

    textBoxNameId: "textBoxName",
    textBoxNIFId: "textBoxNIF",
    textBoxActivityId: "textBoxActivity",
    textBoxAddressId: "textBoxAddress",
    textBoxProvinceId: "textBoxProvince",
    cmbCnae: "CnaeId",
    textEmployeesNumberId: "textEmployeesNumber",
    btnValidateId: "btnValidate",
    errorFromFrontId: "errorFromFront",

    init: function () {
        this.setKendoUIWidgets();
    },

    setKendoUIWidgets: function () {
        $("#" + this.textEmployeesNumberId).kendoNumericTextBox({
            decimals: 0,
            format: "n0"
        });

        $($("#" + this.btnValidateId)).on("click", function (e) {
            var errors = GeneralDataCompany.validateForm();
            GeneralDataCompany.showErrors(errors);
            if (errors.length > 0) {
                e.preventDefault();
            }
        });

        if (GeneralData.userRoleId === Constants.role.ContactPerson) {
            $("#" + this.textBoxNIFId).removeAttr("disabled");
            $("#" + this.textBoxNIFId).prop("disabled", true);
            $("#" + this.cmbCnae).removeAttr("disabled");
            $("#" + this.cmbCnae).prop("disabled", true);

            var numerictextbox = $("#" + this.textEmployeesNumberId).data("kendoNumericTextBox");
            numerictextbox.enable(false);
        }

        if (GeneralData.userRoleId === Constants.role.Super) {
            $("#" + this.btnValidateId).removeAttr("disabled");
            $("#" + this.btnValidateId).prop("disabled", true);
        }
    },

    validateForm: function () {
        var error = [];

        if (!$("#" + this.textBoxNameId).val()) {
            error.push("Debes añadir una Razón Social");
        }

        if (!$("#" + this.textBoxNIFId).val()) {
            error.push("Debes añadir una NIF/CIF");
        }

        if (!$("#" + this.textBoxAddressId).val()) {
            error.push("Debes añadir una Dirección");
        }

        if (!$("#" + this.textBoxProvinceId).val()) {
            error.push("Debes añadir una Provincia");
        }

        if (parseInt($("#" + this.cmbCnae).data("kendoDropDownList").value()) === 0) {
            error.push("Debes añadir una Actividad");
        }

        return error;
    },

    showErrors: function (errors) {
        var divError = $("#" + GeneralDataCompany.errorFromFrontId);

        if (errors != null && errors.length > 0) {
            var html = "<button type='button' class='close' data-dismiss='alert'>&times;</button>";
            html += "<ul>";
            $.each(errors,
                function (index, value) {
                    html += kendo.format("<li>{0}</li>", value);
                });
            html += "</ul>";

            divError.html(html);
            divError.show();
        } else {
            divError.hide();
        }
    }

});