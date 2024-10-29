using Ecoporto.Coletor.Service.DAO;
using Ecoporto.Coletor.Service.Enums;
using Ecoporto.Coletor.Service.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Services;

namespace Ecoporto.Coletor.Service
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class Upload : System.Web.Services.WebService
    {
        private readonly UploadDAO uploadDAO = new UploadDAO();

        private void SalvarImagem(string imagemBase64, string arquivo)
        {
            byte[] imageBytes = Convert.FromBase64String(imagemBase64);

            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                image.Save(arquivo);
            }
        }

        [WebMethod]
        public UploadResult EnviarImagem(UploadRequest uploadRequest)
        {
            try
            {
                if (uploadRequest.TipoArquivo == TipoArquivo.DESCARGA)
                {
                    SalvarImagem(uploadRequest.ImagemBase64, $@"{ConfigurationManager.AppSettings["DiretorioImagensDescargas"].ToString()}\{uploadRequest.Arquivo}");
                }

                if (uploadRequest.TipoArquivo == TipoArquivo.AVARIA)
                {
                    SalvarImagem(uploadRequest.ImagemBase64, $@"{ConfigurationManager.AppSettings["DiretorioImagensAvarias"].ToString()}\{uploadRequest.Arquivo}");
                }

                if (uploadRequest.TipoArquivo == TipoArquivo.LACRE)
                {
                    SalvarImagem(uploadRequest.ImagemBase64, $@"{ConfigurationManager.AppSettings["DiretorioImagensLacres"].ToString()}\{uploadRequest.Arquivo}");
                }

                var id = uploadDAO.Cadastrar(uploadRequest);

                return new UploadResult
                {
                    Sucesso = true,
                    Id = id,
                    Arquivo = uploadRequest.Arquivo,
                    DataInclusao = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    Mensagem = "Imagem gravada com sucesso"
                };
            }
            catch (Exception ex)
            {
                return new UploadResult
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                };
            }
        }

        [WebMethod]
        public UploadResult ExcluirImagem(int id, TipoArquivo tipoArquivo)
        {
            try
            {
                var imagem = uploadDAO.ObterImagemPorId(id);

                if (imagem != null)
                {
                    string caminho = string.Empty;

                    if (tipoArquivo == TipoArquivo.DESCARGA)
                    {
                        caminho = $@"{ConfigurationManager.AppSettings["DiretorioImagensDescargas"].ToString()}\{imagem.Arquivo}";
                    }

                    if (tipoArquivo == TipoArquivo.AVARIA)
                    {
                        caminho = $@"{ConfigurationManager.AppSettings["DiretorioImagensAvarias"].ToString()}\{imagem.Arquivo}";
                    }

                    if (tipoArquivo == TipoArquivo.LACRE)
                    {
                        caminho = $@"{ConfigurationManager.AppSettings["DiretorioImagensLacres"].ToString()}\{imagem.Arquivo}";
                    }

                    if (File.Exists(caminho))
                    {
                        File.Delete(caminho);

                        uploadDAO.Excluir(id);
                    }
                }

                return new UploadResult
                {
                    Sucesso = true,
                    Arquivo = imagem.Arquivo,
                    Mensagem = "Imagem excluída com sucesso"
                };
            }
            catch (Exception ex)
            {
                return new UploadResult
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                };
            }
        }

        [WebMethod]
        public List<UploadResult> ObterImagens(int processoId, TipoArquivo tipoArquivo)
        {
            var lista = new List<UploadResult>();

            var uploads = uploadDAO.ObterImagens(processoId, tipoArquivo);

            string diretorio = string.Empty;

            if (tipoArquivo == TipoArquivo.DESCARGA)
            {
                diretorio = ConfigurationManager.AppSettings["DiretorioImagensDescargas"].ToString();
            }

            if (tipoArquivo == TipoArquivo.AVARIA)
            {
                diretorio = ConfigurationManager.AppSettings["DiretorioImagensAvarias"].ToString();
            }

            if (tipoArquivo == TipoArquivo.LACRE)
            {
                diretorio = ConfigurationManager.AppSettings["DiretorioImagensLacres"].ToString();
            }

            foreach (var upload in uploads)
            {
                var arquivo = Directory
                    .GetFiles(diretorio)
                    .Where(c => c.Contains(upload.Arquivo))
                    .FirstOrDefault();

                if (arquivo == null)
                    continue;

                Byte[] bytes = File.ReadAllBytes(arquivo);
                String imagemBase64 = Convert.ToBase64String(bytes);

                lista.Add(new UploadResult
                {
                    Sucesso = true,
                    Id = upload.Id,
                    Arquivo = upload.Arquivo,
                    Tipo = upload.Tipo,
                    DataInclusao = upload.DataInclusao,
                    ImagemBase64 = imagemBase64
                });
            }

            return lista;
        }
    }
}
