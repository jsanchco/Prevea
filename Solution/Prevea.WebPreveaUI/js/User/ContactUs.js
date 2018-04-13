var ContactUs = kendo.observable({

    gridContactUsId: "gridContactUs",
    confirmId: "confirm",

    contactUsDataSource: null,

    init: function () {
        this.createContactUsDataSource();
        this.createContactUsGrid();
    },

    createContactUsDataSource: function () {
        this.contactUsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        FirstName: { type: "string" },
                        LastName: { type: "string" },
                        Initials: { type: "string" },
                        CompanyName: { type: "string" },
                        CompanyEnrollment: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/User/ContactUs_Read",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { user: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            pageSize: 20
        });
    },

    createContactUsGrid: function () {
        $("#" + this.gridContactUsId).kendoGrid({
            columns: [{
                field: "FirstName",
                title: "Nombre",
                width: 80,
                groupable: "false"
            }, {
                field: "LastName",
                title: "Apellidos",
                width: 130,
                groupable: "false"
            }, {
                field: "Initials",
                title: "Iniciales",
                width: 110,
                groupable: "false",
                template: "#= Templates.getColumnTemplateIncrease(data.Initials) #"
            }, {
                field: "RoleId",
                title: "Rol",
                width: 90,
                editor: Users.rolesDropDownEditor,
                template: "#=RoleDescription#",
                groupHeaderTemplate: "Agrupado : #= Users.getRoleDescription(value) #"
            }, {
                field: "CompanyName",
                title: "Empresa",
                width: 110
            }, {
                field: "CompanyEnrollment",
                title: "Nº Empresa",
                width: 110
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,                
                template: "#= ContactUs.getColumnTemplateCommands(data) #"
            }],
            pageable: {
                buttonCount: 2,
                pageSizes: [20, 40, "all"],
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
            dataSource: this.contactUsDataSource,
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
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";        
        html += kendo.format(
            "<a toggle='tooltip' title='Ir a Usuario' onclick='ContactUs.goToUser(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='fa fa-share-square' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
            data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToContactUs: function() {
        var params = {
            url: "/User/ContactUs",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToUser: function (userId) {
        alert("Usuaio " + userId);
        //var params = {
        //    url: "/User/ContactUs",
        //    data: {}
        //};
        //GeneralData.goToActionController(params);
    }
});