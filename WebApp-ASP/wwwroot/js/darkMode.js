document.addEventListener('DOMContentLoaded', () => {
    // Load darkmode on start from local storage
    var darkmode = localStorage.getItem('darkMode');

    if (darkmode === 'true') {
        document.body.parentElement.classList.add('dark-mode');
        const dmbtn = document.querySelector('.darkmode-btn-toggle')
        if (dmbtn) {
            dmbtn.checked = true
        }
    }
})

// Toogle darkmode
function toggleDarkMode() {
    document.body.parentElement.classList.toggle("dark-mode");
    // Save darkMode to local storage
    localStorage.setItem("darkMode", document.body.parentElement.classList.contains("dark-mode"));
}