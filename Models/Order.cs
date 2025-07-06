using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LojaOnline.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}