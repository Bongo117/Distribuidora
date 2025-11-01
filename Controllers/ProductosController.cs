using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Distribuidora.Data;
using Distribuidora.Models;
using System.IO;

namespace Distribuidora.Controllers
{
    [Authorize(Roles = "Empleado,Administrador")]
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Productos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Productos.Include(p => p.Proveedor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "RazonSocial");
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,PrecioCosto,PrecioVenta,StockActual,ProveedorId")] Producto producto, IFormFile? foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string uploadsDir = Path.Combine(wwwRootPath, "Uploads", "Productos");

                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
                    string filePath = Path.Combine(uploadsDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                    }
                    producto.FotoUrl = "/Uploads/Productos/" + fileName; 
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "RazonSocial", producto.ProveedorId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "RazonSocial", producto.ProveedorId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,PrecioCosto,PrecioVenta,StockActual,ProveedorId")] Producto producto, IFormFile? foto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Si no se sube una foto nueva, necesitamos mantener la antigua.
                    // Para eso, primero recuperamos el producto original de la BD.
                    if (foto == null)
                    {
                        var productoExistente = await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                        if (productoExistente != null) {
                            producto.FotoUrl = productoExistente.FotoUrl;
                        }
                    }
                    if (foto != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string uploadsDir = Path.Combine(wwwRootPath, "Uploads", "Productos");

                        if (!Directory.Exists(uploadsDir))
                        {
                            Directory.CreateDirectory(uploadsDir);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
                        string filePath = Path.Combine(uploadsDir, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await foto.CopyToAsync(fileStream);
                        }
                        producto.FotoUrl = "/Uploads/Productos/" + fileName;
                    }

                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "RazonSocial", producto.ProveedorId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                // --- LÓGICA PARA BORRAR LA FOTO (NUEVO) ---
                if (!string.IsNullOrEmpty(producto.FotoUrl))
                {
                    // Construimos la ruta completa al archivo en wwwroot
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, producto.FotoUrl.TrimStart('/'));

                    // Verificamos si el archivo existe y lo borramos
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
