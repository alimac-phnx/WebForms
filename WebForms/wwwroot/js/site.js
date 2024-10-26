const chk = document.getElementById('chk');
const htmlElement = document.documentElement;
const navbar = document.getElementById('navbar');

const currentTheme = localStorage.getItem('theme');

if (currentTheme) {
    htmlElement.setAttribute('data-bs-theme', currentTheme);
    chk.checked = currentTheme === 'dark';

    if (currentTheme === 'dark') {
        navbar.classList.remove('navbar-light-theme');
        navbar.classList.add('navbar-dark-theme');
    } else {
        navbar.classList.remove('navbar-dark-theme');
        navbar.classList.add('navbar-light-theme');
    }
}

document.addEventListener('DOMContentLoaded', function () {
    chk.addEventListener('change', () => {
        if (chk.checked) {
            htmlElement.setAttribute('data-bs-theme', 'dark');
            localStorage.setItem('theme', 'dark');
            navbar.classList.remove('navbar-light-theme');
            navbar.classList.add('navbar-dark-theme');
        } else {
            htmlElement.setAttribute('data-bs-theme', 'light');
            localStorage.setItem('theme', 'light');
            navbar.classList.remove('navbar-dark-theme');
            navbar.classList.add('navbar-light-theme');
        }
    });
});