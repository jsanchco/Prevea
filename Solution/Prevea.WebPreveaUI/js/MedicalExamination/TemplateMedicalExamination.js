var TemplateMedicalExamination = kendo.observable({

    btnValidateId: "btnValidate",

    model: null,
    medicalExaminationJSONToModel: null,

    init: function (model) {
        kendo.culture("es-ES");

        this.model = model;
        this.medicalExaminationJSONToModel = $.parseJSON(model.MedicalExamination.MedicalExaminationJSON);

        this.setUpPage();
        this.createKendoWidgets();
    },

    setUpPage: function () {
        for (var i = 0; i < this.medicalExaminationJSONToModel.length; i++) {
            var div = $("#" + this.medicalExaminationJSONToModel[i].Name);
            if (div.length) {
                this.setUpInput(this.medicalExaminationJSONToModel[i], div);
            } else {
                GeneralData.showNotification(kendo.format("Elemento [{0}] no encontrado", this.medicalExaminationJSONToModel[i].Name), "", "error");
            }
        }
    },

    createKendoWidgets: function () {

    },

    setUpInput: function (input, div) {
        var value;
        if (input.Text !== null) {
            value = input.Text;
        } else {
            value = input.DefaultText;
        }
        switch (input.Type) {
            case Constants.inputTemplateType.Input:
                var $input = $(kendo.format("<input id='in-{0}' value='{1}' class='form-control' style='width: 100%' onchange: 'TemplateMedicalExamination.onChange()' />", input.Name, value)).appendTo(div);
                $input.change(TemplateMedicalExamination.onChange);
                break;
            case Constants.inputTemplateType.TextArea:
                var $input = $(kendo.format("<textarea id='in-{0}' class='form-control autogrow' rows='2' style='width: 100%' onchange: 'TemplateMedicalExamination.onChange()'>{1}</textarea>", input.Name, value)).appendTo(div);
                $input.change(TemplateMedicalExamination.onChange);
                break;
            case Constants.inputTemplateType.DateTime:
                var $date = $(kendo.format("<input id='in-{0}' value='{1}' class='form-control' style='width: 100%' />", input.Name, value)).appendTo(div);
                $date.kendoDatePicker({
                    change: TemplateMedicalExamination.onChange
                });
                break;
        }
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

    saveMedicalExamination: function () {
        this.model.MedicalExamination.MedicalExaminationJSON = JSON.stringify(this.medicalExaminationJSONToModel);
        $.ajax({
            url: "/MedicalExamination/SaveMedicalExamination",
            data: JSON.stringify({ "requestMedicalExaminationEmployee": this.model }),
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.resultStatus === Constants.resultStatus.Ok) {
                    GeneralData.showNotification(Constants.ok, "", "success");
                } else {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            }
        });
    },

    printMedicalExamination: function () {
        alert("Print!!!");
    },

    onChange: function () {
        var inputTemplate;
        if (typeof this.id === "undefined") {
            inputTemplate = TemplateMedicalExamination.findInputTemplate(this.element.attr("id").substring(3));
        } else {
            inputTemplate = TemplateMedicalExamination.findInputTemplate(this.id.substring(3));
        }
        if (inputTemplate != null) {
            switch (inputTemplate.Type) {
                case Constants.inputTemplateType.DateTime:
                    inputTemplate.Text = kendo.toString(this.value(), "dd/MM/yyyy");
                    break;

                default:
                    inputTemplate.Text = this.value;
                    break;
            }
        } else {
            GeneralData.showNotification(kendo.format("Imposible actualizar [{0}]", this.id), "", "error");
        }
    }, 

    findInputTemplate: function (id) {
        for (var i = 0; i < this.medicalExaminationJSONToModel.length; i++) {
            if (this.medicalExaminationJSONToModel[i].Name === id) {
                return this.medicalExaminationJSONToModel[i];
            }
        }

        return null;
    }
});