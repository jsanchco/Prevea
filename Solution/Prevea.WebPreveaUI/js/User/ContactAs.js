var ContactAs = kendo.observable({

    gridContactAsId: "gridContactAs",
    confirmId: "confirm",

    contactAsDataSource: null,

    init: function () {
        this.createContactAsDataSource();
        this.createContactAsGrid();
    },

    createContactAsDataSource: function () {
        this.contactAsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        FirstName: { type: "string" },
                        LastName: { type: "string" },
                        Initials: { type: "string" },
                        RoleDescription: { type: "string" },
                        CompanyName: { type: "string" },
                        CompanyEnrollment: { type: "string" },
                        Nick: { type: "string" },
                        Password: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/User/ContactAs_Read",
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

    createContactAsGrid: function () {
        $("#" + this.gridContactAsId).kendoGrid({
            columns: [{
                field: "FirstName",
                title: "Nombre",
                width: 120,
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
                field: "RoleDescription",
                title: "Rol",
                width: 140
            }, {
                field: "CompanyName",
                title: "Empresa",
                width: 110
            }, {
                field: "CompanyEnrollment",
                title: "Nº Empresa",
                width: 140
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,                
                template: "#= ContactAs.getColumnTemplateCommands(data) #"
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
            dataSource: this.contactAsDataSource,
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
            "<a toggle='tooltip' title='Ir a Usuario' onclick='ContactAs.goToUser(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'><i class='fa fa-share-square' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
            data.Nick,
            data.Password);
        html += kendo.format("</div>");

        return html;
    },

    goToContactAs: function() {
        var params = {
            url: "/User/ContactAs",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToUser: function (nick, password) {
        $.ajax({
            url: "/Login/CallbacksValidateUser",
            method: "POST",
            dataType: "json",
            data: {
                User: nick,
                Password: password
            },
            success: function (response) {
                var login_status = response.login_status;
                var urlSartPage = response.urlSartPage;

                if (login_status === "invalid") {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
                else
                if (login_status === "success") {
                        window.location.href = urlSartPage;
                }
            }
        });
    }
});