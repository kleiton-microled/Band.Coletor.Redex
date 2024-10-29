jQuery.curCSS = function (element, prop, val) {
    return jQuery(element).css(prop, val);
};

jQuery.browser = {};
(function () {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();

var codigoTalie = '';
var codigoBooking = '';
var codigoPatio = '';
var codigoRegistro = '';
var inicio = '';
var fim = '';
var registro = '';
var placa = '';
var reserva = '';
var cliente = '';
var conferente = '';
var equipe = '';
var operacao = '';
var codigoGate = '';
var codigoReserva = '';
var codigoUsuario = '';
var obs = '';

function setarValoresCampos() {
    codigoTalie = document.getElementById('txtTalie');
    codigoBooking = document.getElementById('lblCodigoBooking');
    codigoPatio = document.getElementById('lblCodigoPatio');
    codigoRegistro = document.getElementById('lblCodigoRegistro');
    inicio = document.getElementById('TxtInicio');
    fim = document.getElementById('TxtFim');
    registro = document.getElementById('TxtRegistro');
    placa = document.getElementById('TxtPlaca');
    reserva = document.getElementById('TxtReserva');
    cliente = document.getElementById('TxtCliente');
    conferente = document.getElementById('DC_Conferente');
    equipe = document.getElementById('DC_Equipe');
    operacao = document.getElementById('DC_Operacao');
    codigoGate = document.getElementById('lblCodigoGate');
    codigoReserva = document.getElementById('lblCodigoReserva');
    codigoUsuario = document.getElementById('lblCodigoUsuario');
    obs = document.getElementById('TxtObservacao');
}

$(document).ready(function () {

    //var b = $(window).height();
    // $("#tblRegistro").css("height", b - 0);       

    setarValoresCampos();

    $("#BtNext").click(function () {

        window.location.href = "TalieDescargaItens.aspx?CodigoTalie=" + codigoTalie.value +
            "&CodigoBooking=" + codigoBooking.value +
            "&CodigoPatio=" + codigoPatio.value +
            "&CodigoRegistro=" + codigoRegistro.value +
            "&temp_registro_txtTalie=" + codigoTalie.value +
            "&temp_registro_TxtInicio=" + inicio.value +
            "&temp_registro_TxtFim=" + fim.value +
            "&temp_registro_TxtRegistro=" + registro.value +
            "&temp_registro_TxtPlaca=" + placa.value +
            "&temp_registro_TxtReserva=" + reserva.value +
            "&temp_registro_TxtCliente=" + cliente.value +
            "&temp_registro_DC_Conferente=" + conferente.value +
            "&temp_registro_DC_Equipe=" + equipe.value +
            "&temp_registro_DC_Operacao=" + operacao.value +
            "&temp_registro_lblCodigoBooking=" + codigoBooking.value +
            "&temp_registro_lblCodigoGate=" + codigoGate.value +
            "&temp_registro_lblCodigoRegistro=" + codigoRegistro.value +
            "&temp_registro_lblCodigoReserva=" + codigoReserva.value +
            "&temp_registro_lblCodigoUsuario=" + codigoUsuario.value +
            "&temp_registro_lblCodigoPatio=" + codigoPatio.value;
        return false;
    });

    $("#btnRetornarDados").click(function () {

        document.getElementById('TxtRegistro').value = getCookie("_coletorTalie_TxtRegistro");
        document.getElementById('TxtInicio').value = getCookie("_coletorTalie_TxtInicio");
        document.getElementById('TxtFim').value = getCookie("_coletorTalie_TxtFim");
        document.getElementById('TxtPlaca').value = getCookie("_coletorTalie_TxtPlaca");
        document.getElementById('TxtReserva').value = getCookie("_coletorTalie_TxtReserva");
        document.getElementById('TxtCliente').value = getCookie("_coletorTalie_TxtCliente");
        document.getElementById('DC_Conferente').value = getCookie("_coletorTalie_DC_Conferente");
        document.getElementById('DC_Equipe').value = getCookie("_coletorTalie_DC_Equipe");
        document.getElementById('DC_Operacao').value = getCookie("_coletorTalie_DC_Operacao");
        document.getElementById('TxtObservacao').value = getCookie("_coletorTalie_TxtObservacao");
        document.getElementById('lblCodigoBooking').value = getCookie("_coletorTalie_lblCodigoBooking");
        document.getElementById('lblCodigoGate').value = getCookie("_coletorTalie_lblCodigoGate");
        document.getElementById('lblCodigoRegistro').value = getCookie("_coletorTalie_lblCodigoRegistro");
        document.getElementById('lblCodigoReserva').value = getCookie("_coletorTalie_lblCodigoReserva");
        document.getElementById('txtTalie').value = getCookie("_coletorTalie_txtTalie");
        document.getElementById('LBStatusTalie').value = getCookie("_coletorTalie_LBStatusTalie");

        setarControles($("#TxtRegistro").val());

        return false;

    });

    $('input[type="text"]').on('blur', function () {
        $(this).removeClass('texto-focus').addClass('texto-blur');
        if ($(this).val() !== '') {
            setarCookies();
        }
    }).on('focus', function () {
        $(this).removeClass('texto-blur').addClass('texto-focus');
    });

    $('input[type="submit"]').on('blur', function () {
        $(this).removeClass('botao-focus').addClass('botao-blur');
    }).on('focus', function () {
        $(this).removeClass('botao-blur').addClass('botao-focus');
    });

    $("#btnBuscarRegistro").click(function () {
        ObterDadosTalie();
        return false;
    });

    $("#BtGravar").click(function () {
        GravarTalie();
        return false;
    });

    $("#BtFinalizar").click(function () {
        FinalizarTalie();
        return false;
    });

    $("#BtExcluir").click(function () {
        ExcluirTalie();
        return false;
    });

    $("#btnFecharObs").click(function () {
        $("#tbObservacao").hide();
        $("#tblRegistro").show();
        return false;
    });

    $("#BtObservacao").click(function () {
        $("#tbObservacao").show();
        $("#tblRegistro").hide();
        return false;
    });

    $("#DC_Operacao").on('change', function () {
        $("#BtGravar").focus();
    });

    setarControles($("#TxtRegistro").val());

    $("#TxtRegistro").focus();

});

$(document).on("keypress", "#TxtRegistro", function (e) {
    if (e.which == 13) {
        e.preventDefault();
        ObterDadosTalie();
    }
});

function ObterDadosTalie() {
    $.ajax({
        type: "POST",
        url: "TalieDescargaRegistro.aspx/ObterDadosTalieAsync",
        data: '{Registro: "' + $("#TxtRegistro")[0].value + '",Inicio: "' + $("#TxtInicio")[0].value + '",Usuario: "' + $("#lblCodigoUsuario")[0].value + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",        
        success: DadosTalieOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtRegistro"));
        }
    });
}

function DadosTalieOnSuccess(response) {

    var retorno = response.d;
    
    if (retorno.Inconsistencia !== null) {
        if (retorno.Inconsistencia !== "") {
            ExibirModal(retorno.Inconsistencia, $("#TxtRegistro"));
            return;
        }
    }
    
    document.getElementById('TxtInicio').value = retorno.Inicio;
    document.getElementById('TxtFim').value = retorno.Termino;
    document.getElementById('TxtPlaca').value = retorno.Placa;
    document.getElementById('TxtReserva').value = retorno.Reserva;
    document.getElementById('TxtCliente').value = retorno.Cliente;
    document.getElementById('DC_Conferente').value = retorno.Conferente;
    document.getElementById('DC_Equipe').value = retorno.Equipe;
    document.getElementById('DC_Operacao').value = retorno.Operacao;
    document.getElementById('TxtObservacao').value = retorno.Observacao;
    document.getElementById('lblCodigoBooking').value = retorno.CodigoBooking;
    document.getElementById('lblCodigoGate').value = retorno.CodigoGate;
    document.getElementById('lblCodigoRegistro').value = retorno.CodigoRegistro;
    document.getElementById('lblCodigoReserva').value = retorno.CodigoReserva;
    document.getElementById('txtTalie').value = retorno.CodigoTalie;
    document.getElementById('LBStatusTalie').value = retorno.StatusTalie;

    setarControles(retorno.CodigoTalie);
    setarCookies();

    if (retorno.ExisteTalieAberto) {
        ExibirModal("Já existe talie aberto para esta placa/reserva - Abrindo para edição", $("#DC_Conferente"));
    }

}

function setarCookies() {

    setarValoresCampos();

    setCookie("_coletorTalie_TxtRegistro", codigoRegistro.value, 1);
    setCookie("_coletorTalie_TxtInicio", inicio.value, 1);
    setCookie("_coletorTalie_TxtFim", fim.value, 1);
    setCookie("_coletorTalie_TxtPlaca", placa.value, 1);
    setCookie("_coletorTalie_TxtReserva", reserva.value, 1);
    setCookie("_coletorTalie_TxtCliente", cliente.value, 1);
    setCookie("_coletorTalie_DC_Conferente", conferente.value, 1);
    setCookie("_coletorTalie_DC_Equipe", equipe.value, 1);
    setCookie("_coletorTalie_DC_Operacao", operacao.value, 1);
    setCookie("_coletorTalie_TxtObservacao", obs.value, 1);
    setCookie("_coletorTalie_lblCodigoBooking", codigoBooking.value, 1);
    setCookie("_coletorTalie_lblCodigoGate", codigoGate.value, 1);
    setCookie("_coletorTalie_lblCodigoRegistro", codigoRegistro.value, 1);
    setCookie("_coletorTalie_lblCodigoReserva", codigoReserva.value, 1);
    setCookie("_coletorTalie_txtTalie", codigoTalie.value, 1);

}

// ============ Finalizar Talie

function FinalizarTalie() {
    $.ajax({
        type: "POST",
        url: "TalieDescargaRegistro.aspx/FinalizarTalieAsync",
        data: '{CodigoTalie: "' + $("#txtTalie").val() +
          '",CodigoBooking: "' + $("#lblCodigoBooking").val() +
          '",Registro: "' + $("#TxtRegistro").val() +
          '",Inicio: "' + $("#TxtInicio").val() + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",   
        success: FinalizarTalieOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtRegistro"));
        }
    });
}

