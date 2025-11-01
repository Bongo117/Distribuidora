using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Distribuidora.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }
        public DateTime FechaPago { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Monto { get; set; }
        public bool Anulado { get; set; }

        [ForeignKey("Pedido")]
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; } 
    }
}