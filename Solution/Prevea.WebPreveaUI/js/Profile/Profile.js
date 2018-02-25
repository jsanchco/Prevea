var Profile = kendo.observable({

    choosePhotoId: "choosePhoto",
    tabStripDetailProfileId: "tabStripDetailProfile",
    simulationsAssignedId: "simulationsAssigned",
    companiesAssignedId: "companiesAssigned",

    choosePhotoWindow: null,

    userId: null,
    selectTabId: null,
    notification: null,

    init: function (userId, selectTabId, notification) {
        this.userId = userId;
        this.selectTabId = selectTabId;

        if (notification) {
            this.notification = notification;
        } else {
            this.notification = null;
        }

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

        this.createTabStripProfile();

        if (this.notification) {
            GeneralData.showNotification(Constants.ok, "", "success");
        } else {
            $("#" + this.spanNotificationId).hide();
        }

        if (GeneralData.userRoleId === Constants.role.ContactPerson ||
            GeneralData.userRoleId === Constants.role.Employee) {
            $("#" + this.simulationsAssignedId).hide();
            $("#" + this.companiesAssignedId).hide();
        }
    },

    createTabStripProfile: function () {
        var tabStrip = $("#" + this.tabStripDetailProfileId).kendoTabStrip().data("kendoTabStrip");
        tabStrip.append({
            text: "DATOS PERSONALES",
            contentUrl: "/Profile/PersonalDataProfile"
        });

        if (GeneralData.userRoleId !== Constants.role.ContactPerson &&
            GeneralData.userRoleId !== Constants.role.Employee) {
            tabStrip.append({
                text: "SEGUIMIENTO ECONÓMICO",
                contentUrl: "/Profile/EconomicTrackingProfile"
            });
        }

        tabStrip.append({
                text: "DOCUMENTOS",
                contentUrl: "/Profile/DocumentsProfile"
            });

        tabStrip = $("#" + this.tabStripDetailProfileId).data("kendoTabStrip");

        tabStrip.bind("activate",
            function () {
                if (tabStrip.select().index() === 0) {
                    PersonalDataProfile.setInitials();
                }
            });

        tabStrip.select(this.selectTabId);
    },

    goToChoosePhoto: function() {
        this.choosePhotoWindow.data("kendoWindow").center().open();
    }
 
});