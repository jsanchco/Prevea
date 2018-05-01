var DetailMedicalExamination = kendo.observable({

    iconMedicalExaminationStateId: "iconMedicalExaminationState",
    tabStripDetailMedicalExaminationId: "tabStripDetailMedicalExamination",
    confirmId: "confirm",

    id: null,
    selectTabId: null,
    medicalExaminationStateId: null,
    model: null,

    listInput: null,

    init: function (id, selectTabId, medicalExaminationStateId) {
        kendo.culture("es-ES");

        this.id = id;
        this.selectTabId = selectTabId;
        this.medicalExaminationStateId = medicalExaminationStateId;        

        this.setUpPage();
        this.createKendoWidgets();     
    },

    setUpPage: function () {
        this.createIconMedicalExaminationState(this.medicalExaminationStateId);
    },

    createKendoWidgets: function () {
        this.createTabStripDetailCompany();
    },

    createIconMedicalExaminationState: function (medicalExaminationState) {
        var html = "";
        if (medicalExaminationState === Constants.documentState.Pending) {
            html = "<div id='circleError' class='pull-right'></div>";
        }
        if (medicalExaminationState === Constants.documentState.InProcess) {
            html = "<div id='circleWarning' class='pull-right'></div>";
        }
        if (medicalExaminationState === Constants.documentState.Finished) {
            html = "<div id='circleSuccess' class='pull-right'></div>";
        }

        $("#" + this.iconMedicalExaminationStateId).html(html);
    },

    createTabStripDetailCompany: function () {
        var tabStrip = $("#" + this.tabStripDetailMedicalExaminationId).kendoTabStrip().data("kendoTabStrip");
        tabStrip.append({
            text: "RECONOCIMIENTO MÉDICO",
            contentUrl: kendo.format("/MedicalExamination/TemplateMedicalExamination?requestMedicalExaminationEmployeeId={0}", this.id)
        });
        tabStrip.append({
            text: "DOCUMENTOS",
            contentUrl: kendo.format("/MedicalExamination/DocumentsMedicalExamination?requestMedicalExaminationEmployeeId={0}", this.id)
        });

        tabStrip = $("#" + this.tabStripDetailMedicalExaminationId).data("kendoTabStrip");
        tabStrip.select(this.selectTabId);
    },

    goToDoctorWorkSheet: function () {
        var params = {
            url: "/DoctorWorkSheet/DoctorWorkSheet",
            data: {
            }
        };
        GeneralData.goToActionController(params);
    },

    goToMedicalExamination: function () {
        var params = {
            url: "/MedicalExamination/DetailMedicalExamination",
            data: {
                medicalExaminationId: this.id
            }
        };
        GeneralData.goToActionController(params);
    }
});