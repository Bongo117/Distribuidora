using Distribuidora.Data;
using Distribuidora.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Distribuidora.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor,Empleado")]
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public PedidosController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var pedidos = await _context.Pedidos.Include(p => p.Cliente).ToListAsync();
            return View(pedidos);
        }

        public IActionResult Create()
        {
            // Creo un PedidoViewModel para pasar datos a la vista de creación.
            // Por ahora está vacío, pero luego contendrá la lógica para buscar clientes y productos.
            var pedidoViewModel = new Pedido(); // Más adelante creo un ViewModel específico.
            return View(pedidoViewModel);
        }
    }
}