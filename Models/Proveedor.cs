using System.ComponentModel.DataAnnotations;

namespace Distribuidora.Models
{
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }
        public string CUIT { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}