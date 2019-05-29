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
    public class AutomaticoPMController : Controller
    {
        private Context db = new Context();

        //
        // GET: /AutomaticoPM/

        public ActionResult Index()
        {
            var automaticopms = db.AutomaticoPMs.Include(a => a.Contaminante).Include(a => a.Estacion);
            return View(automaticopms.ToList());
        }

        //
        // GET: /AutomaticoPM/Details/5

        public ActionResult Details(int id = 0)
        {
            AutomaticoPM automaticopm = db.AutomaticoPMs.Find(id);
            if (automaticopm == null)
            {
                return HttpNotFound();
            }
            return View(automaticopm);
        }
        // by dates
        [HttpPost]
        public JsonResult SearchByDatesA(DateTime fechastart, DateTime fecha)
        {
            var list = db.AutomaticoPMs.Where(x => x.Fecha >= fechastart
                && x.Fecha <= fecha).ToList();
            var resList = new List<AutomaticoPM>();
            var contF = 0;
            int acum=0;
            var listConcetra=new List<int>();
            int promedio = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var day = list.ElementAt(contF).Fecha.Day;
                
                if (list.ElementAt(i).Fecha.Day == day && list.ElementAt(i).Fecha!=null)
                {
                    acum = acum + Convert.ToInt32(list.ElementAt(i).Concentracion);
                    listConcetra.Add(Convert.ToInt32(list.ElementAt(i).Concentracion));
                    promedio++;
                    if (i==list.Count-1)
                    {
                        var concetracionPromedio = listConcetra.Max(); //acum / promedio;
                               resList.Add(new AutomaticoPM
                               {
                                   Fecha = list.ElementAt(contF).Fecha,
                                   Concentracion = concetracionPromedio,
                                   Estacion = list.ElementAt(contF).Estacion
                               });
                    }
                }else
                {
                    var concetracionPromedio = listConcetra.Max();            // acum / promedio;
                   resList.Add(new AutomaticoPM
                   {
                       Fecha = list.ElementAt(contF).Fecha,
                       Concentracion = concetracionPromedio,
                       Estacion=list.ElementAt(contF).Estacion
                   });
                   //listConcetra.Add(concetracionPromedio);
                    contF=i;
                    promedio = 0;
                    i--;
                    acum = 0;
                    listConcetra = new List<int>();
                }
                
                    
                
            }
            //list = resList.ToList();
            
            return Json(resList.ToList());
        }
     
        [HttpPost]
        public JsonResult SearchByPeriod(int id)
        {
            var list = db.AutomaticoPMs.ToList();

            return Json(list);
        }
        //
        // GET: /AutomaticoPM/Create

        public ActionResult Create()
        {
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre");
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name");
            return View();
        }

        //
        // POST: /AutomaticoPM/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AutomaticoPM automaticopm)
        {
            if (ModelState.IsValid)
            {
                db.AutomaticoPMs.Add(automaticopm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", automaticopm.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", automaticopm.EstacionId);
            return View(automaticopm);
        }

        //
        // GET: /AutomaticoPM/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AutomaticoPM automaticopm = db.AutomaticoPMs.Find(id);
            if (automaticopm == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", automaticopm.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", automaticopm.EstacionId);
            return View(automaticopm);
        }

        //
        // POST: /AutomaticoPM/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AutomaticoPM automaticopm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(automaticopm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", automaticopm.ContaminanteID);
            ViewBag.EstacionId = new SelectList(db.Estacions, "EstacionId", "Name", automaticopm.EstacionId);
            return View(automaticopm);
        }

        //
        // GET: /AutomaticoPM/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AutomaticoPM automaticopm = db.AutomaticoPMs.Find(id);
            if (automaticopm == null)
            {
                return HttpNotFound();
            }
            return View(automaticopm);
        }

        //
        // POST: /AutomaticoPM/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AutomaticoPM automaticopm = db.AutomaticoPMs.Find(id);
            db.AutomaticoPMs.Remove(automaticopm);
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
        public ActionResult Importexcel(DateTime FechaRegistro,Contaminante Contaminante, Estacion Estacion)
        {

            int countImported = 0;

            List<AutomaticoPM> DatosAutomatico = new List<AutomaticoPM>();
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

                            AutomaticoPM a = new AutomaticoPM();

                            string date = "20" + dReader[0];
                            a.Fecha = DateTime.Parse(date);
                            //a.Fecha = Convert.ToDateTime(dReader[0]);
                            a.FechaRegistro = FechaRegistro;
                            a.Concentracion = transformarICA(Convert.ToDecimal(dReader[1]), Contaminante);
                            a.ContaminanteID = Contaminante.ContaminanteID;
                            a.EstacionId = Estacion.EstacionId;
                            //a.Name = dReader[1].ToString();
                            //a.Unit = dReader[2].ToString();
                            //a.TownID = 1;
                            db.AutomaticoPMs.Add(a);
                            DatosAutomatico.Add(a);
                            countImported++;
                            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", a.ContaminanteID);

                            //}


                        }

                        if (countImported > 0)
                        {
                            registrarEnNivelContaminacion(FechaRegistro,DatosAutomatico, Icas, Contaminante,Estacion);
                            db.SaveChanges();

                            //Success("EXITO: Se importaron " + countImported + " Registros");
                            RedirectToAction("Index");
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

                    }

                }
            }
            catch (Exception e)
            {

                //Error("ERRO: El archivo Excel está siendo utilizado por otra instancia.");
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        private void registrarEnNivelContaminacion(DateTime Fecha, List<AutomaticoPM> datosPM, List<RangoICA> icas, Contaminante contaminante, Estacion estacion)
        {
            decimal indiceContaminancion = 0;
            var listConcentracion = new List<int>();
           // int promedio = 0;
            //int acum = 0;
            foreach (var item in datosPM)
            {
                listConcentracion.Add(Convert.ToInt32(item.Concentracion));
                //acum = acum + Convert.ToInt32(item.Concentracion);
                //promedio++;
            }
            indiceContaminancion = listConcentracion.Max();// (acum / promedio);
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