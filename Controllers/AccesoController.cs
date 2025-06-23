using Microsoft.AspNetCore.Mvc;
using PracticaLogin.Data;
using PracticaLogin.Models;
using Microsoft.EntityFrameworkCore;
using PracticaLogin.ViewModels;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

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

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            if (model.Clave != model.ConfirmarClave)
            {
                ModelState.AddModelError("ConfirmarClave", "Las contraseñas no coinciden");
                return View(model);
            }


            //
            var correoExiste = await _appDbContext.Usuarios
                .AnyAsync(u => u.Correo == model.Correo);
            if (correoExiste)
            {
                ModelState.AddModelError("Correo", "Ya existe un usuario con ese correo");
                return View(model);
            }

            //
            var hasher = new PasswordHasher<object>();
            var hashedPassword = hasher.HashPassword(null, model.Clave);


            var usuario = new Usuario()
            {
                NombreCompleto = model.NombreCompleto,
                Correo = model.Correo,
                Clave = hashedPassword
            };


            await _appDbContext.Usuarios.AddAsync(usuario);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Login", "Acceso");
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _appDbContext.Usuarios
                .Where(u => u.Correo == model.Correo)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                ViewData["Mensaje"] = "No se encontró el usuario";
                return View(model);
            }

            var hasher = new PasswordHasher<object>();
            var result = hasher.VerifyHashedPassword(null, usuario.Clave, model.Clave);

            if (result != PasswordVerificationResult.Success)
            {
                ViewData["Mensaje"] = "Contraseña incorrecta";
                return View(model);
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.NombreCompleto)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties
            );

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult LoginConGoogle(string returnUrl = "/")
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse", new { returnUrl }) };
            return Challenge(properties, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (email == null)
            {
                return RedirectToAction("Login");
            }

            var usuario = await _appDbContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == email);

            if (usuario == null)
            {
                usuario = new Usuario()
                {
                    NombreCompleto = name ?? email,
                    Correo = email,
                    Clave = "" // o null, según modelo y base de datos
                };
                _appDbContext.Usuarios.Add(usuario);
                await _appDbContext.SaveChangesAsync();
            }

            var appClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Email, usuario.Correo)
            };

            var identity = new ClaimsIdentity(appClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return LocalRedirect(returnUrl);
        }
    }
}

