namespace OrderManagementAPI.Models {
    public enum OrderStatus {
        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Order {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Relacionamento com Customer (N:1 -> vários pedidos pertencem a um cliente)
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        // Um pedido tem vários itens (1:N)
        public List<OrderItem> Items { get; set; } = new();

        // Propriedade calculada, não vai pro banco - soma o total do pedido
        public decimal TotalAmount => Items.Sum(item => item.UnitPrice * item.Quantity);
    }
}