const formModel = {

};


//function CarregarRegistro() {
//    var registro = $("#registro").val();

//    $.ajax({
//        url: '/DescargaExportacao/CarregarRegistro',
//        type: 'GET',
//        data: { codigoRegistro: registro },
//        success: function (response) {
//            // Preencher os campos do formulário com os dados retornados
//            formModel.codigoRegistro = response?.CodigoRegistro || '';
//            formModel.inicio = formatarDataParaInput(response?.Talie?.Inicio) || '';
//            document.getElementById('inicio').value = formModel.inicio;

//            formModel.termino = formatarDataParaInput(response?.Talie?.Termino) || '';
//            formModel.equipe = response?.Talie?.Equipe || '';
//            formModel.conferente = response?.Talie?.Conferente || '';
//            formModel.operacao = response?.Talie?.Operacao || '';
//            formModel.placa = response?.Placa || '';
//            formModel.reserva = response?.Reserva || '';
//            formModel.codigoTalie = response?.Talie?.Id || '';
//            formModel.cliente = response?.Cliente || '';

//            document.getElementById('observacaoInput').value = response?.Talie?.Observacao;

//            setComboBoxValue("cbOperacao", response?.Talie?.Operacao);

//            preencherFormularioComModel();
//            verificarCondicaoParaProximo();

//            //popularTabelaItens(response.Talie?.TalieItem);
//        },
//        error: function (xhr, status, error) {
//            console.error("Erro ao obter dados do Talie:", error);
//            alert("Ocorreu um erro ao buscar os dados. Verifique o console para mais detalhes.");
//        }
//    });
//}
//function CarregarRegistro() {
//    // Verifica se o elemento "registro" existe
//    console.log("Função CarregarRegistro foi chamada!");
//    const registroInput = document.getElementById('registro');
//    if (!registroInput) {
//        console.error("Elemento #registro não encontrado.");
//        return;
//    }

//    const registro = registroInput.value;

//    $.ajax({
//        url: '/DescargaExportacao/CarregarRegistro',
//        type: 'GET',
//        data: { codigoRegistro: registro },
//        success: function (response) {
//            console.log("Função CarregarRegistro foi chamada 2!");
//            // Verifica e define os valores apenas se os elementos existirem
//            if (response) {
//                if (document.getElementById('inicio')) {
//                    formModel.inicio = formatarDataParaInput(response?.Talie?.Inicio) || '';
//                    document.getElementById('inicio').value = formModel.inicio;
//                }

//                if (document.getElementById('termino')) {
//                    formModel.termino = formatarDataParaInput(response?.Talie?.Termino) || '';
//                    document.getElementById('termino').value = formModel.termino;
//                }

//                if (document.getElementById('observacaoInput')) {
//                    document.getElementById('observacaoInput').value = response?.Talie?.Observacao || '';
//                }

//                setComboBoxValue("cbOperacao", response?.Talie?.Operacao);

//                preencherFormularioComModel();
//                verificarCondicaoParaProximo();
//            } else {
//                console.warn("Nenhuma resposta retornada da API.");
//            }
//        },
//        error: function (xhr, status, error) {
//            console.error("Erro ao obter dados do Talie:", error);
//            alert("Ocorreu um erro ao buscar os dados. Verifique o console para mais detalhes.");
//        }
//    });
//}





// Função auxiliar para setar o valor no combo
function setComboBoxValue(selectId, value) {
    const selectElement = document.getElementById(selectId);

    // Verifica se o elemento existe
    if (!selectElement) {
        console.error(`Elemento com ID "${selectId}" não encontrado.`);
        return;
    }

    // Verifica se o valor não é nulo ou indefinido
    if (value === null || value === undefined) {
        console.warn(`Valor para o combo "${selectId}" é nulo ou indefinido.`);
        return;
    }

    // Converta o valor para string para garantir a correspondência
    const valueToSet = value.toString();

    // Verifique se o valor existe nas opções do combo
    const optionExists = Array.from(selectElement.options).some(
        (option) => option.value == valueToSet
    );

    if (optionExists) {
        selectElement.value = valueToSet;
        console.log(`Valor "${valueToSet}" foi setado no combo "${selectId}".`);
    } else {
        console.warn(`Valor "${valueToSet}" não encontrado no combo com ID "${selectId}".`);
    }
}

function preencherFormularioComModel() {
    // Itera sobre as propriedades do modelo e preenche os campos do formulário
    for (const key in formModel) {
        if (formModel.hasOwnProperty(key)) {
            const elemento = document.querySelector(`[name="${key}"]`); // Seleciona o campo pelo atributo name

            if (elemento) {
                if (elemento.tagName === 'SELECT') {
                    // Para campos de combo box (select), define o valor somente se a opção existir
                    const optionExists = Array.from(elemento.options).some(option => option.value === formModel[key]);
                    if (optionExists) {
                        elemento.value = formModel[key];
                    } else {
                        elemento.value = ''; // Define o valor padrão caso o valor não exista
                    }
                } else {
                    // Para outros campos, apenas define o valor
                    elemento.value = formModel[key] || '';
                }
            }
        }
    }
}

