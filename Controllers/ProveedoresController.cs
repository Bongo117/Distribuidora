using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Distribuidora.Data;
using Distribuidora.Models;
using Microsoft.AspNetCore.Authorization; 

namespace Distribuidora.Controllers
{
    [Authorize] 
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [AllowAnonymous] 
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proveedores.ToListAsync());
        }

        [AllowAnonymous] 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

       
        [Authorize(Roles = "Empleado,Administrador")] 
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado,Administrador")] 
        public async Task<IActionResult> Create([Bind("Id,CUIT,RazonSocial,Email,Telefono,Direccion")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        
        [Authorize(Roles = "Empleado,Administrador")] 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado,Administrador")] 
        public async Task<IActionResult> Edit(int id, [Bind("Id,CUIT,RazonSocial,Email,Telefono,Direccion")] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.Id))
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
            return View(proveedor);
        }

        
        [Authorize(Roles = "Administrador")] 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")] 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExists(int id)
        {
            return _context.Proveedores.Any(e => e.Id == id);
        }

        /****************************************************
         * * DESDE ACÁ, LOS MÉTODOS PARA VUE.JS (API)
         * ****************************************************/

        // GET: /Proveedores/Listado
        // Este es el método que fallaba con 404
        [HttpGet]
        public async Task<IActionResult> Listado()
        {
            var proveedores = await _context.Proveedores.ToListAsync();
            // Devolvemos los datos en formato JSON
            return Json(proveedores);
        }

        // POST: /Proveedores/CreateAPI
        [HttpPost]
        // [FromBody] le dice a C# que el dato viene como JSON en el cuerpo del request
        public async Task<IActionResult> CreateAPI([FromBody] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                // Devolvemos el proveedor recién creado (con su nuevo Id)
                return Json(proveedor);
            }
            return BadRequest(ModelState); // Si falla, devuelve un error 400
        }

        // PUT: /Proveedores/EditAPI/5
        [HttpPut]
        public async Task<IActionResult> EditAPI(int id, [FromBody] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return BadRequest("IDs no coinciden");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(proveedor); // Devolvemos el objeto actualizado
            }
            return BadRequest(ModelState);
        }

        // DELETE: /Proveedores/DeleteAPI/5
        [HttpDelete]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return Ok(); // Devolvemos un simple "200 OK"
        }
    }
}