using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Band.Coletor.Redex.Site.Helpers
{
    public class Codes
    {
        public string Ok() => "200";
        public string Forbidden() => "403";
        public string BadRequest() => "500";
        public string emptyInput() => "501";
        public string notFound() => "404";

        public string retOK() => "OK";
        public string retAttOK() => "ATUALIZADO COM SUCESSO";

    }
}