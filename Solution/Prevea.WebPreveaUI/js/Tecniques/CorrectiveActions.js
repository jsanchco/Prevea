var CorrectiveActions = kendo.observable({    

    gridCorrectiveActionsId: "gridCorrectiveActions",
    confirmId: "confirm",

    riskEvaluationId: null,
    correctiveActionsDataSource: null,
    priorityCorrectiveActionsDataSource: null,

    init: function (riskEvaluationId) {
        kendo.culture("es-ES");

        this.riskEvaluationId = riskEvaluationId;

        this.createCorrectiveActionsDataSource();
        this.createPriorityCorrectiveActionsDataSource();

        this.createCorrectiveActionsGrid();
    },

    createCorrectiveActionsDataSource: function () {
        this.correctiveActionsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Description: { type: "string" },
                        PriorityCorrectiveActionId: { type: "number" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/PreventivePlan/PreventivePlans_Read",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/PreventivePlan/PreventivePlans_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/PreventivePlan/PreventivePlans_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { preventivePlan: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            requestEnd: function (e) {
                if ((e.type === "update" || e.type === "destroy" || e.type === "create") &&
                    e.response !== null) {
                    if (typeof e.response.Errors !== "undefined") {
                        GeneralData.showNotification(Constants.ko, "", "error");
                        if (e.type === "create") {
                            this.data().remove(this.data().at(0));
                        } else {
                            this.cancelChanges();
                        }
                    } else {
                        GeneralData.showNotification(Constants.ok, "", "success");
                    }
                }
            },
            change: function (e) {
                if (e.field === "CompanyId") {

                    $.ajax({
                        url: "/PreventivePlan/GetContract",
                        data: { companyId: e.items[0].CompanyId },
                        type: "post",
                        dataType: "json",
                        success: function (response) {
                            if (response.resultStatus === Constants.resultStatus.Ok) {
                                e.items[0].set("DocumentBeginDate", response.contract.BeginDate);
                                e.items[0].set("DocumentEndDate", response.contract.EndDate);

                                e.items[0].set("DocumentId", response.contract.Id);
                            } else {
                                GeneralData.showNotification("Esta Empresa aún no tiene contrato", "", "error");
                            }
                        }
                    });
                }
            },
            pageSize: 10
        });
   
    },

    createPriorityCorrectiveActionsDataSource: function () {

    },

    createCorrectiveActionsGrid: function () {

    }

});