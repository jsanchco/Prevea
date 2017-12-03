var Notifications = kendo.observable({

    gridNotificationsId: "gridNotifications",
    confirmId: "confirm",

    notificationsDataSource: null,

    init: function () {
        this.createNotificationsDataSource();
        this.createGridNotifications();
    },

    createNotificationsDataSource: function () {
        this.notificationsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        NotificationTypeDescription: { type: "string" },
                        NotificationStateDescription: { type: "string" },
                        Observations: { type: "string" },
                        DateCreation: { type: "date" },
                        DateModification: { type: "date" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Notification/Notifications_Read",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Notification/Notifications_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Notification/Notifications_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { notification: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            requestEnd: function (e) {
                if ((e.type === "update" || e.type === "destroy" || e.type === "create") &&
                    e.response !== null) {
                    if (typeof e.response.Errors !== "undefined") {
                        GeneralData.showNotification(Constants.ko, "", "error");
                        this.cancelChanges();
                    } else {
                        GeneralData.showNotification(Constants.ok, "", "success");
                    }
                }
            },
            pageSize: 10
        });
    },

    createGridNotifications: function () {
        $("#" + this.gridNotificationsId).kendoGrid({
            columns: [
                {
                    field: "NotificationTypeDescription",
                    title: "Tipo de Notificación",
                    width: 200
                }, {
                    field: "NotificationStateDescription",
                    title: "Estado",
                    width: 180
                }, {
                    field: "Observations",
                    title: "Observaciones",
                    groupable: "false"
                }, {
                    field: "DateCreation",
                    title: "Fecha de Creación",
                    width: 200,
                    template: "#= Templates.getColumnTemplateDateWithHour(data.DateCreation) #"
                }, {
                    field: "DateModification",
                    title: "Fecha de Modificación",
                    width: 200,
                    template: "#= Notifications.getColumnTemplateDateModication(data.DateModification) #"
                }, {
                    title: "Comandos",
                    field: "Commands",
                    width: 130,
                    groupable: "false",
                    filterable: false,
                    template: "#= Notifications.getColumnTemplateCommands(data) #"
                }],
            pageable: {
                buttonCount: 2,
                pageSizes: [10, 20, "all"],
                refresh: true,
                messages: {
                    display: "Elementos mostrados {0} - {1} de {2}",
                    itemsPerPage: "Elementos por página",
                    allPages: "Todos",
                    empty: "No existen registros para mostrar"
                }
            },
            filterable: {
                messages: {
                    info: "Filtrar por: ",
                    and: "Y",
                    or: "O",
                    filter: "Aplicar",
                    clear: "Limpiar"
                },
                operators: {
                    string: {
                        contains: "Contiene",
                        eq: "Igual a",
                        neq: "No igual a",
                        startswith: "Empieza con",
                        endswith: "Termina con",
                        doesnotcontain: "No contiene",
                        isempty: "Está vacio",
                        isnotnull: "No está vacio"
                    },
                    number: {
                        eq: "Igual a",
                        gt: "Más grande que",
                        lt: "Más pequeño que"
                    },
                    date: {
                        eq: "Igual a",
                        gt: "Antes que",
                        lt: "Después que",
                        isnull: "Está vacio"
                    }
                }
            },
            dataSource: this.notificationsDataSource,
            toolbar: this.getTemplateToolBar(),
            editable: {
                mode: "inline",
                confirmation: false
            },
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: {
                messages: {
                    empty: "Arrastre un encabezado de columna y póngalo aquí para agrupar por ella"
                }
            },
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }

        });
        kendo.bind($("#" + this.gridNotificationsId), this);
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";

        html += "<span style='float: right;'>";
        html += "<a id='showAll' class='btn btn-prevea' role='button' onclick='Notifications.applyFilter()'> Ver todos</a>";
        html += "</span>";

        html += "</div>";

        return html;
    },

    getColumnTemplateDateModication: function (data) {
        var html = "<div align='center'>";
        if (data === null) {
            html += kendo.format("<div align='center'>{0}</div>", "");
        } else {
           html += kendo.format("<div align='center'>{0}</div>", kendo.toString(date, "dd/MM/yy HH:mm"));
        }
        html += kendo.format("</div>");

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        if (data.IsBlocked === true) {
            html += kendo.format("<a toggle='tooltip' title='Ir a Empresa' onclick='Notifications.goToCompanyFromSimulator(\"{0}\", true)' target='_blank' style='cursor: pointer;'><i class='fa fa-share-square' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        } else {
            html += kendo.format("<a toggle='tooltip' title='Detalle' onclick='Simulators.goToEditSimulator(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
            html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Simulators.goToDeleteSimulator(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }
        html += kendo.format("</div>");

        return html;
    },

    goToNotifications: function () {
        var params = {
            url: "/Notification/Notifications",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToEditNotification: function (id) {
        var params = {
            url: "/Notification/EditSimulator",
            data: {
                simulatorId: id
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeleteNotification: function (simulatorId) {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Simulaciones</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> esta Simulación?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + Simulators.gridSimulatorsId).data("kendoGrid");
                        var item = grid.dataSource.get(simulatorId);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    }

});