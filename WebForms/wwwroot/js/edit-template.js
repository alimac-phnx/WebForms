function previewImage(input) {
    const file = input.files[0];
    if (file) {
        const reader = new FileReader();

        reader.onload = function (e) {
            const imgElement = document.getElementById('current-image');
            imgElement.src = e.target.result;
        };

        document.getElementById('remove-image-hidden').value = 'false';
        reader.readAsDataURL(file);
    }
}

function removeImage() {
    const imgElement = document.getElementById('current-image');
    imgElement.src = '';
    imgElement.alt = 'No image';

    const removeButton = document.getElementById('remove-image-btn');
    removeButton.style.display = 'none';

    document.getElementById('remove-image-hidden').value = 'true';
}