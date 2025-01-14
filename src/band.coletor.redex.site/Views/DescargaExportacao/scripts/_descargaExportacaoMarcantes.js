$(document).ready(function () {
    $.ajax({
        url: '/DescargaExportacao/GetArmazens',
        type: 'GET',
        success: function (data) {
            let $select = $('#cbArmazen');
            $select.empty();
            $select.append($('<option>', { value: '', text: 'Selecione um armazém' })); 

            // Adiciona os armazéns ao combo
            $.each(data, function (index, item) {
                $select.append($('<option>', { value: item.Id, text: item.Descricao }));
            });

            // Ativa o combobox caso estivesse desabilitado
            $select.prop('disabled', false);
        },
        error: function (xhr, status, error) {
            console.error('Erro ao carregar os armazéns: ', error);
        }
    });
});
