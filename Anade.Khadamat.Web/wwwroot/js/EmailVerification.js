let timeout = null;
document.getElementById('Email').addEventListener('input', function () {
    clearTimeout(timeout);
    var email = this.value;

    timeout = setTimeout(function () {
        const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;

        if (emailRegex.test(email)) {
            fetch('/Inscription/CheckEmailExists?email=' + encodeURIComponent(email))
                .then(response => response.json())
                .then(data => {
                    const messageElement = document.getElementById('emailValidationMessage');
                    if (data.exists) {
                        messageElement.textContent = "هذا البريد الإلكتروني مسجل من قبل";
                        messageElement.style.color = "red";
                    } else {
                        messageElement.textContent = "البريد الإلكتروني متاح.";
                        messageElement.style.color = "#006D6A";
                    }
                });
        } else {
            document.getElementById('emailValidationMessage').textContent = '';
        }
    }, 500);
});

