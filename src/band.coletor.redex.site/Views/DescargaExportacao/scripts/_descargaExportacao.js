function obterDadosTalie() {
    var registro = $("#registro").val();

    $.ajax({
        url: '/DescargaExportacao/ObterDadosTaliePorRegistro',
        type: 'GET',
        data: { registro: registro },
        success: function(response) {
            // Preencher os campos do formulário com os dados retornados
            $("#registro").val(response.Registro);
            $("#codigoTalie").val(response.CodigoTalie);
            $("#cliente").val(response.Cliente);
            $("#placa").val(response.Placa);
            $("#reserva").val(response.Reserva);
            $("#codigoGate").val(response.CodigoGate);
            $("#codigoBooking").val(response.CodigoBooking);
            $("#inicio").val(response.Inicio);
            $("#termino").val(response.Termino);
            $("#observacao").val(response.Observacao);
            $("#statusTalie").val(response.StatusTalie);
            $("#conferente").val(response.Conferente);
            $("#equipe").val(response.Equipe);
            $("#camera").val(response.Camera);
            $("#operacao").val(response.Operacao);

            console.log("Dados do Talie preenchidos no formulário.");
        },
        error: function(xhr, status, error) {
            console.error("Erro ao obter dados do Talie:", error);
            alert("Ocorreu um erro ao buscar os dados. Verifique o console para mais detalhes.");
        }
    });
}
