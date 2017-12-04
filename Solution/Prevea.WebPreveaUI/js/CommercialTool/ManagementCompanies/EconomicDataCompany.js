var EconomicDataCompany = kendo.observable({
    lblNumberEmployeesId: "lblNumberEmployees",
    textActualNumberEmployeesId: "textActualNumberEmployees",
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

    errorFromFrontId: "errorFromFront",

    companyId: null,

    percentege: 80,
    numberEmployees: 0,
    total: 0,

    init: function (id) {
        kendo.culture("es-ES");

        this.companyId = id;

        this.setKendoUIWidgets();
    },

    updateView: function () {
        this.numberEmployees = DetailCompany.subscribeNumberEmployees;
        $("#" + this.lblNumberEmployeesId).text(DetailCompany.subscribeNumberEmployees);

        var strValue = kendo.format("{0} €", DetailCompany.stretchCalculate.AmountByEmployeeInTecniques);
        $("#" + this.lblAmountByEmployeeInTecniquesId).text(strValue);
        this.onChangeTextAmountTecniques();

        strValue = kendo.format("{0} €", DetailCompany.stretchCalculate.AmountByEmployeeInHealthVigilance);
        $("#" + this.lblAmountByEmployeeInHealthVigilanceId).text(strValue);
        this.onChangeTextAmountHealthVigilance();

        strValue = kendo.format("{0} €", DetailCompany.stretchCalculate.AmountByEmployeeInMedicalExamination);
        $("#" + this.lblAmountByEmployeeInMedicalExaminationId).text(strValue);
        this.onChangeTextAmountMedicalExamination();
    },

    setKendoUIWidgets: function () {
        $("#" + this.textActualNumberEmployeesId).kendoNumericTextBox({
            decimals: 0,
            format: "0"
        });
        $("#" + this.textAmountTecniquesId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: EconomicDataCompany.onChangeTextAmountTecniques
        });
        $("#" + this.textAmountHealthVigilanceId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: EconomicDataCompany.onChangeTextAmountHealthVigilance
        });
        $("#" + this.textAmountMedicalExaminationId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: EconomicDataCompany.onChangeTextAmountMedicalExamination
        });
    },

    onChangeTextAmountTecniques: function () {        
        var value = parseFloat($("#" + EconomicDataCompany.textAmountTecniquesId).val());
        if (isNaN(value)) {
            return;
        }

        var widget = $("#" + EconomicDataCompany.textAmountTecniquesId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + EconomicDataCompany.lblPercentegeByEmployeeInTecniquesId).text("%");
            $("#" + EconomicDataCompany.lblTotalByEmployeeInTecniquesId).text("€");

            EconomicDataCompany.calculateTotal();

            return;
        }

        var strVal = $("#" + EconomicDataCompany.lblAmountByEmployeeInTecniquesId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * EconomicDataCompany.percentege / 100;
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
        $("#" + EconomicDataCompany.lblPercentegeByEmployeeInTecniquesId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + EconomicDataCompany.lblTotalByEmployeeInTecniquesId).text((value * EconomicDataCompany.numberEmployees).toFixed(2) + " €");

        EconomicDataCompany.calculateTotal();
    },

    onChangeTextAmountHealthVigilance: function () {        
        var value = parseFloat($("#" + EconomicDataCompany.textAmountHealthVigilanceId).val());
        if (isNaN(value)) {
            return;
        }

        var widget = $("#" + EconomicDataCompany.textAmountHealthVigilanceId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + EconomicDataCompany.lblPercentegeByEmployeeInHealthVigilanceId).text("%");
            $("#" + EconomicDataCompany.lblTotalByEmployeeInHealthVigilanceId).text("€");

            EconomicDataCompany.calculateTotal();

            return;
        }

        var strVal = $("#" + EconomicDataCompany.lblAmountByEmployeeInHealthVigilanceId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * EconomicDataCompany.percentege / 100;
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
        $("#" + EconomicDataCompany.lblPercentegeByEmployeeInHealthVigilanceId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + EconomicDataCompany.lblTotalByEmployeeInHealthVigilanceId).text((value * EconomicDataCompany.numberEmployees).toFixed(2) + " €");

        EconomicDataCompany.calculateTotal();
    },

    onChangeTextAmountMedicalExamination: function () {        
        var value = parseFloat($("#" + EconomicDataCompany.textAmountMedicalExaminationId).val());
        if (isNaN(value)) {
            return;
        }

        var widget = $("#" + EconomicDataCompany.textAmountMedicalExaminationId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + EconomicDataCompany.lblPercentegeByEmployeeInMedicalExaminationId).text("%");
            $("#" + EconomicDataCompany.lblTotalByEmployeeInMedicalExaminationId).text("€");

            EconomicDataCompany.calculateTotal();

            return;
        }

        var strVal = $("#" + EconomicDataCompany.lblAmountByEmployeeInMedicalExaminationId).text();
        var valueFix = parseFloat(strVal.substring(0, strVal.length - 2));
        var percentegeValueFix = valueFix * EconomicDataCompany.percentege / 100;
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
        $("#" + EconomicDataCompany.lblPercentegeByEmployeeInMedicalExaminationId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + EconomicDataCompany.lblTotalByEmployeeInMedicalExaminationId).text((value * EconomicDataCompany.numberEmployees).toFixed(2) + " €");

        EconomicDataCompany.calculateTotal();
    },

    calculateTotal: function () {
        var strValue = $("#" + EconomicDataCompany.lblTotalByEmployeeInTecniquesId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInTecniques = 0;
        if (strValue !== "") {
            totalByEmployeeInTecniques = parseFloat(strValue);
        }

        strValue = $("#" + EconomicDataCompany.lblTotalByEmployeeInHealthVigilanceId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInHealthVigilance = 0;
        if (strValue !== "") {
            totalByEmployeeInHealthVigilance = parseFloat(strValue);
        }

        strValue = $("#" + EconomicDataCompany.lblTotalByEmployeeInMedicalExaminationId).text();
        strValue = strValue.substring(0, strValue.length - 1);
        var totalByEmployeeInMedicalExamination = 0;
        if (strValue !== "") {
            totalByEmployeeInMedicalExamination = parseFloat(strValue);
        }

        $("#" + EconomicDataCompany.lblTotalId).text(
            (totalByEmployeeInTecniques +
            totalByEmployeeInHealthVigilance +
            totalByEmployeeInMedicalExamination).toFixed(2) + " €");
    },

    goToCompanies: function () {
        var params = {
            url: "/Company/Companies"
        };
        GeneralData.goToActionController(params);
    },

    goToEconomicDataCompany: function () {
        var params = {
            url: "/Company/EconomicDataCompany"
        };
        GeneralData.goToActionController(params);
    },

    goToDetailCompany: function () {
        var params = {
            url: "/Company/DetailCompany",
            data: {
                id: this.companyId,
                selectTabId: 4
            }
        };
        GeneralData.goToActionController(params);
    }

});