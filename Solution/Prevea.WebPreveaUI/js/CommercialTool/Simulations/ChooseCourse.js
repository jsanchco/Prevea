var ChooseCourse = kendo.observable({

    chooseCourseId: "chooseCourse",
    btnSelectCourseId: "btnSelectCourse",
    confirmId: "confirm",
    inpCourseId: "inpCourse",
    trvCoursesId: "trvCourses",

    trainingCoursesDataSource: null,

    init: function () {
        this.createTreeViewCourses();

        $("#" + ChooseCourse.btnSelectCourseId).removeAttr("disabled");
        $("#" + ChooseCourse.btnSelectCourseId).prop("disabled", true);
    },

    createTreeViewCourses: function () {
        ChooseCourse.trainingCoursesDataSource = new kendo.data.HierarchicalDataSource({
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
        
        $("#" + ChooseCourse.trvCoursesId).kendoTreeView({
            template: "#= ChooseCourse.getTemplateNode(data) #",
            dataSource: ChooseCourse.trainingCoursesDataSource,
            dataTextField: "Name",
            dataBound: function () {
                this.expand(".k-item");
            },
            select: ChooseCourse.onSelectNode
        });
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

    onOpenChooseCourse: function () {       
        $("#" + ChooseCourse.btnSelectCourseId).removeAttr("disabled");
        $("#" + ChooseCourse.btnSelectCourseId).prop("disabled", true);
    },

    onCloseChooseCourseWindow: function () {
    },

    onSelectNode: function (e) {
        var dataItem = ChooseCourse.trainingCoursesDataSource.getByUid(e.node.dataset.uid);
        if (dataItem == null) {
            return;
        }

        if (dataItem.IsRoot) {
            $("#" + ChooseCourse.btnSelectCourseId).removeAttr("disabled");
            $("#" + ChooseCourse.btnSelectCourseId).prop("disabled", true);

            return;
        }

        if (dataItem.IsFamily) {
            $("#" + ChooseCourse.btnSelectCourseId).removeAttr("disabled");
            $("#" + ChooseCourse.btnSelectCourseId).prop("disabled", true);

            return;
        }

        if (dataItem.IsTitle) {
            $("#" + ChooseCourse.btnSelectCourseId).removeAttr("disabled");
            $("#" + ChooseCourse.btnSelectCourseId).prop("disabled", true);

            return;
        }

        if (dataItem.IsCourse) {  
            $("#" + ChooseCourse.btnSelectCourseId).removeAttr("disabled");
            $("#" + ChooseCourse.btnSelectCourseId).prop("disabled", false);
        }
    },

    searchCourse: function () {
        var textSearch = $("#" + ChooseCourse.inpCourseId).val();
        if (textSearch === "") {
            GeneralData.showNotification("Debes escribir un texto para poder buscar", "", "error");
            return;
        }

        var treeview = $("#" + ChooseCourse.trvCoursesId).data("kendoTreeView");
        treeview.dataSource.filter({
            field: "Name",
            operator: "contains",
            value: textSearch
        });
    },

    searchCourseInBack: function () {
        var textSearch = $("#" + ChooseCourse.inpCourseId).val();
        if (textSearch === "") {
            GeneralData.showNotification("Debes escribir un texto para poder buscar", "", "error");
            return;
        }

        $.ajax({
            url: "/Simulations/FindNode",
            data: {
                text: textSearch
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result.Status === Constants.resultStatus.Ok) {
                    var treeview = $("#" + ChooseCourse.trvCoursesId).data("kendoTreeView");
                    var dataItem = treeview.dataSource.get(data.result.Object.Id);
                    var node = treeview.findByUid(dataItem.uid);
                    treeview.select(node);

                    if (data.result.Object.IsCourse === true) {
                        $("#" + ChooseCourse.btnSelectCourseId).removeAttr("disabled");
                        $("#" + ChooseCourse.btnSelectCourseId).prop("disabled", false);
                    }

                    GeneralData.showNotification(data.result.Message, "", "success");
                }
                if (data.result.Status === Constants.resultStatus.Error) {
                    GeneralData.showNotification(data.result.Message, "", "error");
                }
            },
            error: function () {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    },

    selectCourse: function() {
        var treeview = $("#" + ChooseCourse.trvCoursesId).data("kendoTreeView");
        var selectedNode = treeview.select();
        TrainingService.selectCourse(ChooseCourse.trainingCoursesDataSource.getByUid(selectedNode[0].dataset.uid));

        var chooseCourseWindow = $("#" + this.chooseCourseId);
        chooseCourseWindow.data("kendoWindow").close();
    }
});