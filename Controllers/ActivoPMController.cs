using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVClog.Models;
using System.Data.OleDb;

namespace MVClog.Controllers
{
    public class ActivoPMController : Controller
    {
        private Context db = new Context();

        //
        // GET: /ActivoPM/

        public ActionResult Index()
        {
            var activopms = db.ActivoPMs.Include(a => a.Contaminante).Include(a => a.Estacion);
            return View(activopms.ToList());
        }

        //
        // GET: /ActivoPM/Details/5

        public ActionResult Details(int id = 0)
        {
            ActivoPM activopm = db.ActivoPMs.Find(id);
            if (activopm == null)
            {
                return HttpNotFound();
            }
            return View(activopm);
        }

        //
        // GET: /ActivoPM/Create

        public ActionResult Create()
        {
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre");
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name");
            return View();
        }

        //
        // POST: /ActivoPM/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActivoPM activopm)
        {
            if (ModelState.IsValid)
            {
                db.ActivoPMs.Add(activopm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", activopm.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", activopm.EstacionId);
            return View(activopm);
        }

        //
        // GET: /ActivoPM/Edit/5
        // for search by filters or datetime
        [HttpPost]
        public JsonResult SearchByDates(DateTime fechastart, DateTime fecha)
        {
            var list = db.ActivoPMs.Where(x => x.Fecha >= fechastart
                && x.Fecha <= fecha).ToList();

            return Json(list);
        }
        [HttpPost]
        public JsonResult SearchByStation(Estacion Estacion)
        {
            var list = db.ActivoPMs.Where(x=>x.EstacionId==Estacion.EstacionId).ToList();

            return Json(list);
        }
        public ActionResult Edit(int id = 0)
        {
            ActivoPM activopm = db.ActivoPMs.Find(id);
            if (activopm == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", activopm.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", activopm.EstacionId);
            return View(activopm);
        }

        //
        // POST: /ActivoPM/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActivoPM activopm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activopm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", activopm.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", activopm.EstacionId);
            return View(activopm);
        }

        //
        // GET: /ActivoPM/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ActivoPM activopm = db.ActivoPMs.Find(id);
            if (activopm == null)
            {
                return HttpNotFound();
            }
            return View(activopm);
        }

        //
        // POST: /ActivoPM/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActivoPM activopm = db.ActivoPMs.Find(id);
            db.ActivoPMs.Remove(activopm);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult Import()
        {
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre");
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name");
            return View();
        }
        public ActionResult Importexcel(DateTime FechaRegistro, Contaminante id, Estacion Estacion)
        {

            int countImported = 0;
            List<ActivoPM> DatosActivos = new List<ActivoPM>();
            List<RangoICA> Icas = db.RangoICAs.ToList();
            try
            {

                if (Request.Files["FileUpload1"].ContentLength > 0)
                {

                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName);
                    string path1 = this.Server.MapPath("~/Content/Upload/" + Request.Files["FileUpload1"].FileName);
                    if (System.IO.File.Exists(path1))
                        System.IO.File.Delete(path1);



                    Request.Files["FileUpload1"].SaveAs(path1);
                    //string sqlConnectionString = @"Data Source=.\SQLEXPRESS;Database=ADRA.Models.DBContext;Trusted_Connection=true;Persist Security Info=True";

                    //Create connection string to Excel work book
                    string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                    //Create Connection to Excel work book
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    //Create OleDbCommand to fetch data from Excel
                    OleDbCommand cmd = new OleDbCommand("Select [Fecha],[Concentracion] from [Hoja1$]", excelConnection);

                    try
                    {

                        excelConnection.Open();
                        OleDbDataReader dReader;

                        dReader = cmd.ExecuteReader();

                        while (dReader.Read())
                        {
                         
                            ActivoPM a = new ActivoPM();
                            a.Fecha = Convert.ToDateTime(dReader[0]);
                            a.FechaRegistro = FechaRegistro;
                            a.Concentracion = transformarICA(Convert.ToDecimal(dReader[1]));
                            a.ContaminanteID = id.ContaminanteID;
                            a.EstacionId = Estacion.EstacionId;
                            db.ActivoPMs.Add(a);
                            DatosActivos.Add(a);
                            countImported++;
                            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", a.ContaminanteID);

                        }
                        //registrarEnNivelContaminacion(DatosActivos,Icas);

                        if (countImported > 0)
                        {
                            registrarEnNivelContaminacion(FechaRegistro, DatosActivos, Icas, id, Estacion);
                            db.SaveChanges();

                            //Success("EXITO: Se importaron " + countImported + " Registros");

                        }
                        else
                        {
                            //Information("EXITO: Se importaron " + countImported + " Registros");

                        }

                        excelConnection.Close();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.error = "no funciona ";
                        //Error("ERROR: Incompatibilidad del archivo excel " + ex.Message);
                    }
                    //SQL Server Connection String


                }
            }
            catch (Exception e)
            {

                //Error("ERRO: El archivo Excel está siendo utilizado por otra instancia.");
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        private void registrarEnNivelContaminacion(DateTime Fecha, List<ActivoPM> datosPM, List<RangoICA> icas, Contaminante contaminante, Estacion estacion)
        {
            decimal indiceContaminancion = 0;
            int promedio = 0;
            int acum = 0;
            foreach (var item in datosPM)
            {
                acum = acum + Convert.ToInt32(item.Concentracion);
                promedio++;
            }
            indiceContaminancion = (acum / promedio);
            var nivelContaminante = new NivelContaminacion();

            nivelContaminante.Fecha = Fecha;
            nivelContaminante.Concentracion = indiceContaminancion;
            nivelContaminante.Color = buscarRango(indiceContaminancion, icas);
            nivelContaminante.Calificativo = buscarCalificativo(indiceContaminancion, icas);
            nivelContaminante.Recomendacion = buscarRecomendacion(indiceContaminancion, icas);
            nivelContaminante.ContaminanteID = contaminante.ContaminanteID;
            nivelContaminante.EstacionId = estacion.EstacionId;
            //nivelContaminante.Contaminante = contaminante;
            db.NivelContaminantes.Add(nivelContaminante);


        }

        private string buscarCalificativo(decimal indiceContaminancion, List<RangoICA> icas)
        {
            string res = "";
            foreach (var item in icas)
            {
                if (indiceContaminancion <= item.ValorMax)
                {
                    res = item.Calificativo;
                    return res;
                }
            }
            return res;
        }

        private string buscarRecomendacion(decimal indiceContaminancion, List<RangoICA> icas)
        {
            string res = "";
            foreach (var item in icas)
            {
                if (indiceContaminancion <= item.ValorMax)
                {
                    res = item.Efectos;
                    return res;
                }
            }
            return res;
        }

        private string buscarRango(decimal indiceContaminancion, List<RangoICA> Icas)
        {
            string res = "";
            foreach (var item in Icas)
            {
                if (indiceContaminancion <= item.ValorMax)
                {
                    res = item.Color;
                    return res;
                }
            }
            return res;
        }

        private decimal transformarICA(Decimal concentracion)
        {
            decimal res = 0;
            res = (concentracion * 100) / 150;
            return res;
        }
        public ActionResult goPrincipal()
        {
            return RedirectToAction("Index", "Admin");
        }
    }
}