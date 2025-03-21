﻿using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Models;
using System;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface ITalieRepositorio
    {
       int ObterConferentes(int idConferente);

        IEnumerable<Models.Equipe> ObterEquipes();

        IEnumerable<Operacao> ObterOperacoes();

        Talie ObterDadosTaliePorRegistro(int registro);

        Talie ObterDadosRegistro(int registroId);

        int ObterCodigoConferentePorLogin(int usuarioId);

        int CadastrarTalie(Talie talie);

        void AtualizarTalie(Talie talie);

        Talie ObterTaliePorId(int id);

        bool ExisteCargaCadastrada(int id);

        int ObterQuantidadeDescarga(int id);

        int ObterQuantidadeAssociada(int talieId, int bookingId);

        int ObterQuantidadeRegistro(int id);

        bool ObrigatorioDescargaYard();

        bool ExistemItensSemPosicao(int id);

        bool ExistemEtiquetas(int id);

        void GerarAlertaEtiqueta(int id, int alerta);

        bool ExistemEtiquetasComPendencia(int id);

        bool ExistemMarcantesComPendencia(int id);

        int ObterPesoBruto(int id);

        void ExcluirTalie(int id);

        void FinalizarTalie(int id, DateTime inicio, decimal pesoBruto, int bookingId);

        TalieItem ObterItemNF(int registroId, string nf);

        decimal ObterQuantidadeNF(int registroId, string notaFiscal);

        string ObterEmbalagemPorSigla(string sigla);

        int CadastrarItemTalie(TalieItem item, BrowserInfo browserInfo);

        void AtualizarItemTalie(TalieItem item);

        void ExcluirTalieItem(int id);

        ResumoQuantidadeDescarga ObterResumoQuantidadeDescarga(int talieId, string nf);

        TalieItem ObterItemTaliePorId(int id);

        IEnumerable<TalieItem> ObterItens(int talieId);

        IEnumerable<NotaFiscalDTO> ObterBalancoTalie(int registroId);

        bool IMOValido(string imo);

        bool UNOValido(string uno);

        IEnumerable<Armazem> ObterArmazens(int patioId);

        IEnumerable<Armazem> ObterDetalhesArmazem(int armazemId);

        Marcante ObterMarcantePorId(int id);

        void GravarMarcante(Marcante marcante);

        bool ArmazemColetor(int id);

        Armazem ObterPosicaoPatio(int armazemId, string yard);

        IEnumerable<Marcante> ObterMarcantes(int bookingId, int talieId);

        void ExcluirMarcante(int id);

        IEnumerable<NF> ObterNFs(int talieId);

        IEnumerable<ItemNF> ObterItensPorNF(int nfId);

        IEnumerable<Produto> ObterProdutosPorNF(int nfId);

        IEnumerable<Embalagem> ObterEmbalagensPorNF(int nfId);

        NF ObterNFPorId(int nfId);

        int ObterRegCSIdPorEmbalagem(int idEmbalagem, int idNF);

        IEnumerable<TalieConteiner> ObterTaliesConteinersPorId(int id);

        Talie ObterDadosTaliePorId(int idTalie);

        int ObterProximoIdPatioCS();
        int ObterProximoIdCargaSoltaYard();

    }
}