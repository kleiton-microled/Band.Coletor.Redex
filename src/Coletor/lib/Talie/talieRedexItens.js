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

//var cookieToday = new Date();
//var expiryDate = new Date(cookieToday.getTime() + (1 * 86400000));

String.prototype.InsertAt = function (CharToInsert, Position) {
    return this.slice(0, Position) + CharToInsert + this.slice(Position)
}

String.prototype.contains = function (it) { return this.indexOf(it) != -1; };

function formatarComZeros(n) {

    var numero = 0;

    if (n !== null) {
        numero = n;
        if (numero <= 9999) { numero = ("0000" + numero).slice(-4); }
        if (!numero.contains('.')) {
            numero = numero.InsertAt('.', 2);
        }
    }
    return numero;
}

var nf = '';
var peso = '';
var qtde = '';
var codEmbalagem = '';
var embalagem = '';
var claa = '';
var clac = '';
var clal = '';
var local = '';
var imo1 = '';
var imo2 = '';
var imo3 = '';
var imo4 = '';
var uno1 = '';
var uno2 = '';
var uno3 = '';
var uno4 = '';
var remonte = '';
var fumigacao = '';
var saldo = '';
var pesoBruto = '';
var quantidade = '';
var codigoRegCs = '';
var codigoTalie = '';
var codigoNf = '';
var codigoBooking = '';
var codigoItem = '';
var fimNota = '';
var codigoPatio = '';
var codigoRegistro = '';
var flagMadeira = '';
var flagFragil = '';

function setarValoresCampos() {

    nf = document.getElementById('TxtNF');
    peso = document.getElementById('TxtPeso');
    qtde = document.getElementById('TxtQtde');
    codEmbalagem = document.getElementById('TxtCodEmb');
    embalagem = document.getElementById('DcEmbalagem');
    claa = document.getElementById('TxtCLAA');
    clac = document.getElementById('TxtCLAC');
    clal = document.getElementById('TxtCLAL');
    local = document.getElementById('txtLocal');
    imo1 = document.getElementById('TxtIMO1');
    imo2 = document.getElementById('TxtIMO2');
    imo3 = document.getElementById('TxtIMO3');
    imo4 = document.getElementById('TxtIMO4');
    uno1 = document.getElementById('TxtUNO1');
    uno2 = document.getElementById('TxtUNO2');
    uno3 = document.getElementById('TxtUNO3');
    uno4 = document.getElementById('TxtUNO4');
    remonte = document.getElementById('TxtRemonte');
    fumigacao = document.getElementById('TxtFumigacao');
    saldo = document.getElementById('TXTSALDO');
    pesoBruto = document.getElementById('lblPesoBruto');
    quantidade = document.getElementById('lblQuantidade');
    codigoRegCs = document.getElementById('lblCodigoRegCS');
    codigoTalie = document.getElementById('lblCodigoTalie');
    codigoBooking = document.getElementById('lblCodigoBooking');
    codigoItem = document.getElementById('lblCodigoItem');
    fimNota = document.getElementById('lblFimNota');
    codigoPatio = document.getElementById('lblCodigoPatio');
    codigoRegistro = document.getElementById('lblCodigoRegistro');
    flagMadeira = document.getElementById('CheckBox2');
    flagFragil = document.getElementById('CheckBox3');

}



$('input[type="submit"]').on('blur', function () {
    $(this).removeClass('botao-focus').addClass('botao-blur');
}).on('focus', function () {
    $(this).removeClass('botao-blur').addClass('botao-focus');
});

