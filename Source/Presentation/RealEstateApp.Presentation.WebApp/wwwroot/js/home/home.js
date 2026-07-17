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

        var blockedKeys = ["-", "+", "e", "E"];
        var numberInputs = filter.querySelectorAll("input[type='number']");
        Array.prototype.forEach.call(numberInputs, function (input) {
            input.addEventListener("keydown", function (event) {
                if (blockedKeys.indexOf(event.key) !== -1) {
                    event.preventDefault();
                }
            });
            input.addEventListener("input", function () {
                if (input.value !== "" && Number(input.value) < 0) {
                    input.value = "";
                }
            });
            input.addEventListener("paste", function (event) {
                var text = (event.clipboardData || window.clipboardData).getData("text");
                if (text && (text.indexOf("-") !== -1 || isNaN(Number(text)))) {
                    event.preventDefault();
                }
            });
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
})();
