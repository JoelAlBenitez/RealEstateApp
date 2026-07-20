// Acciones JavaScript para el módulo Administrador

document.addEventListener("DOMContentLoaded", function () {
    // Configurar modal dinámico de confirmación de estado
    const modalToggleStatus = document.getElementById("modalToggleStatus");
    if (modalToggleStatus) {
        modalToggleStatus.addEventListener("show.bs.modal", function (event) {
            const btn = event.relatedTarget;
            const userId = btn.getAttribute("data-user-id");
            const userActive = btn.getAttribute("data-user-active") === "true";
            const userName = btn.getAttribute("data-user-name");

            const inputId = document.getElementById("toggleStatusId");
            const inputActive = document.getElementById("toggleStatusActive");
            const modalBody = document.getElementById("toggleStatusBodyText");
            const modalTitle = document.getElementById("modalToggleStatusLabel");

            if (inputId) inputId.value = userId;
            if (inputActive) inputActive.value = userActive ? "false" : "true";

            if (modalTitle) {
                modalTitle.textContent = userActive ? "Inactivar Usuario" : "Activar Usuario";
            }

            if (modalBody) {
                modalBody.innerHTML = `¿Está seguro de que desea <strong>${userActive ? "inactivar" : "activar"}</strong> al usuario <strong>${userName}</strong>?`;
            }
        });
    }
});