$("#btnRetornarDados").click(function () {

    var cookiesTela = getCookie("ColTalieItens");
    var resultado = JSON.parse(cookiesTela);

    document.getElementById('TxtNF').value = resultado.CTINF;
    document.getElementById('TxtPeso').value = resultado.CTIPeso;
    document.getElementById('TxtQtde').value = resultado.CTIQtde;
    document.getElementById('TxtCodEmb').value = resultado.CTICodEmb;
    document.getElementById('DcEmbalagem').value = resultado.CTIEmb;
    document.getElementById('TxtCLAA').value = resultado.CTICLAA;
    document.getElementById('TxtCLAC').value = resultado.CTICLAC;
    document.getElementById('TxtCLAL').value = resultado.CTICLAL;
    document.getElementById('txtLocal').value = resultado.CTILocal;
    document.getElementById('TxtIMO1').value = resultado.CTIIMO1;
    document.getElementById('TxtIMO2').value = resultado.CTIIMO2;
    document.getElementById('TxtIMO3').value = resultado.CTIIMO3;
    document.getElementById('TxtIMO4').value = resultado.CTIIMO4;
    document.getElementById('TxtUNO1').value = resultado.CTIUNO1;
    document.getElementById('TxtUNO2').value = resultado.CTIUNO2;
    document.getElementById('TxtUNO3').value = resultado.CTIUNO3;
    document.getElementById('TxtUNO4').value = resultado.CTIUNO4;
    document.getElementById('TxtRemonte').value = resultado.CTIRemonte;
    document.getElementById('TxtFumigacao').value = resultado.CTIFumigacao;
    document.getElementById('TXTSALDO').value = resultado.CTISaldo;
    document.getElementById('lblPesoBruto').value = resultado.CTIPesoBruto;
    document.getElementById('lblQuantidade').value = resultado.CTIQuantidade;
    document.getElementById('lblCodigoRegCS').value = resultado.CTICodRegCS;
    document.getElementById('lblCodigoTalie').value = resultado.CTICodTalie;
    document.getElementById('lblCodigoNF').value = resultado.CTICodNF;
    document.getElementById('lblCodigoBooking').value = resultado.CTICodBooking;
    document.getElementById('lblCodigoItem').value = resultado.CTICodItem;
    document.getElementById('lblFimNota').value = resultado.CTIFimNota;
    document.getElementById('lblCodigoPatio').value = resultado.CTICodPatio;
    document.getElementById('lblCodigoRegistro').value = resultado.CTICodRegistro;
    document.getElementById('CheckBox2').value = resultado.CTIFlMadeira;
    document.getElementById('CheckBox3').value = resultado.CTIFlFragil;

    if (resultado.CTINF != '') {
        $("#TxtNF").attr("disabled", true);
        $("#BtBuscaNF").attr("disabled", true);
    }

    $("#BtNovo").attr("disabled", true);
    $("#BtExcluir").attr("disabled", true);
    $("#btLimpar").attr("disabled", true);

    $("#BtCancelar").removeAttr('disabled');
    $("#BtGravar").removeAttr('disabled');

    if ($("#TxtNF").is(':disabled')) {
        $("#TxtQtde").focus();
    } else {
        $("#TxtNF").focus();
    }

    return false;

});


$("#CheckBox2").change(function () {
    GravarCookies();
});

$("#CheckBox3").change(function () {
    GravarCookies();
});

$("#TxtNF").on('keyup change', function () {
    if ($(this).val().length == 10) {
        ConsultarDadosNF();
    }
});

$(document).ready(function () {

    var b = $(window).height();
    $("body").css({ "overflow": "hidden" });

    $('input[type="text"]').on('blur', function () {
        $(this).removeClass('texto-focus').addClass('texto-blur');
        if ($(this).val() !== '') {
            GravarCookies();
        }
    }).on('focus', function () {
        $(this).removeClass('texto-blur').addClass('texto-focus');
    });

    $("#TxtCLAA").blur(function () {
        $("#TxtCLAA").val(formatarComZeros($(this).val()));
    });

    $("#TxtCLAC").blur(function () {
        $("#TxtCLAC").val(formatarComZeros($(this).val()));
    });

    $("#TxtCLAL").blur(function () {
        $("#TxtCLAL").val(formatarComZeros($(this).val()));
    });

    $("#TxtQtde").blur(function () {
        if ($("#TxtQtde").val().length > 0) {
            CalcularPeso();
        }
    });

    $("#txtLocal").blur(function () {
        $("#TxtRemonte").focus();
    });

    $("#TxtCodEmb").blur(function () {
        if ($("#TxtCodEmb").val().length > 0) {
            ObterEmbalagemPorCodigo();
        }
    });

    $("#BtBuscaNF").click(function () {
        ConsultarDadosNF();
        return false;
    });

    $("#BtCancelar").click(function () {
        Limpar(true);
        return false;
    });

    $("#BtGravar").click(function () {
        GravarTalieItem();
        return false;
    });

    $("#BtExcluir").click(function () {
        ExcluirTalieItem();
        return false;
    });

    $("#btLimpar").click(function () {
        Limpar(true);
        return false;
    });

    $("#BtAbreItem").click(function () {
        AbrirItemNF();
        return false;
    });

    $("#BtNovo").click(function () {
        Novo();
        return false;
    });

    PopularDescarga();

    $("#TxtNF").focus();

});

