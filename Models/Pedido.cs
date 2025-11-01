using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Distribuidora.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; } = string.Empty;

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; } 

        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
        public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
    }
}