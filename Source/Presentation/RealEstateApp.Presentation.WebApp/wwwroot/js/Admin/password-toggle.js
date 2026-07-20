// Alterna la visibilidad de los campos de contraseña marcados con [data-password-toggle].
// El valor del atributo es el id del <input> a mostrar/ocultar.
(function () {
    document.querySelectorAll("[data-password-toggle]").forEach(function (button) {
        button.addEventListener("click", function () {
            var input = document.getElementById(button.getAttribute("data-password-toggle"));
            if (!input) {
                return;
            }

            var show = input.type === "password";
            input.type = show ? "text" : "password";

            var eyeShow = button.querySelector(".pw-eye--show");
            var eyeHide = button.querySelector(".pw-eye--hide");
            if (eyeShow) eyeShow.style.display = show ? "none" : "";
            if (eyeHide) eyeHide.style.display = show ? "" : "none";

            button.setAttribute("aria-pressed", show ? "true" : "false");
            button.setAttribute("aria-label", show ? "Ocultar contraseña" : "Mostrar contraseña");
        });
    });
})();
