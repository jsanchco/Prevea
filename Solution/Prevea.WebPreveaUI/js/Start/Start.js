var Start = kendo.observable({

    numberNotificationsId: "numberNotifications",
    footerRoleId: "footerRole",

    init: function (numberNotifications, userId, userInitials, userRoleId, userRoleName, userRoleDescription) {
        $("#" + this.numberNotificationsId).text(numberNotifications);

        GeneralData.userId = userId;
        GeneralData.userInitials = userInitials;
        GeneralData.userRoleId = userRoleId;
        GeneralData.userRoleName = userRoleName;
        GeneralData.userRoleDescription = userRoleDescription;

        this.setUpPage();
    },

    setUpPage: function() {
        $("#" + this.footerRoleId).html(GeneralData.userRoleDescription);
    }
});