// ------------- Consultar NF

$(document).on("keypress", "#TxtNF", function (e) {
    if (e.which == 13) {
        ConsultarDadosNF();
        return false;
    }
});

// ------------- Abrir Item NF
function isNull(item) {
    if (item === undefined || item === null)
        return true;
}

function AbrirItemNF() {

    var item = $("#ListaDescarga").val();

    if (isNull(item)) {
        alert("Um item deve estar selecionado");
        Limpar(true);
        return;
    }

    $.ajax({
        type: "POST",
        url: "TalieDescargaItens.aspx/CarregaItemAsync",
        data: '{Item: "' + $("#ListaDescarga").val() + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: AbrirItemNFOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtNF"));
        }
    });
}

function AbrirItemNFOnSuccess(response) {

    var retorno = response.d;
    var inconsistencia = "";

    if (!isNull(retorno.Inconsistencia)) {
        if (retorno.Inconsistencia !== "") {
            alert(retorno.Inconsistencia);
            Limpar(true);
            return;
        }
    }

    document.getElementById('TxtPeso').value = '';
    document.getElementById('TxtQtde').value = '';
    document.getElementById('TxtRemonte').value = '';
    document.getElementById('TxtFumigacao').value = '';
    document.getElementById('TxtCLAA').value = '';
    document.getElementById('TxtCLAC').value = '';
    document.getElementById('TxtCLAL').value = '';
    document.getElementById('DcEmbalagem').value = '';
    document.getElementById('TXTSALDO').value = '0 / 0';

    document.getElementById('TxtNF').value = retorno.NumNF;
    document.getElementById('TxtPeso').value = retorno.Peso;
    document.getElementById('TxtQtde').value = retorno.QtdeDescarga;
    document.getElementById('TxtCodEmb').value = retorno.CodigoEmbalagem;
    document.getElementById('DcEmbalagem').value = retorno.Embalagem;
    document.getElementById('TxtCLAA').value = retorno.CLAA;
    document.getElementById('TxtCLAC').value = retorno.CLAC;
    document.getElementById('TxtCLAL').value = retorno.CLAL;
    document.getElementById('txtLocal').value = retorno.Local;
    document.getElementById('TxtIMO1').value = retorno.IMO1;
    document.getElementById('TxtIMO2').value = retorno.IMO2;
    document.getElementById('TxtIMO3').value = retorno.IMO3;
    document.getElementById('TxtIMO4').value = retorno.IMO4;
    document.getElementById('TxtUNO1').value = retorno.UNO1;
    document.getElementById('TxtUNO2').value = retorno.UNO2;
    document.getElementById('TxtUNO3').value = retorno.UNO3;
    document.getElementById('TxtUNO4').value = retorno.UNO4;
    document.getElementById('TxtRemonte').value = retorno.Remonte;
    document.getElementById('TxtFumigacao').value = retorno.Fumigacao;
    document.getElementById('TXTSALDO').value = retorno.Resumo.Saldo;
    document.getElementById('lblPesoBruto').value = retorno.PesoBruto;
    document.getElementById('lblQuantidade').value = retorno.Quantidade;
    document.getElementById('lblCodigoRegCS').value = retorno.CodigoRegCs;
    document.getElementById('lblCodigoNF').value = retorno.CodigoNF;
    document.getElementById('lblCodigoItem').value = retorno.Item;

    $("#CheckBox2").attr('checked', retorno.FlagMadeira);
    $("#CheckBox3").attr('checked', retorno.FlagFragil);

    GravarCookies();

    if (inconsistencia == '') {

        $("#TxtNF").attr("disabled", true);
        $("#BtBuscaNF").attr("disabled", true);
        $("#BtNovo").attr("disabled", true);
        $("#BtExcluir").removeAttr('disabled');
        $("#BtCancelar").removeAttr('disabled');
        $("#BtGravar").removeAttr('disabled');
        $("#btLimpar").removeAttr('disabled');

    }

    $("#TxtQtde").focus();

}

