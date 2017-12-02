var DeleteContactPersonCompany = kendo.observable({
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
        $($("#" + DeleteContactPersonCompany.btnCancelId)).on("click", function () {
            DeleteContactPersonCompany.goToDetailCompany();
        });

        $($("#" + DeleteContactPersonCompany.btnDeleteId)).on("click", function () {
            DeleteContactPersonCompany.deleteContactPersonCompany();
        });

        $($("#" + DeleteContactPersonCompany.btnUnsubscribeId)).on("click", function () {
            DeleteContactPersonCompany.unsubscribeContactPersonCompany();
        });
    },

    deleteContactPersonCompany: function () {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "Empresas",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> a la Persona de Contacto?",
            actions: [
                { text: "Cancelar", primary: true },
                {
                    text: "Aceptar", action: function () {
                        var params = {
                            url: "/Company/DeleteContactPerson",
                            data: {
                                companyId: DeleteContactPersonCompany.companyId,
                                userId: DeleteContactPersonCompany.userId
                            }
                        };
                        GeneralData.goToActionController(params);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();      
    },

    unsubscribeContactPersonCompany: function () {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "Empresas",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Dar de Baja</strong> a la Persona de Contacto?",
            actions: [
                { text: "Cancelar", primary: true },
                {
                    text: "Aceptar", action: function () {
                        var params = {
                            url: "/Company/SubscribeContactPersonCompany",
                            data: {
                                companyId: DeleteContactPersonCompany.companyId,
                                userId: DeleteContactPersonCompany.userId,
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
                selectTabId: 1
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeleteContactPersonCompany: function () {
        var params = {
            url: "/Company/DeleteContactPersonCompany",
            data: {
                companyId: this.companyId,
                userId: this.userId
            }
        };
        GeneralData.goToActionController(params);
    }

});