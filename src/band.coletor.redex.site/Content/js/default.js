var links = $('.navbar ul li a');

$.each(links, function (key, va) {
    if (va.href == document.URL) {
        $(this).addClass('active');
    }
});

$(".moeda").mask('#.##0,00', {
    reverse: true,
    allowNegative: false,
    thousands: '.',
    decimal: ',',
    affixesStay: false,
    clearIfNotMatch: true
});

$(".data").mask('00/00/0000');
$(".data-hora").mask('00/00/0000 00:00');
$(".placa").mask('SSS-9A99');
$('.telefone').mask('(00) 0000-0000');
$('.celular').mask('(00) 00000-0000');
$('.cep').mask('00000-000');
$('.cpf').mask('000.000.000-00');
$('.cnpj').mask('00.000.000/0000-00');
$('.protocolo').mask('00000000');

$(".inteiro").on("keypress keyup blur", function (event) {
    $(this).val($(this).val().replace(/[^\d].+/, ""));
    if ((event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});

var isNumero = function (numero) {
    return !isNaN(numero - parseFloat(numero));
}

var isInteiro = function (numero) {
    return /^\d+$/.test(numero);
}

var isMoeda = function (numero) {

    var valor = numero
        .replace(/\./g, '')
        .replace(',', '.');

    return !isNaN(valor - parseFloat(valor));
}

var formataMoedaCalculo = function (numero) {

    return numero
        .replace(/\./g, '')
        .replace(',', '.');
}

var formataMoedaPtBr = function (numero) {

    var numero = numero.toFixed(2).split('.');
    numero[0] = numero[0].split(/(?=(?:...)*$)/).join('.');
    return numero.join(',');
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-top-center",
    "preventDuplicates": true,
    "onclick": null,
    "showDuration": "800",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

$("input[type=text]").keyup(function () {

    var start = $(this)[0].selectionStart;
    var end = $(this)[0].selectionEnd;

    $(this).val($(this).val().toUpperCase());
    $(this).selectRange(start, end);
});

$.fn.selectRange = function (start, end) {
    $(this).each(function () {
        var el = $(this)[0];

        if (el) {
            el.focus();

            if (el.setSelectionRange) {
                el.setSelectionRange(start, end);

            } else if (el.createTextRange) {
                var range = el.createTextRange();
                range.collapse(true);
                range.moveEnd('character', end);
                range.moveStart('character', start);
                range.select();

            } else if (el.selectionStart) {
                el.selectionStart = start;
                el.selectionEnd = end;
            }
        }
    });
};

$(document).on('keyup keypress', 'form input', function (e) {
    if (e.keyCode === 13) {
        e.preventDefault();
        return false;
    }
});


var mensagemCadastroSucesso = function (data) {

    $('#msgErro')
        .addClass('invisivel');

    toastr.success('Registro salvo com sucesso!', 'Coletor REDEX');
};

var mensagemCadastroErro = function (xhr, status) {

    if (xhr.responseJSON != null) {

        toastr.error('Falha ao atualizar o Registro. Verifique mensagens.', 'Coletor REDEX');

        var msg = $('#msgErro');

        msg.html('');
        msg.removeClass('invisivel');

        var resultado = JSON.parse(xhr.responseText);

        var mensagens = resultado.erros.map(function (erro) {
            return '<li>' + erro.ErrorMessage + '</li>'
        });

        msg.html(mensagens);

    } else {
        if (xhr.statusText !== '')
            toastr.error(xhr.statusText, 'Coletor REDEX');
    }
};

function Spinner(btn) {

    if (btn !== null) {

        $('#' + btn.id)
            .html('<i class="fa fa-spinner fa-spin"></i> aguarde...')
            .addClass('disabled');
    }
}