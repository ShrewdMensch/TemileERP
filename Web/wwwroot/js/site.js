// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
var offlineInterval = 3600000; //time in ms = 1hr
setTimeout(function () {
    location.href = "/";
}, offlineInterval);