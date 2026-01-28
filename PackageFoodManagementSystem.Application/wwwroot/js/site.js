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
    let cart = JSON.parse(localStorage.getItem("myCart")) || [];

    const productId = card.dataset.id;
    const itemData = {
        id: productId,
        name: card.dataset.name,
        price: parseFloat(card.dataset.price),
        image: card.querySelector("img").src,
        qty: qty
    };

    const existingItemIndex = cart.findIndex(item => item.id === productId);

    if (existingItemIndex > -1) {
        if (qty > 0) {
            cart[existingItemIndex].qty = qty;
        } else {
            cart.splice(existingItemIndex, 1);
        }
    } else if (qty > 0) {
        cart.push(itemData);
    }

    localStorage.setItem("myCart", JSON.stringify(cart));
}

function updateCartBadge() {
    const cart = JSON.parse(localStorage.getItem("myCart")) || [];
    const badge = document.getElementById("cart-count");

    if (badge) {
        const totalItems = cart.reduce((sum, item) => sum + parseInt(item.qty), 0);
        badge.innerText = totalItems;

        if (totalItems > 0) {
            badge.style.display = "flex";
        } else {
            badge.style.display = "none";
        }
    }
}

function syncCardNumbers() {
    const cart = JSON.parse(localStorage.getItem("myCart")) || [];
    document.querySelectorAll(".product-card").forEach(card => {
        const productId = card.dataset.id;
        const qtySpan = card.querySelector(".qty");

        // Added safety check to prevent "null" error if .qty is missing
        if (qtySpan) {
            const item = cart.find(i => i.id === productId);
            qtySpan.innerText = item ? item.qty : "0";
        }
    });
}

// --- Location Logic ---

function filterLocations() {
    let input = document.getElementById('locationSearchInput').value.toLowerCase();
    let list = document.getElementById('locationList');
    let items = list.getElementsByClassName('loc-item');

    for (let i = 0; i < items.length; i++) {
        let name = items[i].querySelector('.loc-name').innerText.toLowerCase();
        if (name.includes(input)) {
            items[i].style.display = "flex";
        } else {
            items[i].style.display = "none";
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

// --- Combined Initialization ---
document.addEventListener("DOMContentLoaded", () => {
    // 1. Initialize Cart
    updateCartBadge();
    syncCardNumbers();

    // 2. Initialize Profile Edit Logic (Fix for the 'null' error)
    const editToggle = document.getElementById('editToggle');
    const updateBtn = document.getElementById('btnUpdate');
    const inputs = document.querySelectorAll('#profileForm input');

    if (editToggle && updateBtn) {
        editToggle.addEventListener('click', () => {
            const isEditing = updateBtn.classList.toggle('d-none');

            // UI text and style changes
            editToggle.innerHTML = isEditing ?
                '<i class="fa-solid fa-pen-to-square me-1"></i> Edit Mode' :
                '<i class="fa-solid fa-xmark me-1"></i> Cancel';

            editToggle.classList.toggle('btn-outline-danger', !isEditing);

            // Toggle input states
            inputs.forEach(input => {
                input.readOnly = isEditing;
                input.classList.toggle('bg-light', isEditing);
            });
        });

        updateBtn.addEventListener('click', function () {
            const btn = this;
            const originalContent = btn.innerHTML;

            btn.disabled = true;
            btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> Saving...';

            setTimeout(() => {
                btn.innerHTML = '<i class="fa-solid fa-check-double me-2"></i> Profile Updated';
                btn.className = "btn btn-primary px-5 py-2 rounded-pill fw-bold shadow-sm";

                setTimeout(() => {
                    btn.innerHTML = originalContent;
                    btn.className = "btn btn-success px-5 py-2 rounded-pill fw-bold shadow-sm d-none";
                    btn.disabled = false;
                    editToggle.click(); // Reset UI to view mode
                }, 2000);
            }, 1200);
        });
    }
});