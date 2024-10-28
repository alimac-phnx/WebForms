document.addEventListener('DOMContentLoaded', function () {
    const chk = document.getElementById('chk');
    const lightThemeForm = document.getElementById('lightThemeForm');
    const darkThemeForm = document.getElementById('darkThemeForm');

    chk.addEventListener('change', function () {
        if (chk.checked) {
            darkThemeForm.submit();
        }
        else {
            lightThemeForm.submit();
        }
    });
});