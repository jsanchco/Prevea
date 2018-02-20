var ChangePassword = kendo.observable({

    textBoxOldPasswordId: "textBoxOldPassword",
    textBoxNewPasswordId: "textBoxNewPassword",    
    textBoxRepeatPasswordId: "textBoxRepeatPassword",
    changePasswordId: "changePassword",

    btnValidateId: "btnValidatePassword",

    init: function () {
        this.setUpWidgets();
    },

    setUpWidgets: function () {
        $($("#" + this.btnValidateId)).on("click", function () {
            ChangePassword.validateForm();
        });
    },

    validateForm: function () {
        var oldPassword = $("#" + this.textBoxOldPasswordId).val();
        var newPassword = $("#" + this.textBoxNewPasswordId).val();
        var repeatPassword = $("#" + this.textBoxRepeatPasswordId).val();
        
        $.ajax({
            url: "/Profile/UpdatePassword",
            data: {
                oldPassword: oldPassword,
                newPassword: newPassword
            },
            type: "post",
            dataType: "json",
            success: function (response) {
                var error = false;
                if (response.resultStatus === Constants.resultStatus.Error) {
                    GeneralData.showNotification("No coincide la Contraseña anterior con la almacenada", "", "error");
                    error = true;
                } else {
                    if (newPassword !== repeatPassword) {
                        GeneralData.showNotification("No coincide la nueva Contraseña con la repetida", "", "error");
                        error = true;
                    } else {
                        if (newPassword.length < 6) {
                            GeneralData.showNotification("La nueva contraseña debe tener al menos 6 caracteres", "", "error");
                            error = true;
                        }
                    }                    
                }
                if (error !== true) {
                    var changePasswordWindow = $("#" + ChangePassword.changePasswordId);
                    changePasswordWindow.data("kendoWindow").close();
                    GeneralData.showNotification(Constants.ok, "", "success");
                }
            }
        });
    },

    resetFields: function() {
        $("#" + this.textBoxOldPasswordId).val("");
        $("#" + this.textBoxNewPasswordId).val("");
        $("#" + this.textBoxRepeatPasswordId).val("");
    }
});