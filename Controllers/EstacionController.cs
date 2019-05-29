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
    public class EstacionController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Estacion/

        public ActionResult Index()
        {
            return View(db.Estacions.ToList());
        }

        //
        // GET: /Estacion/Details/5

        public ActionResult Details(int id = 0)
        {
            Estacion estacion = db.Estacions.Find(id);
            if (estacion == null)
            {
                return HttpNotFound();
            }
            return View(estacion);
        }

        //
        // GET: /Estacion/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Estacion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Estacion estacion)
        {
            if (ModelState.IsValid)
            {
                db.Estacions.Add(estacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estacion);
        }

        //
        // GET: /Estacion/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Estacion estacion = db.Estacions.Find(id);
            if (estacion == null)
            {
                return HttpNotFound();
            }
            return View(estacion);
        }

        //
        // POST: /Estacion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Estacion estacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estacion);
        }

        //
        // GET: /Estacion/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Estacion estacion = db.Estacions.Find(id);
            if (estacion == null)
            {
                return HttpNotFound();
            }
            return View(estacion);
        }

        //
        // POST: /Estacion/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estacion estacion = db.Estacions.Find(id);
            db.Estacions.Remove(estacion);
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


