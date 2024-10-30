using Synapse.OrderStatus.Domain.Entities.Base;
using OrderStatusEnum = Synapse.OrderStatus.Domain.Enums.OrderStatus;

namespace Synapse.OrderStatus.Domain.Entities.Order;
public class OrderItem : BaseEntity
{
    public string ProductIdentifier { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public OrderStatusEnum Status { get; set; }
    public int DeliveryNotification { get; set; }
}
