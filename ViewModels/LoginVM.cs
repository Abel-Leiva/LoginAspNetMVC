using System.ComponentModel.DataAnnotations;

namespace PracticaLogin.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Formato de correo inválido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "La clave debe tener entre 6 y 15 caracteres")]
        public string Clave { get; set; }
    }
}
