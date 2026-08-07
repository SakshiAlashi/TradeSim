document.addEventListener("DOMContentLoaded", () => {

    const watchlistItems = document.querySelectorAll(".watchlist-item");
    const detailsPanel = document.getElementById("stockDetails");

    watchlistItems.forEach(item => {

        item.addEventListener("click", () => {

            // Remove previous selection
            watchlistItems.forEach(x => x.classList.remove("active"));

            // Highlight selected item
            item.classList.add("active");

            // Read data attributes
            const symbol = item.dataset.symbol;
            const name = item.dataset.name;
            const price = item.dataset.price;
            const open = item.dataset.open;
            const high = item.dataset.high;
            const low = item.dataset.low;
            const volume = item.dataset.volume;
            const change = item.dataset.change;
            const percent = item.dataset.percent;

            detailsPanel.innerHTML = `
                <h2>${symbol}</h2>
                <h4>${name}</h4>

                <div class="price">
                    ₹${price}
                </div>

                <div class="${change >= 0 ? "positive" : "negative"}">
                    ${change >= 0 ? "▲" : "▼"} ${change}
                    (${percent}%)
                </div>

                <hr/>

                <div class="details-grid">

                    <div>
                        <strong>Open</strong>
                        <p>₹${open}</p>
                    </div>

                    <div>
                        <strong>High</strong>
                        <p>₹${high}</p>
                    </div>

                    <div>
                        <strong>Low</strong>
                        <p>₹${low}</p>
                    </div>

                    <div>
                        <strong>Volume</strong>
                        <p>${Number(volume).toLocaleString()}</p>
                    </div>

                </div>

                <button class="btn btn-success mt-4">
                    Buy
                </button>

                <button class="btn btn-danger mt-4 ms-2">
                    Sell
                </button>
            `;
        });

    });

});
const tabs = document.querySelectorAll(".tab-btn");

const contents = document.querySelectorAll(".tab-content");

tabs.forEach(tab => {

    tab.addEventListener("click", () => {

        tabs.forEach(t => t.classList.remove("active"));

        contents.forEach(c => c.classList.remove("active"));

        tab.classList.add("active");

        document
            .getElementById(tab.dataset.tab + "Tab")
            .classList.add("active");

    });

});