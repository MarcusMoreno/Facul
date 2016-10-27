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
using System.Text;
using System.IO;
using System.Web.Helpers;


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

#region Create
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
                return RedirectToAction("Created");
            }

            return View(avaliacao);
        }

        public ActionResult Created()
        {
            var avaliacaos = db.Avaliacaos.Include(a => a.Perfil).Include(a => a.Usuario);
            return View(avaliacaos.ToList());
        }
#endregion 

#region Edit
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
                foreach (var item in db.AvaliacaoPerguntas.Where(d => d.IdAvaliacao == avaliacao.IdAvaliacao).ToList())
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


        #endregion

#region Answer

        public ActionResult AnswerSucess()
        {   
            return View();
        }

        public ActionResult AnswerNotId()
        {
            return View();
        }

        public ActionResult Answer(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Avaliacao avaliacao = db.Avaliacaos.Find(id);

            if(avaliacao != null)
            {
                //Lista de perugntas que pertencem a avaliacao
                var listPerguntasJoin = db.AvaliacaoPerguntas
                       .Join(db.Perguntas, j => j.IdPergunta, k => k.IdPergunta, (j, k) => new { j, k }).Where(x => x.j.IdAvaliacao == id)
                       .Join(db.Avaliacaos, a => a.j.IdAvaliacao, b => b.IdAvaliacao, (a, b) => new { a, b }).Select(s => new SelectListItem { Value = s.a.j.IdPergunta.ToString(), Text = s.a.k.DescricaoPergunta });

                //Avaliacao que vai ser retornada para a view
                Avaliacao avaliacaoCompleta = new Avaliacao();
                avaliacaoCompleta.IdAvaliacao = id;
                avaliacaoCompleta.Titulo = avaliacao.Titulo;
                avaliacaoCompleta.DescricaoAvaliacao = avaliacao.DescricaoAvaliacao;

                //Para cada pergunta da lista, pega suas opções de respostas
                foreach (var item in listPerguntasJoin)
                {
                    Pergunta pergunta = new Pergunta();
                    pergunta.DescricaoPergunta = item.Text;
                    pergunta.IdPergunta = Convert.ToInt32(item.Value);

                    foreach (var item2 in db.PerguntaTipoRespostas.Where(x => x.IdPergunta.ToString() == item.Value))
                    {
                        TipoResposta tipoResposta = new TipoResposta();

                        var tipoRespostaNovo = db.TipoRespostas.FirstOrDefault(x => x.IdTipoResposta == item2.IdtipoResposta);

                        tipoResposta.IdTipoResposta = tipoRespostaNovo.IdTipoResposta;
                        tipoResposta.DescricaoTipoResposta = tipoRespostaNovo.DescricaoTipoResposta;

                        pergunta.TipoRespostas.Add(tipoResposta);
                    }
                    avaliacaoCompleta.Pergunta.Add(pergunta);
                }

                if (avaliacaoCompleta == null)
                {
                    return HttpNotFound();
                }

                return View(avaliacaoCompleta);
            }
           
            return RedirectToAction("AnswerNotId");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Answer(List<string> pergunta, FormCollection form)
        {
            foreach(var item in pergunta)
            {
                var u = item.Split(',').ToList();
                var idPergunta = u[0];
                var idTipoResposta = u[1];
                var idAvaliacao = u[2];

                AvaliacaoResposta av1 = new AvaliacaoResposta();
                av1.IdAvaliacao = Convert.ToInt32(idAvaliacao);
                av1.IdUsuario = 1;
                av1.IdPergunta = Convert.ToInt32(idPergunta);
                av1.IdTipoResposta = Convert.ToInt32(idTipoResposta);

                db.AvaliacaoRespostas.Add(av1);
            }

               db.SaveChanges();
               return RedirectToAction("AnswerSucess");
          
        }

#endregion

#region Send
        public ActionResult Sent(int idPerfil, int idAvaliacao)
        {
            if (idPerfil < 0 || idAvaliacao < 0) //Mudar
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            //Se conseguiu manadar e-mails
            if (SendedEmails(idPerfil, idAvaliacao))
            {
                List<string> listaEmailsEnviados = new List<string>();

                foreach (var item in db.Usuarios.Where(x => x.IdPerfil == idPerfil))
                {
                    listaEmailsEnviados.Add(item.Email);
                }

                ViewBag.Emails = new SelectList(listaEmailsEnviados);

                return View();
            }


            //return View("Details", idAvaliacao);
            return RedirectToAction("NotSent", new { id = idAvaliacao });
           
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

            var response = Models.Email.SendedEmail(emails, idAvaliacao);
            return response;
        }

        public ActionResult NotSent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacaos.Find(id);

            //Lista de perugntas que pertencem a avaliacao
            var listPerguntasJoin = db.AvaliacaoPerguntas
                   .Join(db.Perguntas, j => j.IdPergunta, k => k.IdPergunta, (j, k) => new { j, k }).Where(x => x.j.IdAvaliacao == id)
                   .Join(db.Avaliacaos, a => a.j.IdAvaliacao, b => b.IdAvaliacao, (a, b) => new { a, b }).Select(s => new SelectListItem { Value = s.a.j.IdPergunta.ToString(), Text = s.a.k.DescricaoPergunta });

            //Todas as perguntas
            ViewBag.Perguntas = listPerguntasJoin.ToList();

            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            return View(avaliacao);
        }

        #endregion

        // GET: Avaliacao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacaos.Find(id);

            //Lista de perugntas que pertencem a avaliacao
            var listPerguntasJoin = db.AvaliacaoPerguntas
                   .Join(db.Perguntas, j => j.IdPergunta, k => k.IdPergunta, (j, k) => new { j, k }).Where(x => x.j.IdAvaliacao == id)
                   .Join(db.Avaliacaos, a => a.j.IdAvaliacao, b => b.IdAvaliacao, (a, b) => new { a, b }).Select(s => new SelectListItem { Value = s.a.j.IdPergunta.ToString(), Text = s.a.k.DescricaoPergunta });

            //Todas as perguntas
            ViewBag.Perguntas = listPerguntasJoin.ToList();

            if (avaliacao == null)
            {
                return HttpNotFound();
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

        public ActionResult TesteCSV(int id)
        {
            string caminho = string.Format("C:\\Users\\Marcus\\Desktop\\Particular\\Relatorios\\{0}.txt", DateTime.Now.ToString("ddMMyyyyHHmmss"));
            StreamWriter valor = new StreamWriter(caminho);
            valor.WriteLine("Descricão da avaliação;Descrição da pergunta; Descrição da resposta");      
            var listAvaliacaoResposta = db.AvaliacaoRespostas.Where(x => x.IdAvaliacao == id).ToList();          
            
            foreach( var idPergunta in listAvaliacaoResposta.Where(x => x.IdAvaliacao == id).GroupBy(y => new{y.IdPergunta}).ToList())
            {
                IDictionary<string, string> dict = new Dictionary<string, string>();
     
                foreach(var idTipoResposta in listAvaliacaoResposta.Where(x => x.IdPergunta == idPergunta.Key.IdPergunta).Select(x => x.IdTipoResposta).ToList())
                {
                    //adiciona no dicionário o idRespostas selecionados com suas qty
                    dict.Add(idTipoResposta.ToString(), listAvaliacaoResposta.Where(x => x.IdTipoResposta == idTipoResposta && x.IdPergunta == idPergunta.Key.IdPergunta).Count().ToString());
                }

                foreach (var items in db.PerguntaTipoRespostas.Where(x => x.IdPergunta == idPergunta.Key.IdPergunta).ToList())
                {
                    //se nao existe a opcao no no dicionario, adiciona tambem
                    if(!dict.Keys.Contains(items.IdtipoResposta.ToString()))
                    {
                        dict.Add(items.IdtipoResposta.ToString(), "0");
                    }
                }
 
            }            
                           

            foreach (var item in listAvaliacaoResposta)
            {
                var descAvaliacao = db.Avaliacaos.FirstOrDefault(x => x.IdAvaliacao == item.IdAvaliacao);
                var descPergunta = db.Perguntas.FirstOrDefault(x => x.IdPergunta == item.IdPergunta);
                var descTipoResposta = db.TipoRespostas.FirstOrDefault(x => x.IdTipoResposta == item.IdTipoResposta);

                valor.WriteLine(string.Format("{0};{1};{2}", descAvaliacao.DescricaoAvaliacao, descPergunta.DescricaoPergunta, descTipoResposta.DescricaoTipoResposta));
            }
            valor.Close();          

            return RedirectToAction("RelatorioCreated");
        }

        public ActionResult RelatorioCreated()
        {
            var avaliacaos = db.Avaliacaos.Include(a => a.Perfil).Include(a => a.Usuario).ToList();
            return View(avaliacaos);
        }
    }
}
