const profileTrigger = document.querySelector(".profile-trigger");
const profileMenu = document.querySelector(".profile-menu");
const profileDropdown = document.querySelector(".profile-dropdown");


// ==========================================================
// PROFILE DROPDOWN
// ==========================================================

if (profileTrigger && profileMenu) {

    profileTrigger.addEventListener("click", function (event) {

        event.stopPropagation();

        profileMenu.classList.toggle("show");

    });

    document.addEventListener("click", function () {

        profileMenu.classList.remove("show");

    });

}


// ==========================================================
// GLOBAL MARKET SEARCH
// ==========================================================

const searchInput =
    document.getElementById("globalMarketSearch");

const searchResults =
    document.getElementById("marketSearchResults");


if (searchInput && searchResults) {

    let searchTimeout;


    searchInput.addEventListener("input", function () {

        const searchText =
            this.value.trim();


        clearTimeout(searchTimeout);


        // Clear results when search is empty

        if (!searchText) {

            searchResults.innerHTML = "";

            searchResults.classList.remove("show");

            return;
        }


        // Small delay so we don't call the server
        // on every single keystroke

        searchTimeout = setTimeout(async function () {

            try {

                const response =
                    await fetch(
                        `/Market/Search?searchText=${encodeURIComponent(searchText)}`
                    );

                if (!response.ok) {

                    throw new Error(
                        "Search request failed."
                    );

                }


                const results =
                    await response.json();


                displaySearchResults(results);

            }
            catch (error) {

                console.error(
                    "Market search error:",
                    error
                );

                searchResults.innerHTML = "";

                searchResults.classList.remove("show");

            }

        }, 300);

    });


    // ======================================================
    // DISPLAY RESULTS
    // ======================================================
    function displaySearchResults(results) {

        searchResults.innerHTML = "";


        if (!results || results.length === 0) {

            searchResults.innerHTML = `
            <div class="market-search-empty">
                No stocks or indices found.
            </div>
        `;

            searchResults.classList.add("show");

            return;
        }


        results.forEach(stock => {

            const result =
                document.createElement("div");

            result.className =
                "market-search-item";


            const changeClass =
                stock.changePercent >= 0
                    ? "positive"
                    : "negative";


            const arrow =
                stock.changePercent >= 0
                    ? "▲"
                    : "▼";


            result.innerHTML = `

            <div class="market-search-info">

                <div class="market-search-symbol">
                    ${stock.symbol}
                </div>

                <div class="market-search-name">
                    ${stock.name}
                </div>

            </div>


            <div class="market-search-price">

                <div>
                    ₹${Number(stock.lastPrice).toLocaleString(
                "en-IN",
                {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                }
            )}
                </div>

                <div class="${changeClass}">
                    ${arrow}
                    ${Math.abs(stock.changePercent).toFixed(2)}%
                </div>

            </div>


            <button type="button"
                class="market-search-add"
                data-instrument-id="${stock.tradableInstrumentId}"
                ${stock.isInWatchlist ? "disabled" : ""}>

                <i class="bi ${stock.isInWatchlist ? "bi-check" : "bi-plus"}"></i>
    
            </button>

        `;


            // ======================================================
            // ADD TO WATCHLIST
            // ======================================================

            const addButton =
                result.querySelector(".market-search-add");


            addButton.addEventListener("click", async function (event) {

                event.stopPropagation();

                const instrumentId =
                    this.dataset.instrumentId;


                try {

                    const response =
                        await fetch(
                            `/Watchlist/Add?instrumentId=${instrumentId}`,
                            {
                                method: "POST"
                            }
                        );


                    if (!response.ok) {

                        throw new Error(
                            "Failed to add stock to watchlist."
                        );

                    }


                    this.innerHTML =
                        '<i class="bi bi-check"></i>';

                    this.disabled = true;


                }
                catch (error) {

                    console.error(
                        "Add to watchlist error:",
                        error
                    );

                }

            });


            // ======================================================
            // OPEN STOCK DETAILS
            // ======================================================

            result.addEventListener("click", function (event) {

                if (
                    event.target.closest(
                        ".market-search-add"
                    )
                ) {
                    return;
                }


                window.location.href =
                    `/Stock/Details?symbol=${encodeURIComponent(
                        stock.symbol
                    )}`;

            });


            searchResults.appendChild(result);

        });


        searchResults.classList.add("show");

    }


    // ======================================================
    // CLOSE SEARCH RESULTS WHEN CLICKING OUTSIDE
    // ======================================================

    document.addEventListener("click", function (event) {

        if (
            !event.target.closest(".topbar-search")
        ) {

            searchResults.classList.remove("show");

        }

    });

}