function FinalizarTalieOnSuccess(response) {

    var retorno = response.d;

    if (retorno !== null) {
        if (retorno > 0) {
            ExibirModal('Carga transferida para o estoque. Finalizado com Sucesso!', $("#TxtRegistro"));
            window.location.href = "TalieDescargaRegistro.aspx?usuario=" + $("#lblCodigoUsuario").val() + "&patio=" + $("#lblCodigoPatio").val() + "";
        } else {
            ExibirModal(retorno, $("#TxtRegistro"));
        }
    }

}

// ============ Gravar Talie

function GravarTalie() {

    var obs = $("#TxtObservacao").val();

    if (obs === undefined || obs === null) {
        obs = "";
    }

    $.ajax({
        type: "POST",
        url: "TalieDescargaRegistro.aspx/GravarTalieAsync",
        data: '{CodigoTalie: "' + $("#txtTalie").val() +
          '",CodigoBooking: "' + $("#lblCodigoBooking").val() +
          '",Placa: "' + $("#TxtPlaca").val() +
          '",Inicio: "' + $("#TxtInicio").val() +
          '",Conferente: "' + $("#DC_Conferente").val() +
          '",Equipe: "' + $("#DC_Equipe").val() +
          '",Operacao: "' + $("#DC_Operacao").val() +
          '",CodigoGate: "' + $("#lblCodigoGate").val() +
          '",Registro: "' + $("#TxtRegistro").val() +
          '",Observacao: "' + obs + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",   
        success: GravarTalieOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtRegistro"));
        }
    });
}

