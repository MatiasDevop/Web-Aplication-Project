using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVClog.Models
{
    public class Role
    {

        [Key]
        [DisplayName("ID")]
        public int RoleID { get; set; }
        [Required(ErrorMessage = "No puede dejar este campo en blanco.")]
        [DisplayName("Rol")]
        public string RoleName { get; set; }
        // 1 a muchos
        public ICollection<User> Users { get; set; }
    }
}