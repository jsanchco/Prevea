var DetailMailing = kendo.observable({

    iconMailingStateId: "iconMailingState",
    tabStripDetailMailingId: "tabStripDetailMailing",
    numberDataMailsId: "numberDataMails",

    id: null,
    mailingState: null,

    init: function (id, mailingState) {
        this.id = id;
        this.mailingState = mailingState.toLowerCase();

        this.createIconMailingState();
        this.createTabStripDetailMailing();
    },

    createTabStripDetailMailing: function () {
        var tabStrip = $("#" + this.tabStripDetailMailingId).kendoTabStrip().data("kendoTabStrip");
        tabStrip.append({
            text: "PERSONAS para ENVIAR",
            contentUrl: kendo.format("/Mailings/DataMails?mailingId={0}", this.id)
        });
        //tabStrip.append({
        //    text: "EXCEL",
        //    contentUrl: kendo.format("/Mailings/ExcelMails?mailingId={0}", this.id)
        //});
        tabStrip.append({
            text: "CONTENIDO del MAIL",
            contentUrl: kendo.format("/Mailings/Mail?mailingId={0}", this.id)
        });

        tabStrip = $("#" + this.tabStripDetailMailingId).data("kendoTabStrip");

        tabStrip.select(0);
    },

    createIconMailingState: function () {
        var html;
        if (this.mailingState === "false") {
            html = "<div id='circleError' class='pull-right'></div>";
        } else {
            html = "<div id='circleSuccess' class='pull-right'></div>";
        }

        $("#" + this.iconMailingStateId).html(html);
    },

    goToMailings: function () {
        var params = {
            url: "/Mailings/Mailings",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToDetailMailing: function () {
        var params = {
            url: "/Mailings/DetailMailing",
            data: {
                id: this.id
            }
        };
        GeneralData.goToActionController(params);
    }
});