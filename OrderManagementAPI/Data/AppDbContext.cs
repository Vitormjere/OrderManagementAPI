using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Data {
    // Contexto do banco de dados da aplicação
    public class AppDbContext : DbContext {
        // Recebe as configurações do banco de dados
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }

        // Tabelas do banco de dados
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // Configurações adicionais das entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Define a precisão do preço do produto como decimal(10,2)
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            // Define a precisão do preço unitário do item do pedido como decimal(10,2)
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(10, 2);
        }
    }
}