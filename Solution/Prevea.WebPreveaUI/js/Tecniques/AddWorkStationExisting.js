var AddWorkStationExisting = kendo.observable({

    cnaeSelected: null, 

    gridWorkStationsExistingId: "gridWorkStationsExisting",
    confirmId: "confirm",

    workStationsExistingDataSource: null,

    init: function (cnaeSelected, title) {
        this.cnaeSelected = cnaeSelected;

        var windowAddWorkStationExisting = $("#addWorkStationExisting").data("kendoWindow");
        windowAddWorkStationExisting.title(title);

        this.createWorkStationsExistingDataSource();
        this.createWorkStationsExistingGrid();
    },

    createWorkStationsExistingDataSource: function () {
        this.workStationsExistingDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        Name: { type: "string" },
                        Description: { type: "string" },
                        CnaeDescription: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Tecniques/WorkSations_Read",
                    dataType: "jsonp"
                }
            },
            group: { field: "CnaeDescription" },            
            groupable: false,
            pageSize: 10
        });
    },

    createWorkStationsExistingGrid: function () {
        $("#" + this.gridWorkStationsExistingId).kendoGrid({
            columns: [
                {
                    field: "Name",
                    title: "Nombre",
                    width: 200
                }, {
                    field: "Description",
                    title: "Descripción",
                    width: 200,
                    groupable: "false"
                }, {
                    field: "CnaeDescription",
                    title: "CNAE",
                    width: 200
                }
            ],
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
            dataSource: this.workStationsExistingDataSource,
            resizable: true,
            autoScroll: true,
            selectable: "multiple",
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

    goToCloseWindow: function() {
        var windowAddWorkStationExisting = $("#addWorkStationExisting").data("kendoWindow");
        windowAddWorkStationExisting.close();
    },

    goToAddWorkStationToCNAE: function () {
        var windowAddWorkStationExisting = $("#addWorkStationExisting").data("kendoWindow");

        var grid = $("#" + this.gridWorkStationsExistingId).data("kendoGrid");
        var rows = grid.select();
        var ids = [];
        rows.each(function (e) {
            var id = grid.dataItem(this).Id;
            ids.push(id);
        });

        $.ajax({
            url: "/Tecniques/SaveWorkStationsInCNAE",
            data: JSON.stringify({
                "cnaeSelected": this.cnaeSelected,
                "workStationsSelected": ids
            }),
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.result.Status === Constants.resultStatus.Ok) {

                    var grid = $("#gridWorkStations").find(AddWorkStationExisting.cnaeSelected + "gridWorkStations").prevObject.data("kendoGrid");                    
                    if (typeof grid !== "undefined") {
                        grid.dataSource.read();
                    }                    
                    GeneralData.showNotification(Constants.ok, "", "success");
                } else {
                    GeneralData.showNotification(error, "", "error");
                }
            },
            error: function (xhr, status, error) {
                GeneralData.showNotification(error, "", "error");
            }
        }); 
        windowAddWorkStationExisting.close();
    }
});