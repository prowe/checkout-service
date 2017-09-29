using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ShoppingCarts
{
    public class LineItem
    {
        public int Id { get; internal set; }

        [Required]
        public int ProductId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
    }
}