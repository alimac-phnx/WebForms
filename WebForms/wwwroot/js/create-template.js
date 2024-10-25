function previewImage(input) {
    const file = input.files[0];
    if (file) {
        const reader = new FileReader();

        reader.onload = function (e) {
            const imgElement = document.getElementById('image');
            imgElement.src = e.target.result;
            document.getElementById('remove-image-btn').style.display = 'block';
        };

        reader.readAsDataURL(file);
    }
}

function removeImage() {
    const imgElement = document.getElementById('image');
    imgElement.src = '';
    document.querySelector('input[name="ImageFile"]').value = '';
    document.getElementById('remove-image-btn').style.display = 'none';
}