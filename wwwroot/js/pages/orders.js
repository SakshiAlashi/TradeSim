// document.addEventListener("DOMContentLoaded", function () {

//     const tabs = document.querySelectorAll(".order-tab");
//     const rows = document.querySelectorAll(
//         ".orders-table tbody tr[data-order-status]"
//     );

//     tabs.forEach(function (tab) {

//         tab.addEventListener("click", function () {

//             const selectedStatus = tab.dataset.status;

//             Remove active state from all tabs
//             tabs.forEach(function (item) {
//                 item.classList.remove("active");
//             });

//             Activate clicked tab
//             tab.classList.add("active");

//             Filter rows
//             rows.forEach(function (row) {

//                 const rowStatus = row.dataset.orderStatus;

//                 if (
//                     selectedStatus === "ALL" ||
//                     rowStatus === selectedStatus
//                 ) {
//                     row.style.display = "";
//                 }
//                 else {
//                     row.style.display = "none";
//                 }

//             });

//         });

//     });

// });
document.addEventListener("DOMContentLoaded", function () {

    // ============================================================
    // STATUS TABS
    // ============================================================

    const tabs = document.querySelectorAll(".order-tab");
    const rows = document.querySelectorAll(
        ".orders-table tbody tr[data-order-status]"
    );

    tabs.forEach(function (tab) {

        tab.addEventListener("click", function () {

            const selectedStatus = tab.dataset.status;

            // Remove active state from all tabs
            tabs.forEach(function (item) {
                item.classList.remove("active");
            });

            // Activate clicked tab
            tab.classList.add("active");

            // Filter order rows
            rows.forEach(function (row) {

                const rowStatus = row.dataset.orderStatus;

                if (
                    selectedStatus === "ALL" ||
                    rowStatus === selectedStatus
                ) {
                    row.style.display = "";
                }
                else {
                    row.style.display = "none";
                }

            });

        });

    });


    // ============================================================
    // CANCEL OPEN ORDER
    // ============================================================

    document.addEventListener("click", async function (event) {

        const cancelButton =
            event.target.closest(".order-cancel-trigger");

        if (!cancelButton) {
            return;
        }

        const orderId = cancelButton.dataset.orderId;

        if (!orderId) {
            console.error("Order ID is missing.");
            return;
        }

        const confirmed = confirm(
            "Are you sure you want to cancel this open order?"
        );

        if (!confirmed) {
            return;
        }

        cancelButton.disabled = true;

        try {

            const response = await fetch("/Order/CancelOrder", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    orderId: parseInt(orderId)
                })
            });

            if (!response.ok) {
                throw new Error(
                    `HTTP error: ${response.status}`
                );
            }

            const result = await response.json();

            if (result.success) {

                alert(result.message);

                // Refresh the Orders page so:
                // - status changes to CANCELLED
                // - tab counts update
                // - badge becomes non-clickable
                window.location.reload();

            }
            else {

                alert(result.message);

                cancelButton.disabled = false;
            }

        }
        catch (error) {

            console.error(
                "Cancel order error:",
                error
            );

            alert(
                "Something went wrong while cancelling the order."
            );

            cancelButton.disabled = false;
        }

    });

});