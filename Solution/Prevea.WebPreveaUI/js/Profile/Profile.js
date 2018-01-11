var Profile = kendo.observable({

    userId: null,

    confirmId: "confirm",

    init: function (userId) {
        this.userId = userId;
    },

    goToProfile: function() {
        var params = {
            url: "/Profile/ProfileUser",
            data: {}
        };
        GeneralData.goToActionController(params);
    }
 
});