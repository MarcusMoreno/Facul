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
    public class PerfilController : Controller
    {
        private ReviewEntities db = new ReviewEntities();

        // GET: Perfil
        public ActionResult Index()
        {
            return View(db.Perfils.ToList());
        }

        public ActionResult Created()
        {
            return View(db.Perfils.ToList());
        }

        // GET: Perfil/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var t = Perfil.GetDeatailsPerfil(id);

            Perfil perfil = db.Perfils.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }


        // GET: Perfil/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Perfil/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPerfil,DescricaoPerfil,PerfilStatus")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                db.Perfils.Add(perfil);
                db.SaveChanges();
                
                return RedirectToAction("Created");
            }

            return View(perfil);
        }


        // GET: Perfil/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = db.Perfils.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }


        // POST: Perfil/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPerfil,DescricaoPerfil,PerfilStatus")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(perfil).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(perfil);
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
