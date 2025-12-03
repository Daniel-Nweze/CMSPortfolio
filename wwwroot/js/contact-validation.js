// wwwroot/js/contact-validation.js
document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("[data-contact-form]");
    if (!form) return;

    const fields = Array.from(form.querySelectorAll("[data-validate]"));

    function showError(input, message) {
        const field = input.closest(".form-field");
        if (!field) return;

        field.classList.add("has-error");

        let errorEl = field.querySelector(".field-error");
        if (!errorEl) {
            errorEl = document.createElement("span");
            errorEl.className = "field-error";
            field.appendChild(errorEl);
        }

        errorEl.textContent = message;
    }

    function clearError(input) {
        const field = input.closest(".form-field");
        if (!field) return;

        field.classList.remove("has-error");

        const errorEl = field.querySelector(".field-error");
        if (errorEl) {
            errorEl.textContent = "";
        }
    }

    function validateField(input) {
        const value = input.value.trim();
        const required = input.dataset.required === "true";
        const type = input.dataset.type || input.type; // <-- ändrat här

        if (required && value.length === 0) {
            showError(input, input.dataset.requiredMessage || "Required field.");
            return false;
        }

        if (type === "email" && value.length > 0) {
            const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;

            if (!emailRegex.test(value)) {
                showError(input, input.dataset.emailMessage || "Invalid email address.");
                return false;
            }
        }

        clearError(input);
        return true;
    }


    // Rensa fel när man börjar skriva igen
    fields.forEach(input => {
        input.addEventListener("input", () => validateField(input));
        input.addEventListener("blur", () => validateField(input));
    });

    // Blockera submit om något är fel
    form.addEventListener("submit", function (e) {
        let isValid = true;

        fields.forEach(input => {
            if (!validateField(input)) {
                isValid = false;
            }
        });

        if (!isValid) {
            e.preventDefault();
        }
    });
});
