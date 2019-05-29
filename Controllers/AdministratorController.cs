using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVClog.Controllers
{
    
    public class AdministratorController : Controller
    {
        //
        // GET: /Administrator/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Panel()
        {
            return PartialView();
        }
        public ActionResult Blank()
        {
            return PartialView();
        }

    }
}
