using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVClog.Models
{
    public class Estacion
    {
        public int EstacionId { get; set; }
        public string Name { get; set; }
        public string Direction { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
      
    }
}