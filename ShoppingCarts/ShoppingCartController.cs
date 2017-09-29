using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using checkout_service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCarts
{
    public class ShoppingCartController : Controller
    {
        private readonly CheckoutDbContext dbContext;

        public ShoppingCartController(CheckoutDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Route("/shopping-carts"), HttpGet]
        public async Task<IActionResult> List()
        {
            var results = await dbContext.ShoppingCart.ToListAsync();
            return Ok(results);
        }

        [Route("/shopping-carts"), HttpPost]
        public async Task<IActionResult> Create([FromBody] ShoppingCart cart)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbContext.ShoppingCart.Add(cart);
            await dbContext.SaveChangesAsync();

            return Ok(cart);
        } 

        [Route("/shopping-carts/{id}"), HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var cart = await dbContext.ShoppingCart
                .Include(sc => sc.LineItems)
                .FirstOrDefaultAsync(c => c.Id == id);

            return cart == null 
                ? (IActionResult)NotFound() 
                : Ok(cart);
        }

        [Route("/shopping-carts/{cartId}/line-items"), HttpPost]
        public async Task<IActionResult> AddLineItem(int cartId, [FromBody] LineItem lineItem)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cart = await dbContext.ShoppingCart
                .Include(sc => sc.LineItems)
                .FirstOrDefaultAsync(c => c.Id == cartId);
            if(cart == null)
            {
                return NotFound();
            }
            if(cart.LineItems == null)
            {
                cart.LineItems = new List<LineItem>();
            }

            var existingLi = cart.LineItems
                .FirstOrDefault(li => li.ProductId == lineItem.ProductId);

            if(existingLi != null)
            {
                existingLi.Quantity += lineItem.Quantity;
            }
            else
            {
                //existingLi.ShoppingCartId = cartId;
                cart.LineItems.Add(lineItem);
            }

            await dbContext.SaveChangesAsync();
            return Ok(cart);
        }

        [Route("/shopping-carts/{cartId}/line-items/{lineItemId}"), HttpDelete]
        public async Task<IActionResult> DeleteLineItem(int cartId, int lineItemId)
        {
            var cart = await dbContext.ShoppingCart
                .Include(sc => sc.LineItems)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            var lineItem = cart.LineItems.FirstOrDefault(li => li.Id == lineItemId);
            cart.LineItems.Remove(lineItem);

            await dbContext.SaveChangesAsync();
            return Ok(cart);
        }
    }
}