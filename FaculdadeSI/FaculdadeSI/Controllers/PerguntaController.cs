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
    public class PerguntaController : Controller
    {
        private ReviewEntities db = new ReviewEntities();

        // GET: Pergunta
        public ActionResult Index()
        {
            return View(db.Perguntas.ToList());

           
        }

        // GET: Pergunta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pergunta pergunta = db.Perguntas.Find(id);

            //Lista de TipoResposta que pertencem a pergunta
            var listTipoRespostaJoin = db.PerguntaTipoRespostas
                   .Join(db.TipoRespostas, j => j.IdtipoResposta, k => k.IdTipoResposta, (j, k) => new { j, k }).Where(x => x.j.IdPergunta == id)
                   .Join(db.Perguntas, a => a.j.IdPergunta, b => b.IdPergunta, (a, b) => new { a, b }).Select(s => new SelectListItem { Value = s.a.j.IdtipoResposta.ToString(), Text = s.a.k.DescricaoTipoResposta });

            //Todas os tipo de resposta
            ViewBag.TipoResposta = listTipoRespostaJoin.ToList();
            
            if (pergunta == null)
            {
                return HttpNotFound();
            }
            return View(pergunta);
        }

        // GET: Pergunta/Create
        public ActionResult Create()
        {
            ViewBag.TipoResposta = new SelectList(db.TipoRespostas.ToList().Where(x => x.TipoRespostaStatus == true).Select(g => g.DescricaoTipoResposta));
                    
            return View();
        }

        // POST: Pergunta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pergunta pergunta, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                //pergunta.TipoResposta.Add(ViewBag.TipoResposta);
                db.Perguntas.Add(pergunta);

                //Cria uma lista do que foi passado no dorpdown
                var listaTipoRespostaRequest = form["TipoResposta"].Split(',').ToList();

                //Lista dos Tipo de resposta que existem no banco
                var listaTipoRespostaBd = db.TipoRespostas.ToList();

                //Para cada tipo de resposta enviada na pergunta, inseri na tabela perguntaTipoResposta
                foreach (var item in listaTipoRespostaRequest)
                {
                    //PEga objeto no db.TipoResposta que seja igual a item
                    var descTipoResposta = listaTipoRespostaBd.FirstOrDefault(f => f.DescricaoTipoResposta == item);

                    PerguntaTipoResposta perguntaTipoResposta = new PerguntaTipoResposta();
                    perguntaTipoResposta.IdPergunta = pergunta.IdPergunta;
                    perguntaTipoResposta.IdtipoResposta = descTipoResposta.IdTipoResposta;

                    //Adiciona no banco
                    db.PerguntaTipoRespostas.Add(perguntaTipoResposta);
                }                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pergunta);
        }

        // POST: Pergunta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateList(string pergunta)
        {
            List<string> t = new List<string>();
            t.Add(pergunta);

           return View(pergunta);
        }


        // GET: Pergunta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Cria lista de perguntaTipoResposta que tem o mesmo id enviado
            var listaPerguntaTipoRespostasDb = db.PerguntaTipoRespostas.ToList().FindAll(f => f.IdPergunta == id);

            //Lista de TipoResposta que pertencem a pergunta
            var listaTipoRespostaJoin = db.PerguntaTipoRespostas
                 .Join(db.TipoRespostas, j => j.IdtipoResposta, k => k.IdTipoResposta, (j, k) => new { j, k }).Where(x => x.j.IdPergunta == id)
                 .Join(db.Perguntas, a => a.j.IdPergunta, b => b.IdPergunta, (a, b) => new { a, b }).Select(s => new SelectListItem { Value = s.a.j.IdtipoResposta.ToString(), Text = s.a.k.DescricaoTipoResposta });

            ViewBag.TipoResposta = new SelectList(db.TipoRespostas.ToList().Where(x => x.TipoRespostaStatus == true).Select(g => g.DescricaoTipoResposta));

            //Lista dos Tipo de resposta que existem no banco
            //var doisjoin2 = db.TipoRespostas.Select(s => new SelectListItem { Value = s.IdTipoResposta.ToString(), Text = s.DescricaoTipoResposta });

            //ViewBag.TipoResposta = new SelectList(db.TipoRespostas.ToList().Select(g => g.DescricaoTipoResposta));
            //ViewBag.TipoRespostaFiltrado = listaTipoRespostaJoin.ToList();
            //ViewBag.TipoResposta= doisjoin2.ToList(); 

            Pergunta pergunta = new Pergunta();
            pergunta.DescricaoPergunta = db.Perguntas.FirstOrDefault(f => f.IdPergunta == id).DescricaoPergunta;
            pergunta.PerguntaStatus = true;

            if (pergunta == null)
            {
                return HttpNotFound();
            }

            return View(pergunta);
        }

        // POST: Pergunta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPergunta,DescricaoPergunta,PerguntaStatus")] Pergunta pergunta, FormCollection form, int? id)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pergunta).State = EntityState.Modified;

                //Lista dos Tipo de resposta que existem no banco
                var listaTipoRespostaBd = db.TipoRespostas.ToList();

                //Cria uma lista do que foi passado no dorpdown
                var listaTipoRespostaRequest = form["TipoResposta"].Split(',').ToList();

                //Cria lista de descricaoTipoResposta que aquela pergunta possui cadastrada
                List<String> listaTipoRespostaAtPergunta = db.PerguntaTipoRespostas
                 .Join(db.TipoRespostas, j => j.IdtipoResposta, k => k.IdTipoResposta, (j, k) => new { j, k }).Where(x => x.j.IdPergunta == id)
                 .Join(db.Perguntas, a => a.j.IdPergunta, b => b.IdPergunta, (a, b) => new { a, b }).Select(hj => hj.a.k.DescricaoTipoResposta).ToList();

                foreach(var item in listaTipoRespostaRequest) 
                {
                    if (!listaTipoRespostaAtPergunta.Contains(item)) //Verifica se ja existe aquele tipoResposta naquela perguntaTipoResposta
                    {
                        //PEga objeto no db.TipoResposta que seja igual a item
                        var descTipoResposta = listaTipoRespostaBd.FirstOrDefault(f => f.DescricaoTipoResposta == item);

                        PerguntaTipoResposta perguntaTipoResposta = new PerguntaTipoResposta();
                        perguntaTipoResposta.IdPergunta = pergunta.IdPergunta;
                        perguntaTipoResposta.IdtipoResposta = descTipoResposta.IdTipoResposta;

                        db.PerguntaTipoRespostas.Add(perguntaTipoResposta);

                    }
                 
                }
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            return View(pergunta);
        }

        //// GET: Pergunta/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Pergunta pergunta = db.Perguntas.Find(id);
        //    if (pergunta == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(pergunta);
        //}

        //// POST: Pergunta/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Pergunta pergunta = db.Perguntas.Find(id);
        //    db.Perguntas.Remove(pergunta);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
