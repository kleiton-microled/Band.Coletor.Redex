const formModel = {
   
};


function obterDadosTalie() {
    var registro = $("#registro").val();

    $.ajax({
        url: '/DescargaExportacao/ObterDadosTaliePorRegistro',
        type: 'GET',
        data: { registro: registro },
        success: function(response) {
            // Preencher os campos do formulário com os dados retornados
            formModel.codigoRegistro = response.Registro || '';
            formModel.inicio = formatarDataParaInput(response.Inicio) || '';
            formModel.termino = formatarDataParaInput(response.Termino) || '';
            formModel.equipe = response.Equipe || '';
            formModel.conferente = response.Conferente || '';
            formModel.operacao = response.Operacao || '';
            formModel.placa = response.Placa || '';
            formModel.reserva = response.Reserva || '';
            formModel.codigoTalie = response.CodigoTalie || '';
            formModel.cliente = response.Cliente || '';
            //$("#codigoGate").val(response.CodigoGate);
            //$("#codigoBooking").val(response.CodigoBooking);
            //$("#inicio").val(response.Inicio);
            //$("#termino").val(response.Termino);
            //$("#observacao").val(response.Observacao);
            //$("#statusTalie").val(response.StatusTalie);
            //$("#conferente").val(response.Conferente);
            //$("#equipe").val(response.Equipe);
            //$("#camera").val(response.Camera);
            //$("#operacao").val(response.Operacao);
            preencherFormularioComModel();
            verificarCondicaoParaProximo();
        },
        error: function(xhr, status, error) {
            console.error("Erro ao obter dados do Talie:", error);
            alert("Ocorreu um erro ao buscar os dados. Verifique o console para mais detalhes.");
        }
    });
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

document.addEventListener("DOMContentLoaded", function() {
    verificarCondicaoParaProximo();
    document.getElementById("talie").addEventListener("input", verificarCondicaoParaProximo);
});


//GRAVAR TALIE
document.getElementById('lnkGravar').addEventListener('click', function (e) {
    e.preventDefault(); // Evita a navegação padrão

    // Validação simples antes de enviar
    //if (!formModel.registro || !formModel.inicio || !formModel.equipe || !formModel.conferente || !formModel.operacao) {
    //    alert("Preencha todos os campos obrigatórios.");
    //    return;
    //}

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
                alert("Dados gravados com sucesso!");
                location.reload(); // Atualiza ou redireciona conforme necessário
            } else {
                alert(response.mensagem || "Erro ao gravar os dados.");
            }
        },
        error: function () {
            $('#preloader').hide();
            alert("Erro ao gravar os dados. Tente novamente.");
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