// ------------- Carregar NF

function ConsultarDadosNF() {
    $.ajax({
        type: "POST",
        url: "TalieDescargaItens.aspx/BuscaNF",
        data: '{NumNF: "' + $("#TxtNF").val() + '",CodigoBooking: "' + $("#lblCodigoBooking").val() + '",CodigoRegistro: "' + $("#lblCodigoRegistro").val() + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: ConsultarDadosNFOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtNF"));
        }
    });
}

function stringEmpty(value) {
    return value == '';
}

function ConsultarDadosNFOnSuccess(response) {

    var retorno = response.d;
    var inconsistencia = "";

    if (!isNull(retorno.Inconsistencia)) {
        if (retorno.Inconsistencia !== '') {
            alert(retorno.Inconsistencia);
            Limpar(true);
            return;
        }
    }

    document.getElementById('TxtPeso').value = '';
    document.getElementById('TxtQtde').value = '';
    document.getElementById('TxtRemonte').value = '';
    document.getElementById('TxtFumigacao').value = '';
    document.getElementById('TxtCLAA').value = '';
    document.getElementById('TxtCLAC').value = '';
    document.getElementById('TxtCLAL').value = '';
    document.getElementById('DcEmbalagem').value = '';
    document.getElementById('TXTSALDO').value = '0 / 0';

    document.getElementById('TxtNF').value = retorno.NumNF;
    document.getElementById('DcEmbalagem').value = retorno.Embalagem;
    document.getElementById('TxtCodEmb').value = retorno.CodigoEmbalagem;
    document.getElementById('TxtIMO1').value = retorno.IMO1;
    document.getElementById('TxtIMO2').value = retorno.IMO2;
    document.getElementById('TxtIMO3').value = retorno.IMO3;
    document.getElementById('TxtIMO4').value = retorno.IMO4;
    document.getElementById('TxtUNO1').value = retorno.UNO1;
    document.getElementById('TxtUNO2').value = retorno.UNO2;
    document.getElementById('TxtUNO3').value = retorno.UNO3;
    document.getElementById('TxtUNO4').value = retorno.UNO4;
    document.getElementById('lblPesoBruto').value = retorno.PesoBruto;
    document.getElementById('lblQuantidade').value = retorno.Quantidade;
    document.getElementById('lblCodigoRegCS').value = retorno.CodigoRegCs;
    document.getElementById('lblCodigoNF').value = retorno.CodigoNF;
    document.getElementById('TXTSALDO').value = retorno.Resumo.Saldo;

    GravarCookies();

    if (stringEmpty(retorno.Inconsistencia)) {

        $("#TxtNF").attr("disabled", true);
        $("#BtBuscaNF").attr("disabled", true);

        $("#BtNovo").attr("disabled", true);
        $("#BtExcluir").attr("disabled", true);
        $("#btLimpar").attr("disabled", true);

        $("#BtCancelar").removeAttr('disabled');
        $("#BtGravar").removeAttr('disabled');

    }

    PopularDescarga();

    $("#TxtQtde").focus();

}

// ------------- Carregar Descarga

function PopularDescarga() {
    $.ajax({
        type: "POST",
        url: "TalieDescargaItens.aspx/CarregarDescarga",
        data: '{Talie: "' + $("#lblCodigoTalie").val() + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: PopularDescargaOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtNF"));
        }
    });
}

