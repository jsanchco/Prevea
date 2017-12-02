var DeleteEmployeeCompany = kendo.observable({
    btnDeleteId: "btnDelete",
    btnUnsubscribeId: "btnUnsubscribe",
    btnCancelId: "btnCancel",
    confirmId: "confirm",

    companyId: null,
    userId: null,

    init: function (companyId, userId) {
        this.companyId = companyId;
        this.userId = userId;

        this.createKendoWidgets();
    },

    createKendoWidgets: function () {
        $($("#" + DeleteEmployeeCompany.btnCancelId)).on("click", function () {
            DeleteEmployeeCompany.goToDetailCompany();
        });

        $($("#" + DeleteEmployeeCompany.btnDeleteId)).on("click", function () {
            DeleteEmployeeCompany.deleteEmployeeCompany();
        });

        $($("#" + DeleteEmployeeCompany.btnUnsubscribeId)).on("click", function () {
            DeleteEmployeeCompany.unsubscribeEmployeeCompany();
        });
    },

    deleteEmployeeCompany: function () {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "Empresas",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> al Trabajador?",
            actions: [
                { text: "Cancelar", primary: true },
                {
                    text: "Aceptar", action: function () {
                        var params = {
                            url: "/Company/DeleteEmployee",
                            data: {
                                companyId: DeleteEmployeeCompany.companyId,
                                userId: DeleteEmployeeCompany.userId
                            }
                        };
                        GeneralData.goToActionController(params);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();      
    },

    unsubscribeEmployeeCompany: function () {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "Empresas",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Dar de Baja</strong> al Trabajador?",
            actions: [
                { text: "Cancelar", primary: true },
                {
                    text: "Aceptar", action: function () {
                        var params = {
                            url: "/Company/SubscribeEmployeeCompany",
                            data: {
                                companyId: DeleteEmployeeCompany.companyId,
                                userId: DeleteEmployeeCompany.userId,
                                subscribe: false
                            }
                        };
                        GeneralData.goToActionController(params);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();      
    },

    goToCompanies: function () {
        var params = {
            url: "/Company/Companies"
        };
        GeneralData.goToActionController(params);
    },

    goToDetailCompany: function() {
        var params = {
            url: "/Company/DetailCompany",
            data: {
                id: this.companyId,
                selectTabId: 3
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeleteEmployeeCompany: function () {
        var params = {
            url: "/Company/DeleteEmployeeCompany",
            data: {
                companyId: this.companyId,
                userId: this.userId
            }
        };
        GeneralData.goToActionController(params);
    }

});