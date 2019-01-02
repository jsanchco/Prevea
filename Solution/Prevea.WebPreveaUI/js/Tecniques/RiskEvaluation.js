var RiskEvaluation = kendo.observable({

    cnaeId: null,
    workStationId: null,

    gridRiskEvaluationId: "gridRiskEvaluation",
    confirmId: "confirm",

    riskEvaluationDataSource: null,
    deltaCodesDataSource: null,
    probabilityDataSource: null,
    severityDataSource: null,

    init: function (cnaeId, workStationId) {
        kendo.culture("es-ES");

        this.cnaeId = cnaeId;
        this.workStationId = workStationId;

        this.createRiskEvaluationDataSource();
        this.createDeltaCodesDataSource();
        this.createProbabilityDataSource();
        this.createSeverityDataSource();

        this.createRiskEvaluationGrid();
    },

    goToWorkStations: function () {
        var params = {
            url: "/Tecniques/WorkStations",
            data: {
                cnaeSelected: this.cnaeId 
            }
        };
        GeneralData.goToActionController(params);
    },

    goToRiskEvaluation: function () {
        var params = {
            url: "/Tecniques/RiskEvaluation",
            data: {
                cnaeId: this.cnaeId,
                workStationId: this.workStationId
            }
        };
        GeneralData.goToActionController(params);
    },

    createRiskEvaluationDataSource: function () {
        this.riskEvaluationDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        WorkStationId: { type: "number", defaultValue: RiskEvaluation.workStationId },
                        DeltaCodeId: { type: "number" },
                        DuplicateDeltaCodeId: { type: "string", editable: false },
                        DeltaCodeName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Probability: { type: "number" },
                        ProbabilityName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Severity: { type: "number" },
                        SeverityName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        RiskValue: { type: "number" },
                        RiskValueName: { type: "string", validation: { required: { message: " Campo Obligatorio " } }, editable: false },
                        Priority: { type: "number" },
                        PriorityName: { type: "string", validation: { required: { message: " Campo Obligatorio " } }, editable: false }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Tecniques/RiskEvaluations_Read",
                    dataType: "jsonp",
                    data: {
                        cnaeId: this.cnaeId,
                        workStationId: this.workStationId
                    }
                },
                create: {
                    url: "/Tecniques/RiskEvaluations_Create",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Tecniques/RiskEvaluations_Destroy",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Tecniques/RiskEvaluations_Update",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read" && options) {
                        return {
                            cnaeId: options.cnaeId,
                            workStationId: options.workStationId
                        };
                    }

                    if (operation !== "read" && options) {
                        return { riskEvaluation: kendo.stringify(options) };
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
            change: function (e) {
                var grid = $("#" + RiskEvaluation.gridRiskEvaluationId).data("kendoGrid");
                if (e.field === "DeltaCodeId") {
                    e.items[0].set("DuplicateDeltaCodeId", e.items[0].DeltaCodeId.toString());

                    grid.element.find("tr[data-uid='" + e.items[0].uid + "'] td:eq(" + 0 + ")").text(e.items[0].DeltaCodeId.toString());
                }
                if (e.field === "Probability" || e.field === "Severity") {
                    var valueRisk = RiskEvaluation.getRiskValueAndPriority(e.items[0]);
                    if (valueRisk != null) {
                        e.items[0].set("RiskValue", valueRisk.value);
                        e.items[0].set("Priority", valueRisk.value);
                        e.items[0].set("RiskValueName", valueRisk.risk);
                        e.items[0].set("PriorityName", valueRisk.priority);

                        grid.element.find("tr[data-uid='" + e.items[0].uid + "'] td:eq(" + 4 + ")").text(valueRisk.risk);
                        grid.element.find("tr[data-uid='" + e.items[0].uid + "'] td:eq(" + 5 + ")").text(valueRisk.priority);
                    } else {
                        grid.element.find("tr[data-uid='" + e.items[0].uid + "'] td:eq(" + 4 + ")").text("");
                        grid.element.find("tr[data-uid='" + e.items[0].uid + "'] td:eq(" + 5 + ")").text("");
                    } 
                }
            },
            pageSize: 10
        });
    },

    createRiskEvaluationGrid: function () {
        $("#" + this.gridRiskEvaluationId).kendoGrid({
            columns: [
                {
                    field: "DuplicateDeltaCodeId",
                    title: "Código",
                    width: 100,
                    template: "#= RiskEvaluation.getColumnTemplateDuplicateDeltaCode(data.DuplicateDeltaCodeId) #"
                },
                {
                    field: "DeltaCodeId",
                    title: "Identificación del Riesgo",
                    width: 220,
                    editor: RiskEvaluation.deltaCodesDropDownEditor,
                    template: "#=DeltaCodeName#"
                },
                {
                    field: "Probability",
                    title: "Probabilidad",
                    width: 140,
                    editor: RiskEvaluation.probabilityDropDownEditor,
                    template: "#=ProbabilityName#"
                },
                {
                    field: "Severity",
                    title: "Severidad",
                    width: 120,
                    editor: RiskEvaluation.severityDropDownEditor,
                    template: "#=SeverityName#"
                },
                {
                    field: "RiskValueName",
                    title: "Valor del Riesgo",
                    width: 160,
                    template: "#=RiskValueName#"
                },
                {
                    field: "PriorityName",
                    title: "Prioridad",
                    width: 120,
                    template: "#=PriorityName#"
                },
                {
                    field: "Preventive",
                    title: "Medidas Preventivas"
                    //encoded: false
                    //editor: RiskEvaluation.preventiveEditor,
                    //template: "#= RiskEvaluation.getColumnTemplatePreventive(data.Preventive) #"
                },
                {
                    title: "Comandos",
                    width: 120,
                    groupable: "false",
                    filterable: false,
                    template: "#= RiskEvaluation.getColumnTemplateCommands(data) #"
                }
            ],
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
            dataSource: this.riskEvaluationDataSource,
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
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }
        });
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' id='createRiskEvaluation'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html;

        html = "<div align='center'>";        
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='RiskEvaluation.goToEditRiskEvaluation(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Detalle' onclick='RiskEvaluation.goToDetailRiskEvaluation(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id, this.cnaeId, this.workStationId);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='RiskEvaluation.goToDeleteRiskEvaluation(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToEditRiskEvaluation: function (id) {
        var grid = $("#" + RiskEvaluation.gridRiskEvaluationId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDetailRiskEvaluation: function (id) {
        var params = {
            url: "/Tecniques/DetailRiskEvaluation",
            data: {
                riskEvaluationId: id,
                cnaeId: RiskEvaluation.cnaeId, 
                workStationId: RiskEvaluation.workStationId,
                selectTabId: 0,
                notification: ""
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeleteRiskEvaluation: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Técnicas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> esta Evaluación de Riesgos?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + that.gridRiskEvaluationId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    createDeltaCodesDataSource: function () {
        this.deltaCodesDataSource = new kendo.data.DataSource({
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
                    url: "/Tecniques/GetDeltaCodes",
                    dataType: "jsonp",
                    data: {
                        workStationId: RiskEvaluation.workStationId
                    }
                }
            }
        });
    },

    deltaCodesDropDownEditor: function (container, options) {
        RiskEvaluation.deltaCodesDataSource.read();

        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Name",
                optionLabel: "Selecciona ...",
                dataValueField: "Id",
                dataSource: RiskEvaluation.deltaCodesDataSource,
                dataBound: function (e) {
                    e.sender.list.width("auto").find("li").css({ "white-space": "nowrap", "padding-right": "25px" });
                }
            });
    },

    getColumnTemplateDuplicateDeltaCode: function (code) {
        if (code === null || code === 0) {
            return "";
        }

        var html = kendo.format("<div style='font-size: 15px; font-weight: bold'>{0}</div>",
            code);

        return html;
    },

    getColumnTemplatePreventive: function (text) {
        var html = kendo.format("<textarea rows='10'>{0}</textarea>",
            text);

        return html;
    },

    createProbabilityDataSource: function () {
        this.probabilityDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        Name: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Tecniques/GetProbabilities",
                    dataType: "jsonp"
                }
            }
        });

        this.probabilityDataSource.read();
    },

    probabilityDropDownEditor: function (container, options) {
        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Name",
                optionLabel: "Selecciona ...",
                dataValueField: "Id",
                dataSource: RiskEvaluation.probabilityDataSource
            });
    },

    createSeverityDataSource: function () {
        this.severityDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        Name: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Tecniques/GetSeverities",
                    dataType: "jsonp"
                }
            }
        });

        this.severityDataSource.read();
    },

    severityDropDownEditor: function (container, options) {
        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Name",
                optionLabel: "Selecciona ...",                
                dataValueField: "Id",
                dataSource: RiskEvaluation.severityDataSource,
                dataBound: function (e) {
                    e.sender.list.width("auto").find("li").css({ "white-space": "nowrap", "padding-right": "25px" });
                }
            });
    },

    preventiveEditor: function (container, options) {
        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoEditor({
                resizable: {
                    content: false,
                    toolbar: true
                }
        });
    },

    getRiskValueAndPriority: function(data) {
        if (data.Probability === 1 && data.Severity === 1) {
            return {
                value: 1,
                risk: "Trivial",
                priority: "Baja"
            };
        }
        if (data.Probability === 1 && data.Severity === 2) {
            return {
                value: 2,
                risk: "Tolerable",
                priority: "Mediana"
            };
        }
        if (data.Probability === 1 && data.Severity === 3) {
            return {
                value: 3,
                risk: "Moderado",
                priority: "Mediana - Alta"
            };
        }
        if (data.Probability === 2 && data.Severity === 1) {
            return {
                value: 2,
                risk: "Tolerable",
                priority: "Mediana"
            };
        }
        if (data.Probability ===2 && data.Severity === 2) {
            return {
                value: 3,
                risk: "Moderado",
                priority: "Mediana - Alta"
            };
        }
        if (data.Probability === 2 && data.Severity === 3) {
            return {
                value: 4,
                risk: "Importante",
                priority: "Alta"
            };
        }
        if (data.Probability === 3 && data.Severity === 1) {
            return {
                value: 3,
                risk: "Moderado",
                priority: "Mediana - Alta"
            };
        }
        if (data.Probability === 3 && data.Severity === 2) {
            return {
                value: 4,
                risk: "Importante",
                priority: "Alta"
            };
        }
        if (data.Probability === 3 && data.Severity === 3) {
            return {
                value: 5,
                risk: "Intolerable",
                priority: "Inmediata"
            };
        }

        return null;
    }
});