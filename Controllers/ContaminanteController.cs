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
    public class ContaminanteController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Contaminante/

        public ActionResult Index()
        {
            return View(db.Contaminantes.ToList());
        }

        //
        // GET: /Contaminante/Details/5

        public ActionResult Details(int id = 0)
        {
            Contaminante contaminante = db.Contaminantes.Find(id);
            if (contaminante == null)
            {
                return HttpNotFound();
            }
            return View(contaminante);
        }

        //
        // GET: /Contaminante/Create

        public ActionResult Create()
        {
           var contaminante = new Contaminante();
            return View(contaminante);
        }

        //
        // POST: /Contaminante/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contaminante contaminante)
        {
            if (ModelState.IsValid)
            {
                db.Contaminantes.Add(contaminante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contaminante);
        }

        //
        // GET: /Contaminante/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Contaminante contaminante = db.Contaminantes.Find(id);
            if (contaminante == null)
            {
                return HttpNotFound();
            }
            return View(contaminante);
        }

        //
        // POST: /Contaminante/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contaminante contaminante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contaminante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contaminante);
        }

        //
        // GET: /Contaminante/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Contaminante contaminante = db.Contaminantes.Find(id);
            if (contaminante == null)
            {
                return HttpNotFound();
            }
            return View(contaminante);
        }

        //
        // POST: /Contaminante/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contaminante contaminante = db.Contaminantes.Find(id);
            db.Contaminantes.Remove(contaminante);
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