var BI = {
    AjaxGetHtml: function (url, loading = true) {
        loading = typeof loading !== 'undefined' ? loading : true;

        $.ajaxSetup({
            beforeSend: function (xhr) {
                xhr.setRequestHeader('X-CSRF-TOKEN-HEADERNAME', $('input[name="AntiforgeryFieldname"]').val())
                if (loading === true) {
                    $("#divLoading").show();
                }
            },
            complete: function () {
                if (loading === true) {
                    $("#divLoading").css('display', 'none');
                }
            }
        });

        var rsp = null;
        $.ajax({
            type: "get",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "html",
            success: function (response) {
                rsp = response;
            },
            error: function (request, status, error) {
                alert(request.statusText);
                console.error(request.responseText);
            }
        });
        return rsp;
    },

    AjaxHtml: function (url, exito, error, loading = true) {
        loading = typeof loading !== 'undefined' ? loading : true;

        $.ajaxSetup({
            beforeSend: function (xhr) {
                xhr.setRequestHeader('X-CSRF-TOKEN-HEADERNAME', $('input[name="AntiforgeryFieldname"]').val())
                if (loading === true) {
                    $("#divLoading").show();
                }
            },
            complete: function () {
                if (loading === true) {
                    $("#divLoading").css('display', 'none');
                }
            }
        });

        //var rsp = null;
        $.ajax({
            type: "get",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "html",
            success: exito,
            error: error
        });
        //return rsp;
    },

    AjaxJson: function (type, url, parameters, async, exito, error, loading = true) {
        loading = typeof loading !== 'undefined' ? loading : true;

        $.ajaxSetup({
            beforeSend: function (xhr) {
                xhr.setRequestHeader('X-CSRF-TOKEN-HEADERNAME', $('input[name="AntiforgeryFieldname"]').val())
                if (async == true) {
                    if (loading === true) {
                        $("#divLoading").show();
                    }
                }
            },
            complete: function () {
                if (async == true) {
                    if (loading === true) {
                        $("#divLoading").css('display', 'none');
                    }
                }
            }
        });

        $.ajax({
            type: type,
            url: url,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: async,
            data: JSON.stringify(parameters),

            statusCode: {
                404: function (entidad) {

                    // errorbatuta(entidad.responseJSON.apiMensaje);

                    //swal({
                    //    title: "Error",
                    //    text: entidad.responseJSON.apiMensaje,
                    //    icon: "error"
                    //});
                    BI.MostrarMensajeError(entidad.responseJSON.apiMensaje);
                },
                401: function (entidad) {

                    // errorbatuta(entidad.responseJSON.apiMensaje);

                    //swal({
                    //    title: "Error",
                    //    text: entidad.responseJSON.apiMensaje,
                    //    icon: "error"
                    //});
                    BI.MostrarMensajeError(entidad.responseJSON.apiMensaje);
                },
                422: function (entidad) {

                    //console.log(entidad.responseJSON.apiMensaje);
                    var arreglo = Array();
                    arreglo = entidad.responseJSON.apiMensaje.split(".");
                    let texto = "";
                    arreglo.forEach(function (elemento) {
                        if (elemento.length != 0) {
                            texto += '<li class="text-left">' + elemento + '.</li>';
                        }
                    });
                    const wrapper = document.createElement('ul');
                    wrapper.innerHTML = texto;

                    // errorbatuta(wrapper);
                    //swal({
                    //    title: "Error",
                    //    content: wrapper,
                    //    icon: "error"
                    //});
                    BI.MostrarMensajeError(wrapper);

                },
                500: function (entidad) {

                    //console.log(entidad.responseJSON.apiMensaje);
                    // errobatuta("Comuniquese con Soporte Técnico");
                    //swal({
                    //    title: "Error",
                    //    text: "Comuniquese con Soporte Técnico",
                    //    icon: "error"
                    //});
                    BI.MostrarMensajeError("Comuniquese con Soporte Técnico");
                }
                // 404: function () { alert("404"); },
                // 200: function () { alert("200"); },
                // 201: function () { alert("201"); },
                // 202: function () { alert("202"); }
            },

            success: exito,
            error: error
        });

    },

    AjaxFormData: function (url, formData, exito, error, loading = true) {
        loading = typeof loading !== 'undefined' ? loading : true;

        $.ajaxSetup({
            beforeSend: function (xhr) {
                xhr.setRequestHeader('X-CSRF-TOKEN-HEADERNAME', $('input[name="AntiforgeryFieldname"]').val())
                if (loading === true) {
                    $("#divLoading").show();
                }
            },
            complete: function () {
                if (loading === true) {
                    $("#divLoading").css('display', 'none');
                }
            }
        });

        $.ajax({
            type: "POST",
            url: url,
            data: formData,

            statusCode: {
                401: function (entidad) {

                    //console.log(entidad.responseJSON.apiMensaje);
                    //  errorbatuta(entidad.responseJSON.apiMensaje);

                    //swal({
                    //    title: "Error",
                    //    text: entidad.responseJSON.apiMensaje,
                    //    icon: "error"
                    //});
                    $('#divLoading').modal('hide');
                    BI.MostrarMensajeError("Comuniquese con Soporte Técnico");
                },
                422: function (entidad) {

                    //console.log(entidad.responseJSON.apiMensaje);
                    //errorbatuta(entidad.responseJSON.apiMensaje);
                    //swal({
                    //    title: "Error",
                    //    text: entidad.responseJSON.apiMensaje,
                    //    icon: "error"
                    //});
                    $('#divLoading').modal('hide');
                    BI.MostrarMensajeError(entidad.responseJSON.apiMensaje);

                },
                500: function (entidad) {

                    //console.log(entidad.responseJSON.apiMensaje);
                    //errorbatuta(entidad.responseJSON.apiMensaje);
                    //swal({
                    //    title: "Error",
                    //    text: "Comuniquese con Soporte Técnico",
                    //    icon: "error"
                    //});
                    $('#divLoading').modal('hide');
                    BI.MostrarMensajeError(entidad.responseJSON.apiMensaje);
                }
                // 404: function () { alert("404"); },
                // 200: function () { alert("200"); },
                // 201: function () { alert("201"); },
                // 202: function () { alert("202"); }
            },


            //dataType: 'json',
            contentType: false,
            processData: false,
            success: exito,
            error: error
        }).done(function (data) {
            //console.log('');
        });
    },
}