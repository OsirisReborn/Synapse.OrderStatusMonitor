using Microsoft.Extensions.Logging;
using Synapse.OrderStatus.Domain.Common;
using Synapse.OrderStatus.Domain.Interfaces;
using Synapse.OrderStatus.Domain.Logging;
using OrderStatusEnum = Synapse.OrderStatus.Domain.Enums.OrderStatus;


namespace Synapse.OrderStatus.Application.Services;

public class OrderService(IApiOrderService orderApiService, IApiAlertService alertService, 
    ILogger<OrderService> logger) 
    :IOrderService
{
    public async Task<OperationResult<bool>> ProcessOrdersAsync()
    {
        try
        {
            var ordersResult = await orderApiService.FetchOrdersAsync();
            if (!ordersResult.Success)
            {
                return OperationResult<bool>.Failure(ordersResult.Message);
            }

            foreach (var order in ordersResult.Data)
            {
                foreach (var item in order.Items)
                {
                    if (item.Status == OrderStatusEnum.Delivered)
                    {
                        var alertResult = await RetryAsync(() => alertService.SendDeliveryAlertAsync(order.CustomerOrderNumber, item), maxAttempts: 3); 

                        if (!alertResult.Success)
                        {
                            logger.LogError(ErrorTemplates.AlertSendFailed, order.CustomerOrderNumber, item.ProductIdentifier, alertResult.Message);
                        }
                    }
                    
                }
                await orderApiService.UpdateOrderAsync(order);
            }

            return OperationResult<bool>.SuccessResult(true);
        }
        catch (Exception ex)        {
          
            logger.LogError(ErrorTemplates.FetchOrdersFailed, ex.Message);           
            return OperationResult<bool>.Failure(ex.Message);
        }
    }

    private async Task<OperationResult<bool>> RetryAsync(Func<Task<OperationResult<bool>>> action, int maxAttempts)
    {
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            var result = await action();
            if (result.Success)
                return result;

            // Optional: Add delay between retries for exponential backoff
            await Task.Delay(TimeSpan.FromSeconds(2 * attempt));
        }

        return OperationResult<bool>.Failure("Maximum retry attempts reached.");
    }

}
