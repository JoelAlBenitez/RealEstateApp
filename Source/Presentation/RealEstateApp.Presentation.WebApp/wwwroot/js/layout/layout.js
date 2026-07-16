(function () {
    "use strict";

    
    var themeToggle = document.getElementById("themeToggle");

    function applyTheme(theme) {
        document.documentElement.setAttribute("data-theme", theme);
        document.documentElement.setAttribute("data-bs-theme", theme);

        try {
            localStorage.setItem("rea-theme", theme);
        } catch (e) {
           
        }
    }

    if (themeToggle) {
        themeToggle.addEventListener("click", function () {
            var current = document.documentElement.getAttribute("data-theme") === "dark" ? "dark" : "light";
            applyTheme(current === "dark" ? "light" : "dark");
        });
    }

   
    var SCROLL_THRESHOLD = 32;
    var ticking = false;

    function updateScrollState() {
        document.body.classList.toggle("is-scrolled", window.scrollY > SCROLL_THRESHOLD);
        ticking = false;
    }

    window.addEventListener("scroll", function () {
        if (!ticking) {
            ticking = true;
            window.requestAnimationFrame(updateScrollState);
        }
    }, { passive: true });

    updateScrollState();
})();
