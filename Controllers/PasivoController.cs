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
    public class PasivoController : Controller
    {
        private Context db = new Context();

        //
        // GET: /Pasivo/

        public ActionResult Index()
        {
            var pasivoes = db.Pasivoes.Include(p => p.Contaminante).Include(p => p.Estacion);
            return View(pasivoes.ToList());
        }

        //
        // GET: /Pasivo/Details/5

        public ActionResult Details(int id = 0)
        {
            Pasivo pasivo = db.Pasivoes.Find(id);
            if (pasivo == null)
            {
                return HttpNotFound();
            }
            return View(pasivo);
        }

        //
        // GET: /Pasivo/Create

        public ActionResult Create()
        {
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre");
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name");
            return View();
        }

        //
        // POST: /Pasivo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pasivo pasivo)
        {
            if (ModelState.IsValid)
            {
                db.Pasivoes.Add(pasivo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", pasivo.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", pasivo.EstacionId);
            return View(pasivo);
        }

        //
        // GET: /Pasivo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Pasivo pasivo = db.Pasivoes.Find(id);
            if (pasivo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", pasivo.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", pasivo.EstacionId);
            return View(pasivo);
        }

        //
        // POST: /Pasivo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pasivo pasivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pasivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", pasivo.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", pasivo.EstacionId);
            return View(pasivo);
        }

        //
        // GET: /Pasivo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Pasivo pasivo = db.Pasivoes.Find(id);
            if (pasivo == null)
            {
                return HttpNotFound();
            }
            return View(pasivo);
        }

        //
        // POST: /Pasivo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pasivo pasivo = db.Pasivoes.Find(id);
            db.Pasivoes.Remove(pasivo);
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
            var pasivo = new Pasivo();
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre");
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name");
            return View(pasivo);
        }
        public ActionResult Importexcel(DateTime FechaRegistro, Contaminante Contaminante, Estacion Estacion)
        {

            int countImported = 0;

            List<Pasivo> DatosPasivo = new List<Pasivo>();
            List<RangoICA> Icas = db.RangoICAs.ToList();

            if (Contaminante.ContaminanteID != 0)
            {
                try
                {

                    if (Request.Files["FileUpload1"].ContentLength > 0)
                    {

                        string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName);
                        string path1 = this.Server.MapPath("~/Content/Upload/" + Request.Files["FileUpload1"].FileName);
                        if (System.IO.File.Exists(path1))
                            System.IO.File.Delete(path1);



                        Request.Files["FileUpload1"].SaveAs(path1);

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

                                Pasivo a = new Pasivo();
                            
                                a.FechaRegistro = FechaRegistro;
                                a.Fecha = Convert.ToString(dReader[0]); 
                                a.Concentracion = transformarICA(Convert.ToDecimal(dReader[1]), Contaminante);//transformarICA(Convert.ToDecimal(dReader[1]), Contaminante);
                                a.ContaminanteID = Contaminante.ContaminanteID;
                                a.EstacionId = Estacion.EstacionId;

                                db.Pasivoes.Add(a);
                                DatosPasivo.Add(a);

                                countImported++;
                                ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", a.ContaminanteID);
                            }

                            if (countImported > 0)
                            {
                                registrarEnNivelContaminacion(FechaRegistro,DatosPasivo, Icas, Contaminante,Estacion);
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
                            ViewBag.error = "no funciona";
                            //Error("ERROR: Incompatibilidad del archivo excel " + ex.Message);
                        }


                    }
                }
                catch (Exception e)
                {

                    //Error("ERRO: El archivo Excel está siendo utilizado por otra instancia.");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                //if you dont upload any contaminante
                ViewBag.ErrorMessage = "Tipo de contaminante no encontrado verifique por favor";
                return View("Import");
            }

            return RedirectToAction("Index");
        }
        private void registrarEnNivelContaminacion(DateTime Fecha, List<Pasivo> datosPM, List<RangoICA> icas, Contaminante contaminante, Estacion estacion)
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
        private decimal transformarICA(Decimal concentracion, Contaminante contaminante)
        {
            var vlmp = 0;
            var limites = db.Contaminantes.ToList();
            foreach (var item in limites)
            {
                if (item.ContaminanteID == contaminante.ContaminanteID)
                {
                    vlmp = item.ValorLimite;
                }
            }
            decimal res = 0;
            res = (concentracion * 100) / vlmp;
            return res;
        }
    }
}