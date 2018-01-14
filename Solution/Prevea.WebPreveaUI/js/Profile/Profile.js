var Profile = kendo.observable({

    choosePhotoId: "choosePhoto",
    choosePhotoWindow: null,

    userId: null,

    init: function (userId) {
        this.userId = userId;

        this.setUpWidgets();
    },

    goToProfile: function() {
        var params = {
            url: "/Profile/ProfileUser",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    setUpWidgets: function() {
        this.choosePhotoWindow = $("#" + this.choosePhotoId);
        this.choosePhotoWindow.kendoWindow({
            width: "330px",
            title: "Cambiar Foto",
            visible: false,
            modal: true,
            actions: [
                "Close"
            ],
            content: "/Profile/ChoosePhoto",
            close: ChoosePhoto.onCloseChoosePhotoWindow,
            open: ChoosePhoto.onOpenChoosePhotoWindow
        });
    },

    goToChoosePhoto: function() {
        this.choosePhotoWindow.data("kendoWindow").center().open();
    }
 
});