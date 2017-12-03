var Start = kendo.observable({

    numberNotificationsId: "numberNotifications",

    init: function(numberNotifications) {
        $("#" + this.numberNotificationsId).text(numberNotifications);
    }
});