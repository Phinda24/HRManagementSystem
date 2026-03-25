// Sidebar Toggle
document.getElementById('sidebarToggle')?.addEventListener('click', function () {
    document.getElementById('wrapper').classList.toggle('toggled');
});

// Auto dismiss alerts after 4 seconds
setTimeout(function () {
    document.querySelectorAll('.alert').forEach(function (alert) {
        var bsAlert = new bootstrap.Alert(alert);
        bsAlert.close();
    });
}, 4000);