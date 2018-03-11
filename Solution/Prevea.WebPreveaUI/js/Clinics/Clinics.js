var Clinics = kendo.observable({

    gridClinicsId: "gridClinics",
    confirmId: "confirm",

    clinicsDataSource: null,

    init: function () {
        this.createClinicsDataSource();
        this.createClinicsGrid();
    },

    createClinicsDataSource: function () {
        this.clinicsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Name: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Address: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Province: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Description: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Clinics/Clinics_Read",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Clinics/Clinic_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Clinics/Clinic_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Clinics/Clinic_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { clinic: kendo.stringify(options) };
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

    createClinicsGrid: function () {
        $("#" + this.gridClinicsId).kendoGrid({
            columns: [{
                field: "Name",
                title: "Nombre",
                width: 200,
                template: "#= Templates.getColumnTemplateIncrease(data.Name) #"
            }, {
                field: "Address",
                title: "Dirección",
                width: 200
            }, {
                field: "Province",
                title: "Provincia",
                width: 100
            },{
                field: "Description",
                title: "Descripción",
                width: 200,
                groupable: "false"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,                
                template: "#= Clinics.getColumnTemplateCommands(data) #"
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
            dataSource: this.clinicsDataSource,
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
        kendo.bind($("#" + this.gridClinicsId), this);
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createClinic'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";        
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='Clinics.goToEditClinic(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Clinics.goToDeleteClinic(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToEditClinic: function (id) {
        var grid = $("#" + Clinics.gridClinicsId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteClinic: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Vigilancia de la Salud</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> la Clínica?",
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

    goToClinics: function() {
        var params = {
            url: "/Clinics/Clinics",
            data: {}
        };
        GeneralData.goToActionController(params);
    }
});