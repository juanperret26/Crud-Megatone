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
    public class MarcasController : Controller
    {
        private readonly MercaderiaContext _context;

        public MarcasController(MercaderiaContext context)
        {
            _context = context;
        }

        // GET: Marcas
        public async Task<IActionResult> Index()
        {
              return _context.Marcas != null ? 
                          View(await _context.Marcas.ToListAsync()) :
                          Problem("Entity set 'MercaderiaContext.Marcas'  is null.");
        }

        // GET: Marcas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // GET: Marcas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Marcas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,FechaModificacion,Baja,FechaBaja")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                marca.FechaModificacion = DateTime.Now;  // Establecer la fecha de modificación
                _context.Add(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }

        // GET: Marcas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,FechaModificacion,Baja,FechaBaja")] Marca marca)
        {
            if (id != marca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Solo permitir modificar la descripción y actualizar la fecha de modificación
                var marcaExistente = await _context.Marcas.FindAsync(id);
                if (marcaExistente != null)
                {
                    marcaExistente.Descripcion = marca.Descripcion;
                    marcaExistente.FechaModificacion = DateTime.Now;
                    _context.Update(marcaExistente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            return View(marca);
        }

        // GET: Marcas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // POST: Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Marcas == null)
            {
                return Problem("Entity set 'MercaderiaContext.Marcas' is null.");
            }

            var marca = await _context.Marcas.FindAsync(id);
            if (marca != null)
            {
                // Verificar si hay productos asociados activos
                var productosAsociados = await _context.Productos.AnyAsync(p => p.IdMarca == id && p.Baja == false);
                if (productosAsociados)
                {
                    ModelState.AddModelError(string.Empty, "No se puede borrar la marca porque tiene productos asociados activos.");
                    return View("Delete", marca);
                }

                // Marcar como baja lógica y establecer la fecha de baja
                marca.Baja = true;
                marca.FechaBaja = DateTime.Now;

                _context.Update(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();

        }

        private bool MarcaExists(int id)
        {
          return (_context.Marcas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
