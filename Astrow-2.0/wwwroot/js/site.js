// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//René did this...
function NormalButtonThings() {
    var body = document.body;

    if (body.className != "NormalBody") {
        body.classList.add("NormalBody")
        body.classList.remove("body");
    }
    else {
        body.classList.add("body");
        body.classList.remove("NormalBody")
    }

}

