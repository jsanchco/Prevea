var DocumentsProfile = kendo.observable({

    userId: null,

    init: function (userId) {
        this.userId = userId;

        this.setUpWidgets();
    },

    setUpWidgets: function () {

    }
});