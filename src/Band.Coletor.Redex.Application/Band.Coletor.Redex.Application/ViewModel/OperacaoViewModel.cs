using System.Collections.Generic;

namespace Band.Coletor.Redex.Application.ViewModel
{
    public class OperacaoViewModel : BaseViewModel
    {
        public static List<OperacaoViewModel> Create()
        {
            var operacao = new List<OperacaoViewModel>();
            operacao.Add(new OperacaoViewModel() { Id = 1, Descricao = "Manual" });
            operacao.Add(new OperacaoViewModel() { Id = 1, Descricao = "Automatizada" });

            return operacao;
        }
    }
}
