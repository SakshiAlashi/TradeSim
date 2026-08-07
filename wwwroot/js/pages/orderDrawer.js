const overlay = document.getElementById("orderDrawerOverlay");

const buyButton = document.getElementById("buyButton");

const sellButton = document.getElementById("sellButton");

const closeButton = document.getElementById("closeOrderDrawer");

buyButton.addEventListener("click", () => {

    overlay.classList.add("open");

});

sellButton.addEventListener("click", () => {

    overlay.classList.add("open");

});

closeButton.addEventListener("click", () => {

    overlay.classList.remove("open");

});

overlay.addEventListener("click", e => {

    if (e.target === overlay)

        overlay.classList.remove("open");

});