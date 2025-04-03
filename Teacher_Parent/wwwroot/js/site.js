// Function to toggle the visibility of the password field
function togglePassword() {
    var passwordField = document.getElementById('password');
    var confirmPasswordField = document.getElementById('confirmPassword');
    var passwordToggle = document.getElementById('passwordToggle');

    if (passwordField.type === "password") {
        passwordField.type = "text";
        confirmPasswordField.type = "text";
        passwordToggle.innerHTML = "Hide Password";
    } else {
        passwordField.type = "password";
        confirmPasswordField.type = "password";
        passwordToggle.innerHTML = "Show Password";
    }
}

// Form validation
function validateForm() {
    var password = document.getElementById('password').value;
    var confirmPassword = document.getElementById('confirmPassword').value;

    if (password !== confirmPassword) {
        alert("Passwords do not match!");
        return false;
    }
    return true;
}

