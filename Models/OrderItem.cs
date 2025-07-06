using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaOnline.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }
        // Você pode criar uma relação Product se já existir um Model Product
        // public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}