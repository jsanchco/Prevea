var Users = kendo.observable({

    gridUsersId: "gridUsers",
    confirmId: "confirm",

    usersDataSource: null,
    rolesDataSorce: null,

    showAll: false,

    init: function () {
        this.createUsersDataSource();
        this.createRolesDataSource();
        this.createUsersGrid();

        this.usersDataSource.filter({ field: "UserStateId", operator: "eq", value: 1 });
    },

    createUsersDataSource: function () {
        this.usersDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        FirstName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        LastName: { type: "string" },
                        RoleId: { type: "number", validation: { required: { message: " Campo Obligatorio " } } },
                        RoleName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        RoleDescription: { type: "string" },
                        PhoneNumber: { type: "string" },
                        Email: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        DNI: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        UserStateId: { type: "number", defaultValue: 1 }
                    }
                }
            },
            transport: {
                read: {
                    url: "/User/Users_Read",
                    dataType: "jsonp"
                },
                update: {
                    url: "/User/Users_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/User/Users_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/User/Users_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { user: kendo.stringify(options) };
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

    createRolesDataSource: function() {
        Users.rolesDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "RoleId",
                    fields: {
                        RoleId: { type: "number" },
                        RoleName: { type: "string" },
                        RoleDescription: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/User/GetRoles",
                    dataType: "jsonp"
                }
            }
        });

        Users.rolesDataSource.read();
    },

    createUsersGrid: function () {
        var that = this;
        $("#" + this.gridUsersId).kendoGrid({
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
                field: "RoleId",
                title: "Rol",
                width: "180px",
                editor: Users.rolesDropDownEditor,
                template: "#=RoleName#",
                groupHeaderTemplate: "Agrupado : #= Users.getRoleName(value) #"
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
            dataSource: this.usersDataSource,
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
                var grid = $("#" + that.gridUsersId).data("kendoGrid");
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
        kendo.bind($("#" + this.gridUsersId), this);
    },

    rolesDropDownEditor: function (container, options) {
        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "RoleName",
                optionLabel: "Selecciona ...",
                dataValueField: "RoleId",
                dataSource: Users.rolesDataSource
            });
    },

    getRoleName: function (roleId) {
        if (Users.rolesDataSource.data().length === 0) {
            Users.rolesDataSource.read();
        }
        for (var index = 0; index < Users.rolesDataSource.data().length; index++) {
            if (Users.rolesDataSource.data()[index].RoleId === roleId) {
                return Users.rolesDataSource.data()[index].RoleName;
            }
        }
        return null;
    },
          
    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createUser'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span>";

        html += "<span style='float: right;'>";
        html += "<a id='showAll' class='btn btn-prevea' role='button' onclick='Users.applyFilter()'> Ver todos</a>";
        html += "</span>";

        html += "</div>";

        return html;
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

    goToUsers: function() {
        var params = {
            url: "/User/Users",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToEditUser: function (userId) {
        var grid = $("#" + Users.gridUsersId).data("kendoGrid");
        var item = grid.dataSource.get(userId);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteUser: function (userId) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "Usuarios",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Dar de Baja/Borrar</strong> al Usuario?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Dar de Baja", action: function () {
                        that.goToSubscribeUser(userId, false);
                    }
                },
                {
                    text: "Borrar", action: function ()
                    {
                        var grid = $("#" + Users.gridUsersId).data("kendoGrid");
                        var item = grid.dataSource.get(userId);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToSubscribeUser: function (userId, subscribe) {
        var that = this;

        $.ajax({
            url: "/User/Users_Subscribe",
            type: "post",
            cache: false,
            datatype: "json",
            data: {
                userId: userId,
                subscribe: subscribe
            },
            success: function (result) {
                var grid = $("#" + Users.gridUsersId).data("kendoGrid");
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

                Debug.writeln(result.Message);
            },
            error: function (result) {
                GeneralData.showNotification(Constants.ko, "", "error");

                Debug.writeln(result);
            }
        });
    },

    applyFilter: function () {
        if (this.showAll) {
            this.usersDataSource.filter({ field: "UserStateId", operator: "eq", value: 1 });
            $("a#showAll").text("Ver todos");
            this.showAll = false;
        } else {
            this.usersDataSource.filter({});
            $("a#showAll").text("Ver altas");
            this.showAll = true;
        }
    }

});