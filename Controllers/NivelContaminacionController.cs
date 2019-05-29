using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVClog.Models;

namespace MVClog.Controllers
{
    public class NivelContaminacionController : Controller
    {
        private Context db = new Context();

        //
        // GET: /NivelContaminacion/

        public ActionResult Index()
        {
            var nivelcontaminantes = db.NivelContaminantes.Include(n => n.Contaminante).Include(n => n.Estacion);
            return View(nivelcontaminantes.ToList());
        }

        //
        // GET: /NivelContaminacion/Details/5

        public ActionResult Details(int id = 0)
        {
            NivelContaminacion nivelcontaminacion = db.NivelContaminantes.Find(id);
            if (nivelcontaminacion == null)
            {
                return HttpNotFound();
            }
            return View(nivelcontaminacion);
        }

        //
        // GET: /NivelContaminacion/Create

        public ActionResult Create()
        {
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre");
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name");
            return View();
        }

        //
        // POST: /NivelContaminacion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NivelContaminacion nivelcontaminacion)
        {
            if (ModelState.IsValid)
            {
                db.NivelContaminantes.Add(nivelcontaminacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", nivelcontaminacion.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", nivelcontaminacion.EstacionId);
            return View(nivelcontaminacion);
        }

        //
        // GET: /NivelContaminacion/Edit/5

        public ActionResult Edit(int id = 0)
        {
            NivelContaminacion nivelcontaminacion = db.NivelContaminantes.Find(id);
            if (nivelcontaminacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", nivelcontaminacion.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", nivelcontaminacion.EstacionId);
            return View(nivelcontaminacion);
        }

        //
        // POST: /NivelContaminacion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NivelContaminacion nivelcontaminacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nivelcontaminacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", nivelcontaminacion.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", nivelcontaminacion.EstacionId);
            return View(nivelcontaminacion);
        }

        //
        // GET: /NivelContaminacion/Delete/5

        public ActionResult Delete(int id = 0)
        {
            NivelContaminacion nivelcontaminacion = db.NivelContaminantes.Find(id);
            if (nivelcontaminacion == null)
            {
                return HttpNotFound();
            }
            return View(nivelcontaminacion);
        }

        //
        // POST: /NivelContaminacion/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NivelContaminacion nivelcontaminacion = db.NivelContaminantes.Find(id);
            db.NivelContaminantes.Remove(nivelcontaminacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}