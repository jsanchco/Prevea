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
                        Id: { type: "number" },
                        SimulationId: { type: "number" },
                        NotificationTypeDescription: { type: "string" },
                        NotificationStateDescription: { type: "string" },
                        ToUserInitials: { type: "string" },
                        Observations: { type: "string" },
                        DateCreation: { type: "date" },
                        DateModification: { type: "date" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Notifications/Notifications_Read",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { notification: kendo.stringify(options) };
                    }

                    return null;
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
                },
                //{
                //    field: "NotificationStateDescription",
                //    title: "Estado",
                //    width: 180
                //},
                {
                    field: "ToUserInitials",
                    title: "Asignado a",
                    width: 150,
                    template: "#= Templates.getColumnTemplateIncrease(data.ToUserInitials) #"
                }, {
                    field: "Observations",
                    title: "Observaciones",
                    groupable: "false"
                }, {
                    field: "DateCreation",
                    title: "Fecha de Creación",
                    width: 200,
                    template: "#= Templates.getColumnTemplateDateWithHour(data.DateCreation) #"
                },
                //{
                //    field: "DateModification",
                //    title: "Fecha de Modificación",
                //    width: 200,
                //    template: "#= Notifications.getColumnTemplateDateModication(data.DateModification) #"
                //},
                {
                    title: "Comandos",
                    field: "Commands",
                    width: 130,
                    groupable: "false",
                    filterable: false,
                    template: "#= Notifications.getColumnTemplateCommands(data) #"
                }
            ],
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
            }

        });
        kendo.bind($("#" + this.gridNotificationsId), this);

        if (GeneralData.userRoleId === Constants.role.PreveaCommercial) {
            var grid = $("#" + this.gridNotificationsId).data("kendoGrid");
            grid.hideColumn("ToUserInitials");
        }
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
           html += kendo.format("<div align='center'>{0}</div>", kendo.toString(data, "dd/MM/yy HH:mm"));
        }
        html += kendo.format("</div>");

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";

        html += kendo.format("<a toggle='tooltip' title='Ir a Simulación' onclick='Notifications.goToSimulationFromNotification(\"{0}\", true)' target='_blank' style='cursor: pointer;'><i class='fa fa-share-square' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.SimulationId);
        html += kendo.format("</div>");

        return html;
    },

    goToNotifications: function () {
        var params = {
            url: "/Notifications/Notifications",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToAssignNotification: function (notificationId) {
        $.ajax({
            url: "/Notifications/AssignNotification",
            data: {
                notificationId: notificationId
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result.Status === 0) {
                    Notifications.notificationsDataSource.read();
                    GeneralData.showNotification(Constants.ok, "", "success");
                }
                if (data.result.Status === 1) {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            },
            error: function () {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    },

    goToSimulationFromNotification: function(simulationId) {
        var params = {
            url: "/CommercialTool/Simulations/DetailSimulation",
            data: {
                simulationId: simulationId,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    }

});