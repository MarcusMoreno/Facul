using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FaculdadeSI.Models;
using System.Net.Mail;

namespace FaculdadeSI.Controllers
{
    public class AvaliacaoController : Controller
    {
        private ReviewEntities db = new ReviewEntities();

        // GET: Avaliacao
        public ActionResult Index()
        {
            var avaliacaos = db.Avaliacaos.Include(a => a.Perfil).Include(a => a.Usuario);
            return View(avaliacaos.ToList());
        }

        // GET: Avaliacao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacaos.Find(id);
            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            return View(avaliacao);
        }

        // GET: Avaliacao/Create
        public ActionResult Create()
        {
            ViewBag.Perguntas = new SelectList(db.Perguntas.ToList().Where(x => x.PerguntaStatus == true).Select(g => g.DescricaoPergunta));
            ViewBag.IdPerfil = new SelectList(db.Perfils, "IdPerfil", "DescricaoPerfil");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nome");

            return View();
        }

        // POST: Avaliacao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Avaliacao avaliacao, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                //Cria uma lista do que foi passado no dorpdown
                var listaPerguntasRequest = form["Perguntas"].Split(',').ToList();

                if (listaPerguntasRequest.Count < 5)
                {
                    avaliacao.AvaliacaoStatus = false;
                }

                db.Avaliacaos.Add(avaliacao);

                //Lista das perguntas que existem no banco
                var listaPerguntasBd = db.Perguntas.ToList();

                //Para cada pergunta enviada na avaliacao, inseri na tabela AvaliacaòPergunta
                foreach (var item in listaPerguntasRequest)
                {
                    //PEga objeto no db.TipoResposta que seja igual a item
                    var descPergunta = listaPerguntasBd.FirstOrDefault(f => f.DescricaoPergunta == item);

                    AvaliacaoPergunta avaliacaoPergunta = new AvaliacaoPergunta();
                    avaliacaoPergunta.IdAvaliacao = avaliacao.IdAvaliacao;
                    avaliacaoPergunta.IdPergunta = descPergunta.IdPergunta;

                    //Adiciona no banco
                    db.AvaliacaoPerguntas.Add(avaliacaoPergunta);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(avaliacao);
        }


        // GET: Avaliacao/Edit/{IdAvaliacao}
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Perguntas = new SelectList(db.Perguntas.ToList().Where(x => x.PerguntaStatus == true).Select(g => g.DescricaoPergunta));
            ViewBag.IdPerfil = new SelectList(db.Perfils, "IdPerfil", "DescricaoPerfil");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nome");

            //Lista de perguntas que pertencem a avaliacao
            //var listaPerguntasJoin = db.AvaliacaoPerguntas
            //    .Join(db.Perguntas, x => x.IdPergunta, y => y.IdPergunta, (x, y) => new { x, y }).Where(z => z.x.IdAvaliacao == id)
            //    .Join(db.Avaliacaos, w => w.x.IdAvaliacao, z => z.IdAvaliacao, (w, z) => new { w, z }).Select(s => new SelectListItem { Value = s.w.x.IdPergunta.ToString(), Text = s.w.y.DescricaoPergunta});

            Avaliacao avaliacao = db.Avaliacaos.Find(id);
            if (avaliacao == null)
            {
                return HttpNotFound();
            }

            return View(avaliacao);
        }


        public ActionResult Edit2(int idPerfil, int idAvaliacao)
        {
            if (idPerfil == null  || idAvaliacao < 0) //Mudar
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Envia emails
            var t = SendedEmails(idPerfil, idAvaliacao);

            //List<string> listaEmails = db.Usuarios.Where(x => x.IdPerfil == idPerfil).Select(y => y.Email).ToList();

            List<string> lista56 = new List<string>();

            foreach( var item in db.Usuarios.Where(x => x.IdPerfil == idPerfil) )
            {
                lista56.Add(item.Email);
            }
            
            ViewBag.Emails = new SelectList(lista56);

            //ViewBag.Perguntas = new SelectList(db.Perguntas.ToList().Where(x => x.PerguntaStatus == true).Select(g => g.DescricaoPergunta));
            //ViewBag.IdPerfil = new SelectList(db.Perfils, "IdPerfil", "DescricaoPerfil");
            //ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nome");

            Avaliacao avaliacao = db.Avaliacaos.Find(idAvaliacao);
            if (avaliacao == null)
            {
                return HttpNotFound();
            }

            return View(avaliacao);
        }

        // POST: Avaliacao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAvaliacao,DescricaoAvaliacao,Expiracao,Titulo,IdUsuario,IdPerfil,AvaliacaoStatus")] Avaliacao avaliacao, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(avaliacao).State = EntityState.Modified;

                //Cria uma lista do que foi passado no dropdown
                var listaPerguntaRequest = form["Perguntas"].Split(',').ToList();

                if (listaPerguntaRequest.Count < 5)
                {
                    avaliacao.AvaliacaoStatus = false;
                }

                //Lista das perguntas que existem no banco
                var listaPerguntasBd = db.Perguntas.ToList();

                //Apaga da tabela associativa os registros referentes aquela avaliacao
                foreach (var item in db.AvaliacaoPerguntas.Where(f => f.IdAvaliacao == avaliacao.IdAvaliacao))
                {
                    db.AvaliacaoPerguntas.Remove(item);
                }

                //Inclui os novos registros na tabela associativa
                //Para cada tipo de pergunta enviada na avaliação, inseri na tabela AvaliacaoPergunta
                foreach (var item in listaPerguntaRequest)
                {
                    //PEga objeto no db.pergunta que seja igual a item
                    var pergunta = listaPerguntasBd.FirstOrDefault(f => f.DescricaoPergunta == item);

                    AvaliacaoPergunta avaliacaoPergunta = new AvaliacaoPergunta();
                    avaliacaoPergunta.IdAvaliacao = avaliacao.IdAvaliacao;
                    avaliacaoPergunta.IdPergunta = pergunta.IdPergunta;

                    //Adiciona no banco
                    db.AvaliacaoPerguntas.Add(avaliacaoPergunta);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(avaliacao);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public bool SendedEmails(int idPerfil, int idAvaliacao)
        {
            //Cria lista de e-mails
            List<string> emails = new List<string>();

            //Adiciona todos email dos usuarios que possuam aquele perfil
            foreach (var item in db.Usuarios.Where(f => f.IdPerfil == idPerfil))
            {
                emails.Add(item.Email);
            }

            var response = Models.Email.SendedEmail(emails);
            return response;
        }

    }
}
