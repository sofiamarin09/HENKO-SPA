// ServicioController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HankoSpa.Data;
using HankoSpa.Models;

namespace HankoSpa.Controllers
{
    public class ServicioController : Controller
    {
        private readonly AppDbContext _context;

        public ServicioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Servicio
        public async Task<IActionResult> Index()
        {
            return View(await _context.Servicios.ToListAsync());
        }

        // GET: Servicio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var servicio = await _context.Servicios
                .FirstOrDefaultAsync(m => m.ServicioId == id);
            if (servicio == null) return NotFound();

            return View(servicio);
        }
    }
}

