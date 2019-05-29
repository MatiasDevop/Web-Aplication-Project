using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVClog.Models
{
    public class RangoICA
    {
        
        public int ID { get; set; }
        public int ValorMin { get; set; }
        public int ValorMax { get; set; }
        public string Color { get; set; }
        public string Calificativo { get; set; }
        public string Riesgo { get; set; }
        public string Efectos { get; set; }
    }
}