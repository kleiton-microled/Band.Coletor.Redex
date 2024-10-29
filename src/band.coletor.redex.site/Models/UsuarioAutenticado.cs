namespace Band.Coletor.Redex.Site.Models
{
    public class UsuarioAutenticado
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Ativo { get; set; }
        public string PatioColetorId { get; set; }
        public string LocalPatio { get; set; }
    }
}