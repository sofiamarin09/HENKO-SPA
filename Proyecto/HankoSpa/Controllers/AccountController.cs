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
            return RedirectToAction(nameof(Login));
        }
    }
}
