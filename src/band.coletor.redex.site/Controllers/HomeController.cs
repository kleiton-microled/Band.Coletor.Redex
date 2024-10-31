using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Enums;
using Band.Coletor.Redex.Business.Extensions;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Site.Helpers;
using Band.Coletor.Redex.Site.Models;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Band.Coletor.Redex.Site.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUsuarioLoginRepositorio _usuarioLoginRepositorio;

        public HomeController(
           IUsuarioLoginRepositorio usuarioLoginRepositorio) => _usuarioLoginRepositorio = usuarioLoginRepositorio;

        [HttpGet]
        public ActionResult Index()
            => View();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(Index));

            return View(new UsuarioLoginViewModel
            {
                LocalPatio = LocalPatio.Patio,
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UsuarioLoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                // Mock de usuário
                var usuario = new
                {
                    Id = 1,
                    Nome = "Kleiton",
                    Login = viewModel.Login.Trim(),
                    Ativo = true,
                    PatioColetorId = 1
                };

                if (usuario.Ativo == false)
                    throw new Exception($"O usuário {usuario.Login} está inativo");

                var usuarioJson = JsonConvert.SerializeObject(new
                {
                    usuario.Id,
                    usuario.Nome,
                    usuario.Login,
                    usuario.Ativo,
                    usuario.PatioColetorId,
                    LocalPatio = viewModel.LocalPatio.ToValue()
                });

                FormsAuthentication.SignOut();

                Session["USUARIO"] = usuario.Id;
                Session["AUTONUMPATIO"] = viewModel.LocalPatio.ToValue();

                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    usuario.Login,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    viewModel.Lembrar,
                    usuarioJson);

                Response.Cookies.Add(
                    new HttpCookie(
                        FormsAuthentication.FormsCookieName,
                        FormsAuthentication.Encrypt(authTicket)));

                if (!string.IsNullOrEmpty(viewModel.ReturnUrl))
                    return Redirect(Server.UrlDecode(viewModel.ReturnUrl));

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = ex.Message;
            }

            return View(viewModel);
        }

        //public ActionResult Login(UsuarioLoginViewModel viewModel)
        //{
        //    if (!ModelState.IsValid)
        //        return View(viewModel);

        //    if (string.IsNullOrEmpty(viewModel.Senha))
        //    {
        //        ModelState.AddModelError("Senha", "A senha é obrigatória.");
        //        return View(viewModel);
        //    }

        //    try
        //    {
        //        var usuario = _usuarioLoginRepositorio.Busca(viewModel.Login.Trim(), viewModel.Senha);

        //        if (usuario != null)
        //        {
        //            var usuarioJson = JsonConvert.SerializeObject(new
        //            {
        //                usuario.Id,
        //                usuario.Nome,
        //                usuario.Login,
        //                usuario.Ativo,
        //                usuario.PatioColetorId,
        //                LocalPatio = viewModel.LocalPatio.ToValue()
        //            });

        //            FormsAuthentication.SignOut();

        //            Session["USUARIO"] = usuario.Id;
        //            Session["AUTONUMPATIO"] = viewModel.LocalPatio.ToValue();

        //            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
        //                1,
        //                usuario.Login,
        //                DateTime.Now,
        //                DateTime.Now.AddMinutes(30),
        //                viewModel.Lembrar,
        //                usuarioJson);

        //            Response.Cookies.Add(
        //               new HttpCookie(
        //                   FormsAuthentication.FormsCookieName,
        //                   FormsAuthentication.Encrypt(authTicket)));

        //            if (!string.IsNullOrEmpty(viewModel.ReturnUrl))
        //                return Redirect(Server.UrlDecode(viewModel.ReturnUrl));

        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, ex.Message);  // Adiciona a mensagem de erro ao ModelState
        //    }

        //    return View(viewModel);
        //}


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            HttpCookie aCookie;

            string cookieName;
            int limit = Request.Cookies.Count;
            if (limit > 0)
            {
                for (int i = 0; i < limit; i++)
                {
                    cookieName = Request.Cookies[i].Name;
                    aCookie = new HttpCookie(cookieName);
                    if (aCookie != null)
                    {
                        aCookie.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(aCookie);
                    }
                }

            }

            return RedirectToAction("Index", "Home");
        }
    }
}