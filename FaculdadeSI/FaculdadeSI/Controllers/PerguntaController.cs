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

        public ActionResult Created()
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
                //Cria uma lista do que foi passado no dorpdown
                var listaTipoRespostaRequest = form["TipoResposta"].Split(',').ToList();
                
                //Se a paergunta tiver menos que 5 opçoes de resposta, será cadastrada como inativa
                if(listaTipoRespostaRequest.Count < 5)
                {
                    pergunta.PerguntaStatus = false;
                }
                //pergunta.TipoResposta.Add(ViewBag.TipoResposta);
                db.Perguntas.Add(pergunta);

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
                return RedirectToAction("Created");
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
            //var listaPerguntaTipoRespostasDb = db.PerguntaTipoRespostas.ToList().FindAll(f => f.IdPergunta == id);

            //Lista de TipoResposta que pertencem a pergunta
            //var listaTipoRespostaJoin = db.PerguntaTipoRespostas
            //     .Join(db.TipoRespostas, j => j.IdtipoResposta, k => k.IdTipoResposta, (j, k) => new { j, k }).Where(x => x.j.IdPergunta == id)
            //     .Join(db.Perguntas, a => a.j.IdPergunta, b => b.IdPergunta, (a, b) => new { a, b }).Select(s => new SelectListItem { Value = s.a.j.IdtipoResposta.ToString(), Text = s.a.k.DescricaoTipoResposta });

            //ViewBag.TipoResposta = new SelectList(db.TipoRespostas.ToList().Where(x => x.TipoRespostaStatus == true), "IdTipoResposta", "DescricaoTipoResposta");
            ViewBag.TipoResposta = new SelectList(db.TipoRespostas.ToList().Where(x => x.TipoRespostaStatus == true).Select( Y => Y.DescricaoTipoResposta));

            //Lista dos Tipo de resposta que existem no banco
            //var doisjoin2 = db.TipoRespostas.Select(s => new SelectListItem { Value = s.IdTipoResposta.ToString(), Text = s.DescricaoTipoResposta });

            //ViewBag.TipoResposta = new SelectList(db.TipoRespostas.ToList().Select(g => g.DescricaoTipoResposta));
            //ViewBag.TipoRespostaFiltrado = listaTipoRespostaJoin.ToList();
            //ViewBag.TipoResposta= doisjoin2.ToList(); 

            Pergunta pergunta = db.Perguntas.Find(id);
            if (pergunta == null)
            {
                return HttpNotFound();
            }

            return View(pergunta);
        }

        // POST: Pergunta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPergunta,DescricaoPergunta,PerguntaStatus")] Pergunta pergunta, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pergunta).State = EntityState.Modified;

                //Lista dos Tipo de resposta que existem no banco
                var listaTipoRespostaBd = db.TipoRespostas.ToList();

                //Cria uma lista do que foi passado no dropdown
                var listaTipoRespostaRequest = form["TipoResposta"].Split(',').ToList();

                if (listaTipoRespostaRequest.Count < 5)
                {
                    pergunta.PerguntaStatus = false;
                }

                //Apaga da tabela associativa os registros referentes aquela pergunta
                foreach (var item in db.PerguntaTipoRespostas.Where(f => f.IdPergunta == pergunta.IdPergunta))
                {
                    db.PerguntaTipoRespostas.Remove(item);
                }

                //Inclui os novos registros na tabela associativa
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
