namespace Ecoporto.Coletor.Service.Models
{
    public class UploadResult
    {
        public int Id { get; set; }

        public bool Sucesso { get; set; }

        public string Arquivo { get; set; }

        public string Mensagem { get; set; }

        public string ImagemBase64 { get; set; }

        public int Tipo { get; set; }

        public string DataInclusao { get; set; }
    }
}