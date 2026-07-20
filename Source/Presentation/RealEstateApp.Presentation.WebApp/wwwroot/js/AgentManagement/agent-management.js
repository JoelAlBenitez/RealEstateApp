(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        var toggle = document.querySelector('.agent-view-toggle');
        var tableView = document.getElementById('agentsTableView');
        var cardsView = document.getElementById('agentsCardsView');

        if (!toggle || !tableView || !cardsView) {
            return;
        }

        var buttons = toggle.querySelectorAll('.agent-view-toggle__btn');

        function setView(view) {
            var showCards = view === 'cards';

            cardsView.hidden = !showCards;
            tableView.hidden = showCards;

            buttons.forEach(function (btn) {
                var isActive = btn.getAttribute('data-view') === view;
                btn.classList.toggle('is-active', isActive);
                btn.setAttribute('aria-pressed', isActive ? 'true' : 'false');
            });

            try {
                localStorage.setItem('agentListView', view);
            } catch (e) {
            }
        }

        buttons.forEach(function (btn) {
            btn.addEventListener('click', function () {
                setView(btn.getAttribute('data-view'));
            });
        });

        var saved;
        try {
            saved = localStorage.getItem('agentListView');
        } catch (e) {
            saved = null;
        }

        if (saved === 'cards') {
            setView('cards');
        }
    });
})();
