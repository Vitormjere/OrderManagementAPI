using OrderManagementAPI.Models;

namespace OrderManagementAPI.DTOs {
    public class OrderResponseDto {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }

        public CustomerSummaryDto Customer { get; set; } = null!;
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }

    public class CustomerSummaryDto {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class OrderItemResponseDto {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
}