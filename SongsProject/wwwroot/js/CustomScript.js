/* Function that change the photo file to html file */
function changePhotoFile() {
    $(document).ready(function () {
        $('.custom-file-input').on("change", function () {
            var fileName = $(this).val().split("\\").pop();
            $(this).next('.custom-file-label').html(fileName);
        });
    });
}

/* Function that check if the user want to delete the user or role */
function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}
