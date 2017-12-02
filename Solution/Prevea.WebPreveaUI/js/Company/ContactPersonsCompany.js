var ContactPersonsCompany = kendo.observable({

    gridContactPersonsCompanyId: "gridContactPersonsCompany",
    btnAddContactPersonCompanyId: "btnAddContactPersonCompany",
    confirmId: "confirm",

    companyId: null,

    contactPersonsCompanyDataSource: null,

    init: function (companyId) {
        this.companyId = companyId;

        this.createContactPersonsCompanyDataSource();
        this.createContactPersonsCompanyGrid();
    },

    createContactPersonsCompanyGrid: function () {
        $("#" + this.gridContactPersonsCompanyId).kendoGrid({
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
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= ContactPersonsCompany.getColumnTemplateCommands(data) #"
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
            dataSource: this.contactPersonsCompanyDataSource,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: false,
            dataBound: function (e) {
                var grid = $("#" + ContactPersonsCompany.gridContactPersonsCompanyId).data("kendoGrid");
                var items = e.sender.items();
                items.each(function () {
                    var dataItem = grid.dataItem(this);
                    if (dataItem.UserStateId === 3) {
                        this.className = "unSubscribe";
                    }
                });
            }
        });
        kendo.bind($("#" + this.gridContactPersonsCompanyId), this);
    },

    createContactPersonsCompanyDataSource: function () {
        this.contactPersonsCompanyDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        FirstName: { type: "string" },
                        LastName: { type: "string" },
                        PhoneNumber: { type: "string" },
                        Email: { type: "string" },
                        WorkStation: { type: "string" },
                        UserStateId: { type: "number" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Company/ContactPersonsCompany_Read",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                }
            },
            pageSize: 10
        });
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        if (data.UserStateId === 3) {
            html += kendo.format("<a toggle='tooltip' title='Dar de Alta' onclick='ContactPersonsCompany.goToSubscribeContactPersonCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-thumbs-up' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='ContactPersonsCompany.goToEditContactPersonCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='ContactPersonsCompany.goToDeleteContactPersonCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToEditContactPersonCompany: function (userId) {
        var params = {
            url: "/Company/AddContactPersonCompany",
            data: {
                companyId: this.companyId,
                userId: userId
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeleteContactPersonCompany: function (userId) {
        var params = {
            url: "/Company/DeleteContactPersonCompany",
            data: {
                companyId: this.companyId,
                userId: userId
            }
        };
        GeneralData.goToActionController(params);
    },

    goToAddContactPersonCompany: function() {
        var params = {
            url: "/Company/AddContactPersonCompany",
            data: {
                companyId: this.companyId,
                userId: 0
            }
        };
        GeneralData.goToActionController(params);
    },

    goToSubscribeContactPersonCompany: function (userId) {
        var params = {
            url: "/Company/SubscribeContactPersonCompany",
            data: {
                companyId: this.companyId,
                userId: userId,
                subscribe: true
            }
        };
        GeneralData.goToActionController(params);
    }

});
