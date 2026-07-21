(function () {
    "use strict";

    var slides = Array.prototype.slice.call(document.querySelectorAll(".gallery__slide"));
    var dots = Array.prototype.slice.call(document.querySelectorAll(".gallery__dot"));
    var peek = document.getElementById("galleryPeek");
    var peekImg = document.getElementById("galleryPeekImg");
    var expandBtn = document.getElementById("galleryExpand");

    var modal = document.getElementById("galleryModal");
    var modalImg = document.getElementById("galleryModalImg");
    var modalBackdrop = document.getElementById("galleryModalBackdrop");
    var modalClose = document.getElementById("galleryModalClose");
    var modalPrev = document.getElementById("modalPrev");
    var modalNext = document.getElementById("modalNext");
    var modalRotate = document.getElementById("modalRotate");
    var modalPerspective = document.getElementById("modalPerspective");

    var current = 0;
    var modalIndex = 0;
    var rotation = 0;
    var perspectiveOn = false;

    function show(index) {
        if (slides.length === 0) {
            return;
        }
        current = (index + slides.length) % slides.length;
        slides.forEach(function (slide, i) {
            slide.classList.toggle("gallery__slide--active", i === current);
        });
        dots.forEach(function (dot, i) {
            dot.classList.toggle("gallery__dot--active", i === current);
        });
        if (peek && peekImg && slides.length > 1) {
            var next = (current + 1) % slides.length;
            peekImg.src = slides[next].src;
            peek.hidden = false;
        }
    }

    dots.forEach(function (dot) {
        dot.addEventListener("click", function () {
            show(parseInt(dot.dataset.index, 10) || 0);
        });
    });

    function applyModalTransform() {
        if (!modalImg) {
            return;
        }
        var transform = "rotate(" + rotation + "deg)";
        if (perspectiveOn) {
            transform += " rotateY(26deg) scale(.94)";
        }
        modalImg.style.transform = transform;
    }

    function openModal(index) {
        if (!modal || !modalImg || slides.length === 0) {
            return;
        }
        modalIndex = (index + slides.length) % slides.length;
        rotation = 0;
        perspectiveOn = false;
        modalImg.src = slides[modalIndex].src;
        applyModalTransform();
        modal.hidden = false;
        document.body.style.overflow = "hidden";
    }

    function closeModal() {
        if (!modal) {
            return;
        }
        modal.hidden = true;
        document.body.style.overflow = "";
    }

    function moveModal(step) {
        modalIndex = (modalIndex + step + slides.length) % slides.length;
        rotation = 0;
        perspectiveOn = false;
        modalImg.src = slides[modalIndex].src;
        applyModalTransform();
    }

    if (expandBtn) {
        expandBtn.addEventListener("click", function () {
            openModal(current);
        });
    }
    if (modalClose) {
        modalClose.addEventListener("click", closeModal);
    }
    if (modalBackdrop) {
        modalBackdrop.addEventListener("click", closeModal);
    }
    if (modalPrev) {
        modalPrev.addEventListener("click", function () {
            moveModal(-1);
        });
    }
    if (modalNext) {
        modalNext.addEventListener("click", function () {
            moveModal(1);
        });
    }
    if (modalRotate) {
        modalRotate.addEventListener("click", function () {
            rotation = (rotation + 90) % 360;
            applyModalTransform();
        });
    }
    if (modalPerspective) {
        modalPerspective.addEventListener("click", function () {
            perspectiveOn = !perspectiveOn;
            applyModalTransform();
        });
    }

    document.addEventListener("keydown", function (event) {
        if (modal && !modal.hidden) {
            if (event.key === "Escape") {
                closeModal();
            }
            if (event.key === "ArrowLeft") {
                moveModal(-1);
            }
            if (event.key === "ArrowRight") {
                moveModal(1);
            }
        }
        var agentModal = document.getElementById("agentModal");
        if (agentModal && !agentModal.hidden && event.key === "Escape") {
            agentModal.hidden = true;
            document.body.style.overflow = "";
        }
    });

    show(0);

    var agentBtn = document.getElementById("agentPhotoBtn");
    var agentImg = document.getElementById("agentPhotoImg");
    var agentModal = document.getElementById("agentModal");
    var agentModalImg = document.getElementById("agentModalImg");
    var agentModalClose = document.getElementById("agentModalClose");
    var agentModalBackdrop = document.getElementById("agentModalBackdrop");

    function closeAgentModal() {
        if (!agentModal) {
            return;
        }
        agentModal.hidden = true;
        document.body.style.overflow = "";
    }

    if (agentBtn && agentImg && agentModal && agentModalImg) {
        agentBtn.addEventListener("click", function () {
            agentModalImg.src = agentImg.src;
            agentModal.hidden = false;
            document.body.style.overflow = "hidden";
        });
    }
    if (agentModalClose) {
        agentModalClose.addEventListener("click", closeAgentModal);
    }
    if (agentModalBackdrop) {
        agentModalBackdrop.addEventListener("click", closeAgentModal);
    }

    var speech = document.getElementById("guideSpeech");
    if (speech) {
        var phrases = [
            "¡Hola! Soy tu guía, te acompaño en este recorrido.",
            "Desliza las imágenes con los puntos, yo te espero aquí.",
            "Amplía la foto para ver cada rincón de la propiedad.",
            "Revisa el precio, el tamaño y las habitaciones con calma.",
            "¿Te gustó? Los datos del agente están justo al lado.",
            "Puedes rotar la imagen ampliada para verla mejor."
        ];
        var phraseIndex = 0;
        setInterval(function () {
            speech.classList.add("is-fading");
            setTimeout(function () {
                phraseIndex = (phraseIndex + 1) % phrases.length;
                speech.textContent = phrases[phraseIndex];
                speech.classList.remove("is-fading");
            }, 300);
        }, 5000);
    }
})();
