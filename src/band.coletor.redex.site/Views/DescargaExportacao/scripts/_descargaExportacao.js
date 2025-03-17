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

    // Habilitar/desabilitar os outros botões com base no campo "registro"
    const condicaoHabilitar = registro.trim() !== "" && registro > 0;

    atualizarEstadoDosBotoes("lnkAvarias", condicaoHabilitar);
    atualizarEstadoDosBotoes("lnkExcluir", condicaoHabilitar);
    atualizarEstadoDosBotoes("lnkGravar", condicaoHabilitar);
    atualizarEstadoDosBotoes("lnkObservacao", condicaoHabilitar);
    atualizarEstadoDosBotoes("lnkFinalizar", condicaoHabilitar);

    // Verifica se há algum checkbox selecionado
    const isAnyChecked = document.querySelectorAll('.checkbox-item:checked').length > 0;

    // Referência ao botão Marcantes
    const btnMarcantes = document.getElementById("lnkMarcantes");

    if (btnMarcantes) {
        // Habilita ou desabilita o botão adicionando ou removendo a classe "disabled"
        if (isAnyChecked) {
            btnMarcantes.classList.remove("disabled");
        } else {
            btnMarcantes.classList.add("disabled");
        }
    }
}

// Referência ao botão Marcantes
const btnMarcantes = document.getElementById("lnkMarcantes");

// Adiciona o evento "change" de forma delegada ao contêiner da tabela
document.getElementById('tabelaItens').addEventListener('change', function (e) {
    // Verifica se o evento foi disparado por um checkbox   
    if (e.target && e.target.classList.contains('checkbox-item')) {
        verificarCondicaoParaProximo(); // Chama a função ao detectar mudança
    }
});

// Adiciona eventos de mudança para todos os checkboxes
document.querySelectorAll('.checkbox-item').forEach(checkbox => {
    console.log(checkbox);
    checkbox.addEventListener('change', verificarCondicaoParaProximo);
});

document.addEventListener('DOMContentLoaded', function () {
    const tabelaBody = document.querySelector('#tabelaItens tbody'); // Seleciona o <tbody>

    if (tabelaBody) {
        tabelaBody.addEventListener('click', function (event) {
            const target = event.target;
            const tr = target.closest('tr'); // Captura a linha mais próxima

            if (tr) {
                const checkbox = tr.querySelector('.checkbox-item'); // Captura o checkbox dentro da linha

                // Verifica se o clique foi na linha ou no checkbox
                if (checkbox && (target.type === 'checkbox' || target.closest('tr'))) {
                    // Alterna o estado do checkbox (desmarca se já estiver marcado)
                    const isChecked = checkbox.checked;

                    // Desmarca todos os checkboxes primeiro
                    document.querySelectorAll('.checkbox-item').forEach(cb => cb.checked = false);

                    // Marca ou desmarca o checkbox clicado com base no estado anterior
                    checkbox.checked = !isChecked;

                    // Atualiza o estado do botão "Marcantes"
                    verificarCondicaoParaProximo();
                }
            }
        });
    }
});





function abrirModalMarcantes() {
    // Captura o ID selecionado
    const selectedCheckbox = document.querySelector('.checkbox-item:checked');
    if (selectedCheckbox) {
        const selectedId = selectedCheckbox.getAttribute('data-id');

        // Define o ID selecionado no campo hidden do modal
        document.getElementById('selectedId').value = selectedId;
        console.log(selectedId);

        // Abre o modal
        $('#modalMarcantes').modal('show');
    } else {
        alert('Por favor, selecione uma linha para continuar.');
    }
}

function buscarMarcante(marcante) {
    // Verifica se o campo "marcante" está vazio
    if (!marcante.trim()) {
        console.log('Campo marcante vazio.');
        return;
    }

    // Faz a requisição ao backend
    fetch(`/DescargaExportacao/BuscarMarcante?marcante=${encodeURIComponent(marcante)}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Erro ao buscar marcante.');
            }
            return response.json(); // Converte a resposta para JSON
        })
        .then(data => {
            // Manipula os dados retornados pelo backend
            if (data) {
                console.log('Dados da marcante:', data);
                // Atualiza os campos do formulário com os dados retornados
                document.getElementById('quantidade').value = data.quantidade || '';
                document.getElementById('local').value = data.local || '';
            } else {
                console.log('Nenhum dado encontrado para o marcante.');
            }
        })
        .catch(error => {
            console.error('Erro ao buscar marcante:', error);
        });
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


