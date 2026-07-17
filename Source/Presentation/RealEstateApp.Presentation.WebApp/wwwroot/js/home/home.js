(function () {
    "use strict";

    var toggle = document.getElementById("filterToggle");
    var filter = document.getElementById("homeFilter");

    if (toggle && filter) {
        if (filter.dataset.open === "true") {
            filter.hidden = false;
            toggle.setAttribute("aria-expanded", "true");
        }
        toggle.addEventListener("click", function () {
            var willOpen = filter.hidden;
            filter.hidden = !willOpen;
            toggle.setAttribute("aria-expanded", String(willOpen));
            if (willOpen) {
                var firstInput = filter.querySelector("input, select");
                if (firstInput) {
                    firstInput.focus();
                }
            }
        });
    }

    var heroFigs = document.querySelectorAll("[data-hero-fig]");
    Array.prototype.forEach.call(heroFigs, function (fig) {
        var img = fig.querySelector("img");
        if (!img) {
            return;
        }
        function markBroken() {
            fig.classList.add("home-hero__fig--broken");
        }
        if (img.complete && img.naturalWidth === 0) {
            markBroken();
        } else {
            img.addEventListener("error", markBroken);
        }
    });

    var grid = document.getElementById("propertyGrid");
    var pager = document.getElementById("clientPagination");

    if (!grid || !pager || grid.dataset.clientPaginate !== "true") {
        return;
    }

    var pageSize = parseInt(grid.dataset.pageSize, 10) || 12;
    var cards = Array.prototype.slice.call(grid.children);
    var totalPages = Math.ceil(cards.length / pageSize);

    if (totalPages <= 1) {
        return;
    }

    pager.hidden = false;

    function render(page) {
        cards.forEach(function (card, index) {
            card.hidden = index < (page - 1) * pageSize || index >= page * pageSize;
        });
        pager.innerHTML = "";
        for (var i = 1; i <= totalPages; i++) {
            var btn = document.createElement("button");
            btn.type = "button";
            btn.className = "pagination__page" + (i === page ? " pagination__page--active" : "");
            btn.textContent = String(i);
            btn.setAttribute("aria-label", "Ir a la página " + i);
            btn.addEventListener("click", (function (target) {
                return function () {
                    render(target);
                    grid.scrollIntoView({ behavior: "smooth", block: "start" });
                };
            })(i));
            pager.appendChild(btn);
        }
    }

    render(1);
})();
