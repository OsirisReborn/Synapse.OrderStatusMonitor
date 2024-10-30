using Synapse.OrderStatus.Domain.Common;
using Synapse.OrderStatus.Domain.Entities.Order;

namespace Synapse.OrderStatus.Domain.Interfaces;
public interface IApiAlertService
{
    Task<OperationResult<bool>> SendDeliveryAlertAsync(string orderId, OrderItem item);
}
