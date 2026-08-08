document.addEventListener("DOMContentLoaded", () => {

    // ==========================================================
    // ELEMENTS
    // ==========================================================

    const overlay = document.getElementById("orderDrawerOverlay");

    const buyButton = document.getElementById("buyButton");
    const sellButton = document.getElementById("sellButton");

    const closeButton = document.getElementById("closeOrderDrawer");

    const deliveryBtn = document.getElementById("deliveryBtn");
    const intradayBtn = document.getElementById("intradayBtn");

    const cmpCheckbox = document.getElementById("cmpCheckbox");
    const priceInput = document.getElementById("priceInput");

    const currentMarketPrice = parseFloat(priceInput.value);

    const orderTypeButtons = document.querySelectorAll(".type-btn");
    const orderVariantFields = document.getElementById("orderVariantFields");

    const orderDrawerTitle = document.getElementById("orderDrawerTitle");
    const placeOrderBtn = document.getElementById("placeOrderBtn");
        
    const qtyMinus = document.getElementById("qtyMinus");
    const qtyPlus = document.getElementById("qtyPlus");
    const qtyInput = document.getElementById("qtyInput");

    const estimatedOrderValue = document.getElementById("estimatedOrderValue");
    
    // ==========================================================
    // QUANTITY
    // ==========================================================
    qtyMinus.addEventListener("click", () => {

        let quantity = parseInt(qtyInput.value) || 1;

        if (quantity > 1) {
            quantity--;
        }

        qtyInput.value = quantity;
        updateEstimatedOrderValue();
    });

    qtyPlus.addEventListener("click", () => {

        let quantity = parseInt(qtyInput.value) || 1;

        quantity++;

        qtyInput.value = quantity;
        updateEstimatedOrderValue();
    });

    qtyInput.addEventListener("change", () => {

        let quantity = parseInt(qtyInput.value) || 1;

        if (quantity < 1) {
            quantity = 1;
        }

        qtyInput.value = quantity;
        updateEstimatedOrderValue();
    });
    // ==========================================================
    // ESTIMATED ORDER VALUE
    // ==========================================================

    function updateEstimatedOrderValue() {

        const quantity = parseInt(qtyInput.value) || 0;

        const price = parseFloat(priceInput.value) || 0;

        const total = quantity * price;

        estimatedOrderValue.textContent =
            `₹${total.toLocaleString("en-IN", {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            })}`;
    }

    // ==========================================================
    // ORDER SIDE
    // ==========================================================

    let selectedSide = "buy";

    function setOrderSide(side) {

        selectedSide = side;

        // Change drawer visual mode
        const drawer = document.querySelector(".order-drawer");

        drawer.classList.toggle(
            "sell-mode",
            side === "sell"
        );

        if (side === "buy") {

            orderDrawerTitle.textContent = "BUY RELIANCE";
            placeOrderBtn.textContent = "BUY NOW";

        } else {

            orderDrawerTitle.textContent = "SELL RELIANCE";
            placeOrderBtn.textContent = "SELL NOW";

        }
    }


    // ==========================================================
    // OPEN / CLOSE DRAWER
    // ==========================================================

    buyButton.addEventListener("click", () => {

        setOrderSide("buy");

        overlay.classList.add("open");

    });

    sellButton.addEventListener("click", () => {

        setOrderSide("sell");

        overlay.classList.add("open");

    });

    closeButton.addEventListener("click", () => {

        overlay.classList.remove("open");

    });

    overlay.addEventListener("click", (e) => {

        if (e.target === overlay) {

            overlay.classList.remove("open");

        }

    });

    // ==========================================================
    // CMP
    // ==========================================================
    cmpCheckbox.addEventListener("change", () => {

        if (cmpCheckbox.checked) {

            priceInput.value =
                currentMarketPrice.toFixed(2);

            priceInput.disabled = true;

        } else {

            priceInput.disabled = false;

        }

        updateEstimatedOrderValue();
    });
    priceInput.disabled = cmpCheckbox.checked;

    priceInput.addEventListener("input", () => {

        updateEstimatedOrderValue();

    });
    // ==========================================================
    // PRODUCT
    // ==========================================================

    let selectedProduct = "delivery";

    function setProduct(product) {

        selectedProduct = product;

        deliveryBtn.classList.toggle(
            "active",
            product === "delivery"
        );

        intradayBtn.classList.toggle(
            "active",
            product === "intraday"
        );
    }

    deliveryBtn.addEventListener("click", () => {
        setProduct("delivery");
    });

    intradayBtn.addEventListener("click", () => {
        setProduct("intraday");
    });

    // Initial state
    setProduct("delivery");

    // ==========================================================
    // ORDER VARIETY
    // ==========================================================

    let selectedOrderType = "regular";

    function setOrderType(orderType) {

        selectedOrderType = orderType;

        orderTypeButtons.forEach(button => {

            button.classList.toggle(
                "active",
                button.dataset.order === orderType
            );

        });

        renderOrderVariantFields();
    }


    // ==========================================================
    // ORDER VARIANT FIELDS
    // ==========================================================

    function renderOrderVariantFields() {

        orderVariantFields.innerHTML = "";


        // ------------------------------------------------------
        // REGULAR
        // ------------------------------------------------------

        if (selectedOrderType === "regular") {
            return;
        }


        // ------------------------------------------------------
        // SL
        // ------------------------------------------------------

        if (selectedOrderType === "sl") {

        orderVariantFields.innerHTML = `

        <div class="drawer-section">

            <div class="price-fields-row">

                <div class="price-field">

                    <label class="section-label">
                        Trigger Price (₹)
                    </label>

                    <input
                        class="price-input"
                        type="number"
                        value="2838.00"
                        step="0.05" />

                </div>

                <div class="price-field">

                    <label class="section-label">
                        Limit Price (₹)
                    </label>

                    <input
                        class="price-input"
                        type="number"
                        value="2837.00"
                        step="0.05" />

                </div>
    
            </div>

        </div>

        `;

            return;
        }


        // ------------------------------------------------------
        // SL-M
        // ------------------------------------------------------

        if (selectedOrderType === "slm") {

            orderVariantFields.innerHTML = `

                <div class="drawer-section price-section">

                    <label class="section-label">
                        Trigger Price (₹)
                    </label>

                    <input
                        class="price-input"
                        type="number"
                        value="2838.00"
                        step="0.05" />

                </div>

            `;

            return;
        }


        // ------------------------------------------------------
        // AMO
        // ------------------------------------------------------

        if (selectedOrderType === "amo") {
            return;
        }


        // ------------------------------------------------------
        // GTT
        // ------------------------------------------------------

        if (selectedOrderType === "gtt") {

            orderVariantFields.innerHTML = `

                <div class="drawer-section">

                    <div class="price-fields-row">

                        <div class="price-field">

                            <label class="section-label">
                                Trigger Price (₹)
                            </label>
    
                            <input
                                class="price-input"
                                type="number"
                                value="2838.00"
                                step="0.05" />

                        </div>

                        <div class="price-field">

                            <label class="section-label">
                                Limit Price (₹)
                            </label>

                            <input
                                class="price-input"
                                type="number"
                                value="2837.00"
                                step="0.05" />

                        </div>
                   </div>

                </div>

            `;
            return;
        }
    }


    // ==========================================================
    // ORDER VARIETY BUTTONS
    // ==========================================================

    orderTypeButtons.forEach(button => {

        button.addEventListener("click", () => {

            setOrderType(
                button.dataset.order
            );

        });

    });
    // Initial state
    setOrderType("regular");
    updateEstimatedOrderValue();
});