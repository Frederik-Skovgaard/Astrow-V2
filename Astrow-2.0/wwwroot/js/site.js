// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const { contains } = require("jquery");

// Write your JavaScript code.


//René did this...
function NormalButtonThings() {
    var body = document.body;
    var img = document.getElementById("NormalLogo");
    var container = document.getElementById("NormalContainer");
    var btn = document.getElementById("NormalBtn");
    var input = document.getElementById("username");
    var input2 = document.getElementById("pswd");
    var tooltip = document.getElementById("NormalToolTip");

    if (body.className != "NormalBody") {
        body.classList.add("NormalBody")
        body.classList.remove("body");

        img.classList.add("NormalImg");
        container.classList.add("NormalContainer");
        btn.classList.add("NormalBtn");
        input.classList.add("NormalInput");
        input2.classList.add("NormalInput");
        tooltip.classList.add("NormalTooltip");
    }
    else {
        body.classList.add("body");
        body.classList.remove("NormalBody")

        img.classList.remove("NormalImg");
        container.classList.remove("NormalContainer");
        btn.classList.remove("NormalBtn");
        input.classList.remove("NormalInput");
        input2.classList.remove("NormalInput");
        tooltip.classList.remove("NormalTooltip");
    }

}

function SidebarNav() {
    var list = document.getElementById('Operationer');

    if (list.className != "List-styled") {
        list.classList.add("List-styled");
        list.classList.remove("list-unstyled");
    }
    else {
        list.classList.remove("List-styled");
        list.classList.add("list-unstyled");
    }

}

