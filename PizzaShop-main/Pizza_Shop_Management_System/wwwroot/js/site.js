// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ToggleEyeIcon(iconId, iconInputId){
    let eyeIcon = document.getElementById(iconId);

    eyeIcon.className = eyeIcon.className === "fa-solid fa-eye input-icon-placement" ? "fa-solid fa-eye-slash input-icon-placement" : "fa-solid fa-eye input-icon-placement";

    let eyeInputTag = document.getElementById(iconInputId);
    if(eyeIcon.className == "fa-solid fa-eye input-icon-placement"){
        eyeInputTag.setAttribute('type','text');
    }else{
        eyeInputTag.setAttribute('type','password');
    }
}