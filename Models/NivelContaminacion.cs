using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVClog.Models
{
    public class NivelContaminacion
    {
        [Key]
        public int NivelID { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Concentracion { get; set; }
        public string Color { get; set; }
        public string Calificativo { get; set; }
        public string  Recomendacion { get; set; }
        public int ContaminanteID { get; set; }
        public virtual Contaminante Contaminante { get; set; }
        public int EstacionId { get; set; }
        public virtual Estacion Estacion { get; set; }
     
    }
}