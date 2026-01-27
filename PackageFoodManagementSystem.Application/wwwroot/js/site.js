// --- User Menu Logic ---
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

// --- Cart Logic ---

function increase(btn) {
    const card = btn.closest(".product-card");
    const qtySpan = card.querySelector(".qty");

    let qty = parseInt(qtySpan.innerText);
    qty = qty + 1;
    qtySpan.innerText = qty;

    saveToLocalStorage(card, qty);
    updateCartBadge();
}

function decrease(btn) {
    const card = btn.closest(".product-card");
    const qtySpan = card.querySelector(".qty");

    let qty = parseInt(qtySpan.innerText);
    if (qty > 0) {
        qty = qty - 1;
        qtySpan.innerText = qty;
        saveToLocalStorage(card, qty);
        updateCartBadge();
    }
}

function saveToLocalStorage(card, qty) {
    // 1. Get existing cart or empty array
    let cart = JSON.parse(localStorage.getItem("myCart")) || [];

    const productId = card.dataset.id;
    const itemData = {
        id: productId,
        name: card.dataset.name,
        price: parseFloat(card.dataset.price),
        image: card.querySelector("img").src,
        qty: qty
    };

    // 2. Check if this specific product is already in the array
    const existingItemIndex = cart.findIndex(item => item.id === productId);

    if (existingItemIndex > -1) {
        // If it exists, update its quantity or remove it if qty is 0
        if (qty > 0) {
            cart[existingItemIndex].qty = qty;
        } else {
            cart.splice(existingItemIndex, 1);
        }
    } else if (qty > 0) {
        // 3. If it's a NEW item, PUSH it so it doesn't overlap
        cart.push(itemData);
    }

    // 4. Save the full list back to memory
    localStorage.setItem("myCart", JSON.stringify(cart));
}

function updateCartBadge() {
    const cart = JSON.parse(localStorage.getItem("myCart")) || [];
    const badge = document.getElementById("cart-count");

    if (badge) {
        // FIXED: Sum all quantities (3 chips + 3 juices = 6)
        const totalItems = cart.reduce((sum, item) => sum + parseInt(item.qty), 0);

        badge.innerText = totalItems;

        // Start from 0: Hide if empty, show if > 0
        if (totalItems > 0) {
            badge.style.display = "flex";
        } else {
            badge.style.display = "none";
        }
    }
}

// Sync the numbers on the cards when you return to the page
function syncCardNumbers() {
    const cart = JSON.parse(localStorage.getItem("myCart")) || [];
    document.querySelectorAll(".product-card").forEach(card => {
        const productId = card.dataset.id;
        const qtySpan = card.querySelector(".qty");
        const item = cart.find(i => i.id === productId);
        if (item) {
            qtySpan.innerText = item.qty;
        } else {
            qtySpan.innerText = "0";
        }
    });
}

// Run when page loads
document.addEventListener("DOMContentLoaded", () => {
    updateCartBadge();
    syncCardNumbers();
});

function filterLocations() {
    let input = document.getElementById('locationSearchInput').value.toLowerCase();
    let list = document.getElementById('locationList');
    let items = list.getElementsByClassName('loc-item');

    for (let i = 0; i < items.length; i++) {
        let name = items[i].querySelector('.loc-name').innerText.toLowerCase();
        if (name.includes(input)) {
            items[i].style.display = "flex"; // Show
        } else {
            items[i].style.display = "none"; // Hide
        }
    }
}

function updateLocation(name) {
    document.getElementById('displayLocation').innerText = name;
    bootstrap.Modal.getInstance(document.getElementById('locationModal')).hide();
}

function getLocation() {
    document.getElementById('displayLocation').innerText = "Detecting...";
    setTimeout(() => { updateLocation("Chennai, Tamil Nadu"); }, 1000);
}