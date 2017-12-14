﻿var ForeignPreventionService = kendo.observable({
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
    btnValidateId: "btnValidateForeignPreventionService",

    errorFromFrontId: "errorFromFront",

    simulationId: null,
    numberEmployees: null,
    stretchCalculate: null,

    percentege: 80,
    total: 0,
    firstTime: true,

    init: function (id, numberEmployees) {
        kendo.culture("es-ES");

        this.firstTime = true;

        this.simulationId = id;
        this.numberEmployees = numberEmployees;

        this.setKendoUIWidgets();

        this.getStretchCalculateByNumberEmployees();

        this.blockFields();

        DetailSimulation.updateButtonsFromSimulationServices(false);
    },

    updateView: function() {
        var strValue = kendo.format("{0} €", ForeignPreventionService.stretchCalculate.AmountByEmployeeInTecniques);
        $("#" + this.lblAmountByEmployeeInTecniquesId).text(strValue);
        this.onChangeTextAmountTecniques();

        strValue = kendo.format("{0} €", ForeignPreventionService.stretchCalculate.AmountByEmployeeInHealthVigilance);
        $("#" + this.lblAmountByEmployeeInHealthVigilanceId).text(strValue);
        this.onChangeTextAmountHealthVigilance();

        strValue = kendo.format("{0} €", ForeignPreventionService.stretchCalculate.AmountByEmployeeInMedicalExamination);
        $("#" + this.lblAmountByEmployeeInMedicalExaminationId).text(strValue);
        this.onChangeTextAmountMedicalExamination();
    },

    setKendoUIWidgets: function() {
        $("#" + this.textAmountTecniquesId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: ForeignPreventionService.onChangeTextAmountTecniques
        });
        $("#" + this.textAmountHealthVigilanceId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: ForeignPreventionService.onChangeTextAmountHealthVigilance
        });
        $("#" + this.textAmountMedicalExaminationId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: ForeignPreventionService.onChangeTextAmountMedicalExamination
        });

        $("#" + this.btnSendNotificationId).click(function () {
            ForeignPreventionService.sendNotificationFromSimulator();
        });

        $("#" + this.btnSendToCompaniesId).click(function () {
            ForeignPreventionService.sendToCompanies();
        });

        $("#" + this.btnCancelId).click(function () {
            ForeignPreventionService.goToSimulators();
        });
    },

    getStretchCalculateByNumberEmployees: function () {
        $.ajax({
            url: "/CommercialTool/Simulations/GetStretchCalculateByNumberEmployees",
            data: {
                numberEmployees: this.numberEmployees
            },
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.stretchCalculate !== null) {
                    ForeignPreventionService.stretchCalculate = response.stretchCalculate;
                    ForeignPreventionService.updateView();

                    $("#" + ForeignPreventionService.btnValidateId).removeAttr("disabled");
                    $("#" + ForeignPreventionService.btnValidateId).prop("disabled", true);
                }
            }
        });
    },

    onChangeTextAmountTecniques: function () {
        var value = parseFloat($("#" + ForeignPreventionService.textAmountTecniquesId).val());
        if (isNaN(value)) {
            return;
        }

        $("#" + ForeignPreventionService.btnValidateId).removeAttr("disabled");
        $("#" + ForeignPreventionService.btnValidateId).prop("disabled", false);

        var widget = $("#" + ForeignPreventionService.textAmountTecniquesId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + ForeignPreventionService.lblPercentegeByEmployeeInTecniquesId).text("%");
            $("#" + ForeignPreventionService.lblTotalByEmployeeInTecniquesId).text("€");

            ForeignPreventionService.calculateTotal();

            return;
        }

        var strVal = $("#" + ForeignPreventionService.lblAmountByEmployeeInTecniquesId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * ForeignPreventionService.percentege / 100;
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
        $("#" + ForeignPreventionService.lblPercentegeByEmployeeInTecniquesId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + ForeignPreventionService.lblTotalByEmployeeInTecniquesId).text((value * ForeignPreventionService.numberEmployees).toFixed(2) + " €");

        ForeignPreventionService.calculateTotal();
    },

    onChangeTextAmountHealthVigilance: function () {
        var value = parseFloat($("#" + ForeignPreventionService.textAmountHealthVigilanceId).val());
        if (isNaN(value)) {
            return;
        }

        $("#" + ForeignPreventionService.btnValidateId).removeAttr("disabled");
        $("#" + ForeignPreventionService.btnValidateId).prop("disabled", false);

        var widget = $("#" + ForeignPreventionService.textAmountHealthVigilanceId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + ForeignPreventionService.lblPercentegeByEmployeeInHealthVigilanceId).text("%");
            $("#" + ForeignPreventionService.lblTotalByEmployeeInHealthVigilanceId).text("€");

            ForeignPreventionService.calculateTotal();

            return;
        }

        var strVal = $("#" + ForeignPreventionService.lblAmountByEmployeeInHealthVigilanceId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * ForeignPreventionService.percentege / 100;
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
        $("#" + ForeignPreventionService.lblPercentegeByEmployeeInHealthVigilanceId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + ForeignPreventionService.lblTotalByEmployeeInHealthVigilanceId).text((value * ForeignPreventionService.numberEmployees).toFixed(2) + " €");

        ForeignPreventionService.calculateTotal();
    },

    onChangeTextAmountMedicalExamination: function () {
        var value = parseFloat($("#" + ForeignPreventionService.textAmountMedicalExaminationId).val());
        if (isNaN(value)) {
            return;
        }

        $("#" + ForeignPreventionService.btnValidateId).removeAttr("disabled");
        $("#" + ForeignPreventionService.btnValidateId).prop("disabled", false);

        var widget = $("#" + ForeignPreventionService.textAmountMedicalExaminationId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + ForeignPreventionService.lblPercentegeByEmployeeInMedicalExaminationId).text("%");
            $("#" + ForeignPreventionService.lblTotalByEmployeeInMedicalExaminationId).text("€");

            ForeignPreventionService.calculateTotal();

            return;
        }

        var strVal = $("#" + ForeignPreventionService.lblAmountByEmployeeInMedicalExaminationId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * ForeignPreventionService.percentege / 100;
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
        $("#" + ForeignPreventionService.lblPercentegeByEmployeeInMedicalExaminationId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + ForeignPreventionService.lblTotalByEmployeeInMedicalExaminationId).text((value * ForeignPreventionService.numberEmployees).toFixed(2) + " €");

        ForeignPreventionService.calculateTotal();
    },

    calculateTotal: function () {
        var strValue = $("#" + ForeignPreventionService.lblTotalByEmployeeInTecniquesId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInTecniques = 0;
        if (strValue !== "") {
            totalByEmployeeInTecniques = parseFloat(strValue);
        }

        strValue = $("#" + ForeignPreventionService.lblTotalByEmployeeInHealthVigilanceId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInHealthVigilance = 0;
        if (strValue !== "") {
            totalByEmployeeInHealthVigilance = parseFloat(strValue);
        }

        strValue = $("#" + ForeignPreventionService.lblTotalByEmployeeInMedicalExaminationId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInMedicalExamination = 0;
        if (strValue !== "") {
            totalByEmployeeInMedicalExamination = parseFloat(strValue);
        }

        $("#" + ForeignPreventionService.lblTotalId).text(
            (totalByEmployeeInTecniques +
                totalByEmployeeInHealthVigilance +
                totalByEmployeeInMedicalExamination).toFixed(2) + " €");
    },

    goToForeignPreventionService: function () {
        var params = {
            url: "/CommercialTool/Simulations/DetailSimulation",
            data: {
                simulationId: this.simulationId,
                selectTabId: 0
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

        ForeignPreventionService.goToForeignPreventionService();

        DetailSimulation.updateButtonsFromSimulationServices(true);
    },

    sendToCompanies: function () {
        $.ajax({
            url: "/Company/SendToCompanies",
            data: {
                simulationId: this.simulationId
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
                simulationId: this.simulationId
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
    },

    blockFields: function () {
        if (DetailSimulation.simulationStateId === Constants.simulationState.SendToCompany) {
            $("#" + ForeignPreventionService.textAmountTecniquesId).removeAttr("disabled");
            $("#" + ForeignPreventionService.textAmountTecniquesId).prop("disabled", true);
            $("#" + ForeignPreventionService.textAmountHealthVigilanceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.textAmountHealthVigilanceId).prop("disabled", true);
            $("#" + ForeignPreventionService.textAmountMedicalExaminationId).removeAttr("disabled");
            $("#" + ForeignPreventionService.textAmountMedicalExaminationId).prop("disabled", true);

            $("#" + ForeignPreventionService.btnValidateId).removeAttr("disabled");
            $("#" + ForeignPreventionService.btnValidateId).prop("disabled", true);
        }
    }
});