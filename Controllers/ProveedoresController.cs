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
    // Todo el controlador requiere que el usuario haya iniciado sesión y tenga uno de estos roles.
    [Authorize(Roles = "Administrador,Supervisor,Empleado")]
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Proveedores o /Proveedores/Index
        // Esta acción simplemente devuelve la vista que contiene la app de Vue.js.
        // No necesita ser asíncrona ni pasarle datos. Vue se encargará de pedirlos.
        public IActionResult Index()
        {
            return View();
        }

        private bool ProveedorExists(int id)
        {
            return _context.Proveedores.Any(e => e.Id == id);
        }

        /*****************************************************************
         *  API ENDPOINTS PARA VUE.JS
         *  Estos métodos son llamados por Axios desde el frontend.
         *  Están protegidos por el [Authorize] a nivel de clase.
         *****************************************************************/
 
        [HttpGet]
        public async Task<IActionResult> Listado()
        {
            var proveedores = await _context.Proveedores.ToListAsync();
            return Ok(proveedores);
        }

        [HttpPost]
        // El [FromBody] es crucial para que ASP.NET entienda el JSON enviado por Axios.
        public async Task<IActionResult> CreateAPI([FromBody] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                // Devolvemos el objeto creado (con su nuevo ID) y un código 201 (Created).
                return CreatedAtAction(nameof(Listado), new { id = proveedor.Id }, proveedor);
            }
            return BadRequest(ModelState);
        }

        // Es buena práctica incluir el ID en la ruta para seguir el estándar REST.
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAPI(int id, [FromBody] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return BadRequest();
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
                    if (!ProveedorExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Un 204 (No Content) es una respuesta estándar para un PUT exitoso.
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        // Solo los administradores pueden borrar.
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            // Un 204 (No Content) es una respuesta estándar para un DELETE exitoso.
            return NoContent();
        }
    }
}