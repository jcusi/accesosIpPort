const connection = new signalR.HubConnectionBuilder()
                        .withUrl("/SessionHub").build();

connection.on("ReceiveMessage", (user, message) => {
    const usuario = user;
    const msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;")
    const EmitirMensaje = `<strong>${msg}</strong>${user}`
    const spanMsg = document.getElementById('js-div-alert');
    //spanMsg.innerHTML = EmitirMensaje;
    let url = "Home/OutPut";
    let exito = function (rpta) {
        if (rpta.status == true) {
            window.location.href = "Home/CloseConection";
        }
    };
    let error = function () { };
    let data = { usuarioMensaje: usuario, mensaje: message }
    BI.AjaxJson('POST', url,data, false, exito, error);
});

connection.start().catch(err => console.log(err.toString()));