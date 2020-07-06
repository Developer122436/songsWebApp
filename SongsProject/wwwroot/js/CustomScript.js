/* Function that change the photo file to html file */
function changePhotoFile() {
    window.addEventListener('DOMContentLoaded', () => {
        document.querySelector(".custom-file-input").addEventListener("change", () => {
            let fileName = this.val().split("\\").pop();
            this.next(".custom-file-input").html(fileName);
        });
    });
}

/* Function that check if the user want to delete the user or role */
function confirmDelete(uniqueId, isDeleteClicked) {
    let deleteSpan = `deleteSpan_${uniqueId}`;
    let confirmDeleteSpan = `confirmDeleteSpan_${uniqueId}`; 

    if (isDeleteClicked) {
        document.getElementById(deleteSpan).className = "displayNone";
        document.getElementById(confirmDeleteSpan).className = "";
    } else {
        document.getElementById(deleteSpan).className = "";
        document.getElementById(confirmDeleteSpan).className = "displayNone";
    }
}
