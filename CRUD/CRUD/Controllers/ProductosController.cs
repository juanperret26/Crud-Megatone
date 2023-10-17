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
    public class ProductosController : Controller
    {
        private readonly MercaderiaContext _context;

        public ProductosController(MercaderiaContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            var mercaderiaContext = _context.Productos.Include(p => p.IdFamiliaNavigation).Include(p => p.IdMarcaNavigation);
            return View(await mercaderiaContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdFamiliaNavigation)
                .Include(p => p.IdMarcaNavigation)
                .FirstOrDefaultAsync(m => m.CodigoProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["IdFamilia"] = new SelectList(_context.Familia, "Id", "Id");
            ViewData["IdMarca"] = new SelectList(_context.Marcas, "Id", "Id");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodigoProducto,Descripcion,PrecioCosto,PrecioVenta,IdMarca,IdFamilia,FechaModificacion,Baja,FechaBaja")] Producto producto)
        {
            _context.Add(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["IdFamilia"] = new SelectList(_context.Familia, "Id", "Id", producto.IdFamilia);
            ViewData["IdMarca"] = new SelectList(_context.Marcas, "Id", "Id", producto.IdMarca);
            return View(producto);

        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["IdFamilia"] = new SelectList(_context.Familia, "Id", "Id", producto.IdFamilia);
            ViewData["IdMarca"] = new SelectList(_context.Marcas, "Id", "Id", producto.IdMarca);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodigoProducto,Descripcion,PrecioCosto,PrecioVenta,IdMarca,IdFamilia,FechaModificacion,Baja,FechaBaja")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var productoExistente = await _context.Productos.FindAsync(id);
                if (productoExistente != null)
                {
                    // Solo permitir editar ciertos campos y actualizar la fecha de modificación
                    productoExistente.Descripcion = producto.Descripcion;
                    productoExistente.IdMarca = producto.IdMarca;
                    productoExistente.IdFamilia = producto.IdFamilia;
                    productoExistente.FechaModificacion = DateTime.Now;

                    _context.Update(productoExistente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            ViewData["IdFamilia"] = new SelectList(_context.Familia, "Id", "Id", producto.IdFamilia);
            ViewData["IdMarca"] = new SelectList(_context.Marcas, "Id", "Id", producto.IdMarca);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdFamiliaNavigation)
                .Include(p => p.IdMarcaNavigation)
                .FirstOrDefaultAsync(m => m.CodigoProducto==id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'MercaderiaContext.Productos' is null.");
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                // Marcar como baja lógica y establecer la fecha de baja
                producto.Baja = true;
                producto.FechaBaja = DateTime.Now;

                _context.Update(producto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(string id)
        {
            return (_context.Productos?.Any(e => e.CodigoProducto == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Listado(string codigoProducto, int? idMarca, int? idFamilia)
        {
            var productos = _context.Productos
                .Where(p =>
                    (string.IsNullOrEmpty(codigoProducto) || p.CodigoProducto.Contains(codigoProducto)) &&
                    (!idMarca.HasValue || p.IdMarca == idMarca) &&
                    (!idFamilia.HasValue || p.IdFamilia == idFamilia) &&
                    p.Baja == false
                )
                .OrderBy(p => p.FechaModificacion)
                .ToList();

            return View("Index", productos.AsEnumerable());
        }
    }
}
