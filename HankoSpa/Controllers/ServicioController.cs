using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;

namespace HankoSpa.Controllers
{
    public class ServicioController : Controller
    {
        private readonly IServicioService _servicioService;

        public ServicioController(IServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        // GET: Servicio
        public async Task<IActionResult> Index()
        {
            var response = await _servicioService.GetAllAsync();

            if (!response.IsSuccess)
            {
                ViewBag.ErrorMessage = response.Message;
                return View(new List<ServiceDTO>());
            }

            return View(response.Result);
        }

    }
}

