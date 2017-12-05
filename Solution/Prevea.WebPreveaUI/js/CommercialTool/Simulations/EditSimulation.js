var EditSimulation = kendo.observable({
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
    btnSendNotificationId: "btnSendNotification",
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
        var strValue = kendo.format("{0} €", EditSimulation.stretchCalculate.AmountByEmployeeInTecniques);
        $("#" + this.lblAmountByEmployeeInTecniquesId).text(strValue);
        this.onChangeTextAmountTecniques();

        strValue = kendo.format("{0} €", EditSimulation.stretchCalculate.AmountByEmployeeInHealthVigilance);
        $("#" + this.lblAmountByEmployeeInHealthVigilanceId).text(strValue);
        this.onChangeTextAmountHealthVigilance();

        strValue = kendo.format("{0} €", EditSimulation.stretchCalculate.AmountByEmployeeInMedicalExamination);
        $("#" + this.lblAmountByEmployeeInMedicalExaminationId).text(strValue);
        this.onChangeTextAmountMedicalExamination();
    },

    setKendoUIWidgets: function() {
        $("#" + this.textNumberEmployeesId).kendoNumericTextBox({
            decimals: 0,
            format: "0",
            change: EditSimulation.onChangeTextNumberEmployees
        });
        $("#" + this.textAmountTecniquesId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: EditSimulation.onChangeTextAmountTecniques
        });
        $("#" + this.textAmountHealthVigilanceId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: EditSimulation.onChangeTextAmountHealthVigilance
        });
        $("#" + this.textAmountMedicalExaminationId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: EditSimulation.onChangeTextAmountMedicalExamination
        });

        $("#" + this.btnSendNotificationId).click(function () {
            EditSimulation.sendNotificationFromSimulator();
        });

        $("#" + this.btnSendToCompaniesId).click(function () {
            EditSimulation.sendToCompanies();
        });

        $("#" + this.btnCancelId).click(function () {
            EditSimulation.goToSimulators();
        });

        $("#" + this.textBoxNameId).change(function () {
            $("#" + EditSimulation.btnValidateId).removeAttr("disabled");
            $("#" + EditSimulation.btnValidateId).prop("disabled", false);
            $("#" + EditSimulation.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + EditSimulation.btnSendToCompaniesId).prop("disabled", true);
        });

        $("#" + this.textBoxNIFId).change(function () {
            $("#" + EditSimulation.btnValidateId).removeAttr("disabled");
            $("#" + EditSimulation.btnValidateId).prop("disabled", false);
            $("#" + EditSimulation.btnSendToCompaniesId).removeAttr("disabled");
            $("#" + EditSimulation.btnSendToCompaniesId).prop("disabled", true);
        });
    },

    onChangeTextNumberEmployees: function () {
        $.ajax({
            url: "/Company/GetStretchCalculateByNumberEmployees",
            data: {
                numberEmployees: parseFloat($("#" + EditSimulation.textNumberEmployeesId).val())
            },
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.stretchCalculate !== null) {
                    EditSimulation.stretchCalculate = response.stretchCalculate;
                    EditSimulation.updateView();

                    if (!EditSimulation.firstTime) {
                        $("#" + EditSimulation.btnValidateId).removeAttr("disabled");
                        $("#" + EditSimulation.btnValidateId).prop("disabled", false);
                        $("#" + EditSimulation.btnSendToCompaniesId).removeAttr("disabled");
                        $("#" + EditSimulation.btnSendToCompaniesId).prop("disabled", true);
                    } else {
                        $("#" + EditSimulation.btnValidateId).removeAttr("disabled");
                        $("#" + EditSimulation.btnValidateId).prop("disabled", true);
                        $("#" + EditSimulation.btnSendToCompaniesId).removeAttr("disabled");
                        $("#" + EditSimulation.btnSendToCompaniesId).prop("disabled", false);                        
                    }

                    EditSimulation.firstTime = false;
                }
            }
        });
    },

    onChangeTextAmountTecniques: function () {
        var value = parseFloat($("#" + EditSimulation.textAmountTecniquesId).val());
        if (isNaN(value)) {
            return;
        }

        $("#" + EditSimulation.btnValidateId).removeAttr("disabled");
        $("#" + EditSimulation.btnValidateId).prop("disabled", false);
        $("#" + EditSimulation.btnSendToCompaniesId).removeAttr("disabled");
        $("#" + EditSimulation.btnSendToCompaniesId).prop("disabled", true);

        var widget = $("#" + EditSimulation.textAmountTecniquesId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + EditSimulation.lblPercentegeByEmployeeInTecniquesId).text("%");
            $("#" + EditSimulation.lblTotalByEmployeeInTecniquesId).text("€");

            EditSimulation.calculateTotal();

            return;
        }

        var strVal = $("#" + EditSimulation.lblAmountByEmployeeInTecniquesId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * EditSimulation.percentege / 100;
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
        $("#" + EditSimulation.lblPercentegeByEmployeeInTecniquesId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + EditSimulation.lblTotalByEmployeeInTecniquesId).text((value * parseFloat($("#" + EditSimulation.textNumberEmployeesId).val())).toFixed(2) + " €");

        EditSimulation.calculateTotal();
    },

    onChangeTextAmountHealthVigilance: function () {
        var value = parseFloat($("#" + EditSimulation.textAmountHealthVigilanceId).val());
        if (isNaN(value)) {
            return;
        }

        $("#" + EditSimulation.btnValidateId).removeAttr("disabled");
        $("#" + EditSimulation.btnValidateId).prop("disabled", false);
        $("#" + EditSimulation.btnSendToCompaniesId).removeAttr("disabled");
        $("#" + EditSimulation.btnSendToCompaniesId).prop("disabled", true);


        var widget = $("#" + EditSimulation.textAmountHealthVigilanceId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + EditSimulation.lblPercentegeByEmployeeInHealthVigilanceId).text("%");
            $("#" + EditSimulation.lblTotalByEmployeeInHealthVigilanceId).text("€");

            EditSimulation.calculateTotal();

            return;
        }

        var strVal = $("#" + EditSimulation.lblAmountByEmployeeInHealthVigilanceId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * EditSimulation.percentege / 100;
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
        $("#" + EditSimulation.lblPercentegeByEmployeeInHealthVigilanceId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + EditSimulation.lblTotalByEmployeeInHealthVigilanceId).text((value * parseFloat($("#" + EditSimulation.textNumberEmployeesId).val())).toFixed(2) + " €");

        EditSimulation.calculateTotal();
    },

    onChangeTextAmountMedicalExamination: function () {
        var value = parseFloat($("#" + EditSimulation.textAmountMedicalExaminationId).val());
        if (isNaN(value)) {
            return;
        }

        $("#" + EditSimulation.btnValidateId).removeAttr("disabled");
        $("#" + EditSimulation.btnValidateId).prop("disabled", false);
        $("#" + EditSimulation.btnSendToCompaniesId).removeAttr("disabled");
        $("#" + EditSimulation.btnSendToCompaniesId).prop("disabled", true);

        var widget = $("#" + EditSimulation.textAmountMedicalExaminationId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + EditSimulation.lblPercentegeByEmployeeInMedicalExaminationId).text("%");
            $("#" + EditSimulation.lblTotalByEmployeeInMedicalExaminationId).text("€");

            EditSimulation.calculateTotal();

            return;
        }

        var strVal = $("#" + EditSimulation.lblAmountByEmployeeInMedicalExaminationId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * EditSimulation.percentege / 100;
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
        $("#" + EditSimulation.lblPercentegeByEmployeeInMedicalExaminationId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + EditSimulation.lblTotalByEmployeeInMedicalExaminationId).text((value * parseFloat($("#" + EditSimulation.textNumberEmployeesId).val())).toFixed(2) + " €");

        EditSimulation.calculateTotal();
    },

    calculateTotal: function () {
        var strValue = $("#" + EditSimulation.lblTotalByEmployeeInTecniquesId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInTecniques = 0;
        if (strValue !== "") {
            totalByEmployeeInTecniques = parseFloat(strValue);
        }

        strValue = $("#" + EditSimulation.lblTotalByEmployeeInHealthVigilanceId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInHealthVigilance = 0;
        if (strValue !== "") {
            totalByEmployeeInHealthVigilance = parseFloat(strValue);
        }

        strValue = $("#" + EditSimulation.lblTotalByEmployeeInMedicalExaminationId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInMedicalExamination = 0;
        if (strValue !== "") {
            totalByEmployeeInMedicalExamination = parseFloat(strValue);
        }

        $("#" + EditSimulation.lblTotalId).text(
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

    goToEditSimulation: function () {
        var params = {
            url: "/Company/EditSimulation",
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

        EditSimulation.goToEditSimulation();
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
    },

    sendNotificationFromSimulator: function () {
        $.ajax({
            url: "/Company/SendNotificationFromSimulator",
            data: {
                simulatorId: this.simulatorId
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result.Status === 0) {
                    GeneralData.showNotification(Constants.ok, "", "success");
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