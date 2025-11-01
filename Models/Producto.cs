using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Distribuidora.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioCosto { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioVenta { get; set; }
        
        public int StockActual { get; set; }
        public string FotoUrl { get; set; } = string.Empty;

        [ForeignKey("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; } 
    }
}