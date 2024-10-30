using Synapse.OrderStatus.Domain.Common;

namespace Synapse.OrderStatus.Domain.Interfaces;
public interface IOrderService
{
    Task<OperationResult<bool>> ProcessOrdersAsync();
}
