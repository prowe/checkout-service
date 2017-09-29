
using Microsoft.EntityFrameworkCore;
using ShoppingCarts;

namespace checkout_service
{
    public class CheckoutDbContext : DbContext
    {
        public DbSet<ShoppingCart> ShoppingCart {get;set;}

        public CheckoutDbContext(DbContextOptions<CheckoutDbContext> options) : base(options)
        {
        }
    }
}