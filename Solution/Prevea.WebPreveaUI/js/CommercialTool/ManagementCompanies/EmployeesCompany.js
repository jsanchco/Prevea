﻿var EmployeesCompany = kendo.observable({

    gridEmployeesCompanyId: "gridEmployeesCompany",
    confirmId: "confirm",

    companyId: null,

    employeesCompanyDataSource: null,

    init: function (companyId) {
        kendo.culture("es-ES");

        this.companyId = companyId;

        this.createEmployeesCompanyDataSource();
        this.createEmployeesCompanyGrid();
    },

    createEmployeesCompanyGrid: function () {
        var that = this;
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
                field: "BirthDate",
                title: "Fecha de Nacimiento",
                width: 130,
                groupable: "false",
                template: "#= Templates.getColumnTemplateDate(data.BirthDate) #"
            }, {
                field: "Email",
                title: "Email",
                width: 100,
                groupable: "false"
            }, {
                field: "DNI",
                title: "DNI",
                width: 70,
                groupable: "false"
            }, {
                field: "WorkStation",
                title: "Puesto de Trabajo",
                width: 160
            }, {
                field: "ProfessionalCategory",
                title: "Categoría",
                width: 160
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
        kendo.bind($("#" + this.gridEmployeesCompanyId), this);
    },

    createEmployeesCompanyDataSource: function () {
        var that = this;
        this.employeesCompanyDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        FirstName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        LastName: { type: "string" },
                        PhoneNumber: { type: "string" },
                        BirthDate: { type: "date", format: "{0:dd/MM/yy}", defaultValue: new Date() },
                        ChargeDate: { type: "date" },
                        Email: { type: "string" },
                        DNI: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        WorkStation: { type: "string" },
                        ProfessionalCategory: { type: "string" },
                        UserStateId: { type: "number", defaultValue: 1 },
                        CompanyId: { type: "number", defaultValue: that.companyId }
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
                        this.cancelChanges();
                    } else {
                        GeneralData.showNotification(Constants.ok, "", "success");
                    }
                }
            },
            pageSize: 10
        });
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createUser'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
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
                        that.goToSubscribeEmployeeCompany(userId, false);
                    }
                },
                {
                    text: "Borrar", action: function ()
                    {
                        var grid = $("#" + EmployeesCompany.gridEmployeesCompanyId).data("kendoGrid");
                        var item = grid.dataSource.get(userId);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
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
    }

});
