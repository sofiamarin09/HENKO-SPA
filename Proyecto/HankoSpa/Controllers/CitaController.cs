using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HankoSpa.Data;
using HankoSpa.Models;

namespace HankoSpa.Controllers
{
    public class CitasController : Controller
    {
        private readonly AppDbContext _context;

        public CitasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            var citas = await _context.Citas
                .OrderBy(c => c.FechaCita)
                .ThenBy(c => c.HoraCita)
                .ToListAsync();

            return View(citas);
        }

        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.CitaId == id);

            if (cita == null) return NotFound();

            return View(cita);
        }

        // GET: Citas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Citas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitaDTO cita)
        {
            
            if (ModelState.IsValid)
            {

                // Simulaccita.UsuarioID = 1;ión de usuario (reemplazar con ID real de usuario autenticado en el futuro)
                //cita.UsuarioID = 1;
                _context.Add(cita);
                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "✅ La cita fue agendada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(cita);
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return NotFound();

            return View(cita);
        }

        // POST: Citas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cita cita)
        {
            if (id != cita.CitaId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Mantener el valor de UsuarioID si no se muestra en la vista
                    var citaExistente = await _context.Citas.AsNoTracking().FirstOrDefaultAsync(c => c.CitaId == id);
                    if (citaExistente != null)
                    {
                        cita.UsuarioID = citaExistente.UsuarioID;
                    }

                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                    TempData["MensajeExito"] = "✅ La cita fue actualizada correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Citas.Any(e => e.CitaId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.CitaId == id);

            if (cita == null) return NotFound();

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "✅ La cita fue eliminada con éxito.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
