using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Synapse.OrderStatus.Domain.Common;
using Synapse.OrderStatus.Domain.Interfaces;
using Synapse.OrderStatus.Domain.Entities.Order;

namespace Synapse.OrderStatus.Infrastructure.Services;

    public class ApiOrderService(HttpClient httpClient, IConfiguration config)
        : IApiOrderService
    {
        public async Task<OperationResult<List<Order>>> FetchOrdersAsync()
        {
            var fetchOrdersUrl = config.GetSection("FetchMedicalOrdersEndpoint").Value;
            try
            {
                var response = await httpClient.GetAsync(fetchOrdersUrl);
                
                if (!response.IsSuccessStatusCode)
                {
                    return OperationResult<List<Order>>.Failure(
                        $"Failed to fetch orders. Status Code: {response.StatusCode}",
                        (int)response.StatusCode);
                }

                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonSerializer.Deserialize<List<Order>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
               
                return OperationResult<List<Order>>.SuccessResult(orders);
            }
            catch (Exception ex)
            {
                return OperationResult<List<Order>>.Failure(ex);
            }
        }
        public async Task<OperationResult<bool>> UpdateOrderAsync(Order order)
        {

            var updateOrderUrl = config.GetSection("UpdateMedicalOrderEndpoint").Value;
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(order),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await httpClient.PostAsync($"{updateOrderUrl}/{order.CustomerOrderNumber}", content);

                if (!response.IsSuccessStatusCode)
                {
                    return OperationResult<bool>.Failure(
                        $"Failed to update order with ID {order.CustomerOrderNumber}. Status Code: {response.StatusCode}",
                        (int)response.StatusCode);
                }

                return OperationResult<bool>.SuccessResult(true, "Order updated successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure(ex);
            }
        }
    }   
