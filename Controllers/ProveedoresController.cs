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
    // rruta base para todos los endpoints de API en este controlador
    [Route("api/[controller]")]
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Proveedores o /Proveedores/Index
        // Esta acción simplemente devuelve la vista que contiene la app de Vue.js.
        // La hacemos "no-API" para que no entre en conflicto con la ruta base de la API.
        [HttpGet("/Proveedores")] 
        [ApiExplorerSettings(IgnoreApi = true)] // Opcional: Oculta esta acción de la documentación de la API (Swagger)
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
 
        // GET: api/Proveedores
        [HttpGet]
        public async Task<IActionResult> GetProveedores()
        {
            var proveedores = await _context.Proveedores.ToListAsync();
            return Ok(proveedores);
        }

        // POST: api/Proveedores
        [HttpPost]
        // El [FromBody] es crucial para que ASP.NET entienda el JSON enviado por Axios.
        public async Task<IActionResult> CreateProveedor([FromBody] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                // Devolvemos el objeto creado (con su nuevo ID) y un código 201 (Created).
                return CreatedAtAction(nameof(GetProveedor), new { id = proveedor.Id }, proveedor);
            }
            return BadRequest(ModelState);
        }

        // GET: api/Proveedores/5 (Endpoint para obtener un solo proveedor, buena práctica tenerlo)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return Ok(proveedor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProveedor(int id, [FromBody] Proveedor proveedor)
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
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Proveedores/5
        [HttpDelete("{id}")]
        // Solo el admin pueden borrar.
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}