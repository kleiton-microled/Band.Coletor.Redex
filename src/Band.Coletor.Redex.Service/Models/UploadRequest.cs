using Ecoporto.Coletor.Service.Enums;

namespace Ecoporto.Coletor.Service.Models
{
    public class UploadRequest
    {
        public int ProcessoId { get; set; }
        
        public string Arquivo { get; set; }

        public TipoArquivo TipoArquivo { get; set; }

        public int UsuarioId { get; set; }

        public string ImagemBase64 { get; set; }

        public string IdConteiner { get; set; }
    }
}