var Templates = kendo.observable({

    getColumnTemplateUrl: function (data) {
        var html;
        if (data.Icon === "unknown_opt.png") {
            html = "<div align='center'><a toggle='tooltip' title='No se encuentra el Documento'>";
        } else {
            html = kendo.format(
                "<div align='center'><a toggle='tooltip' title='Abrir Documento' onclick='GeneralData.goToOpenFile(\"{0}\")' target='_blank' style='cursor: pointer;'>",
                data.id);
        }

        html += kendo.format("<img src='../../Images/{0}'></a></div>", data.Icon);

        return html;
    },

    getColumnTemplateEdition: function (data) {
        var html = kendo.format("<div align='right'>{0}</div>",
            data.Edition);

        return html;
    },

    getColumnTemplateIncrease: function (text) {
        if (text === null) {
            return "";
        }

        var html = kendo.format("<div style='font-size: 15px; font-weight: bold'>{0}</div>",
            text);

        return html;
    },

    getColumnTemplateBold: function (text) {
        if (text === null) {
            return "";
        }

        var html = kendo.format("<div style='font-weight: bold'>{0}</div>",
            text);

        return html;
    },

    getColumnTemplateIncreaseRight: function (text) {
        if (text === null) {
            return "";
        }

        var html = kendo.format("<div align='right' style='font-size: 15px; font-weight: bold'>{0}</div>",
            text);

        return html;
    },

    getColumnTemplateDate: function (date) {
        if (date === null) {
            return "";
        }

        var html = kendo.format("<div align='center'>{0}</div>",
            kendo.toString(date, "dd/MM/yy"));

        return html;
    },

    getColumnTemplateBoolean: function (checked) {
        var html = "";
        if (checked === true) {
            html = "<div align='center'>Si</div>";
        } else {
            html = "<div align='center'>No</div>";
        }

        return html;
    },

    getColumnTemplateTime: function (date) {
        if (date === null) {
            return "";
        }

        var html = kendo.format("<div align='center'>{0}</div>",
            kendo.toString(date, "mm:ss"));

        return html;
    },

    getColumnTemplateDateIncrease: function (date) {
        if (date === null) {
            return "";
        }

        var html = kendo.format("<div align='center' style='font-size: 15px; font-weight: bold'>{0}</div>",
            kendo.toString(date, "dd/MM/yyyy"));

        return html;
    },

    getColumnTemplateDateWithHour: function (date) {
        if (date === null) {
            return "";
        }

        var html = kendo.format("<div align='center'>{0}</div>",
            kendo.toString(date, "dd/MM/yy HH:mm"));

        return html;
    },

    getColumnTemplateDateWithHourBold: function (date) {
        if (date === null) {
            return "";
        }

        var html = kendo.format("<div align='center' style='font-weight: bold;'>{0}</div>",
            kendo.toString(date, "dd/MM/yyyy HH:mm"));

        return html;
    },

    getColumnTemplateDateOnlyHourBold: function (date) {
        if (date === null) {
            return "";
        }

        var html = kendo.format("<div align='center' style='font-weight: bold;'>{0}</div>",
            kendo.toString(date, "HH:mm"));

        return html;
    },

    getColumnTemplateRight: function (data) {
        var html = kendo.format("<div align='right'>{0}</div>", data);

        return html;
    },

    getColumnTemplateCurrencyRight: function (data, format) {
        var value = kendo.toString(data, format);
        var html = kendo.format("<div align='right'>{0}</div>", value);
      
        return html;
    },

    getColumnTemplateCenter: function (data) {
        var html = kendo.format("<div align='center'>{0}</div>", data);

        return html;
    },

    getColumnTemplateBooleanIncrease: function (value) {
        var text = "No";
        if (value === true) {
            text = "Si";
        }

        var html = kendo.format("<div align='center' style='font-size: 15px; font-weight: bold'>{0}</div>",
            text);

        return html;
    }
});