using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;

namespace HankoSpa.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userServive;

        public UserController(IUserService userServive)
        {
            _userServive = userServive;
        }

        // GET: User
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }

}