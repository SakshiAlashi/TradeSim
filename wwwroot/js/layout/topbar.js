const profileTrigger = document.querySelector(".profile-trigger");
const profileMenu = document.querySelector(".profile-menu");
const profileDropdown = document.querySelector(".profile-dropdown");

profileTrigger.addEventListener("click", function (event) {

    event.stopPropagation();

    profileMenu.classList.toggle("show");

});

document.addEventListener("click", function () {

    profileMenu.classList.remove("show");

});