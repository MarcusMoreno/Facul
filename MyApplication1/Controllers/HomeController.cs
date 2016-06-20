using MyApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyApplication1.Controllers
{
    public class HomeController : Controller
    {

     
        public ActionResult Index()
        {
            if (Session["usuarioLogadoID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

#region Avaliacao  

        public ActionResult AvaliacaoHome()
        {
            ViewBag.Message = "AVALIACAO";

            return View();
        }

        public ActionResult AvaliacaoCreate()
        {
            ViewBag.Message = "AVALIACAO";

            return View();
        }

#endregion


#region usuario

        public ActionResult UsuarioHome()
        {
            ViewBag.Message = "Usuario";

            return View();
        }

        public ActionResult UsuarioCreate()
        {
            ViewBag.Message = "Usuario";

            return View();
        }

        #endregion



#region tipoResposta

        public ActionResult TipoRespostaHome()
        {
            ViewBag.Message = "Tipo resposta";

            return View();
        }

        public ActionResult TipoRespostaCreate()
        {
            ViewBag.Message = "Tipo resposta";

            return View();
        }

        #endregion


#region Pergunta
        public ActionResult PerguntaHome()
        {
            ViewBag.Message = "PERGUNTA";

            return View();
        }

        public ActionResult PerguntaCreate()
        {
            ViewBag.Message = "PERGUNTA";

            return View();
        }


        //Cria pergunta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PerguntaCreate(Pergunta p)
        {
            if (ModelState.IsValid) //verifica se é válido
            {
                using (Model1 dc = new Model1())
                {
                    if (p != null)
                    {
                        var t = p.DescricaoPergunta;
                        dc.Perguntas.Include(t);
                    }
                }
            }
            return View();
        }
        #endregion


#region Perfil
        public ActionResult PerfilHome()
        {
            return View();
        }

        public ActionResult PerfilCreate()
        {
            return View();
        }
        #endregion


#region Login
        public ActionResult Login()
        {
            return View();
        }

        //Faz login no sistema
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario u)
        {
            // esta action trata o post (login)
            if (ModelState.IsValid) //verifica se é válido
            {
                using (Model1 dc = new Model1())
                {
                    var v = dc.Usuarios.Where(a => a.Nome.Equals(u.Nome) && a.Senha.Equals(u.Senha)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["usuarioLogadoID"] = v.IdUsuario.ToString();
                        Session["nomeUsuarioLogado"] = v.Nome.ToString();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(u);
        }
        #endregion


        
       
    }
}