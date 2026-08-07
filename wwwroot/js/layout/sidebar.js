document.addEventListener("DOMContentLoaded", () => {

    const sidebar = document.getElementById("sidebar");
    const toggle = document.getElementById("sidebarToggle");

    // Restore previous state
    if (localStorage.getItem("sidebar") === "collapsed") {
        sidebar.classList.add("collapsed");
    }

    toggle.addEventListener("click", () => {

        sidebar.classList.toggle("collapsed");

        if (sidebar.classList.contains("collapsed")) {
            localStorage.setItem("sidebar", "collapsed");
        }
        else {
            localStorage.setItem("sidebar", "expanded");
        }

    });

});