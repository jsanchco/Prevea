var ErrorPage = kendo.observable({
    
    pageContainerId: "pageContainer",
    framePpalId: "framePpal",
    menuPpalId: "menuPpal",

    init: function (pageError) {
        if (pageError === "UserNoRegistred") {
            $("#" + this.menuPpalId).hide();
        }
    }
});