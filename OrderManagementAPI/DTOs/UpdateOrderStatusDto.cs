using OrderManagementAPI.Models;

namespace OrderManagementAPI.DTOs {
    public class UpdateOrderStatusDto {
        public OrderStatus Status { get; set; }
    }
}