var Companies = kendo.observable({

    gridCompaniesId: "gridCompanies",
    btnAddDocumentId: "btnAddDocument",
    confirmId: "confirm",

    documentsDataSource: null,

    init: function () {
        this.createCompaniesDataSource();
        this.createGridCompanies();
        this.setUpWidgets();
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
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: false

        });
        kendo.bind($("#" + this.gridCompaniesId), this);
    },

    createCompaniesDataSource: function () {
        this.companiesDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Enrollment: { type: "string" },
                        Name: { type: "string" },
                        Adress: { type: "string" },                        
                        NIF: { type: "string" },
                        ContactPersonName: { type: "string" },
                        ContactPersonPhoneNumber: { type: "string" },
                        ContactPersonEmail: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Company/Companies_Read",
                    dataType: "jsonp",
                    data: { }
                }
            },
            pageSize: 10
        });
    },

    setUpWidgets: function () {

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

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";

        html += kendo.format("<a toggle='tooltip' title='Detalle' onclick='Companies.goToDetailCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Companies.goToDeleteCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToCompanies: function () {
        var params = {
            url: "/Company/Companies",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToDetailCompany: function (id) {
        var params = {
            url: "/Company/DetailCompany",
            data: {
                id: id,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeleteCompany: function (id) {
        var params = {
            url: "/Company/DeleteCompany",
            data: {
                id: id
            }
        };
        alert("Sin desarrollar!!!");
        //GeneralData.goToActionController(params);
    },

    goToAddCompany: function () {
        var params = {
            url: "/Company/AddCompany",
            data: {}
        };
        GeneralData.goToActionController(params);
    }

});
