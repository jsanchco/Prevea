var GeneralDataCompany = kendo.observable({

    textBoxNameId: "textBoxName",
    textBoxNIFId: "textBoxNIF",
    textBoxActivityId: "textBoxActivity",
    textBoxAddressId: "textBoxAddress",
    textBoxProvinceId: "textBoxProvince",
    btnValidateId: "btnValidate",
    errorFromFrontId: "errorFromFront",

    init: function () {
        this.setKendoUIWidgets();
    },

    setKendoUIWidgets: function () {
        $($("#" + this.btnValidateId)).on("click", function (e) {
            var errors = GeneralDataCompany.validateForm();
            GeneralDataCompany.showErrors(errors);
            if (errors.length > 0) {
                e.preventDefault();
            }
        });
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