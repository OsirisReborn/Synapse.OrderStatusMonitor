using Synapse.OrderStatus.Domain.Enums;
using OrderStatusEnum = Synapse.OrderStatus.Domain.Enums.OrderStatus;

namespace Synapse.OrderStatus.Domain.Entities.Order;
public class Order
{
    public string CustomerOrderNumber { get; set; } = string.Empty;
    public string PurchaseOrderNumber { get; set; } = string.Empty;
    public OrderStatusEnum OrderStatus { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public DateTime CreatedDate { get; set; }
}
