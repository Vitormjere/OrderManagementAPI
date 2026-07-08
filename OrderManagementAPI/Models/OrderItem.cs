namespace OrderManagementAPI.Models {
    public class OrderItem {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Relacionamento com Order (esse item pertence a um pedido)
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // Relacionamento com Product (esse item referencia um produto)
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}