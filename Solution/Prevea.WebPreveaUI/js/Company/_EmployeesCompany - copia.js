var _EmployeesCompany = kendo.observable({

    containerId: "pageEmployeesCompany",
    gridEmployeesCompanyId: "_gridEmployeesCompany",
    btnAddEmployeesCompanyId: "btnAddEmployeesCompany",
    confirmId: "confirm",

    companyId: null,

    grid: null,
    employeesCompanyDataSource: null,
    
    init: function (companyId) {
        this.companyId = companyId;

        this.createEmployeesCompanyDataSource();
        this.createEmployeesCompanyGrid();

        this.grid = ExpandKendoGrid.init(this.containerId, this.gridEmployeesCompanyId);        
    },

    createEmployeesCompanyGrid: function () {
        $("#" + this.gridEmployeesCompanyId).kendoGrid({
            columns: [{
                field: "FirstName",
                title: "Nombre",
                width: 80
            }, {
                field: "LastName",
                title: "Apellidos",
                width: 130,
                groupable: "false"
            }, {
                field: "PhoneNumber",
                title: "Teléfono",
                width: 80,
                groupable: "false"
            }, {
                field: "Email",
                title: "Email",
                width: 100,
                groupable: "false"
            }, {
                field: "WorkStation",
                title: "Puesto de Trabajo",
                width: 160
            }, {
                title: "Acciones",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= _EmployeesCompany.getColumnTemplateCommands(data) #"
            }],
            editable: "inline",
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
            autoScroll: true,
            selectable: true,            
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: false,
            dataBound: function (e) {
                var grid = $("#" + _EmployeesCompany.gridEmployeesCompanyId).data("kendoGrid");
                var items = e.sender.items();
                items.each(function () {
                    var dataItem = grid.dataItem(this);
                    if (dataItem.UserStateId === 3) {
                        this.className = "unSubscribe";
                    }
                });
            },
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Editar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Borrar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }
        });
        kendo.bind($("#" + this.gridEmployeesCompanyId), this);
    },

    createEmployeesCompanyDataSource: function () {
        this.employeesCompanyDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        FirstName: { type: "string" },
                        LastName: { type: "string" },
                        PhoneNumber: { type: "string" },
                        Email: { type: "string" },
                        WorkStation: { type: "string" },
                        UserStateId: { type: "number" },
                        CompanyId: { type: "number" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Company/_EmployeesCompany_Read",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                },
                update: {
                    url: "/Company/Update",
                    dataType: "jsonp"                    
                },
                destroy: {
                    url: "/Company/Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Company/Create",
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
                if (e.type === "update" && e.response !== null) {
                    if (typeof e.response.Errors !== "undefined") {
                        $("#" + DetailCompany.spanNotificationId).kendoNotification().data("kendoNotification").show(e.response.Errors);
                        this.cancelChanges();
                    } else {
                        GeneralData.showNotification("Operación realizada correctamente", "", "success");

                        //$("#" + GeneralData.notificationId).kendoNotification().data("kendoNotification").show("El Trabajador se ha guardado correctamente");


                        //$("#" + DetailCompany.spanNotificationId).kendoNotification().data("kendoNotification").show("El Trabajador se ha guardado correctamente");
                    }                  
                }
            },
            pageSize: 10
        });
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        if (data.UserStateId === 3) {
            html += kendo.format("<a toggle='tooltip' title='Dar de Alta' onclick='_EmployeesCompany.goToSubscribeEmployeeCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-thumbs-up' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='_EmployeesCompany.goToEditEmployeeCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='event.preventDefault(); _EmployeesCompany.goToDeleteEmployeeCompany(\"{0}\");' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    getColumnTemplateUpdateOrCancel: function (data) {
        var html = "<div align='center'>";
        html += kendo.format("<a toggle='tooltip' title='Guardar' onclick='_EmployeesCompany.goToEditEmployeeCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Cancelar' onclick='_EmployeesCompany.goToDeleteEmployeeCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToEditEmployeeCompany: function (userId) {
        var grid = $("#" + _EmployeesCompany.gridEmployeesCompanyId).data("kendoGrid");
        var dataItem = grid.dataSource.get(userId);
        grid.editRow(dataItem);


        //var row = grid.editRow($("#grid tr:eq(1)"));
        //var commandCell = e.container.find("td:last");
        //commandCell.html('<a class="btn btn-primary k-grid-update">Update</a><a class="btn btn-danger k-grid-cancel">Cancel</a>');
        //var params = {
        //    url: "/Company/AddEmployeeCompany",
        //    data: {
        //        companyId: this.companyId,
        //        userId: userId
        //    }
        //};
        //GeneralData.goToActionController(params);
    },

    goToDeleteEmployeeCompany: function (userId) {
        var grid = $("#" + _EmployeesCompany.gridEmployeesCompanyId).data("kendoGrid");
        var item = grid.dataSource.get(userId);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);    

        var dialog = $("#" + this.confirmId);
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
                        grid.removeRow(tr);

                        var params = {
                            url: "/Company/SubscribeContactPersonCompany",
                            data: {
                                companyId: DeleteContactPersonCompany.companyId,
                                userId: userId,
                                subscribe: false
                            }
                        };
                        GeneralData.goToActionController(params);
                    }
                },
                {
                    text: "Borrar", action: function () {
                        grid.removeRow(tr);

                        var params = {
                            url: "/Company/DeleteContactPersonCompany",
                            data: {
                                companyId: DeleteContactPersonCompany.companyId,
                                userId: userId,
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

    goToAddEmployeeCompany: function () {
        this.grid.set("addCommandColumn", true);
        //var params = {
        //    url: "/Company/AddEmployeeCompany",
        //    data: {
        //        companyId: this.companyId,
        //        userId: 0
        //    }
        //};
        //GeneralData.goToActionController(params);
    },

    goToSubscribeEmployeeCompany: function (userId) {
        var params = {
            url: "/Company/SubscribeEmployeeCompany",
            data: {
                companyId: this.companyId,
                userId: userId,
                subscribe: true
            }
        };
        GeneralData.goToActionController(params);
    }

});