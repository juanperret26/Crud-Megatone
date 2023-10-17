using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD.Models;

namespace CRUD.Controllers
{
    public class FamiliaController : Controller
    {
        private readonly MercaderiaContext _context;
        

        public FamiliaController(MercaderiaContext context)
        {
            _context = context;
            
        }

        // GET: Familia
        public async Task<IActionResult> Index()
        {
              return _context.Familia != null ? 
                          View(await _context.Familia.ToListAsync()) :
                          Problem("Entity set 'MercaderiaContext.Familia'  is null.");
        }

        // GET: Familia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Familia == null)
            {
                return NotFound();
            }

            var familia = await _context.Familia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (familia == null)
            {
                return NotFound();
            }

            return View(familia);
        }

        // GET: Familia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Familia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,FechaModificacion,Baja,FechaBaja")] Familia familia)
        {
            if (ModelState.IsValid)
            {
                familia.FechaModificacion=DateTime.Now;
                _context.Add(familia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(familia);
        }

        // GET: Familia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Familia == null)
            {
                return NotFound();
            }

            var familia = await _context.Familia.FindAsync(id);
            if (familia == null)
            {
                return NotFound();
            }
            return View(familia);
        }

        // POST: Familia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,FechaModificacion,Baja,FechaBaja")] Familia familia)
        {
            if (id != familia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Solo permitir modificar la descripción y actualizar la fecha de modificación
                var existingFamilia = await _context.Familia.FindAsync(id);
                if (existingFamilia != null)
                {
                    existingFamilia.Descripcion = familia.Descripcion;
                    existingFamilia.FechaModificacion = DateTime.Now;
                    _context.Update(existingFamilia);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            return View(familia);
        }

        // GET: Familia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Familia == null)
            {
                return NotFound();
            }

            var familia = await _context.Familia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (familia == null)
            {
                return NotFound();
            }

            return View(familia);
        }

        // POST: Familia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Familia == null)
            {
                return Problem("Entity set 'MercaderiaContext.Familia'  is null.");
            }
            var familia = await _context.Familia.FindAsync(id);
            if (familia != null)
            {
                var productosAsociados = await _context.Productos.AnyAsync(p => p.IdFamilia == id && p.Baja == false);
                if (productosAsociados)
                {
                    ModelState.AddModelError(string.Empty, "No se puede borrar la familia porque tiene productos asociados activos.");
                    return View("Delete", familia);
                }
                familia.Baja = true;
                familia.FechaBaja = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
        public IActionResult FamilyHasProducts()
        {
            return View();
        }
        private bool FamiliaExists(int id)
        {
          return (_context.Familia.Any(e => e.Id == id));
        }
    }
}
