
/* =========================================================================
   PFMS Frontend Script (Sign Up / Sign In + Password Toggle)
   ========================================================================= */

/* --------------------------
 * Password visibility toggle
 * -------------------------- */
(function () {
    function toggle(btn) {
        const id = btn.getAttribute("data-target");
        const input = document.getElementById(id);
        if (!input) return;

        const icon = btn.querySelector("i");
        const hidden = input.type === "password";

        input.type = hidden ? "text" : "password";

        btn.setAttribute("aria-pressed", hidden ? "true" : "false");
        btn.setAttribute("aria-label", hidden ? "Hide password" : "Show password");

        if (icon) {
            icon.classList.toggle("fa-eye", !hidden);
            icon.classList.toggle("fa-eye-slash", hidden);
        }
    }

    document.addEventListener("click", function (e) {
        const btn = e.target.closest(".toggle-password");
        if (!btn) return;
        e.preventDefault();
        toggle(btn);
    });

    document.addEventListener("keydown", function (e) {
        const btn = e.target.closest(".toggle-password");
        if (!btn) return;
        if (e.key === " " || e.key === "Enter") {
            e.preventDefault();
            toggle(btn);
        }
    });
})();

/* --------------------------
 * jQuery: validations & submit
 * -------------------------- */
$(function () {
    /* ========= Sign Up ========= */

    // Strong password: ≥12 chars, at least 1 uppercase, 1 lowercase, 1 digit, 1 special char
    const strongPwdRegex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{12,}$/;

    function showError(id, message) { $(id).text(message); }
    function clearError(id) { $(id).text(""); }

    function validateName() {
        const ok = $("#name").val().trim().length >= 2;
        ok ? clearError("#nameError") : showError("#nameError", "Name must be at least 2 characters.");
        return ok;
    }

    function validateMobile() {
        const ok = /^[0-9]{10}$/.test($("#mobile").val().trim());
        ok ? clearError("#mobileError") : showError("#mobileError", "Enter a valid 10-digit mobile number.");
        return ok;
    }

    function validateEmail() {
        // If you want ANY TLD, change to: /^[^\s@]+@[^\s@]+\.[^\s@]+$/
        const ok = /^[^\s@]+@[^\s@]+\.com$/.test($("#email").val().trim());
        ok ? clearError("#emailError") : showError("#emailError", "Enter a valid email address.");
        return ok;
    }

    function validatePassword() {
        const pwd = $("#password").val();
        const ok = strongPwdRegex.test(pwd);
        ok
            ? clearError("#passwordError")
            : showError("#passwordError",
                "Password must be at least 12 characters and include: uppercase, lowercase, a number, and a special character.");
        return ok;
    }

    function validateConfirmPassword() {
        const pwd = $("#password").val();
        const cpwd = $("#confirmPassword").val();
        const ok = cpwd === pwd;
        ok ? clearError("#confirmPasswordError") : showError("#confirmPasswordError", "Passwords do not match.");
        return ok;
    }

    function validateSignUpAll() {
        const v1 = validateName();
        const v2 = validateMobile();
        const v3 = validateEmail();
        const v4 = validatePassword();
        const v5 = validateConfirmPassword();
        return v1 && v2 && v3 && v4 && v5;
    }

    // Validate on blur AND on input (for immediate feedback)
    $("#name").on("blur input", validateName);
    $("#mobile").on("blur input", validateMobile);
    $("#email").on("blur input", validateEmail);
    $("#password").on("blur input", function () {
        validatePassword();
        // If user changes password, re-check match
        validateConfirmPassword();
    });
    $("#confirmPassword").on("blur input", validateConfirmPassword);

    $("#signupForm").on("submit", function (e) {
        e.preventDefault();

        // Always validate NOW to catch fields the user hasn't blurred
        const ok = validateSignUpAll();
        if (!ok) {
            alert("Please fix the highlighted fields.");
            return;
        }

        // Client-side success demo
        alert("Sign Up successful!");
        window.location.href = "/Home/SignIn";
    });

    /* ========= Sign In ========= */

    function validateSigninUser() {
        const user = $("#signinUser").val().trim().toLowerCase();
        const mobileOk = /^[0-9]{10}$/.test(user);
        const emailOk = /^[^\s@]+@[^\s@]+\.com$/.test(user);
        const roleOk = ["admin", "storemanager"].includes(user);
        const ok = mobileOk || emailOk || roleOk;
        ok ? clearError("#signinUserError") : showError("#signinUserError", "Enter a valid mobile, email, or role username.");
        return ok;
    }

    function validateSigninPassword() {
        const ok = $("#signinPassword").val().length >= 6;
        ok ? clearError("#signinPasswordError") : showError("#signinPasswordError", "Enter a valid password");
        return ok;
    }

    $("#signinUser").on("blur input", validateSigninUser);
    $("#signinPassword").on("blur input", validateSigninPassword);

    $("#signinForm").on("submit", function (e) {
        e.preventDefault();

        const okUser = validateSigninUser();
        const okPass = validateSigninPassword();
        if (!okUser || !okPass) {
            alert("Some fields are not completed");
            return;
        }

        const username = $("#signinUser").val().trim().toLowerCase();
        const password = $("#signinPassword").val();

        // Demo role-based redirects
        if (username === "admin" && password === "Admin@123") {
            alert("Admin login successful!");
            window.location.href = "/Home/AdminDashboard";
            return;
        }
        if (username === "storemanager" && password === "Storemanager@123") {
            alert("Store Manager login successful!");
            window.location.href = "/StoreManager/Home";
            return;
        }

        // Normal user
        const mobileOk = /^[0-9]{10}$/.test(username);
        const emailOk = /^[^\s@]+@[^\s@]+\.com$/.test(username);
        if ((mobileOk || emailOk) && password.length >= 6) {
            alert("User login successful!");
            window.location.href = "/Home/Dashboard";
        } else {
            alert("Invalid credentials. Please try again.");
        }
    });

});
