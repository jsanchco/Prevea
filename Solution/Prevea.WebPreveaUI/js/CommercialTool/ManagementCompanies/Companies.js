var Companies = kendo.observable({

    gridCompaniesId: "gridCompanies",
    confirmId: "confirm",

    companiesDataSource: null,

    showAll: false,

    init: function () {
        this.createCompaniesDataSource();
        this.createGridCompanies();

        this.companiesDataSource.filter(this.getFilter());
    },

    createCompaniesDataSource: function () {
        this.companiesDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Enrollment: { type: "string", editable: false },
                        Name: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Address: { type: "string", editable: false },
                        NIF: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        ContactPersonName: { type: "string", editable: false },
                        ContactPersonPhoneNumber: { type: "string" },
                        ContactPersonEmail: { type: "string" },
                        CompanyStateId: { type: "number", defaultValue: 1 }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Companies/Companies_Read",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                },
                destroy: {
                    url: "/Companies/Companies_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Companies/Companies_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { companyId: options.companyId };
                    }
                    if (operation !== "read" && options) {
                        return { company: kendo.stringify(options) };
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

    createGridCompanies: function () {
        $("#" + this.gridCompaniesId).kendoGrid({
            columns: [
                {
                    field: "Enrollment",
                    title: "Matrícula",
                    width: 250,
                    groupable: "false",
                    template: "#= Companies.getColumnTemplateEnrollment(data) #"
                }, {
                    field: "Name",
                    title: "Nombre",
                    width: 250,
                    groupable: "false"
                }, {
                    field: "NIF",
                    title: "Razón Social",
                    width: 150,
                    groupable: "false",
                    template: "#= Templates.getColumnTemplateRight(data.NIF) #"
                }, {
                    field: "Address",
                    title: "Dirección",
                    groupable: "false"
                }, {
                    field: "ContactPersonName",
                    title: "Persona de Contacto",
                    groupable: "false",
                    filterable: false,
                    template: "#= Companies.getColumnTemplateContactPerson(data) #"
                }, {
                    title: "Comandos",
                    field: "Commands",
                    width: 130,
                    groupable: "false",
                    filterable: false,
                    template: "#= Companies.getColumnTemplateCommands(data) #"
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
            dataSource: this.companiesDataSource,
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
            groupable: false,
            dataBound: function (e) {
                var grid = $("#" + Companies.gridCompaniesId).data("kendoGrid");
                var items = e.sender.items();
                items.each(function () {
                    var dataItem = grid.dataItem(this);
                    if (dataItem.CompanyStateId === 2 || dataItem.CompanyStateId === 3) {
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
        kendo.bind($("#" + this.gridCompaniesId), this);
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        //html += "<span name='create' class='k-grid-add' id='createCompany'>";
        //html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        //html += "</span>";

        html += "<span style='float: right;'>";
        html += "<a id='showAll' class='btn btn-prevea' role='button' onclick='Companies.applyFilter()'> Ver todos</a>";
        html += "</span>";

        html += "</div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        if (data.CompanyStateId === 2 || data.CompanyStateId === 3) {
            html += kendo.format("<a toggle='tooltip' title='Dar de Alta' onclick='Companies.subscribeCompany(\"{0}\", true)' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-thumbs-up' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        } else {
            html += kendo.format("<a toggle='tooltip' title='Detalle' onclick='Companies.goToDetailCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
            html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Companies.goToDeleteCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }
        html += kendo.format("</div>");

        return html;
    },

    getColumnTemplateEnrollment: function (data) {
        var html = kendo.format("<div style='text-align: center; font-size: 16px; font-weight: bold;'>{0}</div>", data.Enrollment);

        return html;
    },

    getColumnTemplateContactPerson: function (data) {
        var html = kendo.format("<div class='one-line'><div><strong>Nombre: </strong></div><div>{0}</div></div><div class='one-line'><div><strong>Teléfono: </strong></div><div>{1}</div></div><div class='one-line'><div><strong>Email: </strong></div><div>{2}</div></div>",
            data.ContactPersonName,
            data.ContactPersonPhoneNumber,
            data.ContactPersonEmail);

        return html;
    },

    goToCompanies: function () {
        var params = {
            url: "/Companies/Companies",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToDetailCompany: function (id) {
        var params = {
            url: "/Companies/DetailCompany",
            data: {
                id: id,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    },

    goToEditCompany: function (userId) {
        var grid = $("#" + Companies.gridCompaniesId).data("kendoGrid");
        var item = grid.dataSource.get(userId);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteCompany: function (companyId) {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Empresas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Dar de Baja</strong> a la Empresa?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Dar de Baja", action: function () {
                        Companies.subscribeCompany(companyId, false);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    subscribeCompany: function (companyId, subscribe) {
        $.ajax({
            url: "/Company/Company_Subscribe",
            type: "post",
            cache: false,
            datatype: "json",
            data: {
                companyId: companyId,
                subscribe: subscribe
            },
            success: function (data) {
                if (data.result.Status === 0) {
                    var grid = $("#" + Companies.gridCompaniesId).data("kendoGrid");
                    var item = grid.dataSource.get(companyId);
                    var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                    if (subscribe) {
                        item.set("CompanyStateId", 1);
                    } else {
                        item.set("CompanyStateId", 2);
                    }

                    var cellName = "Commands";
                    var cellIndex = grid.element.find("th[data-field = '" + cellName + "']").index();
                    var cell = tr.find("td:eq(" + cellIndex + ")");
                    cell.html(Companies.getColumnTemplateCommands(item));

                    GeneralData.showNotification(Constants.ok, "", "success");
                } else {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            },
            error: function (result) {
                GeneralData.showNotification(Constants.ko, "", "error");

                Debug.writeln(result);
            }
        });
    },

    applyFilter: function () {
        if (this.showAll) {
            this.companiesDataSource.filter(this.getFilter());
            $("a#showAll").text("Ver todos");
            this.showAll = false;
        } else {
            this.companiesDataSource.filter({});
            $("a#showAll").text("Ver altas");
            this.showAll = true;
        }           
    },

    getFilter: function() {
        var filter = {
            field: "CompanyStateId",
            operator: "eq",
            value: 1
        };

        return filter;
    }
});