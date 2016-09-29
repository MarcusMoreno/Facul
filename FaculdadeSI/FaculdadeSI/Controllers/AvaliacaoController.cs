using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FaculdadeSI.Models;

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
            ViewBag.Perguntas = new SelectList(db.Perguntas.ToList().Where(f => f.PerguntaStatus == true).Select(g => g.DescricaoPergunta));
            ViewBag.IdPerfil = new SelectList(db.Perfils.ToList().Where(f => f.PerfilStatus == true).Select(g => g.DescricaoPerfil));
            ViewBag.IdUsuario = new SelectList(db.Usuarios.ToList().Where(f => f.UsuarioStatus == true).Select(g => g.Nome));
               
            return View();
        }

        // POST: Avaliacao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAvaliacao,DescricaoAvaliacao,Expiracao,Titulo,IdUsuario,IdPerfil,AvaliacaoStatus")] Avaliacao avaliacao)
        {
            if (ModelState.IsValid)
            {
                db.Avaliacaos.Add(avaliacao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Perguntas = new SelectList(db.Perguntas.ToList().Where(f => f.PerguntaStatus == true).Select(g => g.DescricaoPergunta));
            ViewBag.IdPerfil = new SelectList(db.Perfils.ToList().Where(f => f.PerfilStatus == true).Select(g => g.DescricaoPerfil));
            ViewBag.IdUsuario = new SelectList(db.Usuarios.ToList().Where(f => f.UsuarioStatus == true).Select(g => g.Nome));

            return View(avaliacao);
        }


        // GET: Avaliacao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Cria lista de AvaliacaoPerguntas que tem o mesmo id enviado
            var listaAvaliacaoPerguntasDb = db.AvaliacaoPerguntas.ToList().FindAll(f => f.IdAvaliacao == id);

            //Lista de perguntas que pertencem a avaliacao
            //var listaPerguntasJoin = db.AvaliacaoPerguntas
            //    .Join(db.Perguntas, x => x.IdPergunta, y => y.IdPergunta, (x, y) => new { x, y }).Where(z => z.x.IdAvaliacao == id)
            //    .Join(db.Avaliacaos, w => w.x.IdAvaliacao, z => z.IdAvaliacao, (w, z) => new { w, z }).Select(s => new SelectListItem { Value = s.w.x.IdPergunta.ToString(), Text = s.w.y.DescricaoPergunta});

            Avaliacao avaliacao = db.Avaliacaos.Find(id);
            if (avaliacao == null)
            {
                return HttpNotFound();
            }
        
            ViewBag.Perguntas = new SelectList(db.Perguntas.ToList().Where(f => f.PerguntaStatus == true).Select(g => g.DescricaoPergunta));
            ViewBag.IdPerfil = new SelectList(db.Perfils.ToList().Where(f => f.PerfilStatus == true).Select(g => g.DescricaoPerfil));
            ViewBag.IdUsuario = new SelectList(db.Usuarios.ToList().Where(f => f.UsuarioStatus == true).Select(g => g.Nome));

            return View(avaliacao);
        }

        // POST: Avaliacao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAvaliacao,DescricaoAvaliacao,Expiracao,Titulo,IdUsuario,IdPerfil,AvaliacaoStatus")] Avaliacao avaliacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(avaliacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPerfil = new SelectList(db.Perfils.ToList().Where(f => f.PerfilStatus == true).Select(g => g.DescricaoPerfil));
            ViewBag.IdUsuario = new SelectList(db.Usuarios.ToList().Where(f => f.UsuarioStatus == true).Select(g => g.Nome));
            
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
    }
}
