function increase(btn) {
    const card = btn.closest('.card');
    const productId = parseInt(card.dataset.id);

    fetch('/Cart/Add', {
        method: 'POST',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId })
    })
        .then(res => {
            if (!res.ok) throw "Add failed";
            refreshQty(card);
        });
}

function decrease(btn) {
    const card = btn.closest('.card');
    const productId = parseInt(card.dataset.id);

    fetch('/Cart/Decrease', {
        method: 'POST',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId })
    })
        .then(res => {
            if (!res.ok) throw "Decrease failed";
            refreshQty(card);
        });
}

function refreshQty(card) {
    const productId = parseInt(card.dataset.id);

    fetch('/Cart/GetItemQty?productId=' + productId, {
        credentials: 'include',
    })
        .then(res => res.json())
        .then(qty => {
            card.querySelector('.qty').innerText = qty;
        });
}