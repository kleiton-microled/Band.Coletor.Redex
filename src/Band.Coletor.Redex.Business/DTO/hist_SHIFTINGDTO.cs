using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.DTO
{
    public class hist_SHIFTINGDTO
    {
        public int AUTONUMHS { get; set; }
        public int USUARIO { get; set; }
        public DateTime DT { get; set; }
        public string BROWSER_NAME { get; set; }
        public string BROWSER_VERSION { get; set; }
        public string MOBILEDEVICEMODEL { get; set; }
        public string MOBILEDEVICEMANUFACTURER { get; set; }
        public string FLAG_MOBILE { get; set; }
    }
    public class HIST_SHIFTING_CS
    {
        public int AUTONUM { get; set; }
        public int MARCANTE { get; set; }
        public int ARMAZEM { get; set; }
        public string YARD { get; set; }
        public DateTime DT_MOV { get; set; }
        public int AUDIT_MOV { get; set; }
        public int USUARIO { get; set; }
        public string ORIGEM { get; set; }
        public string LOTE { get; set; }
        public int MOTIVO { get; set; }
        public int QTDE { get; set; }
        public string LOCAL_POS { get; set; }
    }
}
