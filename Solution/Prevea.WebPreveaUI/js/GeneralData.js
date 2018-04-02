var GeneralData = kendo.observable({

    spanErrorPpalId: "spanErrorPpal",
    notificationId: "notification",

    months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
    days: ["Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo"],

    userId: null,
    userInitials: null,
    userRoleId: null,
    userRoleName: null,
    userRoleDescription: null,

    setRoleUser: function (id, initials, roleId, roleName, roleDescription) {
        this.userId = id;
        this.userInitials = initials;
        this.userRoleId = roleId;
        this.userRoleName = roleName;
        this.roleDescription = roleDescription;
    },

    loadPage: function (element) {
        var id = $(element).attr("id");
        var url = null;

        if (id === "logOff") {
            this.logOff();
            return;
        }

        switch (id) {
            case "dashboard":
            case "br-dashboard":
                url = "/Dashboard/Index";
                break;

            case "users":
            case "br-users":
                url = "/User/Users";
                break;

            case "library|documents":
            case "br-documents":
                url = "/Document/Documents";
                break;

            case "library|historic":
            case "br-historic":
                url = "/Document/HistoricDownloadDocuments";
                break;

            case "commercialTool|simulations":
                url = "/Simulations/Simulations";
                break;

            case "commercialTool|companies":
                url = "/Companies/Companies";
                break;

            case "br-document-addDocument":
                url = "/Document/AddDocument";
                break;

            case "notifications":
                url = "/Notifications/Notifications";
                break;

            case "profile":
                url = "/Profile/ProfileUser";
                break;

            case "courses|management":
                url = "/ManagementCourses/ManagementCourses";
                break;

            case "myCompany":
                url = "/Companies/DetailCompanyByContactPerson";
                break;

            case "healthSurveillance|medicalHealth|historic":
                url = "/HistoricMedicalExamination/HistoricMedicalExamination";
                break;

            case "medicalHealth|historic":
                url = "/HistoricMedicalExamination/HistoricMedicalExamination";
                break;

            case "healthSurveillance|clinics":
                url = "/Clinics/Clinics";
                break;

            case "healthSurveillance|doctors":
                url = "/Doctors/Doctors";
                break;

            case "medicalHealth|doctorWorkSheet":
                url = "/DoctorWorkSheet/DoctorWorkSheet";
                break;
                                
            default:
                break;
        };

        if (url) {
            var params = {
                url: url
            };

            this.goToActionController(params);
        }
    },

    doneHandler: function(e) {
        //console.log("Error-> " + e);
    },

    failureHandler: function (e) {
        alert("Error-> " + e);
    },

    setError: function(error) {
        if (error !== this.error) {
            this.error = error;
        }
    },

    goToActionController: function (params) {
        if (!this.error) {
            var jqxhr = $.ajax({
                url: params.url,
                type: "GET",
                cache: false,
                datatype: "html",
                data: params.data,
                success: function (result) {
                    $("#framePpal").html("");
                    $("#framePpal").html(result);
                }
            });
            jqxhr.done(this.doneHandler);
            jqxhr.fail(this.failureHandler);
        } else {
            var width = Math.floor(($(window).width() / 2) - 50);
            var heigth = Math.floor(($(window).height() / 2) - 20);

            $("#" + this.spanErrorPpalId).kendoNotification({
                position: {
                    pinned: true,
                    top: heigth,
                    right: width
                }
            }).data("kendoNotification").show(this.error);
            this.error = "";
        }
    },

    logOff: function () {
        $.ajax({
            url: "/Login/LogOff",
            success: function () {
                var url = "/Login/Index";
                window.location = url;
            }
        });
    },

    goToOpenFile: function (id) {
        var url = "/Base/DownloadFile?id=" + id;

        window.location = url;
    },

    goToOpenMedicalExamination: function (id) {
        var url = "/Base/DownloadMedicalExamination?id=" + id;

        window.location = url;
    },

    showNotification: function (message, title, type) {
        var opts;
        if (type === "error") {
            opts = {
                "closeButton": true,
                "debug": false,
                "positionClass": "toast-bottom-full-width",
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };

            toastr.error(message, title, opts);
        }
        if (type === "warning") {
            opts = {
                "closeButton": true,
                "debug": false,
                "positionClass": "toast-bottom-full-width",
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };

            toastr.warning(message, title, opts);
        }
        if (type === "information") {
            toastr.info(message);
        }
        if (type === "success") {
            opts = {
                "closeButton": true,
                "debug": false,
                "positionClass": "toast-bottom-full-width",
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };

            toastr.success(message, title, opts);
        }        
    },

    showKendoNotification: function (title, message, type) {
        var template = "";
        if (type === "error") {
            template = kendo.format("<div class='wrong-pass'><img src='../content/web/notification/error-icon.png' /><h3>{0}</h3><p>{1}</p></div></script>", title, message);
        }
        if (type === "info") {
            template = kendo.format("<div class='wrong-pass'><img src='../content/web/notification/error-icon.png' /><h3>{0}</h3><p>{1}</p></div></script>", title, message);
        }
        if (type === "success") {
            template = kendo.format("<div class='wrong-pass'><img src='../content/web/notification/error-icon.png' /><h3>{0}</h3><p>{1}</p></div></script>", title, message);
        }

        var notification = $("#" + this.notificationId).kendoNotification({
            //position: {
            //    pinned: true,
            //    top: 30,
            //    right: 30
            //},
            autoHideAfter: 0,
            stacking: "down",
            templates: [
                {
                    type: type,
                    template: template
                }
            ]
        }).data("kendoNotification");

        notification.show(
            {
                title: title,
                message: message
            },
            type);


        //$("#" + this.notificationId).kendoNotification().data("kendoNotification").show("El Trabajador se ha guardado correctamente");
    },

    resizeWindow: function() {
        var elements = $("[id*='grid']");
        if (elements !== null && elements.length > 0) {
            for (var index = 0; index < elements.length; index ++) {
                var grid = $("#" + elements[index].id);
                if (grid !== null && (typeof grid.data("kendoGrid") !== "undefined")) {
                    grid.data("kendoGrid").resize();    
                }                
            }
        }
    },

    validateDNI: function (dni) {
        var numero;
        var letr;
        var letra;
        var expresion_regular_dni;
 
        expresion_regular_dni = /^\d{8}[a-zA-Z]$/;
 
        if(expresion_regular_dni.test (dni) === true){
            numero = dni.substr(0,dni.length-1);
            letr = dni.substr(dni.length-1,1);
            numero = numero % 23;
            letra = "TRWAGMYFPDXBNJZSQVHLCKET";
            letra=letra.substring(numero,numero+1);
            if (letra !== letr.toUpperCase()) {
                return false;
            }else {
                return true;
            }
        }else {
            return false;
        }
    },

    getMonth: function (index) {
        return this.months[index];
    },

    getDayOfTheWeek: function(index) {
        return this.days[index];
    }
});

