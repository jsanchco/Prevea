var DetailCompany = kendo.observable({
    titleId: "title",
    navigationId: "navigation",
    tabStripDetailCompanyId: "tabStripDetailCompany",
    spanNotificationId: "spanNotification",
    stretchCalculate: null,
    subscribeNumberEmployees: null,

    // Fields
    id: null,
    selectTabId: null,
    notification: null,

    // Datasources

    init: function (id, selectTabId, notification) {
        this.id = id;
        this.selectTabId = selectTabId;

        if (notification) {
            this.notification = notification;
        } else {
            this.notification = null;
        }

        this.setUpPage();
        this.createKendoWidgets();        
    },

    setUpPage: function () {
        if (GeneralData.userRoleId === Constants.role.ContactPerson) {
            $("#" + this.navigationId).hide();
            $("#" + this.titleId).text("Mi Empresa");
        }
    },

    createKendoWidgets: function () {
        if (this.notification) {
            GeneralData.showNotification(Constants.ok, "", "success");
        } else {
            $("#" + this.spanNotificationId).hide();
        }

        this.createTabStripCompany();
    },

    createDocumentDataSource: function () {
        this.documentDetailDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Edition: { type: "number" },
                        Observations: { type: "string" },
                        Date: { type: "date" },
                        UrlRelative: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Document/DocumentsByParent_Read",
                    dataType: "jsonp",
                    data: {
                        id: DetailDocument.id,
                        documentParentId: DetailDocument.documentParentId
                    }
                }
            },
            pageSize: 10
        });
    },

    createGridDocumentDetail: function () {        
        $("#" + this.gridDocumentDetailId).kendoGrid({
            columns: [{
                    field: "Edition",
                    title: "Edición",
                    width: 90,
                    groupable: "false",
                    template: "#= Templates.getColumnTemplateEdition(data) #"
                },
                {
                    field: "Observations",
                    title: "Observaciones"
                },
                {
                    field: "Date",
                    title: "Fecha",
                    width: 160,
                    template: "#= Templates.getColumnTemplateDate(data.Date) #"
                },
                {
                    field: "UrlRelative",
                    title: "Documento",
                    width: 115,
                    groupable: "false",
                    filterable: false,
                    template: "#= Templates.getColumnTemplateUrl(data) #"
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
            dataSource: this.documentDetailDataSource,
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
        kendo.bind($("#" + this.gridDocumentDetailId), this);

        $("#" + this.gridDocumentDetailId).on("dblclick", "tr", function () {
        });
    },

    createTabStripCompany: function () {
        var tabStrip = $("#" + this.tabStripDetailCompanyId).kendoTabStrip().data("kendoTabStrip");
        tabStrip.append({
            text: "SIMULACIONES",
            contentUrl: kendo.format("/Companies/SimulationsCompany?companyId={0}", this.id)
        });
        tabStrip.append({
            text: "DATOS GENERALES",
            contentUrl: kendo.format("/Companies/GeneralDataCompany?companyId={0}", this.id)
        });
        tabStrip.append({
            text: "PERSONAS de CONTACTO",
            contentUrl: kendo.format("/Companies/ContactPersonsCompany?companyId={0}", this.id)
        });
        tabStrip.append({
            text: "CENTROS de TRABAJO",
            contentUrl: kendo.format("/Companies/WorkCentersCompany?companyId={0}", this.id)
        });
        //tabStrip.append({
        //    text: "GESTORÍA",
        //    contentUrl: kendo.format("/Companies/AgencyCompany?companyId={0}", this.id)
        //});
        tabStrip.append({
            text: "TRABAJADORES",
            contentUrl: kendo.format("/Companies/EmployeesCompany?companyId={0}", this.id)
        });
        tabStrip.append({
            text: "DATOS ECONÓMICOS",
            contentUrl: kendo.format("/Companies/EconomicDataCompany?companyId={0}", this.id)
        });
        tabStrip.append({
            text: "FORMA de PAGO",
            contentUrl: kendo.format("/Companies/PaymentMethodCompany?companyId={0}", this.id)
        });
        tabStrip.append({
            text: "DOCUMENTOS",
            contentUrl: kendo.format("/Companies/ContractualsDocumentsCompany?companyId={0}", this.id)
        });

        tabStrip = $("#" + this.tabStripDetailCompanyId).data("kendoTabStrip");
        tabStrip.select(this.selectTabId);
    },

    goToDetailCompany: function () {
        var params = {
            url: "/Companies/DetailCompany",
            data: {
                id: this.id,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    },

    updateStretchCalculate: function () {
        var that = this;
        $.ajax({
            url: "/Companies/GetStretchCalculate",
            data: {
                companyId: this.id
            },
            type: "post",
            dataType: "json",
            success: function(response) {
                if (response.stretchCalculate !== null) {
                    that.stretchCalculate = response.stretchCalculate;
                    that.subscribeNumberEmployees = response.subscribeNumberEmployees;

                    if (EconomicDataCompany !== null) {
                        EconomicDataCompany.updateView();
                    }
                }
            }
        });
    }

});