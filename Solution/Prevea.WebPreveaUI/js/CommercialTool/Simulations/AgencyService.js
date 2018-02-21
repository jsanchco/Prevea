var AgencyService = kendo.observable({

    textAreaObservationsId: "textAreaObservationsAgencyService",
    btnValidateId: "btnValidateAgencyService",
    cmbEngagementTypeId: "cmbEngagementType",
    textAmountByEngagementTypeId: "textAmountByEngagementType",
    textAmountByRosterId: "textAmountByRoster",
    lblAmountByEngagementTypeId: "lblAmountByEngagementType",
    lblAmountByRosterId: "lblAmountByRoster",
    lblPercentageAmountByEngagementTypeId: "lblPercentageAmountByEngagementType",
    lblPercentageAmountByRosterId: "lblPercentageAmountByRoster",
    lblTotalByRoster: "lblTotalByRoster",
    lblTotalCalculateAgencyServiceId: "lblTotalCalculateAgencyService",

    simulationId: null,
    numberEmployees: null,

    amountAgencyByType: null,
    percentageAgencyByType: null,
    amountAgencyByRoster: null,
    percentageAgencyByRoster: null,

    init: function (id, numberEmployees) {
        this.simulationId = id;
        this.numberEmployees = numberEmployees;

        this.setUpWidgets();

        this.getStretchAgency();

        this.blockFields();
    },

    setUpWidgets: function () {
        $("#" + this.cmbEngagementTypeId).kendoDropDownList({
            dataTextField: "Description",
            dataValueField: "Id",
            optionLabel: "Selecciona ...",
            dataSource: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "/Simulations/GetEngagementTypes",
                        dataType: "jsonp"
                    }
                }
            }),
            change: AgencyService.onSelectEngagementType
        });

        $("#" + this.textAmountByEngagementTypeId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: AgencyService.onChangeTextAmountByEngagementType
        });
        $("#" + this.textAmountByRosterId).kendoNumericTextBox({
            format: "c",
            decimals: 1,
            change: AgencyService.onChangeTextAmountByRoster
        });
        $("#" + this.textAreaObservationsId).change(function () {
            AgencyService.updateButtonsOnChange();
        });

        $($("#" + this.btnValidateId)).on("click", function (e) {
            var errors = AgencyService.validateForm();
            GeneralDataCompany.showErrors(errors);
            if (errors.length > 0) {
                e.preventDefault();
            }
        });

        $("#" + this.btnValidateId).removeAttr("disabled");
        $("#" + this.btnValidateId).prop("disabled", true);

        var numerictextbox = $("#" + AgencyService.textAmountByEngagementTypeId).data("kendoNumericTextBox");     
        if (parseFloat($("#" + this.cmbEngagementTypeId).val()) === 0) {
            numerictextbox.enable(false);

            this.amountAgencyByType = 0;
            this.percentageAgencyByType = 0;

            $("#" + this.lblAmountByEngagementTypeId).text("€");
            $("#" + this.lblPercentageAmountByEngagementTypeId).text("%");
        } else {
            numerictextbox.enable(true);

            this.getStretchAgencyByType($("#" + this.cmbEngagementTypeId).val());            
        }
    },

    goToAgencyService: function () {
        DetailSimulation.goToDetailSimulation(1);
    },

    updateButtonsOnChange: function () {
        $("#" + AgencyService.btnValidateId).removeAttr("disabled");
        $("#" + AgencyService.btnValidateId).prop("disabled", false);

        DetailSimulation.updateButtonsFromSimulationServices(true);
    },

    onSuccessUpdate: function (data) {
        if (data.Status === Constants.resultStatus.Ok) {
            GeneralData.showNotification(Constants.ok, "", "success");
        }
        if (data.Status === Constants.resultStatus.Error) {
            GeneralData.showNotification(Constants.ko, "", "error");
        }

        AgencyService.goToAgencyService();
    },

    onFailureUpdate: function (data) {
        GeneralData.showNotification(Constants.ko, "", "error");

        AgencyService.goToAgencyService();
    },

    blockFields: function () {
        if (DetailSimulation.simulationStateId === Constants.simulationState.SendToCompany) {
            var numerictextbox = $("#" + AgencyService.textAmountByEngagementTypeId).data("kendoNumericTextBox");
            numerictextbox.enable(false);

            numerictextbox = $("#" + AgencyService.textAmountByRosterId).data("kendoNumericTextBox");
            numerictextbox.enable(false);

            var dropdownlist = $("#" + AgencyService.cmbEngagementTypeId).data("kendoDropDownList");
            dropdownlist.enable(false);

            $("#" + AgencyService.textAreaObservationsId).removeAttr("disabled");
            $("#" + AgencyService.textAreaObservationsId).prop("disabled", true);

            $("#" + AgencyService.btnValidateId).removeAttr("disabled");
            $("#" + AgencyService.btnValidateId).prop("disabled", true);
        }
    },

    getStretchAgency: function() {
        $.ajax({
            url: "/Simulations/GetStretchAgencyByNumberEmployees",
            data: {
                numberEmployees: this.numberEmployees
            },
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.stretchAgency !== null) {
                    AgencyService.amountAgencyByRoster = response.stretchAgency.AmountByRoster;
                    AgencyService.percentageAgencyByRoster = response.stretchAgency.Percentege;

                    AgencyService.updateView();
                } else {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            }
        });        
    },

    getStretchAgencyByType: function (typeId) {
        $.ajax({
            url: "/Simulations/GetStretchAgencyByType",
            data: {
                type: typeId
            },
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.amountAgencyByType != null && response.percentageAgencyByType != null) {
                    AgencyService.amountAgencyByType = response.amountAgencyByType;
                    AgencyService.percentageAgencyByType = response.percentageAgencyByType;

                    var strValue = kendo.format("{0} €", AgencyService.amountAgencyByType);
                    $("#" + AgencyService.lblAmountByEngagementTypeId).text(strValue);
                    $("#" + AgencyService.lblPercentageAmountByEngagementTypeId).text("%");

                    AgencyService.onChangeTextAmountByEngagementType(true);
                } else {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            }
        });
    },

    onSelectEngagementType: function () {
        var valueCmbEngagementType = $("#" + AgencyService.cmbEngagementTypeId).data("kendoDropDownList").value();

        AgencyService.updateButtonsOnChange();

        var numerictextbox = $("#" + AgencyService.textAmountByEngagementTypeId).data("kendoNumericTextBox");
        if (valueCmbEngagementType === "") {
            $("#" + AgencyService.lblAmountByEngagementTypeId).text("€");
            $("#" + AgencyService.lblPercentageAmountByEngagementTypeId).text("%");
            
            numerictextbox.enable(false);
        } else {
            numerictextbox.enable(true);
        }
        numerictextbox.value(0);

        AgencyService.getStretchAgencyByType(valueCmbEngagementType);
    },

    updateView: function () {
        var strValue = kendo.format("{0} €", AgencyService.amountAgencyByType);
        $("#" + AgencyService.lblAmountByEngagementTypeId).text(strValue);
        this.onChangeTextAmountByEngagementType(true);

        strValue = kendo.format("{0} €", AgencyService.amountAgencyByRoster);
        $("#" + this.lblAmountByRosterId).text(strValue);
        this.onChangeTextAmountByRoster(true);
    },

    onChangeTextAmountByEngagementType: function (fromUpdateView) {
        var value = parseFloat($("#" + AgencyService.textAmountByEngagementTypeId).val());
        if (isNaN(value)) {
            return;
        }

        var widget = $("#" + AgencyService.textAmountByEngagementTypeId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + AgencyService.lblPercentageAmountByEngagementTypeId).text("%");

            AgencyService.calculateTotal();

            return;
        }

        var percentegeValueFix = parseFloat(AgencyService.amountAgencyByType) * (100 - AgencyService.percentageAgencyByType) / 100;
        if (value < percentegeValueFix) {
            widget.wrapper.find("input").css("background-color", "#f56954");
            widget.wrapper.find("input").css("color", "black");
        } else {
            if (value >= percentegeValueFix && value <= parseFloat(AgencyService.amountAgencyByType)) {
                widget.wrapper.find("input").css("background-color", "white");
                widget.wrapper.find("input").css("color", "#4b4b4b");
            } else {
                widget.wrapper.find("input").css("background-color", "#008d4c");
                widget.wrapper.find("input").css("color", "black");
            }
        }
        widget.wrapper.width("100%");
        var percentegeCalculate = value * 100 / parseFloat(AgencyService.amountAgencyByType);
        $("#" + AgencyService.lblPercentageAmountByEngagementTypeId).text((percentegeCalculate - 100).toFixed(2) + "%");

        if ($.type(fromUpdateView) !== "boolean" &&
            fromUpdateView !== true) {
            $("#" + AgencyService.btnValidateId).removeAttr("disabled");
            $("#" + AgencyService.btnValidateId).prop("disabled", false);

            AgencyService.updateButtonsOnChange();
        } else {
            $("#" + AgencyService.btnValidateId).removeAttr("disabled");
            $("#" + AgencyService.btnValidateId).prop("disabled", true);
        }

        AgencyService.calculateTotal();
    },

    onChangeTextAmountByRoster: function (fromUpdateView) {
        var value = parseFloat($("#" + AgencyService.textAmountByRosterId).val());
        if (isNaN(value)) {
            return;
        }

        var widget = $("#" + AgencyService.textAmountByRosterId).kendoNumericTextBox().data("kendoNumericTextBox");
        if (value === 0) {
            widget.wrapper.find("input").css("background-color", "white");
            widget.wrapper.find("input").css("color", "#4b4b4b");
            widget.wrapper.width("100%");
            $("#" + AgencyService.lblPercentageAmountByRosterId).text("%");
            $("#" + AgencyService.lblTotalByRoster).text("€");

            AgencyService.calculateTotal();

            return;
        }

        var percentegeValueFix = parseFloat(AgencyService.amountAgencyByRoster) * (100 - AgencyService.percentageAgencyByRoster) / 100;
        if (value < percentegeValueFix) {
            widget.wrapper.find("input").css("background-color", "#f56954");
            widget.wrapper.find("input").css("color", "black");
        } else {
            if (value >= percentegeValueFix && value <= parseFloat(AgencyService.amountAgencyByRoster)) {
                widget.wrapper.find("input").css("background-color", "white");
                widget.wrapper.find("input").css("color", "#4b4b4b");
            } else {
                widget.wrapper.find("input").css("background-color", "#008d4c");
                widget.wrapper.find("input").css("color", "black");
            }
        }
        widget.wrapper.width("100%");
        var percentegeCalculate = value * 100 / parseFloat(AgencyService.amountAgencyByRoster);
        $("#" + AgencyService.lblPercentageAmountByRosterId).text((percentegeCalculate - 100).toFixed(2) + "%");
        $("#" + AgencyService.lblTotalByRoster).text((value * AgencyService.numberEmployees).toFixed(2) + "€");

        if ($.type(fromUpdateView) !== "boolean" &&
            fromUpdateView !== true) {
            $("#" + AgencyService.btnValidateId).removeAttr("disabled");
            $("#" + AgencyService.btnValidateId).prop("disabled", false);

            AgencyService.updateButtonsOnChange();
        } else {
            $("#" + AgencyService.btnValidateId).removeAttr("disabled");
            $("#" + AgencyService.btnValidateId).prop("disabled", true);
        }

        AgencyService.calculateTotal();
    },

    calculateTotal: function () {
        var total = parseFloat($("#" + AgencyService.textAmountByEngagementTypeId).val());

        var strValue = $("#" + AgencyService.lblTotalByRoster).text();
        strValue = strValue.substring(0, strValue.length - 1);
        if (strValue !== "") {
            total += parseFloat(strValue);
        }

        $("#" + AgencyService.lblTotalCalculateAgencyServiceId).text(total + " €");
        $("#Total").val(total);
    },

    validateForm: function () {
        var error = [];

        if (parseInt($("#" + this.cmbEngagementTypeId).data("kendoDropDownList").value()) === 0) {
            error.push("Debes añadir un Tipo de Contratación");
        }

        return error;
    }
});