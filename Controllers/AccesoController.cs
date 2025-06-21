using Microsoft.AspNetCore.Mvc;
using PracticaLogin.Data;
using PracticaLogin.Models;
using Microsoft.EntityFrameworkCore;
using PracticaLogin.ViewModels;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MyApp.Namespace
{
    public class AccesoController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public AccesoController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpGet]
        public ActionResult Registrarse()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Registrarse(UsuarioVM model)
        {
            if (model.Clave != model.ConfirmarClave)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            var usuario = new Usuario()
            {
                NombreCompleto = model.NombreCompleto,
                Correo = model.Correo,
                Clave = model.Clave,

            };
            await _appDbContext.Usuarios.AddAsync(usuario);
            await _appDbContext.SaveChangesAsync();
            if (usuario.IdUsuario != 0) return RedirectToAction("Login", "Acceso");
            ViewData["Mensaje"] = "Error al crear el usuario";
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> Login()

        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");





            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginVM model)
        {
            Usuario? usuario_enconttrado = await _appDbContext.Usuarios.
            Where(u => u.Correo == model.Correo && u.Clave == model.Clave).FirstOrDefaultAsync();
            if (usuario_enconttrado == null)
            {
                ViewData["Mensaje"] = "No se encontro coincidencia";
                return View();
            }
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_enconttrado.NombreCompleto)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties
            );
            return RedirectToAction("Index", "Home");
        }

    }
}
