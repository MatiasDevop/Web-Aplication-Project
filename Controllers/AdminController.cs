using MVClog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVClog.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        Context db = new Context();
        public ActionResult Index()
        {
            var muestraPm = new AutomaticoPM();
            List<PmActivo> list= db.PmActivos.ToList();
            foreach (PmActivo automatico in list)
            {
                muestraPm.Concentracion = automatico.Concentracion;
            }
           
            return View("Index",muestraPm);
        }
        public ActionResult Charts()
        {
            return View();
        }
        public ActionResult Estaciones()
        {
            return View();
        }


    }
}
