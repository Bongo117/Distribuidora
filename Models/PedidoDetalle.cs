using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Distribuidora.Models
{
    public class PedidoDetalle
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pedido")]
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; } 

        [ForeignKey("Producto")]
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; } 

        public int Cantidad { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioUnitario { get; set; }
    }
}