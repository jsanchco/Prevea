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
                        Id: { type: "number", defaultValue: 0 },
                        FirstName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        LastName: { type: "string" },
                        Initials: { type: "string", editable: false },
                        RoleId: { type: "number", validation: { required: { message: " Campo Obligatorio " } } },
                        RoleName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        RoleDescription: { type: "string" },
                        PhoneNumber: { type: "string" },
                        Address: { type: "string" },
                        Province: { type: "string" },
                        Email: { type: "string" },
                        DNI: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        UserStateId: { type: "number", defaultValue: 1 },
                        UserParentId: { type: "number", defaultValue: GeneralData.userId },
                        UserParentInitials: { type: "string", editable: false, defaultValue: GeneralData.userInitials },
                        BirthDate: { type: "date", defaultValue:  new Date() },
                        ChargeDate: { type: "date", defaultValue:  new Date() }
                    }
                }
            },
            transport: {
                read: {
                    url: "/User/Users_Read",
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
                field: "UserParentInitials",
                title: "Creado por",
                width: 90
            }, {
                field: "PhoneNumber",
                title: "Teléfono",
                width: 80,
                groupable: "false"
            }, {
                field: "Address",
                title: "Dirección",
                width: 150,
                groupable: "false"
            }, {
                field: "Province",
                title: "Provincia",
                width: 100
            }, {
                field: "Email",
                title: "Email",
                width: 100,
                groupable: "false"
            }, {
                field: "DNI",
                title: "DNI",
                width: 100,
                groupable: "false"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,                
                template: "#= Users.getColumnTemplateCommands(data) #"
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
        if (data.UserStateId === 2 || data.UserStateId === 3) {
            html += kendo.format("<a toggle='tooltip' title='Dar de Alta' onclick='Users.goToSubscribeUser(\"{0}\", true)' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-thumbs-up' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        } else {
            html += kendo.format("<a toggle='tooltip' title='Editar' onclick='Users.goToEditUser(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
            html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Users.goToDeleteUser(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }        
        html += kendo.format("</div>");

        return html;
    },

    goToContactUs: function() {
        var params = {
            url: "/User/ContactUs",
            data: {}
        };
        GeneralData.goToActionController(params);
    }
});