function PopularDescargaOnSuccess(response) {

    var retorno = response.d;
    $("#ListaDescarga option").remove();

    $(retorno).each(function () {
        $("#ListaDescarga").append(
            $("<option></option>")
                .text(this.Descricao)
                .val(this.CodigoItem)
        );
    });

    ObterSaldo();
}

// ------------- Obter Embalagem por Código

function ObterEmbalagemPorCodigo() {
    $.ajax({
        type: "POST",
        url: "TalieDescargaItens.aspx/ObterEmbalagemPorCodigo",
        data: '{Codigo: "' + $("#TxtCodEmb").val() + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: ObterEmbalagemPorCodigoOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtNF"));
        }
    });
}

function ObterEmbalagemPorCodigoOnSuccess(response) {
    var retorno = response.d;
    $("#DcEmbalagem").val(retorno);
}

// ------------- Calcular Peso

function CalcularPeso() {
    $.ajax({
        type: "POST",
        url: "TalieDescargaItens.aspx/CalcularPeso",
        data: '{PesoBrutoCarga: "' + $("#lblPesoBruto").val() + '",QuantidadeCarga: "' + $("#lblQuantidade").val() + '",QuantidadeInformada: "' + $("#TxtQtde").val() + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: CalcularPesoOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtNF"));
        }
    });
}

function CalcularPesoOnSuccess(response) {
    var retorno = response.d;
    if ($("#TxtPeso").val() != '____.___' || $("#TxtPeso").val() != '') {
        $("#TxtPeso").val(retorno);
    }
}

// ------------- Obter Saldo

function ObterSaldo() {

    $.ajax({
        type: "POST",
        url: "TalieDescargaItens.aspx/ObterSaldoRestante",
        data: '{CodigoRegCs: "' + $("#lblCodigoRegCS").val() + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: ObterSaldoOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtNF"));
        }
    });
}

function ObterSaldoOnSuccess(response) {

    var retorno = response.d;
    var saldo = retorno.Saldo;

    $("#TXTSALDO").val(retorno.Saldo);
    $("#lblFimNota").val(retorno.FimNota);

    if ($("#lblFimNota").val().toLowerCase() == 'true') {

        $("#BtGravar").attr("disabled", true);
        $("#BtCancelar").attr("disabled", true);
        $("#BtNovo").attr("disabled", true);
        $("#BtExcluir").attr("disabled", true);
        $("#BtVoltar").removeAttr('disabled');
        $("#BtAbreItem").removeAttr('disabled');
        $("#btLimpar").attr("disabled", true);

        Limpar(true);        

        $("#TXTSALDO").val(saldo);

    } else {

        $("#BtGravar").removeAttr('disabled');
        $("#BtCancelar").removeAttr('disabled');
        $("#BtNovo").removeAttr('disabled');
        $("#BtExcluir").removeAttr('disabled');
        $("#btLimpar").removeAttr('disabled');

        //$("#TxtQtde").focus();
        // $("#TxtNF").focus();
    }

}

// ============ Gravar item do Talie

function GravarTalieItem() {

    var flagFragil = false;
    var flagMadeira = false;

    if ($("#CheckBox3").is(":checked")) {
        flagFragil = true
    }

    if ($("#CheckBox2").is(":checked")) {
        flagMadeira = true
    }

    $.ajax({
        type: "POST",
        url: "TalieDescargaItens.aspx/GravarItemAsync",
        data: '{CodigoRegCs: "' + $("#lblCodigoRegCS").val() +
        '",CodigoTalie: "' + $("#lblCodigoTalie").val() +
        '",CodigoItem: "' + $("#lblCodigoItem").val() +
        '",Qtde: "' + $("#TxtQtde").val() +
        '",Patio: "' + $("#lblCodigoPatio").val() +
        '",Yard: "' + $("#txtLocal").val() +
        '",CLAC: "' + $("#TxtCLAC").val() +
        '",CLAL: "' + $("#TxtCLAL").val() +
        '",CLAA: "' + $("#TxtCLAA").val() +
        '",Peso: "' + $("#TxtPeso").val() +
        '",Remonte: "' + $("#TxtRemonte").val() +
        '",Fumigacao: "' + $("#TxtFumigacao").val() +
        '",Fragil: "' + flagFragil +
        '",Madeira: "' + flagMadeira +
        '",IMO1: "' + $("#TxtIMO1").val() +
        '",IMO2: "' + $("#TxtIMO2").val() +
        '",IMO3: "' + $("#TxtIMO3").val() +
        '",IMO4: "' + $("#TxtIMO4").val() +
        '",UNO1: "' + $("#TxtUNO1").val() +
        '",UNO2: "' + $("#TxtUNO2").val() +
        '",UNO3: "' + $("#TxtUNO3").val() +
        '",UNO4: "' + $("#TxtUNO4").val() +
        '",CodigoEmbalagem: "' + $("#DcEmbalagem").val() +
        '",CodigoNF: "' + $("#lblCodigoNF").val() +
        '",NumNF: "' + $("#TxtNF").val() + '",CodigoCarga: "' + $("#txtCodigoCarga").val() + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: GravarTalieItemOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtNF"));
        }
    });
}

