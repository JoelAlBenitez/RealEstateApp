(function () {
    "use strict";

    var form = document.querySelector(".agent-consult__form");
    if (form) {
        form.addEventListener("submit", function (event) {
            var input = form.querySelector("input");
            if (!input) {
                return;
            }
            input.value = input.value.trim();
            if (input.value.length === 0) {
                event.preventDefault();
                input.focus();
            }
        });
    }

    var grid = document.getElementById("agentGrid");
    if (grid && "IntersectionObserver" in window) {
        var cards = Array.prototype.slice.call(grid.children);
        cards.forEach(function (card, index) {
            card.style.opacity = "0";
            card.style.transform = "translateY(12px)";
            card.style.transition = "opacity 320ms ease-out " + (index % 3) * 80 + "ms, transform 320ms ease-out " + (index % 3) * 80 + "ms";
        });
        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.style.opacity = "1";
                    entry.target.style.transform = "translateY(0)";
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: .12 });
        cards.forEach(function (card) {
            observer.observe(card);
        });
    }
})();
