var Start = kendo.observable({

    numberNotifications: null,
    numberNotificationsId: "numberNotifications",
    footerRoleId: "footerRole",
    pendingsNotificationsId: "pendingsNotifications",

    init: function (numberNotifications, userId, userInitials, userRoleId, userRoleName, userRoleDescription) {
        $("#" + this.numberNotificationsId).text(numberNotifications);

        this.numberNotifications = numberNotifications;

        GeneralData.userId = userId;
        GeneralData.userInitials = userInitials;
        GeneralData.userRoleId = userRoleId;
        GeneralData.userRoleName = userRoleName;
        GeneralData.userRoleDescription = userRoleDescription;

        this.setUpPage();
    },

    setUpPage: function() {
        $("#" + this.footerRoleId).html(GeneralData.userRoleDescription);
    },

    goToNotifications: function() {
        var params = {
            url: "/Notifications/Notifications",
            data: {
            }
        };
        GeneralData.goToActionController(params);        
    }
});