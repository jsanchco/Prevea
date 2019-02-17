var ContactPersonsCompany = kendo.observable({

    gridContactPersonsCompanyId: "gridContactPersonsCompany",
    confirmId: "confirm",

    companyId: null,

    contactPersonsCompanyDataSource: null,
    contactPersonTypeDataSorce: null,
    isInvited: false,
    isContactPerson: false,

    init: function (companyId) {
        this.companyId = companyId;

        this.createContactPersonsCompanyDataSource();
        this.createContactPersonTypeDataSource();
        this.createContactPersonsCompanyGrid();

        if (GeneralData.userRoleId === Constants.role.ContactPerson) {
            this.isContactPersonInvited();
            //this.isContactPersonContactPerson();
        }        
    },

    createContactPersonsCompanyDataSource: function () {
        var that = this;
        this.contactPersonsCompanyDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        FirstName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        LastName: { type: "string" },
                        PhoneNumber: { type: "string" },
                        Email: { type: "string" },
                        DNI: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        WorkStationCustom: { type: "string" },
                        UserStateId: { type: "number", defaultValue: 1 },
                        CompanyId: { type: "number", defaultValue: that.companyId },
                        BirthDate: { type: "date", defaultValue:  new Date() },
                        ChargeDate: { type: "date" },
                        ContactPersonTypeId: { type: "number" },
                        ContactPersonTypeDescription: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Companies/ContactPersonsCompany_Read",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                },
                update: {
                    url: "/Companies/ContactPersonsCompany_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Companies/ContactPersonsCompany_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Companies/ContactPersonsCompany_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { companyId: options.companyId };
                    }
                    if (operation !== "read" && options) {
                        return { contactPerson: kendo.stringify(options) };
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
            pageSize: 10
        });
    },

    createContactPersonTypeDataSource: function () {
        ContactPersonsCompany.contactPersonTypeDataSorce = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        Name: { type: "string" },
                        Description: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Companies/GetContactPersonTypesRemainingByCompany",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { companyId: options.companyId };
                    }
 
                    return null;
                }
            }
        });
    },

    createContactPersonsCompanyGrid: function () {
        var that = this;
        $("#" + this.gridContactPersonsCompanyId).kendoGrid({
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
                field: "Email",
                title: "Email",
                width: 160,
                groupable: "false"
            }, {
                field: "DNI",
                title: "DNI",
                width: 100,
                groupable: "false"
            }, {
                field: "ContactPersonTypeId",
                title: "Tipo",
                width: 160,
                editor: ContactPersonsCompany.contactPersonTypesDropDownEditor,
                template: "#= ContactPersonTypeDescription #"
            }, {
                field: "WorkStationCustom",
                title: "Puesto de Trabajo",
                width: 180
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
                var grid = $("#" + that.gridContactPersonsCompanyId).data("kendoGrid");
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
            $("#addContactPerson").removeAttr("disabled");
            $("#addContactPerson").prop("disabled", true);

        }
    },

    contactPersonTypesDropDownEditor: function (container, options) {
        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Description",
                optionLabel: "Selecciona ...",
                dataValueField: "Id",
                dataSource: {
                    schema: {
                        model: {
                            id: "Id",
                            fields: {
                                Id: { type: "number" },
                                Name: { type: "string" },
                                Description: { type: "string" }
                            }
                        }
                    },
                    transport: {
                        read: {
                            url: "/Companies/GetContactPersonTypesRemainingByCompany",
                            dataType: "jsonp",
                            data: {
                                companyId: ContactPersonsCompany.companyId,
                                contactPersonTypeSelected: options.model.ContactPersonTypeId
                            }
                        },
                        parameterMap: function(options, operation) {
                            if (operation === "read") {
                                return {
                                    companyId: options.companyId,
                                    contactPersonTypeSelected: options.contactPersonTypeSelected
                                };
                            }

                            return null;
                        }
                    }
                }
            });
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' id='createUser'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button' id='addContactPerson'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";        
        if (data.UserStateId === 2 || data.UserStateId === 3) {
            html += kendo.format("<a toggle='tooltip' title='Dar de Alta' onclick='ContactPersonsCompany.goToSubscribeContactPersonsCompany(\"{0}\", true)' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-thumbs-up' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        } else {
            if (ContactPersonsCompany.isInvited === false) {
                if (GeneralData.userId !== data.Id) {
                    html += kendo.format(
                        "<a toggle='tooltip' title='Editar' onclick='ContactPersonsCompany.goToEditContactPersonsCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                        data.Id);

                    html += kendo.format(
                        "<a toggle='tooltip' title='Borrar' onclick='ContactPersonsCompany.goToDeleteContactPersonsCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                        data.Id);
                }
            }
        }        
        html += kendo.format("</div>");

        return html;
    },

    goToEditContactPersonsCompany: function (userId) {
        var grid = $("#" + ContactPersonsCompany.gridContactPersonsCompanyId).data("kendoGrid");
        var item = grid.dataSource.get(userId);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteContactPersonsCompany: function (userId) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Empresas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Dar de Baja/Borrar</strong> a la Persona de Contacto?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Dar de Baja", action: function () {
                        that.goToSubscribeContactPersonsCompany(userId, false);
                    }
                },
                {
                    text: "Borrar", action: function ()
                    {
                        var grid = $("#" + ContactPersonsCompany.gridContactPersonsCompanyId).data("kendoGrid");
                        var item = grid.dataSource.get(userId);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToSubscribeContactPersonsCompany: function (userId, subscribe) {
        var that = this;

        $.ajax({
            url: "/Companies/ContactPersonsCompany_Subscribe",
            type: "post",
            cache: false,
            datatype: "json",
            data: {
                companyId: that.companyId,
                userId: userId,
                subscribe: subscribe
            },
            success: function (result) {
                var grid = $("#" + ContactPersonsCompany.gridContactPersonsCompanyId).data("kendoGrid");
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

    isContactPersonInvited: function () {
        $.ajax({
            url: "/Companies/IsContactPersonInvited",
            data: {
                userId: GeneralData.userId
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result === true) {
                    $("#addContactPerson").removeAttr("disabled");
                    $("#addContactPerson").prop("disabled", true);

                    ContactPersonsCompany.isInvited = true;
                }
            },
            error: function () {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    },

    isContactPersonContactPerson: function () {
        $.ajax({
            url: "/Companies/IsContactPersonContactPerson",
            data: {
                userId: GeneralData.userId
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result === true) {
                    ContactPersonsCompany.isContactPerson = true;
                }
            },
            error: function () {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    }
});