using Band.Coletor.Redex.Site.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Band.Coletor.Redex.Site.Controllers
{
    public class DefaultController : BaseController
    {
        public class ResponseJson
        {
            public ResponseJson() { }
            public string statusRetorno { get; set; }

            public bool possuiDados { get; set; }

            public object objetoRetorno { get; set; }

            public string Mensagem { get; set; }
        }

        public ResponseJson retornoJson = new ResponseJson();

        public Messages messages = new Messages();

        public Codes codes = new Codes();

    }
}