var ChoosePhoto = kendo.observable({

    filePhotoId: "filePhoto",
    choosePhotoId: "choosePhoto",
    photoProfileId: "photoProfile",
    photoProfileSideBarId: "photoProfileSideBar",

    userId: null,

    init: function (userId) {
        this.userId = userId;

        this.setUpWidgets();
    },

    setUpWidgets: function() {
        $("#" + this.filePhotoId).kendoUpload({
            async: {
                saveUrl: "/Profile/SavePhoto",
                autoUpload: false
            },
            multiple: false,
            localization: {
                select: "Selecciona foto ...",
                clearSelectedFiles: "Eliminar",
                uploadSelectedFiles: "Guardar"
            },
            select: function(e) {
                var fileInfo = e.files[0];

                var wrapper = this.wrapper;

                setTimeout(function(){
                    ChoosePhoto.addPreview(fileInfo, wrapper);
                });
            },
            success: ChoosePhoto.onSuccessSavePhoto,
            error: ChoosePhoto.onErrorSavePhoto
        });
    },

    onCloseChoosePhotoWindow: function() {
        
    },

    onOpenChoosePhotoWindow: function() {

    },

    onSuccessSavePhoto: function (e) {
        if (e.response.Status === Constants.resultStatus.Ok) {
            GeneralData.showNotification(Constants.ok, "", "success");

            var choosePhotoWindow = $("#" + ChoosePhoto.choosePhotoId);
            choosePhotoWindow.data("kendoWindow").close();

            var src;
            if ($("#" + ChoosePhoto.photoProfileId).attr("src") === "/Images/image_not_found.png") {
                src = kendo.format("/Images/user_{0}.png", ChoosePhoto.userId);
            } else {
                src = $("#" + ChoosePhoto.photoProfileId).attr("src") + "?" + Math.random();
            }

            $("#" + ChoosePhoto.photoProfileId).attr("src", src);
            $("#" + ChoosePhoto.photoProfileSideBarId).attr("src", src);
        } else {
            GeneralData.showNotification(Constants.ko, "", "error");
        }
    },

    onErrorSavePhoto: function () {
        GeneralData.showNotification(Constants.ko, "", "error");
    },

    addPreview: function (file, wrapper) {
        var raw = file.rawFile;         
        var reader = new FileReader();

        if (raw) {
            reader.onloadend = function(e) {
                var preview = $("<img class='image-preview'>").attr("src", this.result);

                wrapper.find(".k-file[data-uid='" + file.uid + "'] .k-file-extension-wrapper")
                    .replaceWith(preview);

                var image = new Image();
                image.src = e.target.result;
                image.onload = function () {
                    var height = this.height;
                    var width = this.width;
                    if (height !== Constants.heigthProfilePhoto || width !== Constants.widthProfilePhoto) {
                        GeneralData.showNotification("Las dimensiones recomendadas para la Foto de Perfil son 115x115", "", "warning");
                    }
                };
            };

            reader.readAsDataURL(raw);
        }
    }
});