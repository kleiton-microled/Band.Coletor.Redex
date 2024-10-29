function AutoLoad(){
    $(document).ready(function () {
        ConfigurarTextBox();
    });
}


$(function () {
    $(".Calendario").datepicker({
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior'
    });
});

function ConfigurarTextBox() {

    // Aceitar somente letras
    $(".Alpha").alpha();

    // Aceitar somente letras e números
    $(".AlphaNumeric").alphanumeric();

    // Mascara para CNPJ
    $(".CNPJ").mask("99.999.999/9999-99");

    // Mascara para Container
    $(".Container").mask("aaaa999999-9");

    //Placa
    $(".Placa").mask("aaa-9999");

    // Mascara para CPF
    $(".CPF").mask("999.999.999-99");

    $(".Date").mask("99/99/9999");

    // Aceitar somente números
    $(".Numeric").numeric();
    $(".Decimal").decimal();

    // Mascara para Telefone
    $(".Telefone").mask("(99) 9999-9999");

    $(".Moeda").maskMoney({ decimal: ",", thousands: "." });

    $(".Documento").mask("9999999999/9");

}

//$(document).ready(function () {
//    $('input:text').focus(
//    function () {
//        $(this).css({ 'background-color': '#FFFFEE' });
//    });

//    $('input:text').blur(
//    function () {
//        $(this).css({ 'background-color': '#FFFFFF' });
//    });

//   

//});

