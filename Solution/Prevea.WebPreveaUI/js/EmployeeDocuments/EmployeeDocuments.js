var EmployeeDocuments = kendo.observable({

    gridHeaderEmployeeDocumentsId: "gridHeaderEmployeeDocuments",
    headerEmployeeDocumentsDataSource: null,

    init: function () {
        kendo.culture("es-ES");

        this.createHeaderEmployeeDocumentsDataSource();
        this.createHeaderEmployeeDocumentsGrid();
    },

    goToEmployeeDocuments: function () {
        var params = {
            url: "/Employees/Documents",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    createHeaderEmployeeDocumentsDataSource: function() {
        this.headerEmployeeDocumentsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    fields: {
                        RequestMedicalExaminationEmployeeId: { type: "number" },
                        EmployeeId: { type: "number" },
                        Description: { type: "string" },
                        Date: { type: "date" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Employees/HeaderEmployeeDocuments_Read",
                    dataType: "jsonp"
                }
            },
            pageSize: 10
        });
    },

    createHeaderEmployeeDocumentsGrid: function() {
        $("#" + this.gridHeaderEmployeeDocumentsId).kendoGrid({
            columns: [
                {
                field: "Description",
                title: "Descripcion",
                template: "#= Templates.getColumnTemplateIncrease(data.Description) #"
                },
                {
                    field: "Date",
                    title: "Fecha",
                    template: "#= Templates.getColumnTemplateDate(data.Date) #"
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
            dataSource: this.headerEmployeeDocumentsDataSource,
            detailTemplate: this.getTemplateChildren(),
            detailInit: this.childrenDocuments,
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            }
        });
    },

    getTemplateChildren: function () {
        var html = "<div id='templateGridMedicalExaminationDocuments' class='templateChildren'>";
        html += "<H2 style='text-align: center;'>Documentos</H2><br />";
        html += "<div id='gridDocuments' class='gridGridMedicalExaminationDocuments' style='margin: 5px;'></div><br /><br />";
        html += "</div>";

        return html;
    },

    childrenDocuments: function(e) {
        var dataSourceChildren = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Name: { type: "string" },
                        Description: { type: "string" },
                        AreaId: { type: "number" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Employees/RequestMedicalExaminationDocuments_Read",
                    dataType: "jsonp",
                    data: {
                        requestMedicalExaminationEmployeeId: e.data.RequestMedicalExaminationEmployeeId
                    }
                },
                parameterMap: function(options, operation) {
                    if (operation === "read") {
                        return {
                            requestMedicalExaminationEmployeeId: options.requestMedicalExaminationEmployeeId
                        };
                    }

                    return null;               
                }
            },
            batch: true,
            pageSize: 20
        });

        var detailRow = e.detailRow;
        var classGridDetail =
            kendo.format("{0}gridGridMedicalExaminationDocuments", e.data.id);
        detailRow.find(".gridGridMedicalExaminationDocuments").addClass(classGridDetail);

        detailRow.find(".gridGridMedicalExaminationDocuments").kendoGrid({
            columns: [
                {
                    field: "Name",
                    title: "Nombre",
                    width: 200,
                    template: "#= Templates.getColumnTemplateBold(data.Name) #"
                }, {
                    field: "Description",
                    title: "Descripción",
                    template: "#= Templates.getColumnTemplateBold(data.Description) #"
                }, {
                    title: "Comandos",
                    field: "Commands",
                    width: 120,
                    groupable: "false",
                    filterable: false,
                    template: "#= EmployeeDocuments.getColumnTemplateCommands(data) #"
                }
            ],
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
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: false
        });

        var grid = detailRow.find(".gridGridMedicalExaminationDocuments").data("kendoGrid");
        grid.setDataSource(dataSourceChildren);                
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";

        if (data.AreaStoreInServer === false) {
            html += kendo.format(
                "<a toggle='tooltip' title='Abrir Documento' onclick='DocumentsMedicalExamination.openMedicalExamination(\"{0}\")' target='_blank' style='cursor: pointer;'><img style='margin-top: -9px;' src='../../Images/pdf_opt.png'></a></div></a>&nbsp;&nbsp;",
                data.Id);
        } else {
            html += kendo.format(
                "<a toggle='tooltip' title='Abrir Documento' onclick='GeneralData.goToOpenFile(\"{0}\")' target='_blank' style='cursor: pointer;'><img style='margin-top: -9px;' src='../../Images/pdf_opt.png'></a></div></a>&nbsp;&nbsp;",
                data.Id);
        }   

        return html;
    }
});