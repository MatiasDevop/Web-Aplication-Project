using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVClog.Models
{
    public class PmActivo
    {   [Key]
        public int ActivoID { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Concentracion { get; set; }

        public int ContaminanteID { get; set; }
        public virtual Contaminante Contaminante { get; set; }
    }
}