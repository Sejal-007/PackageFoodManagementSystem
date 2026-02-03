console.log("menu.js loaded");

document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".qty[data-product-id]").forEach(qty => {
        const productId = qty.dataset.productId;
        refreshQty(productId);
    });
});

function updateQty(productId, change, btn) {
    const url = change > 0 ? '/Cart/Add' : '/Cart/Decrease';

    // Disable button to prevent rapid double-clicks
    btn.disabled = true;

    fetch(`${url}?productId=${productId}`, {
        method: 'POST',
        headers: { 'X-Requested-With': 'XMLHttpRequest' }
    })
        .then(response => {
            if (!response.ok) throw new Error("Server error");
            return response.json();
        })
        .then(data => {
            if (data.success) {
                // TARGETED UPDATE: Only update spans for this specific product
                document.querySelectorAll(`.qty[data-product-id="${productId}"]`).forEach(span => {
                    span.innerText = data.newQty;
                });

                // Reload if on Basket page to update totals and summary
                if (window.location.pathname.includes("MyBasket")) {
                    location.reload();
                }

                // Update Global Cart Badge
                const badge = document.getElementById('cart-badge');
                if (badge) badge.innerText = data.cartCount;
            }
        })
        .catch(err => console.error('Error:', err))
        .finally(() => btn.disabled = false);
}

function increase(btn) {
    const card = btn.closest('[data-product-id]');
    const productId = card.getAttribute('data-product-id');
    updateQty(productId, 1, btn);
}

function decrease(btn) {
    const card = btn.closest('[data-product-id]');
    const productId = card.getAttribute('data-product-id');
    const qtySpan = card.querySelector('.qty');

    // Allow decrease if qty > 0 or if we are in the basket
    if (parseInt(qtySpan.innerText) > 0 || window.location.pathname.includes("MyBasket")) {
        updateQty(productId, -1, btn);
    }
}

function refreshQty(productId) {
    return fetch(`/Cart/GetItemQty?productId=${productId}`)
        .then(res => res.json())
        .then(data => {
            const el = document.querySelector(`.qty[data-product-id="${productId}"]`);
            if (el) {
                if (typeof data === 'object' && data !== null) {
                    el.innerText = data.qty ?? data.quantity ?? data.count ?? 0;
                } else {
                    el.innerText = data;
                }
            }
        })
        .catch(err => console.error("Error fetching quantity:", err));
}