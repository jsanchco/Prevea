var AgencyCompany = kendo.observable({
    textBoxNameId: "textBoxName",
    btnValidateId: "btnValidate",
    errorFromFrontId: "errorFromFront",

    init: function () {
        this.setKendoUIWidgets();
    },

    setKendoUIWidgets: function () {
        $($("#" + this.btnValidateId)).on("click", function (e) {
            var errors = AgencyCompany.validateForm();
            AgencyCompany.showErrors(errors);
            if (errors.length > 0) {
                e.preventDefault();
            }
        });
    },

    validateForm: function () {
        var error = [];

        if (!$("#" + this.textBoxNameId).val()) {
            error.push("Debes añadir un Nombre a la Gestoría");
        }

        return error;
    },

    showErrors: function (errors) {
        var divError = $("#" + AgencyCompany.errorFromFrontId);

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