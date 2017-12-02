var AddEmployeeCompany = kendo.observable({

    textBoxFirstNameId: "textBoxFirstName",
    textBoxDNIId: "textBoxDNI",
    textPhoneNumberId: "textPhoneNumber",
    btnValidateId: "btnValidate",
    btnCancelId: "btnCancel",
    errorFromFrontId: "errorFromFront",

    companyId: null,

    init: function (companyId) {
        this.companyId = companyId;

        this.setKendoUIWidgets();
    },

    setKendoUIWidgets: function () {
        $($("#" + this.btnCancelId)).on("click", function () {
            AddEmployeeCompany.goToDetailCompany();
        });

        $($("#" + this.btnValidateId)).on("click", function (e) {
            var errors = AddEmployeeCompany.validateForm();
            AddContactPersonCompany.showErrors(errors);
            if (errors.length > 0) {
                e.preventDefault();
            }
        });
    },

    validateForm: function () {
        var error = [];

        if (!$("#" + this.textBoxFirstNameId).val()) {
            error.push("Debes añadir un Nombre");
        }

        if (!$("#" + this.textBoxDNIId).val()) {
            error.push("Debes añadir una DNI/NIF");
        }

        if (!$("#" + this.textPhoneNumberId).val()) {
            error.push("Debes añadir una Número de Teléfono");
        }

        return error;
    },

    showErrors: function (errors) {
        var divError = $("#" + AddCompany.errorFromFrontId);

        if (errors != null && errors.length > 0) {
            $("#" + AddCompany.informationId).hide();

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
    },

    goToCompanies: function () {
        var params = {
            url: "/Company/Companies"
        };
        GeneralData.goToActionController(params);
    },

    goToEmployeesCompany: function () {
        var params = {
            url: "/Company/EmployeesCompany"
        };
        GeneralData.goToActionController(params);
    },

    goToDetailCompany: function () {
        var params = {
            url: "/Company/DetailCompany",
            data: {
                id: this.companyId,
                selectTabId: 3
            }
        };
        GeneralData.goToActionController(params);
    }

});