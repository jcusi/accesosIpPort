(function () {
    document.getElementsByClassName('js-redirecionar')[0].addEventListener("click", function (e) {
        let url = "/Home/actualizarSesion";
        let exito = function (rpta) {
            console.log(rpta);
        }
           let error = function (rpta) {
               console.log(rpta);
            }
        BI.AjaxJson('Post', url, {}, true, exito, error);
    });


}())