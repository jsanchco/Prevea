var DetailPreventivePlan = kendo.observable({
    titleId: "title",
    navigationId: "navigation",
    tabStripDetailPreventivePlanId: "tabStripDetailPreventivePlan",
    inputTemplatePreventivePlanId: "inputTemplatePreventivePlan",
    spanNotificationId: "spanNotification",
    stretchCalculate: null,
    subscribeNumberEmployees: null,

    // Fields
    id: null,
    selectTabId: null,
    notification: null,

    // Datasources

    init: function (id, selectTabId, notification) {
        this.id = id;
        this.selectTabId = selectTabId;

        if (notification) {
            this.notification = notification;
        } else {
            this.notification = null;
        }

        this.setUpPage();
        this.createKendoWidgets();
    },

    setUpPage: function () {
  
    },

    createKendoWidgets: function () {
        if (this.notification) {
            GeneralData.showNotification(Constants.ok, "", "success");
        } else {
            $("#" + this.spanNotificationId).hide();
        }

        this.createTabStripDetailPreventivePlan();
    },

    createTabStripDetailPreventivePlan: function () {
        var tabStrip = $("#" + this.tabStripDetailPreventivePlanId).kendoTabStrip().data("kendoTabStrip");
        tabStrip.append({
            text: "OFERTAS",
            //contentUrl: kendo.format("/Companies/SimulationsCompany?companyId={0}", this.id)
        });   

        tabStrip = $("#" + this.tabStripDetailPreventivePlanId).data("kendoTabStrip");
        tabStrip.select(this.selectTabId);
    },

    goToPreventivePlans: function () {
        var params = {
            url: "/PreventivePlan/PreventivePlans",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToDetailPlanPreventive: function () {
        var params = {
            url: "/PreventivePlan/DetailPreventivePlan",
            data: {
                id: this.id,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    }
});