function GravarTalieItemOnSuccess(response) {

    var retorno = response.d;

    if (retorno !== null) {
        if (retorno > 0) {

            $("#TxtPeso").val('');
            $("#TxtQtde").val('');
            $("#lblCodigoItem").val('');
            $("#CheckBox2").attr('checked', false);
            $("#CheckBox3").attr('checked', false);

            $("#TxtNF").attr("disabled", true);
            $("#BtNovo").attr("disabled", true);
            $("#BtExcluir").attr("disabled", true);
            $("#btLimpar").attr("disabled", true);

            PopularDescarga();

            alert('Registro cadastrado/alterado!');            

            if ($("#lblFimNota").val().toLowerCase() == 'true') {
                $("#TxtNF").focus();
            } else {
                $("#TxtQtde").focus();
            }

        } else {
            ExibirModal(retorno, $("#TxtNF"));
            $("#BtVoltar").removeAttr('disabled');
        }
    }

}

// ============ Excluir item do Talie

function ExcluirTalieItem() {

    $.ajax({
        type: "POST",
        url: "TalieDescargaItens.aspx/ExcluirAsync",
        data: '{CodigoTalie: "' + $("#lblCodigoRegCS").val() + '",CodigoItem: "' + $("#lblCodigoItem").val() + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#imgCarregando").show();
        },
        success: ExcluirTalieItemOnSuccess,
        failure: function (response) {
            ExibirModal(response.d, $("#TxtNF"));
        }
    });
}

function ExcluirTalieItemOnSuccess(response) {

    var retorno = response.d;
    $("#imgCarregando").hide();

    if (!isNull(retorno)) {
        if (stringEmpty(retorno)) {
            PopularDescarga();
            ExibirModal('Item excluído com sucesso!', $("#TxtQtde"));
            Limpar(true);
        } else {
            ExibirModal(retorno, $("#TxtNF"));
        }
    }

}

function setCookie(name, value, expires, path, theDomain, secure) {
    value = escape(value);
    if (value != '') {
        var theCookie = name + "=" + value +
            ((expires) ? "; expires=" + expires.toGMTString() : "") +
            ((path) ? "; path=" + path : "") +
            ((theDomain) ? "; domain=" + theDomain : "") +
            ((secure) ? "; secure" : "");
        document.cookie = theCookie;
    }
}

function getCookie(Name) {
    var search = Name + "="
    if (document.cookie.length > 0) {
        var offset = document.cookie.indexOf(search)
        if (offset != -1) {
            offset += search.length

            var end = document.cookie.indexOf(";", offset)

            if (end == -1) end = document.cookie.length
            return unescape(document.cookie.substring(offset, end))
        }
    }
}

function delCookie(name, path, domain) {
    if (getCookie(name)) document.cookie = name + "=" +
        ((path) ? ";path=" + path : "") +
        ((domain) ? ";domain=" + domain : "") +
        ";expires=Thu, 01-Jan-70 00:00:01 GMT";
}

