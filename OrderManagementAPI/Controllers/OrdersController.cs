using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context) {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders() {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(item => item.Product)
                .ToListAsync();

            return orders.Select(MapToDto).ToList();
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int id) {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return MapToDto(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder(CreateOrderDto dto) {
            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                return BadRequest("Customer not found.");

            var order = new Order {
                CustomerId = dto.CustomerId,
                Status = OrderStatus.Pending
            };

            foreach (var itemDto in dto.Items) {
                var product = await _context.Products.FindAsync(itemDto.ProductId);

                if (product == null)
                    return BadRequest($"Product with id {itemDto.ProductId} not found.");

                if (product.StockQuantity < itemDto.Quantity)
                    return BadRequest($"Insufficient stock for product '{product.Name}'. Available: {product.StockQuantity}, requested: {itemDto.Quantity}");

                product.StockQuantity -= itemDto.Quantity;

                order.Items.Add(new OrderItem {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var createdOrder = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, MapToDto(createdOrder!));
        }

        // PUT: api/orders/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderStatusDto dto) {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            order.Status = dto.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id) {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static OrderResponseDto MapToDto(Order order) {
            return new OrderResponseDto {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Customer = new CustomerSummaryDto {
                    Id = order.Customer!.Id,
                    Name = order.Customer.Name,
                    Email = order.Customer.Email
                },
                Items = order.Items.Select(item => new OrderItemResponseDto {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    ProductName = item.Product!.Name
                }).ToList()
            };
        }
    }
}