
using System.Collections.Generic;

namespace ShoppingCarts
{
    public class ShoppingCart
    {
        public int Id { get; internal set; }

        public IList<LineItem> LineItems { get; set; }
    }
}