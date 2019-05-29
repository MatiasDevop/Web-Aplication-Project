using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVClog.Models
{
    public class User
    {
        [Key]// id llave primaria
        public int UserID { get; set; }

        [DisplayName("Usuario")]
        [Required(ErrorMessage = "No puede dejar este campo en blanco.")]
        public string UserName { get; set; }

        [DisplayName("Contraseña")]
        [Required(ErrorMessage = "No puedes dejar este campo en blanco"), DataType(DataType.Password)]
        public string Password { get; set; }
            
        [DisplayName("Habilitado")]
        public bool IsApproved { get; set; }

        //tipo fecha
        [DisplayName("Fecha de Registro")]
        [DataType(DataType.Date)] // signo es nulable
        public DateTime? CreateDate { get; set; }

        [DisplayName("Nombres")]
        [Required(ErrorMessage = "No puede dejar este campo en blanco.")]
        public string Name { get; set; }

        [DisplayName("Apellidos")]
        [Required(ErrorMessage = "No puede dejar este campo en blanco.")]
        public string LastName { get; set; }


        [DisplayName("Nombre completo")]
        public string FullName
        {       // para ahorrar campo
            get { return Name + " " + LastName; }
        }

        public virtual ICollection<Role> Roles { get; set; }
    }
}