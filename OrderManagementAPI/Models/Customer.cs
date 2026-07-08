namespace OrderManagementAPI.Models {
    public class Customer {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Um cliente pode ter vários pedidos (relacionamento 1:N)
        public List<Order> Orders { get; set; } = new();
    }
}