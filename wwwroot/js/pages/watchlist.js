  document.addEventListener("DOMContentLoaded", () => {

    // ==========================================================
    // TABS
    // ==========================================================

    const tabs =
        document.querySelectorAll(".watchlist-tab");

    const contents =
        document.querySelectorAll(".watchlist-tab-content");


    tabs.forEach(tab => {

        tab.addEventListener("click", () => {

            const selectedTab =
                tab.dataset.tab;


            tabs.forEach(item => {
                item.classList.remove("active");
            });


            contents.forEach(content => {
                content.classList.remove("active");
            });


            tab.classList.add("active");


            const selectedContent =
                document.getElementById(
                    selectedTab + "-tab"
                );


            if (selectedContent) {
                selectedContent.classList.add("active");
            }

        });

    });


    // ==========================================================
    // ADD TO WATCHLIST
    // ==========================================================

    const addButtons =
        document.querySelectorAll(".add-watchlist-btn");


    addButtons.forEach(button => {

        button.addEventListener("click", async function (event) {

            event.preventDefault();
            event.stopPropagation();


            if (this.disabled) {
                return;
            }


            const instrumentId =
                this.dataset.instrumentId;


            const stockRow =
                this.closest(".stock-row");


            if (!stockRow) {
                return;
            }


            const symbol =
                stockRow.dataset.symbol;


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


                // ==================================================
                // UPDATE ALL STOCKS BUTTON
                // ==================================================

                this.innerHTML =
                    '<i class="bi bi-check"></i>';

                this.disabled = true;


                // ==================================================
                // ADD TO MY WATCHLIST UI
                // ==================================================

                const watchlistList =
                    document.getElementById("watchlist-list");


                if (!watchlistList) {
                    return;
                }


                // Remove empty message

                const emptyMessage =
                    watchlistList.querySelector(
                        ".empty-watchlist"
                    );


                if (emptyMessage) {
                    emptyMessage.remove();
                }


                // Prevent duplicate row

                if (
                    watchlistList.querySelector(
                        `[data-instrument-id="${instrumentId}"]`
                    )
                ) {
                    return;
                }


                const price =
                    stockRow
                        .querySelector(".price")
                        .textContent
                        .trim();


                const change =
                    stockRow.querySelector(".change");


                const changeText =
                    change.textContent.trim();


                const changeClass =
                    change.classList.contains("positive")
                        ? "positive"
                        : "negative";


                const watchlistRow =
                    document.createElement("a");


                watchlistRow.className =
                    "watchlist-row stock-row";


                watchlistRow.href =
                    `/Stock/Details?symbol=${encodeURIComponent(
                        symbol
                    )}`;


                watchlistRow.dataset.symbol =
                    symbol;


                watchlistRow.dataset.instrumentId =
                    instrumentId;


                watchlistRow.innerHTML = `

                    <div class="symbol-section">

                        <button type="button"
                                class="favorite-btn"
                                data-instrument-id="${instrumentId}">

                            <i class="bi bi-star"></i>

                        </button>


                        <button type="button"
                                class="remove-watchlist-btn">

                            <i class="bi bi-dash-circle"></i>

                        </button>


                        <span class="symbol">
                            ${symbol}
                        </span>

                    </div>


                    <span class="price">
                        ${price}
                    </span>


                    <span class="change ${changeClass}">
                        ${changeText}
                    </span>

                `;


                watchlistList.appendChild(
                    watchlistRow
                );

            }
            catch (error) {

                console.error(
                    "Add to watchlist error:",
                    error
                );

            }

        });

    });


    // ==========================================================
    // REMOVE FROM WATCHLIST
    // ==========================================================

    document.addEventListener(
        "click",
        async function (event) {

            const removeButton =
                event.target.closest(
                    ".remove-watchlist-btn"
                );


            if (!removeButton) {
                return;
            }


            event.preventDefault();
            event.stopPropagation();


            const stockRow =
                removeButton.closest(".stock-row");


            if (!stockRow) {
                return;
            }


            const instrumentId =
                stockRow.dataset.instrumentId;


            try {

                const response =
                    await fetch(
                        `/Watchlist/Remove?instrumentId=${instrumentId}`,
                        {
                            method: "POST"
                        }
                    );


                if (!response.ok) {

                    throw new Error(
                        "Failed to remove stock from watchlist."
                    );

                }


                // ==================================================
                // REMOVE FROM MY WATCHLIST
                // ==================================================

                stockRow.remove();


                // ==================================================
                // UPDATE ALL STOCKS
                // ✓ → +
                // ==================================================

                const allStocksButton =
                    document.querySelector(
                        `.add-watchlist-btn[data-instrument-id="${instrumentId}"]`
                    );


                if (allStocksButton) {

                    allStocksButton.disabled =
                        false;


                    allStocksButton.innerHTML =
                        '<i class="bi bi-plus"></i>';

                }


                // ==================================================
                // IF WATCHLIST IS NOW EMPTY
                // ==================================================

                const watchlistList =
                    document.getElementById(
                        "watchlist-list"
                    );


                if (
                    watchlistList &&
                    !watchlistList.querySelector(
                        ".stock-row"
                    )
                ) {

                    watchlistList.innerHTML = `

                        <div class="empty-watchlist">

                            <i class="bi bi-eye"></i>

                            <p>Your watchlist is empty.</p>

                            <small>
                                Add stocks from All Stocks to start tracking them.
                            </small>

                        </div>

                    `;

                }

            }
            catch (error) {

                console.error(
                    "Remove from watchlist error:",
                    error
                );

            }

        }
    );


    // ==========================================================
    // FAVORITES
    // ==========================================================

    document.addEventListener(
        "click",
        async function (event) {

            const favoriteButton =
                event.target.closest(
                    ".favorite-btn"
                );


            if (!favoriteButton) {
                return;
            }


            event.preventDefault();
            event.stopPropagation();


            const instrumentId =
                favoriteButton.dataset.instrumentId;


            if (!instrumentId) {
                return;
            }


            try {

                const response =
                    await fetch(
                        `/Watchlist/ToggleFavorite?instrumentId=${instrumentId}`,
                        {
                            method: "POST"
                        }
                    );


                if (!response.ok) {

                    throw new Error(
                        "Failed to toggle favorite."
                    );

                }


                const result =
                    await response.json();


                const isFavorite =
                    result.isFavorite;


                // ==================================================
                // UPDATE ALL VISIBLE STAR BUTTONS
                // ==================================================

                const allFavoriteButtons =
                    document.querySelectorAll(
                        `.favorite-btn[data-instrument-id="${instrumentId}"]`
                    );


                allFavoriteButtons.forEach(button => {

                    const icon =
                        button.querySelector("i");


                    if (!icon) {
                        return;
                    }


                    if (isFavorite) {

                        icon.classList.remove(
                            "bi-star"
                        );

                        icon.classList.add(
                            "bi-star-fill"
                        );

                    }
                    else {

                        icon.classList.remove(
                            "bi-star-fill"
                        );

                        icon.classList.add(
                            "bi-star"
                        );

                    }

                });


                // ==================================================
                // UPDATE FAVORITES TAB
                // ==================================================

                const favoritesList =
                    document.querySelector(
                        "#favorites-tab .watchlist-list"
                    );


                if (!favoritesList) {
                    return;
                }


                const existingFavoriteRow =
                    favoritesList.querySelector(
                        `.stock-row[data-instrument-id="${instrumentId}"]`
                    );


                if (isFavorite) {

                    // ----------------------------------------------
                    // ADD TO FAVORITES
                    // ----------------------------------------------

                    if (!existingFavoriteRow) {

                        const sourceRow =
                            document.querySelector(
                                `#watchlist-list .stock-row[data-instrument-id="${instrumentId}"]`
                            );


                        if (sourceRow) {

                            const symbol =
                                sourceRow.dataset.symbol;


                            const price =
                                sourceRow
                                    .querySelector(".price")
                                    .textContent
                                    .trim();


                            const change =
                                sourceRow.querySelector(
                                    ".change"
                                );


                            const changeText =
                                change.textContent.trim();


                            const changeClass =
                                change.classList.contains(
                                    "positive"
                                )
                                    ? "positive"
                                    : "negative";


                            const favoriteRow =
                                document.createElement("a");


                            favoriteRow.className =
                                "watchlist-row stock-row";


                            favoriteRow.href =
                                `/Stock/Details?symbol=${encodeURIComponent(
                                    symbol
                                )}`;


                            favoriteRow.dataset.symbol =
                                symbol;


                            favoriteRow.dataset.instrumentId =
                                instrumentId;


                            favoriteRow.innerHTML = `

                                <div class="symbol-section">

                                    <button type="button"
                                            class="favorite-btn"
                                            data-instrument-id="${instrumentId}">

                                        <i class="bi bi-star-fill"></i>

                                    </button>


                                    <span class="symbol">
                                        ${symbol}
                                    </span>

                                </div>


                                <span class="price">
                                    ${price}
                                </span>


                                <span class="change ${changeClass}">
                                    ${changeText}
                                </span>

                            `;


                            const emptyMessage =
                                favoritesList.querySelector(
                                    ".empty-watchlist"
                                );


                            if (emptyMessage) {
                                emptyMessage.remove();
                            }


                            favoritesList.appendChild(
                                favoriteRow
                            );

                        }

                    }

                }
                else {

                    // ----------------------------------------------
                    // REMOVE FROM FAVORITES
                    // ----------------------------------------------

                    if (existingFavoriteRow) {

                        existingFavoriteRow.remove();

                    }


                    // Show empty message if necessary

                    if (
                        !favoritesList.querySelector(
                            ".stock-row"
                        )
                    ) {

                        favoritesList.innerHTML = `

                            <div class="empty-watchlist">

                                <i class="bi bi-star"></i>

                                <p>No favorite stocks yet.</p>

                                <small>
                                    Mark stocks in your watchlist as favorites for quick access.
                                </small>

                            </div>

                        `;

                    }

                }

            }
            catch (error) {

                console.error(
                    "Favorite toggle error:",
                    error
                );

            }

        }
    );

});