using System.Web.Optimization;

namespace Band.Coletor.Redex.Site
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Content/js/jquery-3.3.1.js",
                         "~/Content/js/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-url")
               .Include("~/Content/plugins/jquery-url/jquery-url.js"));

            bundles.Add(new ScriptBundle("~/bundles/site")
               .Include("~/Content/plugins/easyAutocomplete/jquery.easy-autocomplete.js",
                        "~/Content/js/default.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-validate")
                .Include("~/Content/plugins/jquery-validate/jquery.validate.min.js",
                         "~/Content/plugins/jquery-validate/language/messages_pt_BR.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-mask")
                .Include("~/Content/plugins/jquery-mask/jquery.mask.js"));

            bundles.Add(new StyleBundle("~/bundles/css")
                .Include(
                    "~/Content/bootstrap.css",
                    "~/Content/css/estilos.css",
                    "~/Content/css/fontawesome-all.css",
                    "~/Content/plugins/toastr/toastr.css",
                    "~/Content/plugins/easyAutocomplete/easy-autocomplete.css",
                    "~/Content/css/pre-loader.css"
                    ));

            bundles.Add(new StyleBundle("~/bundles/login")
               .Include("~/Content/bootstrap.css",
                        "~/Content/css/login.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Content/js/popper.js",
                         "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/bundles/datatablesCSS")
                .Include("~/Content/plugins/datatables/css/dataTables.bootstrap4.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatables")
                .Include("~/Content/plugins/datatables/js/jquery.dataTables.min.js",
                         "~/Content/plugins/datatables/js/dataTables.bootstrap4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor")
               .Include("~/Content/plugins/ckeditor/ckeditor.js",
                        "~/Content/plugins/ckeditor/translations/pt-br.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastr")
                .Include("~/Content/plugins/toastr/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment")
              .Include("~/Content/plugins/moment/moment-with-locales.js"));

            bundles.Add(new StyleBundle("~/bundles/tagsCSS")
               .Include("~/Content/plugins/tags/tagsinput.css"));

            bundles.Add(new ScriptBundle("~/bundles/tags")
                .Include("~/Content/plugins/tags/tagsinput.js"));

            bundles.Add(new ScriptBundle("~/bundles/quaggaJS")
               .Include("~/Content/js/leitorCodigoDeBarras/JOB.js",
                        "~/Content/js/leitorCodigoDeBarras/Leitura.js"));

            // Bundle para o script descarga_exportacao
            bundles.Add(new ScriptBundle("~/bundles/descargaExportacao").Include(
                       "~/Views/DescargaExportacao/scripts/_descargaExportacao.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}