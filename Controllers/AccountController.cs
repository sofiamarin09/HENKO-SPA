using HankoSpa.DTOs;
using HankoSpa.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HankoSpa.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService usersService)
        {
            _userService = usersService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userService.LoginAsync(dto);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrecta");
            }
            return View(dto);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterClientViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Crear el DTO de usuario para el servicio
            var userDto = new UserDTO
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Document = model.Document,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber, // <-- Línea agregada para guardar el teléfono
                // UserName = model.Email, // Elimina o comenta esta línea
                CustomRolId = 3, // 3 = Cliente
                Password = model.Password
            };

            var result = await _userService.CreateUserAsync(userDto, model.Password);

            if (result.Succeeded)
            {
                // Puedes redirigir al login o mostrar mensaje de éxito
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpGet]
        [Route("/Errors/{statuscode:int}")]
        [AllowAnonymous]
        public IActionResult Error(int statuscode)
        {
            string errorMessage = "Ha ocurrido un error";
            switch (statuscode)
            {
                case StatusCodes.Status401Unauthorized:
                    errorMessage = "Debes iniciar sesion";
                    break;
                case StatusCodes.Status403Forbidden:
                    errorMessage = "No tienes permiso para estar aqui";
                    break;
                case StatusCodes.Status404NotFound:
                    errorMessage = "La pagina que estas intentando acceder no existe";
                    break;
            }
            ViewBag.ErrorMessage = errorMessage;
            return View(statuscode);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult NotAuthorized()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction("Index", "Home"); // Redirige a la página principal
        }
    }
}

