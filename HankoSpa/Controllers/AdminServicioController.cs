// AdminServicioController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HankoSpa.Data;
using HankoSpa.Models;

namespace HankoSpa.Controllers
{
    public class AdminServicioController : Controller
    {
        private readonly AppDbContext _context;

        public AdminServicioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminServicio/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminServicio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servicio);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Servicio");
            }
            return View(servicio);
        }

        // GET: AdminServicio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null) return NotFound();

            return View(servicio);
        }

        // POST: AdminServicio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Servicio servicio)
        {
            if (id != servicio.ServicioId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Servicios.Any(e => e.ServicioId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index", "Servicio");
            }
            return View(servicio);
        }

        // GET: AdminServicio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var servicio = await _context.Servicios
                .FirstOrDefaultAsync(m => m.ServicioId == id);
            if (servicio == null) return NotFound();

            return View(servicio);
        }

        // POST: AdminServicio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Servicio");
        }
    }
}
