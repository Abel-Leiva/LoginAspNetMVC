
using System.ComponentModel.DataAnnotations;

namespace PracticaLogin.ViewModels
{
    public class UsuarioVM
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener al menos 3 letras")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre no puede contener números ni símbolos")]
        public string NombreCompleto { get; set; }




        [Required(ErrorMessage = "El correo es obligatorio")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Formato de correo inválido")]
        public string Correo { get; set; }



        [Required(ErrorMessage = "La clave es obligatoria")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "La clave debe tener entre 6 y 15 caracteres")]
        public string Clave { get; set; }


        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarClave { get; set; }
    }

}