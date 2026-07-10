using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Controllers {
    // Indica que esta classe é um controller de API
    [ApiController]

    // Define a rota base como "api/customers"
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase {
        // Contexto do banco de dados
        private readonly AppDbContext _context;

        // Recebe o AppDbContext por injeção de dependência
        public CustomersController(AppDbContext context) {
            _context = context;
        }

        // Retorna todos os clientes cadastrados
        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomers() {
            var customers = await _context.Customers.ToListAsync();

            // Converte cada Customer para CustomerResponseDto
            return customers.Select(MapToDto).ToList();
        }

        // Retorna um cliente pelo ID
        // GET: api/customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> GetCustomer(int id) {
            var customer = await _context.Customers.FindAsync(id);

            // Retorna 404 caso o cliente não exista
            if (customer == null)
                return NotFound();

            return MapToDto(customer);
        }

        // Cadastra um novo cliente
        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerResponseDto>> CreateCustomer(CreateCustomerDto dto) {
            // Cria um novo objeto Customer com os dados recebidos
            var customer = new Customer {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            // Adiciona o cliente ao contexto
            _context.Customers.Add(customer);

            // Salva as alterações no banco
            await _context.SaveChangesAsync();

            // Retorna 201 Created com a rota do novo recurso
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, MapToDto(customer));
        }

        // Atualiza um cliente existente
        // PUT: api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CreateCustomerDto dto) {
            var customer = await _context.Customers.FindAsync(id);

            // Retorna 404 caso o cliente não exista
            if (customer == null)
                return NotFound();

            // Atualiza os dados do cliente
            customer.Name = dto.Name;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;

            // Salva as alterações
            await _context.SaveChangesAsync();

            // Retorna 204 indicando sucesso sem conteúdo
            return NoContent();
        }

        // Remove um cliente
        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id) {
            var customer = await _context.Customers.FindAsync(id);

            // Retorna 404 caso o cliente não exista
            if (customer == null)
                return NotFound();

            // Remove o cliente do banco
            _context.Customers.Remove(customer);

            // Salva as alterações
            await _context.SaveChangesAsync();

            // Retorna 204 indicando que a exclusão foi realizada
            return NoContent();
        }

        // Converte a entidade Customer em CustomerResponseDto
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