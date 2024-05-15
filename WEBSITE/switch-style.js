function switchStyle() {
    var styleLink = document.getElementById("style-link");
    if (styleLink.getAttribute("href") == "style.css") {
        styleLink.setAttribute("href", "style2.css");
    } else {
        styleLink.setAttribute("href", "style.css");
    }
}