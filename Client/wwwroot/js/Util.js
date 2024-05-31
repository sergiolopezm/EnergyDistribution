function quitarFocoDelInput(elementId) {
    var elemento = document.getElementById(elementId);
    if (elemento) {
        elemento.blur();
    }
}

function limpiarFocoDeTodosLosCampos() {
    var elementos = document.querySelectorAll("input[type='text'], input[type='password']");
    elementos.forEach(function (elemento) {
        elemento.blur();
    });
}

window.blurAllFields = function () {
    var inputFields = document.querySelectorAll('input, select, textarea');
    inputFields.forEach(function (element) {
        element.blur();
    });
};