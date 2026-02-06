console.log("menu.js loaded");

document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".qty[data-product-id]").forEach(qty => {
        const productId = qty.dataset.productId;
        refreshQty(productId);
    });
});

function updateQty(productId, change, btn) {
    const url = change > 0 ? '/Cart/Add' : '/Cart/Decrease';

    // 1. Find the specific span for this product
    const qtySpans = document.querySelectorAll(`.qty[data-product-id="${productId}"]`);

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
                // 2. Update ALL spans for this product immediately
                qtySpans.forEach(span => {
                    span.innerText = data.newQty;
                });

                // Update Global Cart Badge
                const badge = document.getElementById('cart-badge');
                if (badge) badge.innerText = data.cartCount;

                if (window.location.pathname.includes("MyBasket")) {
                    location.reload();
                }
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