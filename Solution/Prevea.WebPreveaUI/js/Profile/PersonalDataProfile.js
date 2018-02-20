var PersonalDataProfile = kendo.observable({
    spInitialsId: "spInitials",
    errorFromFrontId: "errorFromFront",

    textBoxNameId: "textBoxName",
    textBoxLastNameId: "textBoxLastName",
    textBoxDNIId: "textBoxDNI",
    dtpBirthDateId: "dtpBirthDate",
    textBoxEmailId: "textBoxEmail",
    textBoxNickId: "textBoxNick",
    textBoxWorkStationId: "textBoxWorkStation",
    textBoxProfessionalCategoryId: "textBoxProfessionalCategory",

    btnValidateId: "btnValidate",
    btnChangePasswordId: "btnChangePassword",

    changePasswordId: "changePassword",
    changePasswordWindowId: null,

    userId: null,

    init: function (userId) {
        kendo.culture("es-ES");

        this.userId = userId;

        this.setUpWidgets();
    },

    setUpWidgets: function () {
        $("#" + this.textBoxNickId).removeAttr("disabled");
        $("#" + this.textBoxNickId).prop("disabled", true);

        if (GeneralData.userRoleId === Constants.role.Employee) {
            $("#" + this.textBoxWorkStationId).removeAttr("disabled");
            $("#" + this.textBoxWorkStationId).prop("disabled", true);

            $("#" + this.textBoxProfessionalCategoryId).removeAttr("disabled");
            $("#" + this.textBoxProfessionalCategoryId).prop("disabled", true);
        }

        $("#" + this.textBoxNameId).change(function() {
            PersonalDataProfile.setInitials();
        });

        $("#" + this.textBoxLastNameId).change(function () {
            PersonalDataProfile.setInitials();
        });

        $("#" + this.textBoxDNIId).change(function () {
            if (GeneralData.validateDNI($("#" + PersonalDataProfile.textBoxDNIId).val()) === false) {
                GeneralData.showNotification("El formato del DNI/NIF no es correcto", "", "error");

                return;
            }

            PersonalDataProfile.setNick();
        });

        $($("#" + this.btnValidateId)).on("click", function (e) {
            var errors = PersonalDataProfile.validateForm();
            PersonalDataProfile.showErrors(errors);
            if (errors.length > 0) {
                e.preventDefault();
            }
        });

        this.changePasswordWindowId = $("#" + this.changePasswordId);
        this.changePasswordWindowId.kendoWindow({
            width: "330px",
            title: "Cambiar Contraseña",
            visible: false,
            modal: true,
            actions: [
                "Close"
            ],
            content: "/Profile/ChangePassword"
        });
        $($("#" + this.btnChangePasswordId)).on("click", function () {
            PersonalDataProfile.changePasswordWindowId.data("kendoWindow").center().open();
            ChangePassword.resetFields();
        });
    },

    setInitials: function () {
        var completeName = kendo.format("{0} {1}", $("#" + PersonalDataProfile.textBoxNameId).val(), $("#" + PersonalDataProfile.textBoxLastNameId).val());
        var split = completeName.split(" ");
        var initials = "";
        for (var i = 0; i < split.length; i++) {
            if (split[i] === "") {
                continue;
            }
            initials += split[i].trim()[0].toUpperCase();
        }

        $("#" + PersonalDataProfile.spInitialsId).text(initials);
    },

    setNick: function () {
        var nick = $("#" + this.textBoxDNIId).val();

        switch (GeneralData.userRoleId) {
            case Constants.role.Super:
                nick = "SU-" + nick;           
                break;
            case Constants.role.Admin:
                nick = "AD-" + nick;
                break;
            case Constants.role.Library:
                nick = "BI-" + nick;
                break;
            case Constants.role.Agency:
                nick = "GE-" + nick;
                break;
            case Constants.role.ContactPerson:
                nick = "PC-" + nick;
                break;
            case Constants.role.Doctor:
                nick = "ME-" + nick;
                break;
            case Constants.role.Employee:
                nick = "TR-" + nick;
                break;
            case Constants.role.ExternalPersonal:
                nick = "PE-" + nick;
                break;
            case Constants.role.PreveaCommercial:
                nick = "CP-" + nick;
                break;
            case Constants.role.PreveaPersonal:
                nick = "PP-" + nick;
                break;
            case Constants.role.Manager:
                nick = "DI-" + nick;
                break;
            default:
                nick = "??-" + nick;
                break;
        }

        $("#" + this.textBoxNickId).val(nick);
    },

    validateForm: function () {
        var error = [];

        if (!$("#" + this.textBoxNameId).val()) {
            error.push("Debes añadir un Nombre");
        }

        if (GeneralData.validateDNI($("#" + this.textBoxDNIId).val()) === false) {
            error.push("El formato del DNI/NIF no es correcto");
        }

        return error;
    },

    showErrors: function (errors) {
        var divError = $("#" + PersonalDataProfile.errorFromFrontId);

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