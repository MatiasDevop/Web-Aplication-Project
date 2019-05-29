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
    public class PmActivoController : Controller
    {
        private Context db = new Context();

        //
        // GET: /PmActivo/

        public ActionResult Index()
        {
            var pmactivos = db.PmActivos.Include(p => p.Contaminante);
            return View(pmactivos.ToList());
        }

        //
        // GET: /PmActivo/Details/5

        public ActionResult Details(int id = 0)
        {
            PmActivo pmactivo = db.PmActivos.Find(id);
            if (pmactivo == null)
            {
                return HttpNotFound();
            }
            return View(pmactivo);
        }

        //
        // GET: /PmActivo/Create

        public ActionResult Create()
        {
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre");
            return View();
        }

        //
        // POST: /PmActivo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PmActivo pmactivo)
        {
            if (ModelState.IsValid)
            {
                db.PmActivos.Add(pmactivo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", pmactivo.ContaminanteID);
            return View(pmactivo);
        }

        //
        // GET: /PmActivo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PmActivo pmactivo = db.PmActivos.Find(id);
            if (pmactivo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", pmactivo.ContaminanteID);
            return View(pmactivo);
        }

        //
        // POST: /PmActivo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PmActivo pmactivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pmactivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", pmactivo.ContaminanteID);
            return View(pmactivo);
        }

        //
        // GET: /PmActivo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PmActivo pmactivo = db.PmActivos.Find(id);
            if (pmactivo == null)
            {
                return HttpNotFound();
            }
            return View(pmactivo);
        }

        //
        // POST: /PmActivo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PmActivo pmactivo = db.PmActivos.Find(id);
            db.PmActivos.Remove(pmactivo);
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
            return View();
        }
        public ActionResult Importexcel(Contaminante id)
        {

            int countImported = 0;
            try
            {

                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    //var fileName = this.Server.MapPath("~/Uploads/Criterios/" + System.IO.Path.GetFileName(fileData.FileName));
                    //fileData.SaveAs(fileName);
                    //ViewBag.Message = fileData.FileName;

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
                            //string code = Convert.ToString(dReader[0]);
                            //var query = db.Contaminantes.Find(Nombre);

                            //var query = db.AutomaticoPMs.Find(countImported);
                            //if (query == null)
                            //{
                            PmActivo a = new PmActivo();
                            //a.ArticleID = Convert.ToInt32(dReader[0]);
                            a.Fecha = Convert.ToDateTime(dReader[0]);
                            a.Concentracion = Convert.ToDecimal(dReader[1]);
                            a.ContaminanteID = id.ContaminanteID;
                            //a.Name = dReader[1].ToString();
                            //a.Unit = dReader[2].ToString();
                            //a.TownID = 1;
                            db.PmActivos.Add(a);
                            countImported++;
                            ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", a.ContaminanteID);

                            //}


                        }

                        if (countImported > 0)
                        {
                            db.SaveChanges();

                            //Success("EXITO: Se importaron " + countImported + " Registros");

                        }
                        else
                        {
                            //Information("EXITO: Se importaron " + countImported + " Registros");

                        }

                        //SqlBulkCopy sqlBulk = new SqlBulkCopy(sqlConnectionString);
                        ////Give your Destination table name
                        //sqlBulk.DestinationTableName = "Articles";
                        //sqlBulk.WriteToServer(dReader);
                        excelConnection.Close();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.error = "no funciona";
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
    }
}