document.addEventListener("DOMContentLoaded", () => {

    document.querySelectorAll(".card").forEach(card => {

        refreshQty(card);

    });

});

function increase(btn) {

    const card = btn.closest('.card');

    const productId = parseInt(card.dataset.id);

    btn.disabled = true;

    fetch('/Cart/Add', {

        method: 'POST',

        credentials: 'include',

        headers: { 'Content-Type': 'application/json' },

        body: JSON.stringify({ productId })

    })

        .then(res => {

            if (!res.ok) throw "Add failed";

            return refreshQty(card);

        })

        .finally(() => btn.disabled = false);

}

function decrease(btn) {

    const card = btn.closest('.card');

    const productId = parseInt(card.dataset.id);

    btn.disabled = true;

    fetch('/Cart/Decrease', {

        method: 'POST',

        credentials: 'include',

        headers: { 'Content-Type': 'application/json' },

        body: JSON.stringify({ productId })

    })

        .then(res => {

            if (!res.ok) throw "Decrease failed";

            return refreshQty(card);

        })

        .finally(() => btn.disabled = false);

}

function refreshQty(card) {

    const productId = parseInt(card.dataset.id);

    return fetch(`/Cart/GetItemQty?productId=${productId}`, {

        credentials: 'include'

    })

        .then(res => res.json())

        .then(qty => {

            card.querySelector('.qty').innerText = qty;

        });

}
