var ManagementCourses = kendo.observable({

    init: function() {
    },

    goToManagementCourses: function() {
        var params = {
            url: "/Courses/ManagementCourses",
            data: {}
        };
        GeneralData.goToActionController(params);        
    }
});