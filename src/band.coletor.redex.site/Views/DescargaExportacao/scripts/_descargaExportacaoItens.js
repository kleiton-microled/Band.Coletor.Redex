//BUSCAR NOTA FISCAL ITEM
document.getElementById('btnBuscarNotaFiscal').addEventListener('click', function (e) {
    e.preventDefault(); // Evita o comportamento padrão do botão

    // Obtenha o valor do campo de número da nota fiscal
    const numeroNotaFiscal = document.getElementById('numeroNotaFiscal').value;

    // Valide se o número da nota fiscal foi informado
    if (!numeroNotaFiscal) {
        alert('Preencha o número da nota fiscal antes de buscar.');
        return;
    }

    // URL do método no controlador
    const buscarNotaFiscalUrl = document.getElementById('btnBuscarNotaFiscal').dataset.url;

    // Chamada AJAX
    $.ajax({
        url: buscarNotaFiscalUrl,
        type: 'POST',
        data: {
            numeroNotaFiscal: numeroNotaFiscal,
            codigoBooking: codigoBooking, // Passando o valor de TempData
            codigoRegistro: codigoRegistro // Passando o valor de TempData
        },
        beforeSend: function () {
            $('#preloader').show(); // Exibe o preloader, se necessário
        },
        success: function (response) {
            $('#preloader').hide();
            if (response.sucesso) {
                alert('Nota fiscal encontrada!');
                // Processar a resposta
            } else {
                alert(response.mensagem || 'Erro ao buscar a nota fiscal.');
            }
        },
        error: function () {
            $('#preloader').hide(); // Esconde o preloader em caso de erro
            alert('Erro ao realizar a requisição. Tente novamente.');
        }
    });
});
