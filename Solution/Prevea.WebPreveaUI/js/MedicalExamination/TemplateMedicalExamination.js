var TemplateMedicalExamination = kendo.observable({

    btnValidateId: "btnValidate",
    btnPrintId: "btnPrint",

    model: null,
    inputTemplatesJSONToModel: null,

    init: function (model) {
        kendo.culture("es-ES");

        this.model = model;
        this.inputTemplatesJSONToModel = $.parseJSON(model.DocumentInputTemplateJSON);

        this.setUpPage();
    },

    setUpPage: function () {
        for (var i = 0; i < this.inputTemplatesJSONToModel.length; i++) {
            var div = $("#" + this.inputTemplatesJSONToModel[i].Name);
            if (div.length) {
                this.setUpInput(this.inputTemplatesJSONToModel[i], div);
            } else {
                GeneralData.showNotification(kendo.format("Elemento [{0}] no encontrado", this.inputTemplatesJSONToModel[i].Name), "", "error");
            }
        }

        var height = parseFloat($("#in-me-19").val().replace(",", "."));
        var weight = parseFloat($("#in-me-20").val().replace(",", "."));
        var imc = (weight / (height * height)).toFixed(2);
        $("#me-imc").text(imc);

        var value;
        var percentage;
        var constant;

        value = parseFloat($("#in-me-49").val().replace(",", "."));
        constant = 5000;
        percentage = (value * 100 / constant).toFixed(0);
        $("#me-FVC").text(percentage);

        value = parseFloat($("#in-me-50").val().replace(",", "."));
        constant = 4181;
        percentage = (value * 100 / constant).toFixed(0);
        $("#me-FEV1").text(percentage);

        value = parseFloat($("#in-me-52").val().replace(",", "."));
        constant = 4691;
        percentage = (value * 100 / constant).toFixed(0);
        $("#me-FEF").text(percentage);
    },

    setUpInput: function (input, div) {
        var value;
        switch (input.Type) {
            case Constants.inputTemplateType.Input:
                if (input.Text !== null) {
                    value = input.Text;
                } else {
                    value = input.DefaultText;
                }
                var $input = $(kendo.format("<input id='in-{0}' value='{1}' class='form-control' style='width: 100%' onchange: 'TemplateMedicalExamination.onChange()' />", input.Name, value)).appendTo(div);
                $input.change(TemplateMedicalExamination.onChange);
                break;
            case Constants.inputTemplateType.TextArea:
                if (input.Text !== null) {
                    value = input.Text;
                } else {
                    value = input.DefaultText;
                }
                var $textArea = $(kendo.format("<textarea id='in-{0}' class='form-control autogrow' rows='4' style='width: 100%' onchange: 'TemplateMedicalExamination.onChange()'>{1}</textarea>", input.Name, value)).appendTo(div);
                $textArea.change(TemplateMedicalExamination.onChange);
                break;
            case Constants.inputTemplateType.DateTime:
                if (input.Text !== null) {
                    value = input.Text;
                } else {
                    value = input.DefaultText;
                }
                var $date = $(kendo.format("<input id='in-{0}' value='{1}' class='form-control' style='width: 100%' />", input.Name, value)).appendTo(div);
                $date.kendoDatePicker({
                    change: TemplateMedicalExamination.onChange
                });
                break;
            case Constants.inputTemplateType.Single:
                var data = [];
                for (var i = 0; i < input.DataSource.length; i++) {
                    data.push({ id: i, description: input.DataSource [i] });
                }
                var dataSource = new kendo.data.DataSource({
                    data: data
                });

                var $single = $(kendo.format("<input id='in-{0}' style='width: 100%' />", input.Name)).appendTo(div);
                $single.kendoDropDownList({
                    change: TemplateMedicalExamination.onChange,
                    dataTextField: "description",
                    dataValueField: "id",
                    dataSource: dataSource
                });
                if (input.Value !== null) {
                    value = input.Value;
                } else {
                    value = input.DefaultValue;
                }

                $single.data("kendoDropDownList").select(parseInt(value));
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
        this.model.DocumentInputTemplateJSON = JSON.stringify(this.inputTemplatesJSONToModel);
        $.ajax({
            url: "/MedicalExamination/SaveMedicalExamination",
            data: JSON.stringify({ "templateMedicalExaminationViewModel": this.model }),
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.resultStatus === Constants.resultStatus.Ok) {
                    DetailMedicalExamination.createIconMedicalExaminationState(response.documentState);

                    GeneralData.showNotification(Constants.ok, "", "success");
                } else {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            }
        });
    },

    printMedicalExamination_Kendo: function () {
        kendo.drawing.drawDOM($("#pageMedicalExamination"))
            .then(function (group) {
                // Render the result as a PDF file
                return kendo.drawing.exportPDF(group, {
                    paperSize: "auto",
                    margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
                });
            })
            .done(function (data) {
                // Save the PDF file
                kendo.saveAs({
                    dataURI: data,
                    fileName: "prueba.pdf",
                    proxyURL: "https://demos.telerik.com/kendo-ui/service/export"
                });
            });
    },

    printMedicalExamination: function () {
        kendo.ui.progress($("#framePpal"), true);

        this.model.DocumentInputTemplateJSON = JSON.stringify(this.inputTemplatesJSONToModel);
        $.ajax({
            url: "/MedicalExamination/PrintMedicalExamination",
            data: JSON.stringify({ "templateMedicalExaminationViewModel": this.model }),
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.resultStatus === Constants.resultStatus.Ok) {
                    GeneralData.showNotification(Constants.ok, "", "success");
                    GeneralData.goToOpenFile(response.documentId);
                } else {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }

                kendo.ui.progress($("#framePpal"), false);
            }
        });
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
                case Constants.inputTemplateType.Single:
                    inputTemplate.Value = this.value();
                    inputTemplate.Text = this.text();
                    break;

                default:
                    inputTemplate.Text = this.value;
                    break;
            }
        } else {
            GeneralData.showNotification(kendo.format("Imposible actualizar [{0}]", this.id), "", "error");
        }

        if (this.id === "in-me-19" || this.id === "in-me-20") {
            var height = parseFloat($("#in-me-19").val().replace(",", "."));
            var weight = parseFloat($("#in-me-20").val().replace(",", "."));
            var imc = (weight / (height * height)).toFixed(2);
            $("#me-imc").text(imc);
        }

        var value = 0;
        var percentage = 0;
        var constant = 0;
        if (this.id === "in-me-49") {
            value = parseFloat($("#in-me-49").val().replace(",", "."));
            constant = 5000;
            percentage = (value * 100 / constant).toFixed(0);
            $("#me-FVC").text(percentage);
        }
        if (this.id === "in-me-50") {
            value = parseFloat($("#in-me-50").val().replace(",", "."));
            constant = 4181;
            percentage = (value * 100 / constant).toFixed(0);
            $("#me-FEV1").text(percentage);
        }
        if (this.id === "in-me-52") {
            value = parseFloat($("#in-me-52").val().replace(",", "."));
            constant = 4691;
            percentage = (value * 100 / constant).toFixed(0);
            $("#me-FEF").text(percentage);
        }
    }, 

    findInputTemplate: function (id) {
        for (var i = 0; i < this.inputTemplatesJSONToModel.length; i++) {
            if (this.inputTemplatesJSONToModel[i].Name === id) {
                return this.inputTemplatesJSONToModel[i];
            }
        }

        return null;
    }
});