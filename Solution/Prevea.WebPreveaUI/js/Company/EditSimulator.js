var EditSimulator = kendo.observable({
    textBoxNameId: "textBoxName",
    textBoxNIFId: "textBoxNIF",
    textNumberEmployeesId: "textNumberEmployees",
    textAmountTecniquesId: "textAmountTecniques",
    textAmountHealthVigilanceId: "textAmountHealthVigilance",
    textAmountMedicalExaminationId: "textAmountMedicalExamination",
    lblAmountByEmployeeInTecniquesId: "lblAmountByEmployeeInTecniques",
    lblAmountByEmployeeInHealthVigilanceId: "lblAmountByEmployeeInHealthVigilance",
    lblAmountByEmployeeInMedicalExaminationId: "lblAmountByEmployeeInMedicalExamination",
    lblPercentegeByEmployeeInTecniquesId: "lblPercentegeByEmployeeInTecniques",
    lblPercentegeByEmployeeInHealthVigilanceId: "lblPercentegeByEmployeeInHealthVigilance",
    lblPercentegeByEmployeeInMedicalExaminationId: "lblPercentegeByEmployeeInMedicalExamination",
    lblTotalByEmployeeInTecniquesId: "lblTotalByEmployeeInTecniques",
    lblTotalByEmployeeInHealthVigilanceId: "lblTotalByEmployeeInHealthVigilance",
    lblTotalByEmployeeInMedicalExaminationId: "lblTotalByEmployeeInMedicalExamination",
    lblTotalId: "lblTotal",
    btnValidateId: "btnValidate",
    btnSendToCompaniesId: "btnSendToCompanies",
    btnCancelId: "btnCancel",

    errorFromFrontId: "errorFromFront",

    simulatorId: null,
    stretchCalculate: null,

    percentege: 80,
    total: 0,
    firstTime: true,

    init: function(id) {
        kendo.culture("es-ES");

        this.firstTime = true;

        this.simulatorId = id;

        this.setKendoUIWidgets();

        this.onChangeTextNumberEmployees();
    },

    updateView: function() {
        var strValue = kendo.format("{0} €", EditSimulator.stretchCalculate.AmountByEmployeeInTecniques);
        $("#" + this.lblAmountByEmployeeInTecniquesId).text(strValue);
        this.onChangeTextAmountTecniques();

        strValue = kendo.format("{0} €", EditSimulator.stretchCalculate.AmountByEmployeeInHealthVigilance);
        $("#" + this.lblAmountByEmployeeInHealthVigilanceId).text(strValue);
        this.onChangeTextAmountHealthVigilance();

        strValue = kendo.format("{0} €", EditSimulator.stretchCalculate.AmountByEmployeeInMedicalExamination);
        $("#" + this.lblAmountByEmployeeInMedicalExaminationId).text(strValue);
        this.onChangeTextAmountMedicalExamination();
    },

    setKendoUIWidgets: function() {
        $("#" + this.textNumberEmployeesId).kendoNumericTextBox({
            decimals: 0,
            format: "0",
            change: EditSimulator.onChangeTextNumberEmployees
        });
        $("#" + this.textAmountTecniquesId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: EditSimulator.onChangeTextAmountTecniques
        });
        $("#" + this.textAmountHealthVigilanceId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: EditSimulator.onChangeTextAmountHealthVigilance
        });
        $("#" + this.textAmountMedicalExaminationId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: EditSimulator.onChangeTextAmountMedicalExamination
        });

        $("#" + this.btnSendToCompaniesId).click(function () {
            EditSimulator.sendToCompanies();
        });

        $("#" + this.btnCancelId).click(function () {
            EditSimulator.goToSimulators();
        });

        $("#" + this.textBoxNameId).change(function () {
            $("#" + EditSimulator.btnValidateId).removeAttr("disabled");
            $("#" + EditSimulator.btnValidateId).prop("disabled", false);
            $("#" + EditSimulator.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + EditSimulator.btnSendToCompaniesId).prop("disabled", true);
        });

        $("#" + this.textBoxNIFId).change(function () {
            $("#" + EditSimulator.btnValidateId).removeAttr("disabled");
            $("#" + EditSimulator.btnValidateId).prop("disabled", false);
            $("#" + EditSimulator.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + EditSimulator.btnSendToCompaniesId).prop("disabled", true);
        });
    },

    onChangeTextNumberEmployees: function () {
        $.ajax({
            url: "/Company/GetStretchCalculateByNumberEmployees",
            data: {
                numberEmployees: parseFloat($("#" + EditSimulator.textNumberEmployeesId).val())
            },
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.stretchCalculate !== null) {
                    EditSimulator.stretchCalculate = response.stretchCalculate;
                    EditSimulator.updateView();

                    if (!EditSimulator.firstTime) {
                        $("#" + EditSimulator.btnValidateId).removeAttr("disabled");
                        $("#" + EditSimulator.btnValidateId).prop("disabled", false);
                        $("#" + EditSimulator.btnSendToCompaniesId).removeAttr("disabled");
                        $("#" + EditSimulator.btnSendToCompaniesId).prop("disabled", true);
                    } else {
                        $("#" + EditSimulator.btnValidateId).removeAttr("disabled");
                        $("#" + EditSimulator.btnValidateId).prop("disabled", true);
                        $("#" + EditSimulator.btnSendToCompaniesId).removeAttr("disabled");
                        $("#" + EditSimulator.btnSendToCompaniesId).prop("disabled", false);                        
                    }

                    EditSimulator.firstTime = false;
                }
            }
        });
    },

    onChangeTextAmountTecniques: function () {
        var value = parseFloat($("#" + EditSimulator.textAmountTecniquesId).val());
        if (isNaN(value)) {
            return;
        }

        $("#" + EditSimulator.btnValidateId).removeAttr("disabled");
        $("#" + EditSimulator.btnValidateId).prop("disabled", false);
        $("#" + EditSimulator.btnSendToCompaniesId).removeAttr("disabled");
        $("#" + EditSimulator.btnSendToCompaniesId).prop("disabled", true);

        var widget = $("#" + EditSimulator.textAmountTecniquesId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + EditSimulator.lblPercentegeByEmployeeInTecniquesId).text("%");
            $("#" + EditSimulator.lblTotalByEmployeeInTecniquesId).text("€");

            EditSimulator.calculateTotal();

            return;
        }

        var strVal = $("#" + EditSimulator.lblAmountByEmployeeInTecniquesId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * EditSimulator.percentege / 100;
        if (value < percentegeValueFix) {
            widget.wrapper.find("input").css("background-color", "#f56954");
            widget.wrapper.find("input").css("color", "black");
        } else {
            if (value >= percentegeValueFix && value <= valueFix) {
                widget.wrapper.find("input").css("background-color", "white");
                widget.wrapper.find("input").css("color", "#4b4b4b");
            } else {
                widget.wrapper.find("input").css("background-color", "#008d4c");
                widget.wrapper.find("input").css("color", "black");
            }
        }
        widget.wrapper.width("100%");
        var percentegeCalculate = value * 100 / valueFix;
        $("#" + EditSimulator.lblPercentegeByEmployeeInTecniquesId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + EditSimulator.lblTotalByEmployeeInTecniquesId).text((value * parseFloat($("#" + EditSimulator.textNumberEmployeesId).val())).toFixed(2) + " €");

        EditSimulator.calculateTotal();
    },

    onChangeTextAmountHealthVigilance: function () {
        var value = parseFloat($("#" + EditSimulator.textAmountHealthVigilanceId).val());
        if (isNaN(value)) {
            return;
        }

        $("#" + EditSimulator.btnValidateId).removeAttr("disabled");
        $("#" + EditSimulator.btnValidateId).prop("disabled", false);
        $("#" + EditSimulator.btnSendToCompaniesId).removeAttr("disabled");
        $("#" + EditSimulator.btnSendToCompaniesId).prop("disabled", true);


        var widget = $("#" + EditSimulator.textAmountHealthVigilanceId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + EditSimulator.lblPercentegeByEmployeeInHealthVigilanceId).text("%");
            $("#" + EditSimulator.lblTotalByEmployeeInHealthVigilanceId).text("€");

            EditSimulator.calculateTotal();

            return;
        }

        var strVal = $("#" + EditSimulator.lblAmountByEmployeeInHealthVigilanceId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * EditSimulator.percentege / 100;
        if (value < percentegeValueFix) {
            widget.wrapper.find("input").css("background-color", "#f56954");
            widget.wrapper.find("input").css("color", "black");
        } else {
            if (value >= percentegeValueFix && value <= valueFix) {
                widget.wrapper.find("input").css("background-color", "white");
                widget.wrapper.find("input").css("color", "#4b4b4b");
            } else {
                widget.wrapper.find("input").css("background-color", "#008d4c");
                widget.wrapper.find("input").css("color", "black");
            }
        }
        widget.wrapper.width("100%");
        var percentegeCalculate = value * 100 / valueFix;
        $("#" + EditSimulator.lblPercentegeByEmployeeInHealthVigilanceId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + EditSimulator.lblTotalByEmployeeInHealthVigilanceId).text((value * parseFloat($("#" + EditSimulator.textNumberEmployeesId).val())).toFixed(2) + " €");

        EditSimulator.calculateTotal();
    },

    onChangeTextAmountMedicalExamination: function () {
        var value = parseFloat($("#" + EditSimulator.textAmountMedicalExaminationId).val());
        if (isNaN(value)) {
            return;
        }

        $("#" + EditSimulator.btnValidateId).removeAttr("disabled");
        $("#" + EditSimulator.btnValidateId).prop("disabled", false);
        $("#" + EditSimulator.btnSendToCompaniesId).removeAttr("disabled");
        $("#" + EditSimulator.btnSendToCompaniesId).prop("disabled", true);

        var widget = $("#" + EditSimulator.textAmountMedicalExaminationId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + EditSimulator.lblPercentegeByEmployeeInMedicalExaminationId).text("%");
            $("#" + EditSimulator.lblTotalByEmployeeInMedicalExaminationId).text("€");

            EditSimulator.calculateTotal();

            return;
        }

        var strVal = $("#" + EditSimulator.lblAmountByEmployeeInMedicalExaminationId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * EditSimulator.percentege / 100;
        if (value < percentegeValueFix) {
            widget.wrapper.find("input").css("background-color", "#f56954");
            widget.wrapper.find("input").css("color", "black");
        } else {
            if (value >= percentegeValueFix && value <= valueFix) {
                widget.wrapper.find("input").css("background-color", "white");
                widget.wrapper.find("input").css("color", "#4b4b4b");
            } else {
                widget.wrapper.find("input").css("background-color", "#008d4c");
                widget.wrapper.find("input").css("color", "black");
            }
        }
        widget.wrapper.width("100%");
        var percentegeCalculate = value * 100 / valueFix;
        $("#" + EditSimulator.lblPercentegeByEmployeeInMedicalExaminationId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + EditSimulator.lblTotalByEmployeeInMedicalExaminationId).text((value * parseFloat($("#" + EditSimulator.textNumberEmployeesId).val())).toFixed(2) + " €");

        EditSimulator.calculateTotal();
    },

    calculateTotal: function () {
        var strValue = $("#" + EditSimulator.lblTotalByEmployeeInTecniquesId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInTecniques = 0;
        if (strValue !== "") {
            totalByEmployeeInTecniques = parseFloat(strValue);
        }

        strValue = $("#" + EditSimulator.lblTotalByEmployeeInHealthVigilanceId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInHealthVigilance = 0;
        if (strValue !== "") {
            totalByEmployeeInHealthVigilance = parseFloat(strValue);
        }

        strValue = $("#" + EditSimulator.lblTotalByEmployeeInMedicalExaminationId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInMedicalExamination = 0;
        if (strValue !== "") {
            totalByEmployeeInMedicalExamination = parseFloat(strValue);
        }

        $("#" + EditSimulator.lblTotalId).text(
            (totalByEmployeeInTecniques +
                totalByEmployeeInHealthVigilance +
                totalByEmployeeInMedicalExamination).toFixed(2) + " €");
    },

    goToSimulators: function () {
        var params = {
            url: "/Company/Simulators"
        };
        GeneralData.goToActionController(params);
    },

    goToEditSimulator: function () {
        var params = {
            url: "/Company/EditSimulator",
            data: {
                simulatorId: this.simulatorId
            }
        };
        GeneralData.goToActionController(params);
    },

    onSuccessUpdate: function (data) {
        if (data.Status === 0) {
            GeneralData.showNotification(Constants.ok, "", "success");
        }
        if (data.Status === 1) {
            GeneralData.showNotification(Constants.ko, "", "error");
        }

        EditSimulator.goToEditSimulator();
    },

    sendToCompanies: function () {
        $.ajax({
            url: "/Company/SendToCompanies",
            data: {
                simulatorId: this.simulatorId
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result.Status === 0) {
                    GeneralData.showNotification(Constants.ok, "", "success");

                    var params = {
                        url: "/Company/DetailCompany",
                        data: {
                            id: data.result.Object,
                            selectTabId: 0
                        }
                    };
                    GeneralData.goToActionController(params);
                }
                if (data.result.Status === 1) {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            },
            error: function() {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    }
});