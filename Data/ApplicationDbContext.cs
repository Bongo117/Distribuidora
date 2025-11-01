using Microsoft.EntityFrameworkCore;
using Distribuidora.Models; // <-- AÑADIDO
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Distribuidora.Data
{
    // CAMBIADO a IdentityDbContext<Usuario>
    public class ApplicationDbContext : IdentityDbContext<Usuario> 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalle> PedidoDetalles { get; set; }
        public DbSet<Pago> Pagos { get; set; }
    }
}