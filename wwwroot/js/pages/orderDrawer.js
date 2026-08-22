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

    const stockPage = document.querySelector(".stock-page");
    const stockSymbol = stockPage.dataset.symbol;
    const tradableInstrumentId = parseInt(stockPage.dataset.tradableInstrumentId);

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
    function resetOrderDrawer() {

        // Quantity
        qtyInput.value = 1;

        // Price
        priceInput.value = currentMarketPrice.toFixed(2);

        // CMP
        cmpCheckbox.checked = true;
        priceInput.disabled = true;

        // Product
        setProduct("delivery");

        // Order variety
        setOrderType("regular");

        // Validity
        document.getElementById("dayValidity").checked = true;
        document.getElementById("iocValidity").checked = false;

        // Recalculate estimated value
        updateEstimatedOrderValue();
    }
    function setOrderSide(side) {

        selectedSide = side;

        resetOrderDrawer();

        // Change drawer visual mode
        const drawer = document.querySelector(".order-drawer");

        drawer.classList.toggle(
            "sell-mode",
            side === "sell"
        );

        if (side === "buy") {

            orderDrawerTitle.textContent = `BUY ${stockSymbol}`;
            placeOrderBtn.textContent = "BUY NOW";

        } else {

            orderDrawerTitle.textContent = `SELL ${stockSymbol}`;
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

    // ==========================================================
    // PLACE ORDER
    // ==========================================================

    placeOrderBtn.addEventListener("click", async () => {

        const quantity = parseInt(qtyInput.value) || 0;
        const price = parseFloat(priceInput.value) || 0;

        if (quantity <= 0) {
            alert("Quantity must be greater than zero.");
            return;
        }

        if (price <= 0) {
            alert("Price must be greater than zero.");
            return;
        }

        // Get dynamic order variant values
        let triggerPrice = null;
        let limitPrice = null;

        if (selectedOrderType === "regular" && !cmpCheckbox.checked) {
            limitPrice = price;
        }

        const variantInputs =
            orderVariantFields.querySelectorAll(".price-input");

        if (selectedOrderType === "sl") {

            triggerPrice =
                parseFloat(variantInputs[0]?.value) || null;

            limitPrice =
                parseFloat(variantInputs[1]?.value) || null;
        }

        if (selectedOrderType === "slm") {

            triggerPrice =
                parseFloat(variantInputs[0]?.value) || null;
        }

        if (selectedOrderType === "gtt") {

            triggerPrice =
                parseFloat(variantInputs[0]?.value) || null;

            limitPrice =
                parseFloat(variantInputs[1]?.value) || null;
        }

        const request = {
            tradableInstrumentId: tradableInstrumentId,
            side: selectedSide.toUpperCase(),
            product: selectedProduct.toUpperCase(),
            quantity: quantity,
            price: price,
            usesCMP: cmpCheckbox.checked,
            orderType: selectedOrderType.toUpperCase(),
            triggerPrice: triggerPrice,
            limitPrice: limitPrice,
            validity:
                document.querySelector(
                    'input[name="validity"]:checked'
                )?.value || "DAY"
        };

        try {

            placeOrderBtn.disabled = true;

            const response = await fetch("/Order/PlaceOrder", {

                method: "POST",

                headers: {
                    "Content-Type": "application/json"
                },

                body: JSON.stringify(request)
            });

            const result = await response.json();

            if (result.success) {

                alert(
                    `Order placed successfully!\nOrder ID: ${result.orderId}`
                );

                window.location.href = "/Order/MyOrders";

            } else {

                alert(result.message);
            }

        } catch (error) {

            console.error("Order placement failed:", error);

            alert(
                "Something went wrong while placing the order."
            );

        } finally {

            placeOrderBtn.disabled = false;
        }
    });
});