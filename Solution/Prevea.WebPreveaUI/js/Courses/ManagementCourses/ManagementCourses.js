var ManagementCourses = kendo.observable({

    inpFamilyId: "inpFamily",
    inpTitleId: "inpTitle",
    inpCourseNameId: "inpCourseName",
    inpHoursCourseId: "inpHoursCourse",
    inpPriceCourseId: "inpPriceCourse",
    inpModalityCourseId: "inpModalityCourse",
    btnDeleteNodeId: "btnDeleteNode",

    trvCoursesId: "trvCourses",
    confirmId: "confirm",
    
    coursesDataSource: null,

    init: function () {
        kendo.culture("es-ES");

        this.setUpKendoWidget();
        this.createTreeViewCourses();
    },

    setUpKendoWidget: function() {
        $("#" + this.inpHoursCourseId).kendoNumericTextBox({
            decimals: 0,
            format: "0"
        });

        $("#" + this.inpPriceCourseId).kendoNumericTextBox({
            decimals: 2,
            format: "c"
        });

        var data = [
            { text: "Presencial", value: "1" },
            { text: "On Line", value: "2" }
        ];

        $("#" + this.inpModalityCourseId).kendoDropDownList({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: data
        });

        $("#" + this.btnDeleteNodeId).removeAttr("disabled");
        $("#" + this.btnDeleteNodeId).prop("disabled", true);
    },

    createTreeViewCourses: function() {
        this.coursesDataSource = new kendo.data.HierarchicalDataSource({
            transport: {
                read: {
                    url: "/ManagementCourses/TrainingCourses_Read",
                    dataType: "jsonp"
                }
            },
            schema: {
                model: {
                    id: "Id",
                    hasChildren: "HasChildren"
                }
            }
        });      

        $("#" + this.trvCoursesId).kendoTreeView({
            //checkboxes: {
            //    checkChildren: true,
            //    template: "#= ManagementCourses.getTemplateCheckNode(data) #"
            //},

            template: "#= ManagementCourses.getTemplateNode(data) #",
            dataSource: this.coursesDataSource,
            dataTextField: "Name",
            select: ManagementCourses.onSelectNode
        });
        //var treeview = $("#" + this.trvCoursesId).data("kendoTreeView");
        //treeview.bind("check", ManagementCourses.treeCheck);
    },

    getTemplateNode: function (data) {
        var html = "";
        if (data.item.IsRoot === true) {
            html += "&nbsp;<a id='coursesRootNode' style='color: #373E4A;'><i class='fa fa-superscript fa-2x'></a>";
        } else {
            if (data.item.IsCourse === true) {
                var modality;
                if (parseInt(data.item.TrainingCourseModalityId) === 1) {
                    modality = "Presencial";
                } else {
                    modality = "On Line";
                }

                var price;
                if ($.type(data.item.Price) === "string") {
                    price = kendo.toString(parseFloat(data.item.Price), "c2");
                } else {
                    price = kendo.toString(data.item.Price, "c2");
                }

                html += kendo.format("<strong>{0}</strong> -> {1} horas, {2}, {3}", data.item.Name, data.item.Hours, price, modality);
            } else {
                html += data.item.Name;
            }            
        }
        return html;
    },

    getTemplateCheckNode: function (data) {
        var html = "";
        if (data.item.IsRoot === false) {
            if (data.checked) {
                html += "<input type='checkbox' 'checked'>";
                //html +=
                //    "<span class='k-checkbox-wrapper' role='presentation'><input class='k-checkbox' type='checkbox' 'checked'><label class='k-checkbox-label'></label></span>";
            } else {
                //html +=
                //    "<span class='k-checkbox-wrapper' role='presentation'><input class='k-checkbox' type='checkbox'><label class='k-checkbox-label'></label></span>";
                html += "<input type='checkbox'>";
            }
        }

        return html;
    },

    //treeCheck: function (e) {
    //    ManagementCourses.clearCheckSelected();
    //},

    goToManagementCourses: function() {
        var params = {
            url: "/Courses/ManagementCourses",
            data: {}
        };
        GeneralData.goToActionController(params);        
    },

    goToAddFamily: function () {
        var textNewNode = $("#" + this.inpFamilyId).val();

        if (textNewNode === "") {
            GeneralData.showNotification("No se puede añadir esta Familia", "", "error");
            return;
        } else {
            var treeview = $("#" + this.trvCoursesId).data("kendoTreeView");
            if (treeview == null) {
                GeneralData.showNotification("No se puede añadir esta Familia", "", "error");
                return;
            }

            var node = treeview.findByText(textNewNode);

            if (node.length > 0) {
                GeneralData.showNotification("Esta Familia ya existe", "", "error");
                return;
            }

            this.addFamily(textNewNode);          
        }               
    },

    goToAddTitle: function () {
        var textNewNode = $("#" + this.inpTitleId).val();

        if (textNewNode === "") {
            GeneralData.showNotification("No se puede añadir este Título", "", "error");
            return;
        } else {
            var treeview = $("#" + this.trvCoursesId).data("kendoTreeView");
            if (treeview == null) {
                GeneralData.showNotification("No se puede añadir este Título", "", "error");
                return;
            }

            var node = treeview.findByText(textNewNode);

            if (node.length > 0) {
                GeneralData.showNotification("Este Título ya existe", "", "error");
                return;
            }

            this.addTitle(textNewNode);
        }
    },

    goToAddCourse: function () {
        var textNewNode = $("#" + this.inpCourseNameId).val();

        if (textNewNode === "") {
            GeneralData.showNotification("No se puede añadir este Curso", "", "error");
            return;
        } else {
            var treeview = $("#" + this.trvCoursesId).data("kendoTreeView");
            if (treeview == null) {
                GeneralData.showNotification("No se puede añadir este Curso", "", "error");
                return;
            }

            var node = treeview.findByText(textNewNode);

            if (node.length > 0) {
                GeneralData.showNotification("Este Curso ya existe", "", "error");
                return;
            }

            this.addCourse(textNewNode);
        }
    },

    clearCheckSelected: function() {
        var treeview = $("#" + this.trvCoursesId).data("kendoTreeView");
        $("#" + this.trvCoursesId).find('input:checkbox:checked').each(function () {
            var item = treeview.dataItem(this);
            item.set("checked", false);
        });
    },

    addFamily: function (name) {
        $.ajax({
            url: "/ManagementCourses/AddFamily",
            data: {
                name: name
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result.Status === Constants.resultStatus.Ok) {
                    var treeview = $("#" + ManagementCourses.trvCoursesId).data("kendoTreeView");
                    var rootDataItem = treeview.dataSource.get(1);
                    var rootElement = treeview.findByUid(rootDataItem.uid);

                    treeview.append({
                        Id: data.result.Object.Id,
                        Name: name,                        
                        IsRoot: false,
                        IsFamily: true
                    }, rootElement);

                    GeneralData.showNotification(Constants.ok, "", "success");
                }
                if (data.result.Status === Constants.resultStatus.Error) {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            },
            error: function () {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    },

    addTitle: function (name) {
        var treeview = $("#" + ManagementCourses.trvCoursesId).data("kendoTreeView");
        var selectedNode = treeview.select();
        if (selectedNode.length === 0) {
            GeneralData.showNotification("Debes seleccionar una Familia", "", "error");
            return;
        }

        var dataItem = this.coursesDataSource.getByUid(selectedNode.data().uid);
        if (dataItem == null) {
            GeneralData.showNotification("Error en la creación del Título", "", "error");
            return;
        }

        if (dataItem.IsFamily === false) {
            GeneralData.showNotification("Debes seleccionar una Familia", "", "error");
            return;
        }

        $.ajax({
            url: "/ManagementCourses/AddTitle",
            data: {
                familyId: dataItem.Id,
                name: name
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result.Status === Constants.resultStatus.Ok) { 
                    treeview.append({
                        Id: data.result.Object.Id,
                        Name: name,
                        IsRoot: false,
                        IsFamily: false,
                        IsTitle: true
                    }, selectedNode);

                    GeneralData.showNotification(Constants.ok, "", "success");
                }
                if (data.result.Status === Constants.resultStatus.Error) {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            },
            error: function () {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    },

    addCourse: function (name) {
        var treeview = $("#" + ManagementCourses.trvCoursesId).data("kendoTreeView");
        var selectedNode = treeview.select();
        if (selectedNode.length === 0) {
            GeneralData.showNotification("Debes seleccionar un Título", "", "error");
            return;
        }

        var dataItem = this.coursesDataSource.getByUid(selectedNode.data().uid);
        if (dataItem == null) {
            GeneralData.showNotification("Error en la creación del Curso", "", "error");
            return;
        }

        if (dataItem.IsTitle === false) {
            GeneralData.showNotification("Debes seleccionar un Título", "", "error");
            return;
        }

        if (parseInt($("#" + ManagementCourses.inpHoursCourseId).val()) === 0) {
            GeneralData.showNotification("Debes agregar horas al Curso", "", "error");
            return;
        }

        if (parseFloat($("#" + ManagementCourses.inpPriceCourseId).val()) === 0) {
            GeneralData.showNotification("Debes agregar precio al Curso", "", "error");
            return;
        }

        $.ajax({
            url: "/ManagementCourses/AddCourse",
            data: {
                titleId: dataItem.Id,
                name: name,
                hours: $("#" + ManagementCourses.inpHoursCourseId).val(),
                price: $("#" + ManagementCourses.inpPriceCourseId).val(),
                modality: $("#" + ManagementCourses.inpModalityCourseId).val()
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result.Status === Constants.resultStatus.Ok) {
                    treeview.append({
                        Id: data.result.Object.Id,
                        Name: name,
                        IsRoot: false,
                        IsFamily: false,
                        IsTitle: false,
                        IsCourse: true,
                        Hours: $("#" + ManagementCourses.inpHoursCourseId).val(),
                        Price: $("#" + ManagementCourses.inpPriceCourseId).val(),
                        TrainingCourseModalityId: $("#" + ManagementCourses.inpModalityCourseId).val()
                    }, selectedNode);

                    GeneralData.showNotification(Constants.ok, "", "success");
                }
                if (data.result.Status === Constants.resultStatus.Error) {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            },
            error: function () {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    },

    onSelectNode: function (e) {
        var dataItem = ManagementCourses.coursesDataSource.getByUid(e.node.dataset.uid);
        if (dataItem.IsRoot) {
            $("#" + ManagementCourses.btnDeleteNodeId).removeAttr("disabled");
            $("#" + ManagementCourses.btnDeleteNodeId).prop("disabled", true);

            return;
        }

        if (dataItem.IsFamily) {
            if (dataItem.hasChildren === true) {
                $("#" + ManagementCourses.btnDeleteNodeId).removeAttr("disabled");
                $("#" + ManagementCourses.btnDeleteNodeId).prop("disabled", true);
            } else {
                $("#" + ManagementCourses.btnDeleteNodeId).removeAttr("disabled");
                $("#" + ManagementCourses.btnDeleteNodeId).prop("disabled", false);
            }

            return;
        }

        if (dataItem.IsTitle) {
            if (dataItem.hasChildren === true) {
                $("#" + ManagementCourses.btnDeleteNodeId).removeAttr("disabled");
                $("#" + ManagementCourses.btnDeleteNodeId).prop("disabled", true);
            } else {
                $("#" + ManagementCourses.btnDeleteNodeId).removeAttr("disabled");
                $("#" + ManagementCourses.btnDeleteNodeId).prop("disabled", false);
            }

            return;
        }

        if (dataItem.IsCourse) {  
            ManagementCourses.canDeleteCourse(dataItem.Id);
        }
    },

    canDeleteCourse: function(courseId) {
        $.ajax({
            url: "/ManagementCourses/CanDeleteCourse",
            data: {
                courseId: courseId
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result === Constants.resultStatus.Ok) {
                    $("#" + ManagementCourses.btnDeleteNodeId).removeAttr("disabled");
                    $("#" + ManagementCourses.btnDeleteNodeId).prop("disabled", false);
                }
                if (data.result === Constants.resultStatus.Error) {
                    $("#" + ManagementCourses.btnDeleteNodeId).removeAttr("disabled");
                    $("#" + ManagementCourses.btnDeleteNodeId).prop("disabled", true);
                }
            },
            error: function () {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    },

    deleteNode: function () {
        var treeview = $("#" + ManagementCourses.trvCoursesId).data("kendoTreeView");
        var selectedNode = treeview.select();

        if (selectedNode.length === 0) {
            return;
        }

        var dataItem = this.coursesDataSource.getByUid(selectedNode.data().uid);
        if (dataItem == null) {
            return;
        }

        var message = "";

        if (dataItem.IsFamily) {
            message = "¿Quieres <strong>Borrar</strong> esta Familia?";
        }

        if (dataItem.IsTitle) {
            message = "¿Quieres <strong>Borrar</strong> este Título?";
        }

        if (dataItem.IsCourse) {
            message = "¿Quieres <strong>Borrar</strong> este Curso?";
        }

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Gestión de Cursos</strong>",
            closable: false,
            modal: true,
            content: message,
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        $.ajax({
                            url: "/ManagementCourses/DeleteNode",
                            data: {
                                nodeId: dataItem.Id
                            },
                            type: "post",
                            dataType: "json",
                            success: function (data) {
                                if (data.result.Status === Constants.resultStatus.Ok) {
                                    treeview.remove(selectedNode);
                                    GeneralData.showNotification(Constants.ok, "", "success");
                                }
                                if (data.result.Status === Constants.resultStatus.Error) {
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
    }

});