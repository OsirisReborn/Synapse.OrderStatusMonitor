using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Synapse.OrderStatus.Domain.Common;
using Synapse.OrderStatus.Domain.Entities.Order;
using Synapse.OrderStatus.Domain.Interfaces;

namespace Synapse.OrderStatus.Infrastructure.Services;

public class ApiAlertService(HttpClient httpClient, IConfiguration config)
    : IApiAlertService
{
    public async Task<OperationResult<bool>> SendDeliveryAlertAsync(string orderId, OrderItem item)
    {
        try
        {
            var alertEndpoint = config.GetSection("SendAlertMessageEndpoint").Value;

            var alertData = new
            {
                Message = $"Alert for delivered item: Order {orderId}, Item: {item.Description}, " +
                          $"Delivery Notifications: {item.DeliveryNotification}"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(alertData),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync(alertEndpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                return OperationResult<bool>.Failure(
                    $"Failed to send alert for Order ID {orderId}. Status Code: {response.StatusCode}",
                    (int)response.StatusCode);
            }

            return OperationResult<bool>.SuccessResult(true, "Alert sent successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure(ex);
        }
    }
}




