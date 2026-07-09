using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context) {
            _context = context;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomers() {
            var customers = await _context.Customers.ToListAsync();
            return customers.Select(MapToDto).ToList();
        }

        // GET: api/customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> GetCustomer(int id) {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            return MapToDto(customer);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerResponseDto>> CreateCustomer(CreateCustomerDto dto) {
            var customer = new Customer {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, MapToDto(customer));
        }

        // PUT: api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CreateCustomerDto dto) {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            customer.Name = dto.Name;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id) {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static CustomerResponseDto MapToDto(Customer customer) {
            return new CustomerResponseDto {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            };
        }
    }
}