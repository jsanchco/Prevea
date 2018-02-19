var ChangePassword = kendo.observable({

    textBoxNewPasswordId: "textBoxNewPassword",
    textBoxOldPasswordId: "textBoxOldPassword",
    textBoxRepeatPasswordId: "textBoxOldPassword",

    userId: null,

    init: function (userId) {
        this.userId = userId;

        this.setUpWidgets();
    },

    setUpWidgets: function() {
    }
});