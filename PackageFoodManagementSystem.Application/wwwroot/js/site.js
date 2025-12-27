// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function openUserMenu() {
    document.getElementById("userPopup").classList.add("open");
    document.getElementById("popupOverlay").classList.add("show");
}

function closeUserMenu() {
    document.getElementById("userPopup").classList.remove("open");
    document.getElementById("popupOverlay").classList.remove("show");
}

function redirectTo(url) {
    window.location.href = url;
}
function increase(btn) {
    const card = btn.closest(".product-card");
    const qtySpan = card.querySelector(".qty");

    let qty = parseInt(qtySpan.innerText);
    qtySpan.innerText = qty + 1;

    updateCartCount(1);

    // OPTIONAL: send to backend
    sendToCart(card);
}

function decrease(btn) {
    const card = btn.closest(".product-card");
    const qtySpan = card.querySelector(".qty");

    let qty = parseInt(qtySpan.innerText);
    if (qty > 0) {
        qtySpan.innerText = qty - 1;
        updateCartCount(-1);
    }
}

function updateCartCount(change) {
    const badge = document.getElementById("cart-count");
    let count = parseInt(badge.innerText) || 0;

    count += change;
    if (count < 0) count = 0;

    badge.innerText = count;

    // small animation
    badge.style.transform = "scale(1.3)";
    setTimeout(() => badge.style.transform = "scale(1)", 150);
}

function sendToCart(card) {
    const id = card.dataset.id;
    const name = card.dataset.name;
    const price = card.dataset.price;
    const quantity = card.dataset.quantity;

    fetch('/Cart/AddToCart', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: `id=${id}&name=${encodeURIComponent(name)}&quantity=${quantity}&price=${price}`
    });
}

function addToCart(button) {
    // Increase cart count UI
    let cartBadge = document.getElementById("cart-count");
    let currentCount = parseInt(cartBadge.innerText) || 0;
    cartBadge.innerText = currentCount + 1;

    // OPTIONAL: send data to backend
    const card = button.closest(".product-card");

    if (card) {
        const id = card.dataset.id;
        const name = card.dataset.name;
        const quantity = card.dataset.quantity;
        const price = card.dataset.price;

        fetch('/Cart/AddToCart', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `id=${id}&name=${encodeURIComponent(name)}&quantity=${encodeURIComponent(quantity)}&price=${price}`
        });
    }
}
