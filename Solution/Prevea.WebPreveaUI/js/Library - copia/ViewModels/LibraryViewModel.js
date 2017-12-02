var LibraryViewModel = kendo.observable({

    // Fields

    // Datasources
    libraryDataSource: null,

    init: function () {
        this.createDataSources();
    },

    createDataSources: function () {
        this.createLibraryDataSource();
    },

    createLibraryDataSource: function () {
        this.libraryDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        LibraryAreaName: { type: "string" },
                        Name: { type: "string" },
                        Description: { type: "string" },
                        DateInitial: { type: "date" },
                        DateModication: { type: "date" },
                        Edition: { type: "number" },
                        UserName: { type: "string" },
                        Url: { type: "string" }
                    }
                }
            },
            transport: {
                read: {                    
                    url: "/Library/Library_Read",
                    dataType: "jsonp"
                }
            },
            pageSize: 10
        });
    }
});