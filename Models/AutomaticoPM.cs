using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVClog.Models
{
    public class AutomaticoPM
    {
        [Key]
        public int AutomaticoId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Concentracion { get; set; }
        public int ContaminanteID { get; set; }
        public virtual Contaminante Contaminante { get; set; }
        public int EstacionId { get; set; }
        public virtual Estacion Estacion { get; set; }
            
    }
}
 //public ActionResult Import()
 //       {
 //           ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre");
 //           return View();
 //       }
 //       public ActionResult Importexcel(Contaminante id)
 //       {

 //           int countImported = 0;
 //           List<ActivoPM> DatosActivos = new List<ActivoPM>();
 //           List<RangoICA> Icas = db.RangoICAs.ToList();
 //           try
 //           {

 //               if (Request.Files["FileUpload1"].ContentLength > 0)
 //               {

 //                   string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName);
 //                   string path1 = this.Server.MapPath("~/Content/Upload/" + Request.Files["FileUpload1"].FileName);
 //                   if (System.IO.File.Exists(path1))
 //                       System.IO.File.Delete(path1);



 //                   Request.Files["FileUpload1"].SaveAs(path1);
 //                   //string sqlConnectionString = @"Data Source=.\SQLEXPRESS;Database=ADRA.Models.DBContext;Trusted_Connection=true;Persist Security Info=True";

 //                   //Create connection string to Excel work book
 //                   string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=Excel 12.0;Persist Security Info=False";
 //                   //Create Connection to Excel work book
 //                   OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
 //                   //Create OleDbCommand to fetch data from Excel
 //                   OleDbCommand cmd = new OleDbCommand("Select [Fecha],[Concentracion] from [Hoja1$]", excelConnection);

 //                   try
 //                   {

 //                       excelConnection.Open();
 //                       OleDbDataReader dReader;

 //                       dReader = cmd.ExecuteReader();

 //                       while (dReader.Read())
 //                       {
 //                           //string code = Convert.ToString(dReader[0]);
 //                           //var query = db.Contaminantes.Find(Nombre);

 //                           //var query = db.AutomaticoPMs.Find(countImported);
 //                           //if (query == null)
 //                           //{
 //                           ActivoPM a = new ActivoPM();
 //                           //a.ArticleID = Convert.ToInt32(dReader[0]);
 //                           a.Fecha = Convert.ToDateTime(dReader[0]);
 //                           a.Concentracion = transformarICA(Convert.ToDecimal(dReader[1]));
 //                           a.ContaminanteID = id.ContaminanteID;
                           
 //                           db.ActivoPMs.Add(a);
 //                           DatosActivos.Add(a);
 //                           countImported++;
 //                           ViewBag.ContaminanteID = new SelectList(db.Contaminantes, "ContaminanteID", "Nombre", a.ContaminanteID);

 //                       }
 //                       //registrarEnNivelContaminacion(DatosActivos,Icas);

 //                       if (countImported > 0)
 //                       {
 //                           registrarEnNivelContaminacion(DatosActivos, Icas,id);
 //                           db.SaveChanges();
                           
 //                           //Success("EXITO: Se importaron " + countImported + " Registros");

 //                       }
 //                       else
 //                       {
 //                           //Information("EXITO: Se importaron " + countImported + " Registros");

 //                       }

 //                       excelConnection.Close();
 //                   }
 //                   catch (Exception ex)
 //                   {
 //                       ViewBag.error = "no funciona ";
 //                       //Error("ERROR: Incompatibilidad del archivo excel " + ex.Message);
 //                   }
 //                   //SQL Server Connection String


 //               }
 //           }
 //           catch (Exception e)
 //           {

 //               //Error("ERRO: El archivo Excel está siendo utilizado por otra instancia.");
 //               return RedirectToAction("Index");
 //           }

 //           return RedirectToAction("Index");
 //       }

 //       private void registrarEnNivelContaminacion(List<ActivoPM> datosPM,List<RangoICA> icas,Contaminante contaminante)
 //       {
 //           decimal indiceContaminancion = 0;
 //           int promedio = 0;
 //           int acum = 0;
 //           foreach (var item in datosPM)
 //           {
 //               acum = acum + Convert.ToInt32( item.Concentracion);
 //               promedio++;
 //           }
 //           indiceContaminancion = (acum / promedio);
 //           var nivelContaminante = new NivelContaminacion();
            
 //           nivelContaminante.Fecha = datosPM.FirstOrDefault().Fecha;
 //           nivelContaminante.Concentracion = indiceContaminancion;
 //           nivelContaminante.Color = buscarRango(indiceContaminancion , icas);
 //           nivelContaminante.Calificativo = buscarCalificativo(indiceContaminancion, icas);
 //           nivelContaminante.Recomendacion = buscarRecomendacion(indiceContaminancion, icas);
 //           nivelContaminante.ContaminanteID = contaminante.ContaminanteID;
 //           //nivelContaminante.Contaminante = contaminante;
 //           db.NivelContaminantes.Add(nivelContaminante);
        
           
 //       }

 //       private string buscarCalificativo(decimal indiceContaminancion, List<RangoICA> icas)
 //       {
 //           string res = "";
 //           foreach (var item in icas)
 //           {
 //               if (indiceContaminancion <= item.ValorMax)
 //               {
 //                   res = item.Calificativo;
 //                   return res;
 //               }
 //           }
 //           return res;
 //       }

 //       private string buscarRecomendacion(decimal indiceContaminancion, List<RangoICA> icas)
 //       {
 //           string res = "";
 //           foreach (var item in icas)
 //           {
 //               if (indiceContaminancion <= item.ValorMax)
 //               {
 //                   res = item.Efectos;
 //                   return res;
 //               }
 //           }
 //           return res;
 //       }

 //       private string buscarRango(decimal indiceContaminancion, List<RangoICA> Icas)
 //       {
 //           string res = "";
 //           foreach (var item in Icas)
 //           {
 //               if (indiceContaminancion <= item.ValorMax)
 //               {
 //                   res = item.Color;
 //                   return res;
 //               }
 //           }
 //           return res;
 //       }
     
 //       private decimal transformarICA(Decimal concentracion)
 //       {
 //           decimal res = 0;
 //           res = (concentracion * 100) / 150;
 //           return res;
 //       }
 //       public ActionResult goPrincipal()
 //       {
 //           return RedirectToAction("Index", "Admin");
 //       }