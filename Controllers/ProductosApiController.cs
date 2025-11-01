using Distribuidora.Data; 
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; 

public class ProductoDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string? Descripcion { get; set; } = "";
    public decimal PrecioVenta { get; set; }
    public int StockActual { get; set; }
    public string? FotoUrl { get; set; }
    public string ProveedorNombre { get; set; } = "";
}

[ApiController]
[Route("api/productos")]

// ¡¡ESTA ES LA LÍNEA MÁS IMPORTANTE!!
// Este controlador SÓLO funciona con autenticación JWT
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class ProductosApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductosApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductos()
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        
        var productos = await _context.Productos
            .Include(p => p.Proveedor)
            .Select(p => new ProductoDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                PrecioVenta = p.PrecioVenta,
                StockActual = p.StockActual,
                FotoUrl = p.FotoUrl,
                ProveedorNombre = p.Proveedor != null ? p.Proveedor.RazonSocial : "N/A"
            })
            .ToListAsync();

        return Ok(new { 
            message = $"Hola {userEmail}, aquí están los productos.",
            data = productos 
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProducto(int id)
    {
        var producto = await _context.Productos
                                .Include(p => p.Proveedor)
                                .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
        {
            return NotFound();
        }

        return Ok(producto);
    }
}