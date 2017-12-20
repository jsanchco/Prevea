var ManagementCourses = kendo.observable({

    init: function() {
        alert("In ManagementCourses!!!!");
    },

    goToManagementCourses: function() {
        var params = {
            url: "/Courses/ManagementCourses",
            data: {}
        };
        GeneralData.goToActionController(params);        
    }
});