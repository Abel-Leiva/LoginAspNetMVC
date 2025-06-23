using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PracticaLogin.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El nombre no debe superar los 50 caracteres")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ser un correo válido")]
        [MaxLength(50)]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        [MinLength(6, ErrorMessage = "La clave debe tener al menos 6 caracteres")]
        [MaxLength(150)]
        public string Clave { get; set; }
    }
}
