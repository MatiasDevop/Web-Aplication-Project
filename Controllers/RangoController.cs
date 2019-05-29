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
    public class RangoController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Rango/

        public ActionResult Index()
        {
            return View(db.RangoICAs.ToList());
        }

        //
        // GET: /Rango/Details/5

        public ActionResult Details(int id = 0)
        {
            RangoICA rangoica = db.RangoICAs.Find(id);
            if (rangoica == null)
            {
                return HttpNotFound();
            }
            return View(rangoica);
        }

        //
        // GET: /Rango/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Rango/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RangoICA rangoica)
        {
            if (ModelState.IsValid)
            {
                db.RangoICAs.Add(rangoica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rangoica);
        }

        //
        // GET: /Rango/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RangoICA rangoica = db.RangoICAs.Find(id);
            if (rangoica == null)
            {
                return HttpNotFound();
            }
            return View(rangoica);
        }

        //
        // POST: /Rango/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RangoICA rangoica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rangoica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rangoica);
        }

        //
        // GET: /Rango/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RangoICA rangoica = db.RangoICAs.Find(id);
            if (rangoica == null)
            {
                return HttpNotFound();
            }
            return View(rangoica);
        }

        //
        // POST: /Rango/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RangoICA rangoica = db.RangoICAs.Find(id);
            db.RangoICAs.Remove(rangoica);
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