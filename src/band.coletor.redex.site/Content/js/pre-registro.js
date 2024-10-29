
$(document).ready(function () {

    $(".inteiro").on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

    $(".moeda").mask('#.##0,00', { reverse: true });

    $(".numero").mask('##0,00', { reverse: true });

    $('.cpf').mask('000.000.000-00');

    $('.cnpj').mask('00.000.000/0000-00');

    $('.cep').mask('00000-000');

    $('.telefone').mask('(00) 0000-0000');

    $('.celular').mask('(00) 00000-0000');

    $('.data').mask('00/00/0000');

    $('.data-hora').mask('00/00/0000 00:00');

    $('.placa').mask('SSS-9A99');
});