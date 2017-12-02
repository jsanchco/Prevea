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
        var html = kendo.format("<div style='font-size: 15px; font-weight: bold'>{0}</div>",
            text);

        return html;
    },

    getColumnTemplateIncreaseRight: function (text) {
        var html = kendo.format("<div align='right' style='font-size: 15px; font-weight: bold'>{0}</div>",
            text);

        return html;
    },

    getColumnTemplateDate: function (date) {
        var html = kendo.format("<div align='center'>{0}</div>",
            kendo.toString(date, "dd/MM/yy"));

        return html;
    },

    getColumnTemplateDateWithHour: function (date) {
        var html = kendo.format("<div align='center'>{0}</div>",
            kendo.toString(date, "dd/MM/yy HH:mm"));

        return html;
    },

    getColumnTemplateRight: function (data) {
        var html = kendo.format("<div align='right'>{0}</div>", data);

        return html;
    },

    getColumnTemplateCenter: function (data) {
        var html = kendo.format("<div align='center'>{0}</div>", data);

        return html;
    }
});