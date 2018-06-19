var AddSignature = kendo.observable({

    documentId: null,
    type: null,
    userId: null,
    btn_signatureClearId: "btn_signatureClear",
    btn_signatureSaveId: "btn_signatureSave",

    signaturePad: null,

    init: function (type, id) {
        this.type = type;
        if (type === 1) {
            this.documentId = id;
            this.userId = null;
        } else {
            this.documentId = null;
            this.userId = id;
        }
  
        this.setUpSignature();
    },

    setUpSignature: function () {
        if (GeneralData.userRoleId === Constants.role.ContactPerson) {
            $("#" + this.btn_signatureClearId).removeAttr("disabled");
            $("#" + this.btn_signatureClearId).prop("disabled", true);

            $("#" + this.btn_signatureSaveId).removeAttr("disabled");
            $("#" + this.btn_signatureSaveId).prop("disabled", true);
        }

        var wrapper = document.querySelector(".signature-pad");
        var canvas = wrapper.querySelector("canvas");
        var clearButton = wrapper.querySelector(".btn-clear-canvas");
        var hiddenInput = wrapper.querySelector('input[type="hidden"]');

        this.signaturePad = new SignaturePad(canvas);

        // Read base64 string from hidden input
        var base64str = hiddenInput.value;

        if (base64str) {
            // Draws signature image from data URL
            this.signaturePad.fromDataURL("data:image/png;base64," + base64str);
        }

        if (hiddenInput.disabled) {
            this.signaturePad.off();
        } else {
            this.signaturePad.onEnd = function () {
                // Returns signature image as data URL and set it to hidden input
                base64str = AddSignature.signaturePad.toDataURL().split(",")[1];
                hiddenInput.value = base64str;
            };

            clearButton.addEventListener("click", function () {
                // Clear the canvas and hidden input
                AddSignature.signaturePad.clear();
                hiddenInput.value = "";
            });
        }
    },

    clearSignature: function () {
        //var clearButton = wrapper.querySelector(".btn-clear-canvas");
        //var hiddenInput = wrapper.querySelector("input[type=\"hidden\"]");
    },

    goToSaveSignature: function () {
        $.ajax({
            url: "/Base/SaveSignature",
            data: JSON.stringify({
                "documentId": this.documentId,
                "userId": this.userId,
                "data": AddSignature.signaturePad.toDataURL().split(",")[1]
            }),
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.result.Status === Constants.resultStatus.Ok) {
                    GeneralData.showNotification(Constants.ok, "", "success");
                } else {
                    GeneralData.showNotification(error, "", "error");
                }
            },
            error: function (xhr, status, error) {
                GeneralData.showNotification(error, "", "error");
            }
        });
    }
});