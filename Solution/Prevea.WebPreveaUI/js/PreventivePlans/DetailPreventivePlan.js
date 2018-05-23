var DetailPreventivePlan = kendo.observable({
    tabStripDetailPreventivePlanId: "tabStripDetailPreventivePlan",
    inputTemplatePreventivePlanId: "inputTemplatePreventivePlan",
    btnAddTemplateId: "btnAddTemplate",

    // Fields
    id: null,
    selectTabId: null,

    init: function (id, selectTabId) {
        this.id = id;
        this.selectTabId = selectTabId;

        $("#" + this.btnAddTemplateId).removeAttr("disabled");
        $("#" + this.btnAddTemplateId).prop("disabled", true);
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
    },

    addTemplate: function () {
        var dropdownlist = $("#" + this.inputTemplatePreventivePlanId).data("kendoDropDownList");



        
        var tabStrip = $("#" + this.tabStripDetailPreventivePlanId).kendoTabStrip().data("kendoTabStrip");
        tabStrip.append({
            text: dropdownlist.text(),
            contentUrl: kendo.format("/PreventivePlan/EditorTemplatePreventivePlan?preventivePlan={0}&templateId={1}", this.id, dropdownlist.value())
        });
        tabStrip.select(tabStrip.items().length - 1);
    },

    onChangeTemplate: function() {
        var dropdownlist = $("#" + DetailPreventivePlan.inputTemplatePreventivePlanId).data("kendoDropDownList");
        if (dropdownlist.value() !== "") {
            $("#" + DetailPreventivePlan.btnAddTemplateId).removeAttr("disabled");
            $("#" + DetailPreventivePlan.btnAddTemplateId).prop("disabled", false);
        } else {
            $("#" + DetailPreventivePlan.btnAddTemplateId).removeAttr("disabled");
            $("#" + DetailPreventivePlan.btnAddTemplateId).prop("disabled", true);
        }
    }
});