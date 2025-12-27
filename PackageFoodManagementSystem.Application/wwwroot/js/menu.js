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
