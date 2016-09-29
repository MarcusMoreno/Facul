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
    public class TipoRespostaController : Controller
    {
        private ReviewEntities db = new ReviewEntities();

        // GET: TipoResposta
        public ActionResult Index()
        {
            return View(db.TipoRespostas.ToList());
        }

        // GET: TipoResposta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoResposta tipoResposta = db.TipoRespostas.Find(id);
            if (tipoResposta == null)
            {
                return HttpNotFound();
            }
            return View(tipoResposta);
        }

        // GET: TipoResposta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoResposta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTipoResposta,DescricaoTipoResposta,TipoRespostaStatus")] TipoResposta tipoResposta)
        {
            if (ModelState.IsValid)
            {
                db.TipoRespostas.Add(tipoResposta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoResposta);
        }

        // GET: TipoResposta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoResposta tipoResposta = db.TipoRespostas.Find(id);
            if (tipoResposta == null)
            {
                return HttpNotFound();
            }
            return View(tipoResposta);
        }

        // POST: TipoResposta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTipoResposta,DescricaoTipoResposta,TipoRespostaStatus")] TipoResposta tipoResposta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoResposta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoResposta);
        }

        // GET: TipoResposta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoResposta tipoResposta = db.TipoRespostas.Find(id);
            if (tipoResposta == null)
            {
                return HttpNotFound();
            }
            return View(tipoResposta);
        }

        // POST: TipoResposta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoResposta tipoResposta = db.TipoRespostas.Find(id);
            db.TipoRespostas.Remove(tipoResposta);
            db.SaveChanges();
            return RedirectToAction("Index");
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
