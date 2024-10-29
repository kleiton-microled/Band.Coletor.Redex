using System.Text;

namespace Band.Coletor.Redex.Business.Helpers
{
    public class HtmlBuilder
    {
        private readonly StringBuilder _body;

        public HtmlBuilder()
        {
            _body = new StringBuilder();
        }

        public HtmlBuilder AbreHtml()
        {
            this._body.AppendLine($@"<html>");
            return this;
        }

        public HtmlBuilder FechaHtml()
        {
            this._body.AppendLine("</html>");
            return this;
        }

        public HtmlBuilder AbreHead()
        {
            this._body.AppendLine("<head>");
            return this;
        }

        public HtmlBuilder FechaHead()
        {
            this._body.AppendLine("</head>");
            return this;
        }

        public HtmlBuilder Titulo(string titulo)
        {
            this._body.AppendLine($"<title>{titulo}</title>");
            return this;
        }

        public HtmlBuilder AbreTabela(int borda, string estilo)
        {
            this._body.AppendLine($"<table class='tbPreview' border='{borda}' style='{estilo}'>");
            return this;
        }

        public HtmlBuilder FechaTabela()
        {
            this._body.AppendLine("</table>");
            return this;
        }

        public HtmlBuilder AbreLinha(int id)
        {
            this._body.AppendLine($"<tr id='item-{id}' data-id={id}>");
            return this;
        }

        public HtmlBuilder AbreLinha(string estilo, int id)
        {
            this._body.AppendLine($"<tr style='{estilo}' id=item-{id} data-id={id}>");
            return this;
        }

        public HtmlBuilder FechaLinha()
        {
            this._body.AppendLine("</tr>");
            return this;
        }

        public HtmlBuilder AbreColuna(int colspan)
        {
            this._body.AppendLine($"<td colspan='{colspan}'>");
            return this;
        }

        public HtmlBuilder FechaColuna()
        {
            this._body.AppendLine("</td>");
            return this;
        }

        public HtmlBuilder CriarColuna(string estilo, string conteudo)
        {
            this._body.AppendLine($"<td style='{estilo}'>{conteudo}</td>");
            return this;
        }

        public HtmlBuilder CriarColuna(int colspan, string estilo, string conteudo)
        {
            this._body.AppendLine($"<td colspan='{colspan}' style='{estilo}'>{conteudo}</td>");
            return this;
        }

        public HtmlBuilder AdicionaParagrafo(string estilo, string texto)
        {
            this._body.AppendLine($"<p style='{estilo}'>{texto}</p>");
            return this;
        }

        public string Compilar()
        {
            return _body.ToString();
        }
    }
}