function GravarTalieOnSuccess(response) {

    var retorno = response.d;

    if (retorno !== null) {
        if (retorno > 0) {

            $("#BtNext").removeAttr('disabled');
            $("#BtGravar").attr("disabled", true);
            $("#BtNovo").attr("disabled", true);

            $("#txtTalie").val(retorno);

            ExibirModal('Registro cadastrado/alterado!', $("#BtNext"));

        } else {
            ExibirModal(retorno, $("#BtNext"));
        }
    }

    $("#BtNext").focus();

}

// ============ Excluir Talie

function ExcluirTalie() {
    $.ajax({
        type: "POST",
        url: "TalieDescargaRegistro.aspx/ExcluirTalieAsync",
        data: '{CodigoTalie: "' + $("#txtTalie").val() + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",   
        success: ExcluirTalieOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtRegistro"));
        }
    });
}

function ExcluirTalieOnSuccess(response) {

    var retorno = response.d;

    if (retorno !== null) {
        if (retorno == "") {
            ExibirModal('Talie excluído com Sucesso!', $("#TxtRegistro"));
            window.location.href = "TalieDescargaRegistro.aspx";
        } else {
            ExibirModal(retorno, $("#TxtRegistro"));
        }
    }

}

function setarControles(codigoTalie) {

    if (typeof codigoTalie !== "undefined" || codigoTalie !== null) {

        if (codigoTalie == "") {

            $("#BtNovo").removeAttr('disabled');
            $("#BtFinalizar").attr("disabled", true);
            $("#BtNext").attr("disabled", true);
            $("#BtExcluir").attr("disabled", true);
            $("#BtCancelar").attr("disabled", true);
            $("#BtGravar").attr("disabled", true);
            $("#TxtRegistro").focus();

        }

        if (codigoTalie > 0) {

            $("#BtFinalizar").removeAttr('disabled');
            $("#BtNext").removeAttr('disabled');
            $("#BtExcluir").removeAttr('disabled');
            $("#BtCancelar").removeAttr('disabled');
            $("#BtGravar").removeAttr('disabled');
            $("#BtNovo").attr("disabled", true);
            $("#DC_Conferente").focus();

        }

        if (codigoTalie == 0) {

            $("#BtFinalizar").removeAttr('disabled');
            $("#BtNext").attr("disabled", true);
            $("#BtExcluir").attr("disabled", true);
            $("#BtCancelar").removeAttr('disabled');
            $("#BtGravar").removeAttr('disabled');
            $("#BtNovo").attr("disabled", true);
            $("#DC_Conferente").focus();

        }

    }

}

function setCookie(nome, valor, exdias) {
    var exdata = new Date();
    exdata.setDate(exdata.getDate() + exdias);
    var c_valor = escape(valor) + ((exdias == null) ? "" : "; expires=" + exdata.toUTCString());
    document.cookie = nome + "=" + c_valor;
}

function getCookie(c_nome) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_nome) {
            return unescape(y);
        }
    }
}

function ExibirModal(texto, controle) {

    alert(texto);

    if (typeof controle !== "undefined" || controle !== null) {
        controle.focus();
    }

}