function ExibirModal(texto, controle) {

    alert(texto);

    if (typeof controle !== "undefined" || controle !== null) {
        controle.focus();
    }

}

function Novo() {

    $("#BtNovo").attr("disabled", true);
    $("#BtExcluir").attr("disabled", true);
    $("#BtCancelar").removeAttr('disabled');
    $("#BtVoltar").removeAttr('disabled');
    $("#BtGravar").removeAttr('disabled');
    $("#btLimpar").attr("disabled", true);
    $("#BtBuscaNF").removeAttr('disabled');
    $("#TxtNF").removeAttr('disabled');
    $("#TxtNF").val('');
    $("#TxtNF").focus();

}

function Limpar(limparNf) {

    $("#TxtPeso").val('');
    $("#TxtQtde").val('');
    $("#TxtRemonte").val('');
    $("#TxtFumigacao").val('');
    $("#TxtCLAA").val('');
    $("#TxtCLAC").val('');
    $("#TxtCLAL").val('');
    $("#TxtUNO1").val('');
    $("#TxtUNO2").val('');
    $("#TxtUNO3").val('');
    $("#TxtUNO4").val('');
    $("#TxtIMO1").val('');
    $("#TxtIMO2").val('');
    $("#TxtIMO3").val('');
    $("#TxtIMO4").val('');
    $("#txtLocal").val('');
    $("#TxtCodEmb").val('');
    $("#lblCodigoItem").val('');
    $("#DcEmbalagem").val('');
    $("#TXTSALDO").val('0 / 0');
    $("#CheckBox2").attr('checked', false);
    $("#CheckBox3").attr('checked', false);

    $("#BtNovo").removeAttr('disabled');
    $("#BtExcluir").attr("disabled", true);
    $("#BtCancelar").attr("disabled", true);
    $("#BtVoltar").removeAttr('disabled');
    $("#BtGravar").attr("disabled", true);
    $("#btLimpar").attr("disabled", true);
    $("#BtBuscaNF").removeAttr('disabled');
    $("#BtAbreItem").removeAttr('disabled');

    if (limparNf) {
        $("#TxtNF").val('');
        $("#TxtNF").removeAttr('disabled');
        $("#TxtNF").focus();
    } else {
        $("#BtGravar").removeAttr('disabled');
        $("#BtCancelar").removeAttr('disabled');
        $("#TxtQtde").focus();
    }

}

function GravarCookies() {

    var cookieToday = new Date();
    var expiryDate = new Date(cookieToday.getTime() + (1 * 86400000));

    setarValoresCampos();

    var cookiesTela = {
        CTINF: nf.value,
        CTIQtde: qtde.value,
        CTIPeso: peso.value,
        CTILocal: local.value,
        CTISaldo: saldo.value,
        CTICodEmb: codEmbalagem.value,
        CTIEmb: embalagem.value,
        CTICLAC: clac.value,
        CTICLAL: clal.value,
        CTICLAA: claa.value,
        CTIIMO1: imo1.value,
        CTIIMO2: imo2.value,
        CTIIMO3: imo3.value,
        CTIIMO4: imo4.value,
        CTIUNO1: uno1.value,
        CTIUNO2: uno2.value,
        CTIUNO3: uno3.value,
        CTIUNO4: uno4.value,
        CTIRemonte: remonte.value,
        CTIFumigacao: fumigacao.value,
        CTIPesoBruto: pesoBruto.value,
        CTIQuantidade: quantidade.value,
        CTICodRegCS: codigoRegCs.value,
        CTICodTalie: codigoTalie.value,
        CTICodNF: codigoNf.value,
        CTICodBooking: codigoBooking.value,
        CTICodItem: codigoItem.value,
        CTIFimNota: fimNota.value,
        CTICodPatio: codigoPatio.value,
        CTICodRegistro: codigoRegistro.value,
        CTIFlMadeira: $("#CheckBox2").is(':checked'),
        CTIFlFragil: $("#CheckBox3").is(':checked')
    };

    delCookie("ColTalieItens");
    setCookie("ColTalieItens", JSON.stringify(cookiesTela), expiryDate, "/");

}