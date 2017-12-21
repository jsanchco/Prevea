var Start = kendo.observable({

    numberNotifications: null,
    numberNotificationsId: "numberNotifications",
    circleRoleId: "circleRole",
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

    setUpPage: function () {
        switch (GeneralData.userRoleId) {
            case Constants.role.Super:
                $("#" + this.circleRoleId).html("SU");
                break;
            case Constants.role.Admin:
                $("#" + this.circleRoleId).html("AD");
                break;
            case Constants.role.Library:
                $("#" + this.circleRoleId).html("BI");
                break;
            case Constants.role.Agency:
                $("#" + this.circleRoleId).html("GE");
                break;
            case Constants.role.ContactPerson:
                $("#" + this.circleRoleId).html("PC");
                break;
            case Constants.role.Doctor:
                $("#" + this.circleRoleId).html("ME");
                break;
            case Constants.role.Employee:
                $("#" + this.circleRoleId).html("TR");
                break;
            case Constants.role.ExternalPersonal:
                $("#" + this.circleRoleId).html("PE");
                break;
            case Constants.role.PreveaCommercial:
                $("#" + this.circleRoleId).html("CP");
                break;
            case Constants.role.PreveaPersonal:
                $("#" + this.circleRoleId).html("PP");
                break;
            case Constants.role.Manager:
                $("#" + this.circleRoleId).html("DI");
                break;
        }        
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