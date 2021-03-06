﻿var ContractualsDocumentsCompany = kendo.observable({

    confirmId: "confirm",
    gridContractualsDocumentsCompanyId: "gridContractualsDocumentsCompany",

    companyId: null,

    addDocumentFirmedWindow: null,
    addDocumentFirmedId: "addDocumentFirmed",
    addAnnexWindow: null,
    addAnnexId: "addAnnex",

    contractualsDocumentsCompanyDataSource: null,
    contractualDocumentTypeDataSorce: null,

    init: function (companyId) {
        kendo.culture("es-ES");

        this.companyId = companyId;

        this.createContractualsDocumentsCompanyDataSource();
        this.createContractualDocumentTypeDataSource();
        this.createContractualsDocumentsCompanyGrid();
    },

    setUpAddDocumentFirmedWindow: function (contractualDocumentId) {
        var url = "/Companies/AddDocumentFirmed?contractualDocumentId=" + contractualDocumentId;
        this.addDocumentFirmedWindow = $("#" + this.addDocumentFirmedId);
        this.addDocumentFirmedWindow.kendoWindow({
            width: "330px",
            title: "Agregar Documento Firmado",
            visible: false,
            modal: true,
            actions: [
                "Close"
            ],
            content: url
        });
    },

    setUpAddAnnexWindow: function (contractualDocumentId) {
        var url = "/Companies/AddAnnex?contractualDocumentId=" + contractualDocumentId;
        this.addAnnexWindow = $("#" + this.addAnnexId);
        this.addAnnexWindow.kendoWindow({
            width: "330px",
            title: "Agregar Anexo",
            visible: false,
            modal: true,
            actions: [
                "Close"
            ],
            content: url
        });
    },

    createContractualsDocumentsCompanyDataSource: function () {
        var beginDate = new Date();
        var endDate = new Date();
        endDate.setFullYear(beginDate.getFullYear());
        endDate.setDate(endDate.getDate() - 1);

        this.contractualsDocumentsCompanyDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        CompanyId: { type: "number", defaultValue: ContractualsDocumentsCompany.companyId },
                        Enrollment: { type: "string", editable: false },
                        ContractualDocumentTypeId: { type: "number", validation: { required: { message: " Campo Obligatorio " } } },
                        ContractualDocumentTypeName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        BeginDate: { type: "date", defaultValue: beginDate, format: "{0:dd/MM/yy}" },
                        EndDate: { type: "date", defaultValue: endDate, format: "{0:dd/MM/yy}" },
                        UrlRelative: { type: "string" },
                        Observations: { type: "string" },
                        ContractualDocumentCompanyFirmedId: { type: "number", defaultValue: null },
                        ContractualDocumentCompanyFirmedEnrollment: { type: "string" },
                        ContractualDocumentCompanyFirmedUrlRelative: { type: "string" },
                        ContractualDocumentCompanyParentId: { type: "number", defaultValue: null }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Companies/ContractualsDocumentsCompany_Read",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                },
                destroy: {
                    url: "/Companies/ContractualsDocumentsCompany_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Companies/ContractualsDocumentsCompany_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { companyId: options.companyId };
                    }
                    if (operation !== "read" && options) {
                        return { contractualDocument: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            requestEnd: function (e) {
                if ((e.type === "update" || e.type === "destroy" || e.type === "create") && e.response !== null) {
                    var grid = $("#" + ContractualsDocumentsCompany.gridContractualsDocumentsCompanyId).data("kendoGrid");
                    if (typeof e.response.Errors !== "undefined") {
                        GeneralData.showNotification(e.response.Errors, "", "error");                        
                        if (e.type === "create") {
                            this.data().remove(this.data().at(0));

                            kendo.ui.progress(grid.element, false);
                        } else {
                            if (e.type === "destroy") {
                                ContractualsDocumentsCompany.contractualsDocumentsCompanyDataSource.read();
                            }                            
                            this.cancelChanges();
                        }                                   
                    } else {
                        if (e.type === "create") {
                            kendo.ui.progress(grid.element, false);

                            GeneralData.showNotification(Constants.ok, "", "success");
                            if (!e.response.UrlRelative) {
                                ContractualsDocumentsCompany.goToAddAnnex(e.response.Id);
                            }
                        }                                                
                    }
                }
            },
            pageSize: 10
        });        
    },

    createContractualDocumentTypeDataSource: function () {
        ContractualsDocumentsCompany.contractualDocumentTypeDataSorce = new kendo.data.DataSource({
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
                    url: "/Companies/GetContractualDocumentTypes",
                    dataType: "jsonp"
                }
            }
        });

        ContractualsDocumentsCompany.contractualDocumentTypeDataSorce.read();
    },

    createContractualsDocumentsCompanyGrid: function () {
        $("#" + this.gridContractualsDocumentsCompanyId).kendoGrid({
            columns: [
                {
                    field: "Enrollment",
                    title: "Matrícula",
                    width: 250,
                    groupable: "false",
                    template: "#= ContractualsDocumentsCompany.getColumnTemplateEnrollment(data) #"
                }, {
                    field: "ContractualDocumentTypeId",
                    title: "Tipo",
                    width: "150px",
                    editor: ContractualsDocumentsCompany.contractualDocumentTypeDropDownEditor,
                    template: "#= ContractualsDocumentsCompany.getContractualDocumentTypeDescription(data.ContractualDocumentTypeId) #",
                    groupHeaderTemplate: "Agrupado : #= ContractualsDocumentsCompany.getContractualDocumentTypeDescription(value) #"
                }, {
                    field: "BeginDate",
                    title: "Fecha Inicio",
                    template: "#= Templates.getColumnTemplateDate(data.BeginDate) #"
                }, {
                    field: "EndDate",
                    title: "Fecha Fin",
                    template: "#= Templates.getColumnTemplateDate(data.EndDate) #"
                }, {
                    field: "Observations",
                    title: "Observaciones",
                    groupable: "false"
                }, {
                    title: "Comandos",
                    field: "Commands",
                    width: 160,
                    groupable: "false",
                    filterable: false,
                    template: "#= ContractualsDocumentsCompany.getColumnTemplateCommands(data) #"
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
            dataSource: this.contractualsDocumentsCompanyDataSource,
            toolbar: this.getTemplateToolBar(),
            editable: {
                mode: "inline",
                confirmation: false
            },
            resizable: true,
            autoScroll: true,
            selectable: true,
            detailTemplate: this.getTemplateChildren(),
            detailInit: ContractualsDocumentsCompany.childrenDocuments,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: {
                messages: {
                    empty: "Arrastre un encabezado de columna y póngalo aquí para agrupar por ella"
                }
            },
            dataBound: function () {
                ContractualsDocumentsCompany.updateTemplate();

                var grid = this;
                grid.tbody.find(">tr").each(function() {
                    var dataItem = grid.dataItem(this);
                    if (dataItem.ContractualDocumentTypeId === Constants.contractualDocumentType.OfferSPA ||
                        dataItem.ContractualDocumentTypeId === Constants.contractualDocumentType.OfferGES ||
                        dataItem.ContractualDocumentTypeId === Constants.contractualDocumentType.OfferFOR ||
                        dataItem.ContractualDocumentTypeId === Constants.contractualDocumentType.ContractFOR ||
                        dataItem.ContractualDocumentTypeId === Constants.contractualDocumentType.Annex ||
                        dataItem.ContractualDocumentTypeId === Constants.contractualDocumentType.UnSubscribeContract ||
                        dataItem.ContractualDocumentTypeId === Constants.contractualDocumentType.Firmed) {
                        $(this).find(".k-hierarchy-cell a").removeClass("k-icon");
                    }
                });
            },
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' onclick='ContractualsDocumentsCompany.onSave();' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }
        });
        kendo.bind($("#" + this.gridContractualsDocumentsCompanyId), this);
    },

    contractualDocumentTypeDropDownEditor: function (container, options) {
        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Description",
                optionLabel: "Selecciona ...",
                dataValueField: "Id",
                autoWidth: true,
                dataSource: ContractualsDocumentsCompany.contractualDocumentTypeDataSorce
            });
    },

    onSave: function () {
        var grid = $("#" + ContractualsDocumentsCompany.gridContractualsDocumentsCompanyId).data("kendoGrid");
        kendo.ui.progress(grid.element, true);
    },

    getContractualDocumentTypeDescription: function (contractualDocumentType) {
        if (ContractualsDocumentsCompany.contractualDocumentTypeDataSorce.data().length === 0) {
            ContractualsDocumentsCompany.contractualDocumentTypeDataSorce.read();
        }
        for (var index = 0; index < ContractualsDocumentsCompany.contractualDocumentTypeDataSorce.data().length; index++) {
            if (ContractualsDocumentsCompany.contractualDocumentTypeDataSorce.data()[index].Id === contractualDocumentType) {
                return ContractualsDocumentsCompany.contractualDocumentTypeDataSorce.data()[index].Description;
            }
        }
        return null;
    },

    getTemplateToolBar: function () {
       var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createUser'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getTemplateChildren: function () {
        var html = "<div>";
        html += "<H2 style='text-align: center;'>Documentos Relacionados</H2><br />";
        html += "<div class='gridContractualsDocumentsCompanyChildren'></div><br /><br />";
        html += "</div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div style='display: inline-block; margin-left: 37px;'>";

        if (data.UrlRelative) {
            html += kendo.format("<a toggle='tooltip' title='Abrir Documento' onclick='GeneralData.goToOpenContractualDocument(\"{0}\")' target='_blank' style='cursor: pointer;'><img style='margin-top: -9px;' src='../../Images/pdf_opt.png'></a></div></a>&nbsp;&nbsp;", data.Id);
            //html += kendo.format("<a toggle='tooltip' title='Ver Documento' onclick='ContractualsDocumentsCompany.goToOfferView(\"{0}\")' target='_blank' style='cursor: pointer;'><img style='margin-top: -9px;' src='../../Images/pdf_opt.png'></a></div></a>&nbsp;&nbsp;", data.Id);
        } else {
            html += kendo.format("<a toggle='tooltip' title='Agregar Anexo' onclick='ContractualsDocumentsCompany.goToAddAnnex(\"{0}\")' target='_blank' style='cursor: pointer;'><img style='margin-top: -9px;' src='../../Images/unknown_opt.png'></a></div></a>&nbsp;&nbsp;&nbsp;", data.Id);
            //html += kendo.format("<a toggle='tooltip' title='Ver Documento' onclick='ContractualsDocumentsCompany.goToOfferView(\"{0}\")' target='_blank' style='cursor: pointer;'><img style='margin-top: -9px;' src='../../Images/unknown_opt.png'></a></div></a>&nbsp;&nbsp;", data.Id);
        }

        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='ContractualsDocumentsCompany.goToEditContractualsDocumentsCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='ContractualsDocumentsCompany.goToDeleteContractualsDocumentsCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    getColumnTemplateEnrollment: function (data) {
        var html;
        if (data.ContractualDocumentCompanyFirmedId == null) {
            if (data.Id !== 0) {
                html = "<div style='text-align: center'>";
                html += "<div style='text-align: left'>";
                html += kendo.format(
                    "<div id='circleFirmPending' toggle='tooltip' title='Agregar Documento Firmado' onclick='ContractualsDocumentsCompany.goToAddContractualDocumentFirmed(\"{0}\")' style='cursor: pointer; float: left; text-align: left;'>",
                    data.Id);
                html += "</div></div>";
                html += kendo.format("<div style='font-size: 16px; font-weight: bold;'>{0}", data.Enrollment);
                html += "</div></div>";
            } else {
                html = "";
            }
        } else {
            html = "<div style='text-align: center'>";
            html += "<div style='text-align: left'>";
            html += "<div style='cursor: pointer; float: left; text-align: left; margin-top: -10px;'>";
            html += kendo.format("<img toggle='tooltip' title='Ver Documento Firmado' onclick='GeneralData.goToOpenContractualDocument(\"{0}\")' target='_blank' src='../../Images/pdf_opt_little.jpg'>&nbsp;&nbsp;", data.ContractualDocumentCompanyFirmedId);
            html += kendo.format("<a toggle='tooltip' title='Eliminar Documento Firmado' onclick='ContractualsDocumentsCompany.goToDeleteContractualDocumentCompanyFirmed(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 7px;'></i></a>", data.ContractualDocumentCompanyFirmedId);
            html += "</div>";            
            html += "</div>";
            html += kendo.format("<div style='font-size: 16px; font-weight: bold;'>{0}", data.Enrollment);
            html += "</div></div>";
        }
        
        return html;
    },

    goToOfferView: function (id) {
        var params = {
            url: "/Companies/OfferView",
            data: {
                contractualDocumentId: id,
                isPartialView: true
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeleteContractualsDocumentsCompany: function (contractualDocumentId) {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Empresas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> este Documento?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + ContractualsDocumentsCompany.gridContractualsDocumentsCompanyId).data("kendoGrid");
                        var item = grid.dataSource.get(contractualDocumentId);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToEditContractualsDocumentsCompany: function (contractualDocumentId) {
        var grid = $("#" + ContractualsDocumentsCompany.gridContractualsDocumentsCompanyId).data("kendoGrid");
        var item = grid.dataSource.get(contractualDocumentId);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToAddContractualDocumentFirmed: function (contractualDocumentId) {
        this.setUpAddDocumentFirmedWindow(contractualDocumentId);
        this.addDocumentFirmedWindow.data("kendoWindow").center().open();
    },

    goToAddAnnex: function (contractualDocumentId) {
        this.setUpAddAnnexWindow(contractualDocumentId);
        this.addAnnexWindow.data("kendoWindow").center().open();
    },

    goToDeleteContractualDocumentCompanyFirmed: function (contractualDocumentCompanyFirmedId) {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Empresas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> este Documento?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        $.ajax({
                            url: "/Companies/DeleteContractualDocumentCompanyFirmed",
                            data: {
                                ContractualDocumentCompanyFirmedId: contractualDocumentCompanyFirmedId
                            },
                            type: "post",
                            dataType: "json",
                            success: function (data) {
                                if (data.Status === Constants.resultStatus.Ok){
                                    GeneralData.showNotification(Constants.ok, "", "success");

                                    ContractualsDocumentsCompany.updateRow(data.Object);
                                }
                                if (data.Status === Constants.resultStatus.Error) {
                                    GeneralData.showNotification(Constants.ko, "", "error");
                                }
                            },
                            error: function () {
                                GeneralData.showNotification(Constants.ko, "", "error");
                            }
                        });
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    updateTemplate: function() {
        var grid = $("#" + ContractualsDocumentsCompany.gridContractualsDocumentsCompanyId).data("kendoGrid");
        var hasOffer = false;
        var hasContract = false;
        for (var i = 0; i < grid.dataSource.data().length; i++) {
            var row = grid.dataSource.data()[i];
            if (row["ContractualDocumentTypeId"] === Constants.contractualDocumentType.Offer) {
                hasOffer = true;
            }
            if (row["ContractualDocumentTypeId"] === Constants.contractualDocumentType.Contract) {
                hasContract = true;
            }
        }
        if (hasOffer === false && hasContract === false) {
            $("a#btnTypeContractualDocument").text(" Generar oferta");
        }
        if (hasOffer === true && hasContract === false) {
            $("a#btnTypeContractualDocument").text(" Generar contrato");
        }
        if (hasOffer === true && hasContract === true) {
            $("a#btnTypeContractualDocument").text(" Agregar anexo");
        }
    },

    updateRow: function (contractualDocument) {
        var grid = $("#" + this.gridContractualsDocumentsCompanyId).data("kendoGrid");           
        var dataItem = grid.dataSource.get(contractualDocument.Id);
        dataItem.ContractualDocumentCompanyFirmedId = contractualDocument.ContractualDocumentCompanyFirmedId;

        grid.refresh();
    },

    updateRowAnnex: function (contractualDocument) {
        var grid = $("#" + this.gridContractualsDocumentsCompanyId).data("kendoGrid");
        var dataItem = grid.dataSource.get(contractualDocument.Id);
        dataItem.UrlRelative = contractualDocument.UrlRelative;

        grid.refresh();
    },

    childrenDocuments: function (e) {
        var beginDate = new Date();
        var endDate = new Date();
        endDate.setFullYear(beginDate.getFullYear());
        endDate.setDate(endDate.getDate() - 1);

        if (e.data.ContractualDocumentTypeId === Constants.contractualDocumentType.OfferSPA ||
            e.data.ContractualDocumentTypeId === Constants.contractualDocumentType.OfferGES ||
            e.data.ContractualDocumentTypeId === Constants.contractualDocumentType.OfferFOR ||
            e.data.ContractualDocumentTypeId === Constants.contractualDocumentType.ContractFOR ||
            e.data.ContractualDocumentTypeId === Constants.contractualDocumentType.Annex ||
            e.data.ContractualDocumentTypeId === Constants.contractualDocumentType.UnSubscribeContract ||
            e.data.ContractualDocumentTypeId === Constants.contractualDocumentType.Firmed) {
            return;
        }

        var detailRow = e.detailRow;
        detailRow.find(".gridContractualsDocumentsCompanyChildren").kendoGrid({
            dataSource: {
                schema: {
                    model: {
                        id: "Id",
                        fields: {
                            Id: { type: "number", defaultValue: 0 },
                            CompanyId: { type: "number", defaultValue: e.data.CompanyId },
                            Enrollment: { type: "string", editable: false },
                            ContractualDocumentTypeId: {
                                type: "number",
                                validation: { required: { message: " Campo Obligatorio " } }
                            },
                            ContractualDocumentTypeName: {
                                type: "string",
                                validation: { required: { message: " Campo Obligatorio " } }
                            },
                            BeginDate: { type: "date", defaultValue: beginDate, format: "{0:dd/MM/yy}" },
                            EndDate: { type: "date", defaultValue: endDate, format: "{0:dd/MM/yy}" },
                            UrlRelative: { type: "string" },
                            Observations: { type: "string" },
                            ContractualDocumentCompanyFirmedId: { type: "number", defaultValue: null },
                            ContractualDocumentCompanyFirmedEnrollment: { type: "string" },
                            ContractualDocumentCompanyFirmedUrlRelative: { type: "string" },
                            ContractualDocumentCompanyParentId: { type: "number", defaultValue: e.data.Id }
                        }
                    }
                },
                transport: {
                    read: {
                        url: "/Companies/ChildrenContractualsDocumentsCompany_Read",
                        dataType: "jsonp",
                        data: { contractualDocumentId: e.data.Id }
                    },
                    destroy: {
                        url: "/Companies/ContractualsDocumentsCompany_Destroy",
                        dataType: "jsonp"
                    },
                    create: {
                        url: "/Companies/ContractualsDocumentsCompany_Create",
                        dataType: "jsonp"
                    },
                    parameterMap: function(options, operation) {
                        if (operation === "read") {
                            return { contractualDocumentId: options.contractualDocumentId };
                        }
                        if (operation !== "read" && options) {
                            return { contractualDocument: kendo.stringify(options) };
                        }

                        return null;
                    }
                },
                requestEnd: function(e) {
                    if ((e.type === "update" || e.type === "destroy" || e.type === "create") && e.response !== null) {
                        var grid = $("#" + ContractualsDocumentsCompany.gridContractualsDocumentsCompanyId)
                            .data("kendoGrid");
                        if (typeof e.response.Errors !== "undefined") {
                            GeneralData.showNotification(e.response.Errors, "", "error");
                            if (e.type === "create") {
                                this.data().remove(this.data().at(0));

                                kendo.ui.progress(grid.element, false);
                            } else {
                                if (e.type === "destroy") {
                                    ContractualsDocumentsCompany.contractualsDocumentsCompanyDataSource.read();
                                }
                                this.cancelChanges();
                            }
                        } else {
                            if (e.type === "create") {
                                kendo.ui.progress(grid.element, false);

                                GeneralData.showNotification(Constants.ok, "", "success");
                            }
                        }
                    }
                },
                pageSize: 10
            },
            columns: [
                {
                    field: "Enrollment",
                    title: "Matrícula",
                    width: 250,
                    groupable: "false",
                    template: "#= ContractualsDocumentsCompany.getColumnTemplateEnrollment(data) #"
                }, {
                    field: "ContractualDocumentTypeId",
                    title: "Tipo",
                    width: "150px",
                    editor: ContractualsDocumentsCompany.contractualChildrenDocumentTypeDropDownEditor,
                    template: "#= ContractualsDocumentsCompany.getContractualDocumentTypeDescription(data.ContractualDocumentTypeId) #",
                    groupHeaderTemplate: "Agrupado : #= ContractualsDocumentsCompany.getContractualDocumentTypeDescription(value) #"
                }, {
                    field: "BeginDate",
                    title: "Fecha Inicio",
                    template: "#= Templates.getColumnTemplateDate(data.BeginDate) #"
                }, {
                    field: "EndDate",
                    title: "Fecha Fin",
                    template: "#= Templates.getColumnTemplateDate(data.EndDate) #"
                }, {
                    field: "Observations",
                    title: "Observaciones",
                    groupable: "false"
                }, {
                    title: "Comandos",
                    field: "Commands",
                    width: 160,
                    groupable: "false",
                    filterable: false,
                    template: "#= ContractualsDocumentsCompany.getColumnTemplateCommands(data) #"
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
            toolbar: ContractualsDocumentsCompany.getTemplateToolBar(),
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
            dataBound: function () {
                ContractualsDocumentsCompany.updateTemplate();
            },
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' onclick='ContractualsDocumentsCompany.onSave();' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }
        });
    },

    contractualChildrenDocumentTypeDropDownEditor: function (container, options) {
        var contractualParentId = options.model.ContractualDocumentCompanyParentId;
        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Description",
                optionLabel: "Selecciona ...",
                dataValueField: "Id",
                autoWidth: true,
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
                            url: "/Companies/GetChildrenContractualDocumentTypes",
                            dataType: "jsonp",
                            data: { contractualParentId: contractualParentId }
                        },
                        parameterMap: function (options, operation) {
                            if (operation === "read") {
                                return { contractualParentId: options.contractualParentId };
                            }
  
                            return null;
                        }
                    }
                }
            });
    }
});