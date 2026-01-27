console.log("menu.js loaded");

document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".qty[data-product-id]").forEach(qty => {
        const productId = qty.dataset.productId;
        refreshQty(productId);

    });

});

function increase(btn) {

    const card = btn.closest('.card');

    const qtySpan = card.querySelector('.qty');

    const productId = card.getAttribute('data-product-id');

    btn.disabled = true;

    fetch(`/Cart/Add?productId=${productId}`, {

        method: 'POST'


    })

        .then(res => {

            if (res.ok) {

                qtySpan.innerText = parseInt(qtySpan.innerText) + 1;

            }


        })

        .finally(() => btn.disabled = false);

}


function decrease(btn) {

    const card = btn.closest('.card');

    const qtySpan = card.querySelector('.qty');

    const productId = card.getAttribute('data-product-id');

    btn.disabled = true;

    fetch(`/Cart/Decrease?productId=${productId}`, {

        method: 'POST'


    })

        .then(res => {

            if (res.ok) {

                let q = parseInt(qtySpan.innerText);

                if (q > 0) qtySpan.innerText = q - 1;

            }


        })

        .finally(() => btn.disabled = false);

}


function refreshQty(productId) {
    return fetch(`/Cart/GetItemQty?productId=${productId}`)

        .then(res => res.json())

        .then(qty => {
            document
                .querySelector(`.card[data-product-id="${productId}"] .qty`)
                .innerText = qty;
        });

}