// Atualiza o modelo com base nas alterações no formulário
function updateFormModel(event) {
    const { name, value } = event.target;
    if (name in formModel) {
        formModel[name] = value; // Atualiza o modelo
    }
    //displayFormModel(); // Exibe o modelo atualizado
    console.log('Modelo: ', formModel)
}


// Adiciona eventos aos campos do formulário
document.getElementById('formTalie').addEventListener('input', updateFormModel);

// Função para validar e enviar os dados do formulário
document.getElementById('lnkGravar').addEventListener('click', function (event) {
    event.preventDefault();
});

function atualizarEstadoDosBotoes(botaoId, habilitar) {
    const botao = document.getElementById(botaoId);
    if (botao) {
        botao.disabled = !habilitar; // Desabilita o botão se habilitar for false
        botao.classList.toggle("disabled", !habilitar); // Adiciona ou remove a classe 'disabled' para controle visual
    }
}

function verificarCondicaoParaProximo() {
    const registro = document.getElementById("registro").value;

    // Habilita o botão "Próximo" se o campo "registro" estiver preenchido
    atualizarEstadoDosBotoes("lnkMarcantes", registro.trim() !== "" && registro > 0);
    atualizarEstadoDosBotoes("lnkAvarias", registro.trim() !== "" && registro > 0);
    atualizarEstadoDosBotoes("lnkExcluir", registro.trim() !== "" && registro > 0);
    atualizarEstadoDosBotoes("lnkGravar", registro.trim() !== "" && registro > 0);
    atualizarEstadoDosBotoes("lnkObservacao", registro.trim() !== "" && registro > 0);
    atualizarEstadoDosBotoes("lnkFinalizar", registro.trim() !== "" && registro > 0);
}

document.addEventListener("DOMContentLoaded", function () {
    verificarCondicaoParaProximo();
    document.getElementById("talie").addEventListener("input", verificarCondicaoParaProximo);
});


// GRAVAR TALIE
document.getElementById('lnkGravar').addEventListener('click', function (e) {
    e.preventDefault(); // Evita a navegação padrão

    // Envia os dados para o servidor via AJAX
    const gravarTalieUrl = document.getElementById('lnkGravar').dataset.url;
    $.ajax({
        url: gravarTalieUrl,
        type: 'POST',
        data: formModel, // Não precisa de JSON.stringify, pois é um objeto padrão
        beforeSend: function () {
            $('#preloader').show();
        },
        success: function (response) {
            $('#preloader').hide();
            if (response.sucesso) {
                Swal.fire({
                    icon: 'success',
                    title: 'Sucesso!',
                    text: response.mensagem || "Dados gravados com sucesso!",
                    timer: 2000,
                    showConfirmButton: false
                }).then(() => {
                    CarregarRegistro(); // Atualiza ou redireciona conforme necessário
                });
            } else {
                Swal.fire({
                    icon: 'info',
                    title: 'Atenção!',
                    text: response.mensagem || "Erro ao gravar os dados.",
                    showConfirmButton: true
                });
            }
        },
        error: function () {
            $('#preloader').hide();
            Swal.fire({
                icon: 'error',
                title: 'Erro!',
                text: "Erro ao gravar os dados. Tente novamente.",
                showConfirmButton: true
            });
        }
    });
});

//Observacao
document.getElementById('salvarObservacao').addEventListener('click', function () {
    const observacao = document.getElementById('observacaoInput').value;
    const talie = 404843;

    if (!observacao.trim()) {
        Swal.fire({
            icon: 'warning',
            title: 'Atenção',
            text: 'Por favor, preencha a observação antes de salvar.',
            confirmButtonText: 'OK'
        });
        return;
    }

    // Chamada ao backend
    fetch('/DescargaExportacao/GravarObservacao', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ observacao, talie })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Erro ao salvar observação.');
            }
            return response.json();
        })
        .then(data => {
            Swal.fire({
                icon: 'success',
                title: 'Sucesso',
                text: data.mensagem || 'Observação salva com sucesso!',
                confirmButtonText: 'OK'
            }).then(() => {
                // Fecha o modal
                $('#observacaoModal').modal('hide');

                // Limpa o campo do modal
                document.getElementById('observacaoInput').value = '';
            });
        })
        .catch(error => {
            console.error('Erro ao salvar observação:', error);
            Swal.fire({
                icon: 'error',
                title: 'Erro',
                text: 'Ocorreu um erro ao salvar a observação. Tente novamente.',
                confirmButtonText: 'OK'
            });
        });
});


//UTILS
function formatarDataParaInput(data) {
    if (!data) return '';

    // Divida a data e a hora
    const [date, time] = data.split(' ');

    // Reorganize a data para o formato ISO (YYYY-MM-DD)
    const [day, month, year] = date.split('/');
    const formattedDate = `${year}-${month}-${day}`;

    // Combine a data e o horário
    return `${formattedDate}T${time}`;
}


