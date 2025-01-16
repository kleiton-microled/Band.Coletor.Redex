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

//Listar Descargas
document.addEventListener("DOMContentLoaded", function () {
    const talieId = 404843; 

    // Função para carregar os itens
    function carregarDescargas(talie) {
        fetch(`/DescargaExportacao/CarregarDescarga?talie=${talie}`)
            .then(response => response.json())
            .then(data => {
                const selectElement = document.getElementById("cbItensDescarregados");

                if (selectElement) {
                    // Limpa as opções atuais
                    selectElement.innerHTML = "";

                    // Adiciona a opção padrão
                    const defaultOption = document.createElement("option");
                    defaultOption.value = "";
                    defaultOption.textContent = "Selecione um item";
                    selectElement.appendChild(defaultOption);

                    // Preenche o select com os dados retornados
                    data.forEach(item => {
                        const option = document.createElement("option");
                        option.value = item.CodigoItem; 
                        option.textContent = item.Descricao;
                        selectElement.appendChild(option);
                    });
                }
            })
            .catch(error => {
                console.error("Erro ao carregar descargas:", error);
                alert("Erro ao carregar itens descarregados. Verifique o console para mais detalhes.");
            });
    }

    // Chama a função ao carregar a página
    carregarDescargas(talieId);
});

