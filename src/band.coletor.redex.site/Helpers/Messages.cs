using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Band.Coletor.Redex.Site.Helpers
{
    public class Messages
    {
        public string inserirSucess() => "Os dados foram inseridos com sucesso";
        public string inserirFailed() => "Os dados não foram inseridos";
        public string updateSucess() => "Os dados foram atualizados com sucesso";
        public string updateFailed() => "Os dados não foram atualizados";
        public string deleteSucess() => "Dados excluídos com sucesso";
        public string deleteFailed() => "Os dados não foram excluídos";
        public string emptyInput(string nome) => "O campo " + nome + " não pode estar vazio";
        public string emptySelect(string nome) => "Selecione o campo " + nome;
        public string emptyIndisponivel() => "Quantidade indisponível";
        public string armazemCT() => "Armazem CT não informe posição.";
        public string posicInput() => "Posição não informada";
        public string posicInvalid() => "Posição inválida";
        public string posicBloq() => "Posição Bloqueada";
        public string conflitoArmazem(string armazem) => "Conflito com Regra de Armazem : " + armazem;
        public string localNaoInformado() => "Local não informado";
        public string EqualsYard() => "Origem e Destino iguais";
        public string motivoMovimento() => "Informe o motivo do movimento !";
        public string loadDataFailed() => "Erro ao carregar os dados";
        public string wrongCntr() => "Conteiner informado em formato incorreto";
        public string noCntr() => "Nenhum Conteiner encontrado neste patio com este final";
        public string noCntr2() => "Conteiner não encontrado";
        public string moreCntr() => "Mais de um Conteiner encontrado neste patio com este final. Informe a sigla completa";
        public string notFound() => "Não foram encontrados dados";
        public string YardnotFound() => "Posição não encontrada/inválida";
        public string noCarga() => "Sem carga vinculada ao marcante";
        //public string NoAuthorize() => "Sem autorização para prosseguir";
        public string NoAuthorize() => "Sem autorização de saída";

        public string NocntrName() => "Preencha o numero do container ou sigla";

    }
}