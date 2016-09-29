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
    public class UsuarioController : Controller
    {
        private ReviewEntities db = new ReviewEntities();

        // GET: Usuario
        public ActionResult Index()
        {
            var usuarios = db.Usuarios.Include(u => u.Perfil);
            return View(usuarios.ToList());
        }





        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        
        
        
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            ViewBag.IdPerfil = new SelectList(db.Perfils.ToList().Where(x => x.PerfilStatus == true).Select(y => y.DescricaoPerfil));

            return View();
        }




        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUsuario,Nome,Email,Telefone,IdPerfil,UsuarioStatus,Senha")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPerfil = new SelectList(db.Perfils.ToList().Where(x => x.PerfilStatus == true).Select(y => y.DescricaoPerfil));

            return View(usuario);
        }





        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdPerfil = new SelectList(db.Perfils.ToList().Where(x => x.PerfilStatus == true).Select(y => y.DescricaoPerfil));

            return View(usuario);
        }

        // POST: Usuario/EditUsuario/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUsuario,Nome,Email,Telefone,IdPerfil,UsuarioStatus,Senha")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPerfil = new SelectList(db.Perfils.ToList().Where(x => x.PerfilStatus == true).Select(y => y.DescricaoPerfil));

            return View(usuario);
        }

        //// GET: Usuario/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Usuario usuario = db.Usuarios.Find(id);
        //    if (usuario == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(usuario);
        //}

        //// POST: Usuario/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Usuario usuario = db.Usuarios.Find(id);
        //    db.Usuarios.Remove(usuario);
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
