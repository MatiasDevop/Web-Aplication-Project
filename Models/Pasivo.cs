using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVClog.Models
{
    public class Pasivo
    {
        public int PasivoID { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Fecha { get; set; }
        public decimal Concentracion { get; set; }
        public int ContaminanteID { get; set; }
        public virtual Contaminante Contaminante { get; set; }
        public int EstacionId { get; set; }
        public virtual Estacion Estacion { get; set; }
    }
}