document.addEventListener("DOMContentLoaded", loadBasket);

function loadBasket() {
    const cart = JSON.parse(localStorage.getItem("myCart")) || [];

    const basketItems = document.getElementById("basketItems");
    const summaryItems = document.getElementById("summaryItems");
    const totalAmount = document.getElementById("totalAmount");

    basketItems.innerHTML = "";
    summaryItems.innerHTML = "";

    let total = 0;

    cart.forEach(item => {
        const itemTotal = item.price * item.qty;
        total += itemTotal;

        // LEFT SIDE ITEM
        basketItems.innerHTML += `
            <div class="basket-item">
                <h5>${item.name}</h5>
                <p>₹${item.price} × ${item.qty}</p>
                <strong>₹${itemTotal}</strong>
            </div>
        `;
        ////////////////////////////////////////

        // RIGHT POPUP SUMMARY
        summaryItems.innerHTML += `
            <p>${item.name} (${item.qty})</p>
        `;
    });

    totalAmount.innerText = total;
}
item.name
