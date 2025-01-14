const formModel = {

};


function CarregarRegistro() {
    var registro = $("#registro").val();

    $.ajax({
        url: '/DescargaExportacao/CarregarRegistro',
        type: 'GET',
        data: { codigoRegistro: registro },
        success: function (response) {
            // Preencher os campos do formulário com os dados retornados
            formModel.codigoRegistro = response.CodigoRegistro || '';
            formModel.inicio = formatarDataParaInput(response.Inicio) || '';
            formModel.termino = formatarDataParaInput(response.Termino) || '';
            formModel.equipe = response.Equipe || '';
            formModel.conferente = response.Conferente || '';
            formModel.operacao = response.Operacao || '';
            formModel.placa = response.Placa || '';
            formModel.reserva = response.Reserva || '';
            formModel.codigoTalie = response.Talie || '';
            formModel.cliente = response.Cliente || '';
            formModel.statusTali = response.StatusTalie || '';
            
            // Setar os valores nos combos
            setSelectValue("cbEquipe", response.Equipe);
            setSelectValue("cbConferente", response.Conferente);
            setSelectValue("cbOperacao", response.Operacao);

            preencherFormularioComModel();
            verificarCondicaoParaProximo();
        },
        error: function (xhr, status, error) {
            console.error("Erro ao obter dados do Talie:", error);
            alert("Ocorreu um erro ao buscar os dados. Verifique o console para mais detalhes.");
        }
    });
}

// Função auxiliar para setar o valor no combo
function setSelectValue(selectId, value) {
    const selectElement = document.getElementById(selectId);

    if (!selectElement) {
        console.error(`Elemento com ID "${selectId}" não encontrado.`);
        return;
    }

    // Tenta localizar a opção correspondente
    let optionFound = false;
    for (let i = 0; i < selectElement.options.length; i++) {
        if (selectElement.options[i].value === value.toString()) {
            selectElement.options[i].selected = true;
            optionFound = true;
            break;
        }
    }

    if (!optionFound) {
        console.warn(`Valor "${value}" não encontrado no combo com ID "${selectId}".`);
    }
}


//
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
    atualizarEstadoDosBotoes("lnkProximo", registro.trim() !== "");
    atualizarEstadoDosBotoes("lnkMarcantes", registro.trim() !== "");
    atualizarEstadoDosBotoes("lnkAvarias", registro.trim() !== "");
    atualizarEstadoDosBotoes("lnkExcluir", registro.trim() !== "");
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
function formatarDataParaInput(dateString) {
    if (!dateString) return '';

    const date = new Date(dateString);
    if (isNaN(date)) return '';

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Adiciona 0 se necessário
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    return `${year}-${month}-${day}T${hours}:${minutes}`;
}

document.getElementById('salvarObservacao').addEventListener('click', function () {
    const observacao = document.getElementById('observacaoInput').value;

    if (!observacao.trim()) {
        alert('Por favor, preencha a observação antes de salvar.');
        return;
    }

    // Aqui você pode salvar a observação no servidor ou processá-la como necessário.
    console.log('Observação salva:', observacao);

    // Fecha o modal
    $('#observacaoModal').modal('hide');

    // Limpa o campo do modal
    document.getElementById('observacaoInput').value = '';
});



