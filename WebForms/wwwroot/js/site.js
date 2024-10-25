const chk = document.getElementById('chk');
const htmlElement = document.documentElement;

const currentTheme = localStorage.getItem('theme');

if (currentTheme) {
    htmlElement.setAttribute('data-bs-theme', currentTheme);
    chk.checked = currentTheme === 'dark';
}

chk.addEventListener('change', () => {
    if (chk.checked) {
        htmlElement.setAttribute('data-bs-theme', 'dark');
        localStorage.setItem('theme', 'dark');
    } else {
        htmlElement.setAttribute('data-bs-theme', 'light');
        localStorage.setItem('theme', 'light');
    }
});