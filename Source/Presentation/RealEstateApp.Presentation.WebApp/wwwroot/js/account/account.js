(function () {
    "use strict";

    var reducedMotion = window.matchMedia && window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    var themeToggle = document.getElementById("accountThemeToggle");

    if (themeToggle) {
        themeToggle.addEventListener("click", function () {
            var next = document.documentElement.getAttribute("data-theme") === "dark" ? "light" : "dark";
            document.documentElement.setAttribute("data-theme", next);
            document.documentElement.setAttribute("data-bs-theme", next);

            try {
                localStorage.setItem("rea-theme", next);
            } catch (e) {
            }
        });
    }

    document.querySelectorAll("[data-password-toggle]").forEach(function (button) {
        button.addEventListener("click", function () {
            var input = document.getElementById(button.getAttribute("data-password-toggle"));

            if (!input) {
                return;
            }

            var show = input.type === "password";
            input.type = show ? "text" : "password";
            button.classList.toggle("is-visible", show);
            button.setAttribute("aria-pressed", show ? "true" : "false");
            button.setAttribute("aria-label", show ? "Ocultar contraseña" : "Mostrar contraseña");
        });
    });

    document.querySelectorAll("[data-scene-rotate]").forEach(function (container) {
        var items = container.querySelectorAll(".scene-headline");

        if (items.length < 2 || reducedMotion) {
            return;
        }

        var index = 0;

        window.setInterval(function () {
            items[index].classList.remove("is-active");
            index = (index + 1) % items.length;
            items[index].classList.add("is-active");
        }, 4200);
    });

    document.querySelectorAll("a[data-account-nav]").forEach(function (link) {
        link.addEventListener("click", function (event) {
            if (event.defaultPrevented || event.button !== 0 || event.metaKey || event.ctrlKey || event.shiftKey || event.altKey) {
                return;
            }

            var shell = document.querySelector(".account-shell");

            if (!shell || reducedMotion) {
                return;
            }

            event.preventDefault();
            shell.classList.add("is-leaving");

            window.setTimeout(function () {
                window.location.href = link.href;
            }, 280);
        });
    });

    document.querySelectorAll("[data-file-field]").forEach(function (field) {
        var input = field.querySelector("input[type='file']");
        var text = field.querySelector("[data-file-text]");
        var preview = field.querySelector("[data-file-preview]");
        var emptyText = text ? text.textContent : "";

        if (!input) {
            return;
        }

        input.addEventListener("change", function () {
            var file = input.files && input.files[0];

            field.classList.toggle("has-file", !!file);

            if (text) {
                text.textContent = file ? file.name : emptyText;
            }

            if (preview) {
                if (file && file.type.indexOf("image/") === 0) {
                    preview.src = URL.createObjectURL(file);
                    preview.hidden = false;
                } else {
                    preview.removeAttribute("src");
                    preview.hidden = true;
                }
            }
        });
    });

    var registerForm = document.getElementById("registerForm");

    if (registerForm) {
        var steps = registerForm.querySelectorAll(".account-step");
        var progressLabel = registerForm.querySelector("[data-step-label]");

        var goToStep = function (stepNumber) {
            registerForm.setAttribute("data-current-step", String(stepNumber));

            steps.forEach(function (step) {
                step.classList.toggle("is-active", step.getAttribute("data-step") === String(stepNumber));
            });

            if (progressLabel) {
                progressLabel.textContent = "Paso " + stepNumber + " de " + steps.length;
            }

            var active = registerForm.querySelector(".account-step.is-active");

            if (active) {
                var firstInput = active.querySelector("input:not([type='hidden']):not([type='radio']):not([type='file'])");

                if (firstInput && !reducedMotion) {
                    firstInput.focus({ preventScroll: true });
                }
            }
        };

        var validateStep = function (stepNumber) {
            var step = registerForm.querySelector(".account-step[data-step='" + stepNumber + "']");

            if (!step || !window.jQuery || !window.jQuery.fn || !window.jQuery.fn.validate) {
                return true;
            }

            var validator = window.jQuery(registerForm).validate();
            var valid = true;

            step.querySelectorAll("input[name]").forEach(function (input) {
                if (!validator.element(input)) {
                    valid = false;
                }
            });

            return valid;
        };

        registerForm.querySelectorAll("[data-step-next]").forEach(function (button) {
            button.addEventListener("click", function () {
                if (validateStep(1)) {
                    goToStep(2);
                }
            });
        });

        registerForm.querySelectorAll("[data-step-prev]").forEach(function (button) {
            button.addEventListener("click", function () {
                goToStep(1);
            });
        });

        registerForm.addEventListener("submit", function (event) {
            if (!validateStep(1)) {
                event.preventDefault();
                goToStep(1);
                return;
            }

            if (!validateStep(2)) {
                event.preventDefault();
            }
        });

        var firstStepError = registerForm.querySelector(".account-step[data-step='1'] .input-validation-error, .account-step[data-step='1'] .field-validation-error:not(:empty)");

        if (!firstStepError && registerForm.querySelector(".account-step[data-step='2'] .input-validation-error, .account-step[data-step='2'] .field-validation-error:not(:empty)")) {
            goToStep(2);
        }
    }
})();
