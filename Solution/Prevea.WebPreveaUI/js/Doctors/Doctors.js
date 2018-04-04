var Doctors = kendo.observable({

    gridDoctorsId: "gridDoctors",
    confirmId: "confirm",
    //cont: 0,

    doctorsDataSource: null,

    init: function () {
        this.createDoctorsDataSource();
        this.createDoctorsGrid();
    },

    createDoctorsDataSource: function () {
        this.doctorsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        FirstName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        LastName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        CollegiateNumber: { type: "string" },
                        DNI: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        UserStateId: { type: "number", defaultValue: 1 },
                        UserParentId: { type: "number", defaultValue: GeneralData.userId },
                        BirthDate: { type: "date" },
                        ChargeDate: { type: "date" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Doctors/Doctors_Read",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Doctors/Doctor_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Doctors/Doctor_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Doctors/Doctor_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { doctor: kendo.stringify(options) };
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
            //change: function (e) {
            //    if (e.action != null && e.action === "itemchange") {
            //        var dataItem;
            //        var value;
            //        if (e.field === "FirstName") {
            //            Doctors.cont++;

            //            dataItem = e.items[0];

            //            value = kendo.format("{0} {1}", dataItem.LastName, Doctors.cont);
            //            dataItem.set("LastName", value);
            //        }

            //        if (e.field === "LastName") {
            //            Doctors.cont++;

            //            dataItem = e.items[0];

            //            value = kendo.format("{0} {1}", dataItem.FirstName, Doctors.cont);
            //            dataItem.set("LastName", value);
            //        }
            //    }
            //},
            pageSize: 10
        });
    },

    createDoctorsGrid: function () {
        $("#" + this.gridDoctorsId).kendoGrid({
            columns: [{                
                field: "FirstName",
                title: "Nombre",
                width: 300,
                template: "#= Templates.getColumnTemplateIncrease(data.FirstName) #"
            }, {
                field: "LastName",
                title: "Apellidos",
                template: "#= Templates.getColumnTemplateIncrease(data.LastName) #"
            }, {
                field: "CollegiateNumber",
                title: "Nº de Colegiado",
                template: "#= Templates.getColumnTemplateIncrease(data.CollegiateNumber) #"
            }, {
                field: "DNI",
                title: "DNI",
                width: 200,
                template: "#= Templates.getColumnTemplateIncreaseRight(data.DNI) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,                
                template: "#= Doctors.getColumnTemplateCommands(data) #"
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
            dataSource: this.doctorsDataSource,
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
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }
        });
        //kendo.bind($("#" + this.gridDoctorsId), this);

        if (GeneralData.userRoleId !== Constants.role.Super) {
            var grid = $("#" + this.gridDoctorsId).data("kendoGrid");
            grid.hideColumn("Commands");

            $("#createDoctor").removeAttr("disabled");
            $("#createDoctor").prop("disabled", true);
        }
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add'>";
        html += "<a id='createDoctor' class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";        
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='Doctors.goToEditDoctor(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Doctors.goToDeleteDoctor(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    getColumnTemplateNameIncrease: function(data) {
        var html = kendo.format("<div style='font-size: 15px; font-weight: bold'>{0} {1}</div>", data.FirstName, data.LastName);

        return html;
    },

    goToEditDoctor: function (id) {
        var grid = $("#" + Doctors.gridDoctorsId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteDoctor: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Vigilancia de la Salud</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> el Médico?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function ()
                    {
                        var grid = $("#" + that.gridClinicsId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToDoctors: function () {
        var params = {
            url: "/Doctors/Doctors",
            data: {}
        };
        GeneralData.goToActionController(params);
    }
});