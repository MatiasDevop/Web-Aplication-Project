using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVClog.Models
{
    public class Contaminante
    {
        public int ContaminanteID { get; set; }

        [Required(ErrorMessage = "No puede dejar este campo en blanco.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "No puede dejar este campo en blanco.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "No puede dejar este campo en blanco.")]
        public int ValorLimite { get; set; }

        [Required(ErrorMessage = "No puede dejar este campo en blanco.")]
        public string Unidades { get; set; }
        
        
    }
}