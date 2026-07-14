
(function () {
    "use strict";

    var TOAST_LIFETIME_MS = 6000;
    var LEAVE_ANIMATION_MS = 240;

    function dismissToast(toast) {
        if (!toast || toast.classList.contains("is-leaving")) {
            return;
        }

        toast.classList.add("is-leaving");
        window.setTimeout(function () {
            toast.remove();
        }, LEAVE_ANIMATION_MS);
    }

    document.querySelectorAll(".rea-toast").forEach(function (toast) {
        var closeButton = toast.querySelector(".rea-toast__close");

        if (closeButton) {
            closeButton.addEventListener("click", function () {
                dismissToast(toast);
            });
        }

        window.setTimeout(function () {
            dismissToast(toast);
        }, TOAST_LIFETIME_MS);
    });

    var overlay = document.getElementById("reaErrorOverlay");

    if (overlay) {
        var closeButton = document.getElementById("reaErrorClose");
        var previousOverflow = document.body.style.overflow;

        document.body.style.overflow = "hidden";

        var closeOverlay = function () {
            if (overlay.classList.contains("is-leaving")) {
                return;
            }

            overlay.classList.add("is-leaving");
            window.setTimeout(function () {
                overlay.remove();
                document.body.style.overflow = previousOverflow;
            }, LEAVE_ANIMATION_MS);
        };

        if (closeButton) {
            closeButton.addEventListener("click", closeOverlay);
            closeButton.focus();
        }

        overlay.addEventListener("click", function (event) {
            if (event.target === overlay) {
                closeOverlay();
            }
        });

        document.addEventListener("keydown", function (event) {
            if (event.key === "Escape") {
                closeOverlay();
            }
        });
    }
})();
