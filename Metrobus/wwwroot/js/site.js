$(document).ready(function () {
    
});


/*  Funcion para consultar los datos del metrobus proporcionados por la CDMX   */

function ConsultaDataMetrobusesCDMX() {

    identifier = {
        consulta: 1
    }

    $.ajax({
        url: AbsolutePath() + 'Home/Consulta',
        data: identifier,
        type: 'GET',
        async: true,
        success: function (response) {
        
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'La información se consulto correctamente',
                showConfirmButton: false,
                timer: 1500
            })
        },
        error: function () {
            Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'Error al consultar la información',
                showConfirmButton: false,
                timer: 1500
            })
        }
    });

}

function ConsultaMetrobusByID() {

    var id = $("#txtVehicle").val();

    identifier = {
        id: id
    }

    $.ajax({
        url: AbsolutePath() + 'Home/SetHistorial',
        data: identifier,
        type: 'GET',
        async: true,
        success: function (response) {
            window.location = AbsolutePath() + "Home/Historial";
        },
        error: function () {
           
        }
    });

}


/*  Funcion para establecer la url absoluta del sitio   */
function AbsolutePath() {

    var loc = window.location;
    var protocolo = window.location.protocol;
    var host = window.location.hostname;
    var puerto = window.location.port === "" ? "" : ":" + window.location.port;
    var path = "";

    if (loc.pathname == "/")
        path = loc.pathname;
    else {
        var seccion = loc.pathname.split("/");

        if (seccion.length == 4 || seccion.length == 2) {
            path = "/" + seccion[1] + "/";
        }
        else {
            path = "/";
        }
    }
    return protocolo + '//' + host + puerto + path;

}



