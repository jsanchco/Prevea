﻿var HistoricMedicalExamination = kendo.observable({

    gridRequestHistoricMedicalExaminationsId: "gridRequestHistoricMedicalExaminations",

    contactPersonId: null,

    historicRequestMedicalExaminationsDataSource: null,

    init: function (contactPersonId) {
        kendo.culture("es-ES");

        this.contactPersonId = contactPersonId;

        this.createHistoricRequestMedicalExaminationsDataSource();
        this.createHistoricRequestMedicalExaminationsGrid();
    },

    createHistoricRequestMedicalExaminationsDataSource: function () {
        this.historicRequestMedicalExaminationsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Date: { type: "date", format: "{0:dd/MM/yyyy}", defaultValue: new Date() },
                        RequestMedicalExaminationStateId: { type: "number", defaultValue: Constants.requestMedicalExaminationState.Pending },
                        RequestMedicalExaminationStateDescription: { type: "string", defaultValue: "Pendiente" },
                        ContactPersonId: { type: "number", defaultValue: HistoricMedicalExamination.contactPersonId }
                    }
                }
            },
            transport: {
                read: {
                    url: "/HistoricMedicalExamination/RequestMedicalExaminations_Read",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                },
                update: {
                    url: "/HistoricMedicalExamination/RequestMedicalExaminations_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/HistoricMedicalExamination/RequestMedicalExaminations_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/HistoricMedicalExamination/RequestMedicalExaminations_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { requestMedicalExamination: kendo.stringify(options) };
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

    createHistoricRequestMedicalExaminationsGrid: function () {
        $("#" + this.gridRequestHistoricMedicalExaminationsId).kendoGrid({
            columns: [{
                field: "Date",
                title: "Fecha Posible",
                template: "#= Templates.getColumnTemplateDateIncrease(data.Date) #"
            }, {
                field: "RequestMedicalExaminationStateDescription",
                title: "Estado de la Petición",
                width: 400,
                template: "#= HistoricMedicalExamination.getColumnTemplateRequestMedicalState(data) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= HistoricMedicalExamination.getColumnTemplateCommands(data) #"
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
            dataSource: this.historicRequestMedicalExaminationsDataSource,
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
        kendo.bind($("#" + this.gridRequestHistoricMedicalExaminationsId), this);
    },

    getColumnTemplateRequestMedicalState: function (data) {
        var html = kendo.format("<div style='float: left; text-align: left; display: inline;'>{0}</div>",
            data.RequestMedicalExaminationStateDescription);

        if (data.RequestMedicalExaminationStateId === Constants.requestMedicalExaminationState.Pending) {
            html += kendo.format("<div id='circleError' style='float: right; text-align: right;'></div></div>");
        }
        if (data.RequestMedicalExaminationStateId === Constants.requestMedicalExaminationState.Validated) {
            html += kendo.format("<div id='circleSuccess' style='float: right; text-align: right;'></div></div>");
        }

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='WorkCentersCompany.goToEditWorkCentersCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='WorkCentersCompany.goToDeleteWorkCentersCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createRequestHistoricMedicalExamination'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    goToHistoric: function() {
        var params = {
            url: "/HistoricMedicalExamination/HistoricMedicalExamination",
            data: {}
        };
        GeneralData.goToActionController(params);
    }
});