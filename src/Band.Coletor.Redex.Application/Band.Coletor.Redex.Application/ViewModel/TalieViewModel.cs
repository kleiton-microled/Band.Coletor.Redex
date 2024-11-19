namespace Band.Coletor.Redex.Application.ViewModel
{
    public class TalieViewModel
    {
        public bool ExisteTalieAberto { get; set; }
        public int Registro { get; set; }
        public int CodigoTalie { get; set; }
        public int CodigoBooking { get; set; }
        public string Operacao { get; set; }
        public int Conferente { get; set; }
        public int Equipe { get; set; }
        public int Camera { get; set; }
        public string Placa { get; set; }
        public string Reserva { get; set; }
        public int CodigoGate { get; set; }
        public int? CodigoRegistro { get; set; }
        public string Cliente { get; set; }
        public string Inicio { get; set; }
        public string Termino { get; set; }
        public string StatusTalie { get; set; }
        public string Observacao { get; set; }
        public string Inconsistencia { get; set; }

        public static TalieViewModel CreateNew(bool existeTalieAberto, int registro, int codigoTalie, int codigoBooking, string operacao,
                                                int conferente, int equipe, int camera, string placa, string reserva, int codigoGate, int codigoRegistro,
                                                string cliente, string inicio, string termino, string statusTalie, string observacao)
        {
            var talie = new TalieViewModel();
            talie.ExisteTalieAberto = existeTalieAberto;
            talie.Registro = registro;
            talie.CodigoTalie = codigoTalie;
            talie.CodigoBooking = codigoBooking;
            if (string.IsNullOrEmpty(operacao))
            {
                talie.Operacao = "AUTOMATIZADA";
            }
            else
            {
                if (operacao == "M")
                {
                    talie.Operacao = "MANUAL";
                }
                else
                {
                    talie.Operacao = "AUTOMATIZADA";
                }
            }
            talie.Operacao = operacao;
            talie.Conferente = conferente;
            talie.Equipe = equipe;
            talie.Camera = camera;
            talie.Placa = placa;
            talie.Reserva = reserva;
            talie.CodigoGate = codigoGate;
            talie.CodigoRegistro = codigoRegistro;
            talie.Cliente = cliente;
            talie.Inicio = inicio;
            talie.Termino = termino;
            talie.StatusTalie = statusTalie;
            talie.Observacao = observacao;

            return talie;
        }

        
    }
}
