using Synapse.OrderStatus.Domain.Common;
using Synapse.OrderStatus.Domain.Entities.Order;

namespace Synapse.OrderStatus.Domain.Interfaces;
public interface IApiOrderService

{
    Task<OperationResult<List<Order>>> FetchOrdersAsync();
    Task<OperationResult<bool>> UpdateOrderAsync(Order order);
}