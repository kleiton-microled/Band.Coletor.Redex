using System;

namespace Band.Coletor.Redex.Business.Models.Entities
{
    public class TalieEntity
    {
        public int AUTONUM_TALIE { get; set; }
        public int AUTONUM_PATIO { get; set; }
        public string PLACA { get; set; }
        public DateTime? INICIO { get; set; }
        public DateTime? TERMINO { get; set; }
        public bool FLAG_DESCARGA { get; set; }
        public bool FLAG_ESTUFAGEM { get; set; }
        public bool CROSSDOCKING { get; set; }
        public int CONFERENTE { get; set; }
        public int EQUIPE { get; set; }
        public int AUTONUM_BOO { get; set; }
        public bool FLAG_CARREGAMENTO { get; set; }
        public string FORMA_OPERACAO { get; set; }
        public int AUTONUM_GATE { get; set; }
        public bool FLAG_FECHADO { get; set; }
        public string OBS { get; set; }
        public int AUTONUM_RO { get; set; }
        public int AUDIT_225 { get; set; }
        public int ANO_TERMO { get; set; }
        public string TERMO { get; set; }
        public DateTime? DATA_TERMO { get; set; }
        public bool FLAG_PACOTES { get; set; }
        public bool ALERTA_ETIQUETA { get; set; }
        public int? AUTONUM_REG { get; set; }
        public bool FLAG_COMPLETO { get; set; }
        public bool EMAIL_ENVIADO { get; set; }
        public string REFERENCE { get; set; }
        public string FANTASIA { get; set; }
        public int ID_GEO_CAMERA { get; set; }

        public static TalieEntity CreateNew(DateTime? inicio, int conferente, int equipe, string operacao, string obs, int idGeoCamera, int autonumTalie)
        {
            TalieEntity entity = new TalieEntity();
            entity.INICIO = inicio;
            entity.CONFERENTE = conferente;
            entity.EQUIPE = equipe;
            entity.FORMA_OPERACAO = operacao;
            entity.OBS = obs;
            entity.ID_GEO_CAMERA = idGeoCamera;
            entity.AUTONUM_TALIE = autonumTalie;

            return entity;
        }

        public static TalieEntity InsertCommand(int autonumTalie, DateTime inicio, int conferente, int equipe, int autonumBooking, string operacao,
                                                string placa, int autonumGate, int? autonumRegistro, string obs)
        {
            var entity = new TalieEntity();
            entity.AUTONUM_TALIE = autonumTalie;
            entity.INICIO = inicio;
            entity.CONFERENTE = conferente;
            entity.EQUIPE = equipe;
            entity.AUTONUM_BOO = autonumBooking;
            entity.FORMA_OPERACAO = operacao;
            entity.PLACA = placa;
            entity.AUTONUM_GATE = autonumGate;
            entity.AUTONUM_REG = autonumRegistro;
            entity.OBS = obs;

            return entity;
        }

    }


}
