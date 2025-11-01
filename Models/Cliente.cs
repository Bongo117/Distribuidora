using System.ComponentModel.DataAnnotations;

namespace Distribuidora.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        public string CUIT { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string DireccionEnvio { get; set; } = string.Empty;

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}