var EmployeeDocuments = kendo.observable({

    init: function () {
   
    },

    goToEmployeeDocuments: function () {
        var params = {
            url: "/Employees/Documents",
            data: {}
        };
        GeneralData.goToActionController(params);
    }
});