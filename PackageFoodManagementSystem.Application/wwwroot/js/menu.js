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

    fetch(`/Cart/Add?productId=${productId}`, { method: 'POST' })
        .then(res => {
            if (res.ok) {
                // Use a fallback to 0 to prevent NaN
                let currentVal = parseInt(qtySpan.innerText) || 0;
                qtySpan.innerText = currentVal + 1;

                if (typeof window.updateCartBadge === 'function') {
                    window.updateCartBadge();
                }
            }
        })
        .finally(() => btn.disabled = false);
}

function decrease(btn) {
    const card = btn.closest('.card');
    const qtySpan = card.querySelector('.qty');
    const productId = card.getAttribute('data-product-id');

    btn.disabled = true;

    fetch(`/Cart/Decrease?productId=${productId}`, { method: 'POST' })
        .then(res => {
            if (res.ok) {
                let q = parseInt(qtySpan.innerText) || 0;
                if (q > 0) {
                    qtySpan.innerText = q - 1;
                    if (typeof window.updateCartBadge === 'function') {
                        window.updateCartBadge();
                    }
                }
            }
        })
        .finally(() => btn.disabled = false);
}

function refreshQty(productId) {
    return fetch(`/Cart/GetItemQty?productId=${productId}`)
        .then(res => res.json())
        .then(data => {
            const el = document.querySelector(`.card[data-product-id="${productId}"] .qty`);
            if (el) {
                // DEBUG: This will show you exactly what your server is sending
                console.log(`Product ${productId} data:`, data);

                // This logic handles BOTH a single number OR an object
                if (typeof data === 'object' && data !== null) {
                    // Try common property names like qty, quantity, or count
                    el.innerText = data.qty ?? data.quantity ?? data.count ?? 0;
                } else {
                    // If it's just a plain number/string
                    el.innerText = data;
                }
            }
        })
        .catch(err => console.error("Error fetching quantity:", err));
}