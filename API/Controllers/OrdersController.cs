using API.Data;
using API.DTO;
using API.Entities;
using API.Entities.OrderAggregate;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {

        private readonly StoreContext _context;
        public OrdersController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            return await _context.Orders.ProjectOrderToOrderDto().Where(x => x.BuyerId == User.Identity.Name).ToListAsync();
        }
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            return await _context.Orders.ProjectOrderToOrderDto().Where(x => x.BuyerId == User.Identity.Name && x.Id == id).FirstOrDefaultAsync();
        }
        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder(CreateOrderDto orderDto)
        {
            var basket = await _context.Baskets.RetriveBasketWithItems(User.Identity.Name).FirstOrDefaultAsync();

            if (basket == null) return BadRequest(new ProblemDetails { Title = "Could not locate basket" });
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _context.Products.FindAsync(item.ProductId);
                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };
                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;
            }

            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > 10000 ? 0 : 500;

            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = orderDto.ShipppingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee
            };

            _context.Orders.Add(order);
            _context.Baskets.Remove(basket);
            if (orderDto.SaveAddress)
            {
                var user = await _context.Users.Include(a => a.Address).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                var adddress = new UserAddress
                {
                    FullName = orderDto.ShipppingAddress.FullName,
                    Address1 = orderDto.ShipppingAddress.Address1,
                    Address2 = orderDto.ShipppingAddress.Address2,
                    City = orderDto.ShipppingAddress.City,
                    State = orderDto.ShipppingAddress.State,
                    Zip = orderDto.ShipppingAddress.Zip,
                    Country = orderDto.ShipppingAddress.Country,
                };
                user.Address = adddress;
            }
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);


            return BadRequest("Problem creating order");

        }

    }
}