var EmployeesCompany = kendo.observable({

    gridEmployeesCompanyId: "gridEmployeesCompany",
    confirmId: "confirm",

    companyId: null,

    employeesCompanyDataSource: null,
    workStationsDataSource: null,

    init: function (companyId) {
        kendo.culture("es-ES");

        this.companyId = companyId;

        this.createEmployeesCompanyDataSource();
        this.createWorkStationsDataSource();
        this.createEmployeesCompanyGrid();
    },

    createEmployeesCompanyGrid: function () {
        var that = this;
        $("#" + this.gridEmployeesCompanyId).kendoGrid({
            columns: [{
                field: "FirstName",
                title: "Nombre",
                width: 120
            }, {
                field: "LastName",
                title: "Apellidos",
                width: 130,
                groupable: "false"
            }, {
                field: "PhoneNumber",
                title: "Teléfono",
                width: 120,
                groupable: "false"
            }, {
                field: "Address",
                title: "Dirección",
                width: 180,
                groupable: "false"
            }, {
                field: "Province",
                title: "Provincia",
                width: 120
            }, {
                field: "BirthDate",
                title: "Fecha de Nacimiento",
                width: 200,
                groupable: "false",
                template: "#= Templates.getColumnTemplateDate(data.BirthDate) #"
            }, {
                field: "Email",
                title: "Email",
                width: 140,
                groupable: "false"
            }, {
                field: "DNI",
                title: "DNI",
                width: 100,
                groupable: "false"
            }, {
                field: "WorkStationId",
                title: "Puesto de Trabajo",
                width: 180,
                editor: EmployeesCompany.workStationsDropDownEditor,
                template: "#= EmployeesCompany.getColumnTemplateWorkStation(data.WorkStationName) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,                
                template: "#= EmployeesCompany.getColumnTemplateCommands(data) #"
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
            dataSource: this.employeesCompanyDataSource,
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
            dataBound: function (e) {
                var grid = $("#" + that.gridEmployeesCompanyId).data("kendoGrid");
                var items = e.sender.items();
                items.each(function () {
                    var dataItem = grid.dataItem(this);
                    if (dataItem.UserStateId === 2 || dataItem.UserStateId === 3) {
                        this.className = "unSubscribe";
                    }
                });
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

        if (GeneralData.userRoleId === Constants.role.Super) {
            $("#addEmployeeCompany").removeAttr("disabled");
            $("#addEmployeeCompany").prop("disabled", true);
        }
    },

    createEmployeesCompanyDataSource: function () {
        this.employeesCompanyDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        FirstName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        LastName: { type: "string" },
                        PhoneNumber: { type: "string" },
                        Address: { type: "string" },
                        Province: { type: "string" },
                        BirthDate: { type: "date", format: "{0:dd/MM/yy}", defaultValue: new Date() },
                        ChargeDate: { type: "date" },
                        Email: { type: "string" },
                        DNI: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        WorkStationName: { type: "string" },
                        WorkStationId: { type: "number", defaulValue: null },
                        UserStateId: { type: "number", defaultValue: 1 },
                        CompanyId: { type: "number", defaultValue: EmployeesCompany.companyId }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Companies/EmployeesCompany_Read",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                },
                update: {
                    url: "/Companies/EmployeesCompany_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Companies/EmployeesCompany_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Companies/EmployeesCompany_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { companyId: options.companyId };
                    }
                    if (operation !== "read" && options) {
                        return { employee: kendo.stringify(options) };
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
            pageSize: 10,
            resizable: true
        });
    },

    createWorkStationsDataSource: function () {
        this.workStationsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        Name: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Companies/GetWorkStations",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                }
            }
        });
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' id='createUser'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button' id='addEmployeeCompany'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateWorkStation: function (text) {
        if (text == null) {
            return "";
        }
        return text;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";        
        if (data.UserStateId === 2 || data.UserStateId === 3) {
            html += kendo.format("<a toggle='tooltip' title='Dar de Alta' onclick='EmployeesCompany.goToSubscribeEmployeeCompany(\"{0}\", true)' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-thumbs-up' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        } else {
            html += kendo.format("<a toggle='tooltip' title='Editar' onclick='EmployeesCompany.goToEditEmployeeCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
            html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='EmployeesCompany.goToDeleteEmployeeCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }        
        html += kendo.format("</div>");

        return html;
    },

    goToEditEmployeeCompany: function (userId) {
        var grid = $("#" + EmployeesCompany.gridEmployeesCompanyId).data("kendoGrid");
        var item = grid.dataSource.get(userId);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteEmployeeCompany: function (userId) {
        var that = this;
        var dialog = null;

        if (GeneralData.userRoleId === Constants.role.ContactPerson) {
            dialog = $("#" + this.confirmId);
            dialog.kendoDialog({
                width: "400px",
                title: "Empresas",
                closable: false,
                modal: true,
                content: "¿Quieres <strong>Dar de Baja/Borrar</strong> al Trabajador?",
                actions: [
                    {
                        text: "Cancelar", primary: true
                    },
                    {
                        text: "Dar de Baja", action: function () {
                            that.goToSubscribeEmployeeCompany(userId, false);
                        }
                    }
                ]
            });
            dialog.data("kendoDialog").open();
        } else {
            dialog = $("#" + this.confirmId);
            dialog.kendoDialog({
                width: "400px",
                title: "Empresas",
                closable: false,
                modal: true,
                content: "¿Quieres <strong>Dar de Baja/Borrar</strong> al Trabajador?",
                actions: [
                    {
                        text: "Cancelar", primary: true
                    },
                    {
                        text: "Dar de Baja", action: function () {
                            that.goToSubscribeEmployeeCompany(userId, false);
                        }
                    },
                    {
                        text: "Borrar", action: function () {
                            var grid = $("#" + EmployeesCompany.gridEmployeesCompanyId).data("kendoGrid");
                            var item = grid.dataSource.get(userId);
                            var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                            grid.removeRow(tr);
                        }
                    }
                ]
            });
            dialog.data("kendoDialog").open();
        }        
    },

    goToSubscribeEmployeeCompany: function (userId, subscribe) {
        var that = this;

        $.ajax({
            url: "/Companies/EmployeesCompany_Subscribe",
            type: "post",
            cache: false,
            datatype: "json",
            data: {
                companyId: that.companyId,
                userId: userId,
                subscribe: subscribe
            },
            success: function (result) {
                var grid = $("#" + EmployeesCompany.gridEmployeesCompanyId).data("kendoGrid");
                var item = grid.dataSource.get(userId);
                var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                if (subscribe) {
                    item.set("UserStateId", 1);
                } else {
                    item.set("UserStateId", 2);
                }
                
                var cellName = "Commands";
                var cellIndex = grid.element.find("th[data-field = '" + cellName + "']").index();
                var cell = tr.find("td:eq(" + cellIndex + ")");
                cell.html(that.getColumnTemplateCommands(item));

                GeneralData.showNotification(Constants.ok, "", "success");

                console.log(result.Message);
            },
            error: function (result) {
                GeneralData.showNotification(Constants.ko, "", "error");

                console.log(result);
            }
        });
    },

    workStationsDropDownEditor: function (container, options) {
        EmployeesCompany.workStationsDataSource.read();

        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Id",
                dataSource: EmployeesCompany.workStationsDataSource,
                dataBound: function (e) {
                    e.sender.list.width("auto").find("li").css({ "white-space": "nowrap", "padding-right": "25px" });
                }
            });
    }
});
