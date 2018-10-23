var ForeignPreventionService = kendo.observable({
    textAmountTecniquesId: "textAmountTecniques",
    textAmountHealthVigilanceId: "textAmountHealthVigilance",
    textAmountMedicalExaminationId: "textAmountMedicalExamination",
    textTotalId: "textTotal",
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
    lblPercentegeTotalId: "lblPercentegeTotal",
    btnSaveForeignPreventionServiceId: "btnSaveForeignPreventionService",
    btnValidateForeignPreventionServiceId: "btnValidateForeignPreventionService",

    errorFromFrontId: "errorFromFront",

    simulationId: null,
    numberEmployees: null,
    stretchCalculate: null,

    percentege: 80,
    total: 0,

    init: function (id, numberEmployees) {
        kendo.culture("es-ES");

        this.simulationId = id;
        this.numberEmployees = numberEmployees;
        this.changesInNumericTextBox = 0;

        this.setKendoUIWidgets();

        this.getStretchCalculateByNumberEmployees();

        this.blockFields();
    },

    updateView: function() {
        var strValue = kendo.format("{0} €", ForeignPreventionService.stretchCalculate.AmountByEmployeeInTecniques);
        $("#" + this.lblAmountByEmployeeInTecniquesId).text(strValue);
        this.onChangeTextAmountTecniques(true);

        strValue = kendo.format("{0} €", ForeignPreventionService.stretchCalculate.AmountByEmployeeInHealthVigilance);
        $("#" + this.lblAmountByEmployeeInHealthVigilanceId).text(strValue);
        this.onChangeTextAmountHealthVigilance(true);

        strValue = kendo.format("{0} €", ForeignPreventionService.stretchCalculate.AmountByEmployeeInMedicalExamination);
        $("#" + this.lblAmountByEmployeeInMedicalExaminationId).text(strValue);
        this.onChangeTextAmountMedicalExamination(true);

        this.onChangeTextTotal(true);
    },

    setKendoUIWidgets: function() {
        $("#" + this.textAmountTecniquesId).kendoNumericTextBox({
            format: "c",
            decimals: 2,
            change: ForeignPreventionService.onChangeTextAmountTecniques
        });
        $("#" + this.textAmountHealthVigilanceId).kendoNumericTextBox({
            format: "c",
            decimals: 2,
            change: ForeignPreventionService.onChangeTextAmountHealthVigilance
        });
        $("#" + this.textAmountMedicalExaminationId).kendoNumericTextBox({
            format: "c",
            decimals: 2,
            change: ForeignPreventionService.onChangeTextAmountMedicalExamination
        });
        $("#" + this.textTotalId).kendoNumericTextBox({
            format: "c",
            decimals: 2,
            change: ForeignPreventionService.onChangeTextTotal
        });

        $("#" + this.btnSaveForeignPreventionServiceId).removeAttr("disabled");
        $("#" + this.btnSaveForeignPreventionServiceId).prop("disabled", true);
    },

    getStretchCalculateByNumberEmployees: function () {
        $.ajax({
            url: "/Simulations/GetStretchCalculateByNumberEmployees",
            data: {
                numberEmployees: this.numberEmployees
            },
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.stretchCalculate !== null) {
                    ForeignPreventionService.stretchCalculate = response.stretchCalculate;
                    ForeignPreventionService.updateView();                    
                }
            }
        });
    },

    onChangeTextAmountTecniques: function (fromStretchCalculateByNumberEmployees) {
        var value = parseFloat($("#" + ForeignPreventionService.textAmountTecniquesId).val().replace(",", "."));
        if (isNaN(value)) {
            return;
        } 

        var widget = $("#" + ForeignPreventionService.textAmountTecniquesId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + ForeignPreventionService.lblPercentegeByEmployeeInTecniquesId).text("%");
            $("#" + ForeignPreventionService.lblTotalByEmployeeInTecniquesId).text("€");

            ForeignPreventionService.calculateTotal(fromStretchCalculateByNumberEmployees);

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

        if ($.type(fromStretchCalculateByNumberEmployees) !== "boolean" &&
            fromStretchCalculateByNumberEmployees !== true) {
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).prop("disabled", false);
            $("#" + ForeignPreventionService.btnValidateForeignPreventionServiceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.btnValidateForeignPreventionServiceId).prop("disabled", true);

            ForeignPreventionService.updateButtonsOnChange();
        } else {
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).prop("disabled", true);
        }

        ForeignPreventionService.calculateTotal(fromStretchCalculateByNumberEmployees);
    },

    onChangeTextAmountHealthVigilance: function (fromStretchCalculateByNumberEmployees) {
        var value = parseFloat($("#" + ForeignPreventionService.textAmountHealthVigilanceId).val().replace(",", "."));
        if (isNaN(value)) {
            return;
        }

        var widget = $("#" + ForeignPreventionService.textAmountHealthVigilanceId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + ForeignPreventionService.lblPercentegeByEmployeeInHealthVigilanceId).text("%");
            $("#" + ForeignPreventionService.lblTotalByEmployeeInHealthVigilanceId).text("€");

            ForeignPreventionService.calculateTotal(fromStretchCalculateByNumberEmployees);

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

        if ($.type(fromStretchCalculateByNumberEmployees) !== "boolean" &&
            fromStretchCalculateByNumberEmployees !== true) {
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).prop("disabled", false);

            ForeignPreventionService.updateButtonsOnChange();
        } else {
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).prop("disabled", true);
        }

        ForeignPreventionService.calculateTotal(fromStretchCalculateByNumberEmployees);
    },

    onChangeTextAmountMedicalExamination: function (fromStretchCalculateByNumberEmployees) {
        var value = parseFloat($("#" + ForeignPreventionService.textAmountMedicalExaminationId).val().replace(",", "."));
        if (isNaN(value)) {
            return;
        }

        var widget = $("#" + ForeignPreventionService.textAmountMedicalExaminationId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + ForeignPreventionService.lblPercentegeByEmployeeInMedicalExaminationId).text("%");
            $("#" + ForeignPreventionService.lblTotalByEmployeeInMedicalExaminationId).text("€");

            ForeignPreventionService.calculateTotal(fromStretchCalculateByNumberEmployees);

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

        if ($.type(fromStretchCalculateByNumberEmployees) !== "boolean" &&
            fromStretchCalculateByNumberEmployees !== true) {
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).prop("disabled", false);

            ForeignPreventionService.updateButtonsOnChange();
        } else {
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).prop("disabled", true);
        }

        ForeignPreventionService.calculateTotal(fromStretchCalculateByNumberEmployees);
    },

    onChangeTextTotal: function (fromStretchCalculateByNumberEmployees) {
        var strValue = $("#" + ForeignPreventionService.lblTotalByEmployeeInTecniquesId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var valueAmountTecniques = 0;
        if (strValue !== "") {
            valueAmountTecniques = parseFloat(strValue);
        }

        strValue = $("#" + ForeignPreventionService.lblTotalByEmployeeInHealthVigilanceId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var valueAmountHealthVigilance = 0;
        if (strValue !== "") {
            valueAmountHealthVigilance = parseFloat(strValue);
        }

        strValue = $("#" + ForeignPreventionService.lblTotalByEmployeeInMedicalExaminationId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var valueAmountMedicalExamination = 0;
        if (strValue !== "") {
            valueAmountMedicalExamination = parseFloat(strValue);
        }

        var valueTotal = $("#" + ForeignPreventionService.textTotalId)
            .kendoNumericTextBox().data("kendoNumericTextBox").value();

        var widget = $("#" + ForeignPreventionService.textTotalId).kendoNumericTextBox().data("kendoNumericTextBox");

        var sum = valueAmountTecniques + valueAmountHealthVigilance + valueAmountMedicalExamination;
        if (sum === 0) {
            return;
        }

        var percentegeCalculate = (valueTotal * 100) / sum;
        if (percentegeCalculate > 100) {
            widget.wrapper.find("input").css("background-color", "#008d4c");
            widget.wrapper.find("input").css("color", "black");
        } else {
            if (percentegeCalculate < ForeignPreventionService.percentege) {
                widget.wrapper.find("input").css("background-color", "#f56954");
                widget.wrapper.find("input").css("color", "black");
            } else {
                widget.wrapper.find("input").css("background-color", "white");
                widget.wrapper.find("input").css("color", "#4b4b4b");
            }
        }
        widget.wrapper.width("100%");

        widget = $("#" + ForeignPreventionService.textAmountTecniquesId).kendoNumericTextBox().data("kendoNumericTextBox");
        widget.wrapper.width("100%");
        widget = $("#" + ForeignPreventionService.textAmountHealthVigilanceId).kendoNumericTextBox().data("kendoNumericTextBox");
        widget.wrapper.width("100%");
        widget = $("#" + ForeignPreventionService.textAmountMedicalExaminationId).kendoNumericTextBox().data("kendoNumericTextBox");
        widget.wrapper.width("100%");

        $("#" + ForeignPreventionService.lblPercentegeTotalId).text((percentegeCalculate - 100).toFixed(2) + "%");

        if ($.type(fromStretchCalculateByNumberEmployees) !== "boolean" &&
            fromStretchCalculateByNumberEmployees !== true) {
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).prop("disabled", false);

            ForeignPreventionService.updateButtonsOnChange();
        } else {
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).prop("disabled", true);
        }
    },

    calculateTotal: function (fromStretchCalculateByNumberEmployees) {
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

        if ($.type(fromStretchCalculateByNumberEmployees) !== "boolean" &&
            fromStretchCalculateByNumberEmployees !== true) {
            var value = totalByEmployeeInTecniques +
                totalByEmployeeInHealthVigilance +
                totalByEmployeeInMedicalExamination;
            var numerictextbox = $("#" + ForeignPreventionService.textTotalId).data("kendoNumericTextBox");
            numerictextbox.value(value);
        }
    },

    goToForeignPreventionService: function () {
        DetailSimulation.goToDetailSimulation(0);
    },

    goToValidateForeignPreventionService: function () {
        $.ajax({
            url: "/Simulations/SendNotificationValidateToUser",
            data: {
                simulationId: this.simulationId
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.result.Status === Constants.resultStatus.Ok) {
                    GeneralData.showNotification(Constants.ok, "", "success");

                    DetailSimulation.simulationStateId = data.result.Object.SimulationStateId;
                    DetailSimulation.createIconSimulationState();
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

    onSuccessUpdate: function (data) {
        if (data.Status === Constants.resultStatus.Ok) {
            GeneralData.showNotification(Constants.ok, "", "success");
        }
        if (data.Status === Constants.resultStatus.Error) {
            GeneralData.showNotification(Constants.ko, "", "error");
        }

        ForeignPreventionService.goToForeignPreventionService();
    },

    onFailureUpdate: function () {
        GeneralData.showNotification(Constants.ko, "", "error");
        ForeignPreventionService.goToForeignPreventionService();
    },

    blockFields: function () {
        if (DetailSimulation.simulationStateId === Constants.simulationState.SendToCompany) {
            $("#" + ForeignPreventionService.textAmountTecniquesId).removeAttr("disabled");
            $("#" + ForeignPreventionService.textAmountTecniquesId).prop("disabled", true);
            $("#" + ForeignPreventionService.textAmountHealthVigilanceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.textAmountHealthVigilanceId).prop("disabled", true);
            $("#" + ForeignPreventionService.textAmountMedicalExaminationId).removeAttr("disabled");
            $("#" + ForeignPreventionService.textAmountMedicalExaminationId).prop("disabled", true);

            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).removeAttr("disabled");
            $("#" + ForeignPreventionService.btnSaveForeignPreventionServiceId).prop("disabled", true);
        }
    },

    updateButtonsOnChange: function () {
        $("#" + ForeignPreventionService.btnValidateId).removeAttr("disabled");
        $("#" + ForeignPreventionService.btnValidateId).prop("disabled", false);

        DetailSimulation.updateButtonsFromSimulationServices(true);
    }
});