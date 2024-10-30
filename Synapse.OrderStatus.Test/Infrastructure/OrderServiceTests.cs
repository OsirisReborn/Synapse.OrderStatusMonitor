using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Synapse.OrderStatus.Domain.Interfaces;
using Synapse.OrderStatus.Application.Services;
using Synapse.OrderStatus.Domain.Entities;
using Synapse.OrderStatus.Domain.Logging;
using Microsoft.Extensions.Logging;
using Synapse.OrderStatus.Domain.Common;
using Synapse.OrderStatus.Domain.Entities.Order;
using Synapse.OrderStatus.Domain.Interfaces;
using Synapse.OrderStatus.Domain.Logging;
using OrderStatusEnum =  Synapse.OrderStatus.Domain.Enums.OrderStatus;

namespace Synapse.OrderStatus.Test.Infrastructure
{
    public class OrderServiceTests
    {
        private readonly Mock<IApiOrderService> _orderApiServiceMock;
        private readonly Mock<IApiAlertService> _alertServiceMock;
        private readonly Mock<ILogger<OrderService>> _loggerMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderApiServiceMock = new Mock<IApiOrderService>();
            _alertServiceMock = new Mock<IApiAlertService>();
            _loggerMock = new Mock<ILogger<OrderService>>();
            _orderService = new OrderService(_orderApiServiceMock.Object, _alertServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ProcessOrdersAsync_ShouldCallSendDeliveryAlertOnlyForDeliveredItems()
        {
            // Arrange
            var sampleOrders = new List<Order>
            {
                new Order
                {
                    CustomerOrderNumber = "123",
                    Items = new List<OrderItem>
                    {
                    new OrderItem { Status = OrderStatusEnum.Delivered, ProductIdentifier = "A1" },
                    new OrderItem { Status = OrderStatusEnum.Pending, ProductIdentifier = "A2" }
                }
            },
                new Order
                {
                    CustomerOrderNumber = "456",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Status = OrderStatusEnum.Delivered, ProductIdentifier = "B1" },
                        new OrderItem { Status = OrderStatusEnum.Pending, ProductIdentifier = "B2" }
                    }
                }
            };

            _orderApiServiceMock.Setup(api => api.FetchOrdersAsync())
                                .ReturnsAsync(OperationResult<List<Order>>.SuccessResult(sampleOrders));

            _alertServiceMock.Setup(alert => alert.SendDeliveryAlertAsync(It.IsAny<string>(), It.IsAny<OrderItem>()))
                             .ReturnsAsync(OperationResult<bool>.SuccessResult(true))
                             .Verifiable();

            // Act
            var result = await _orderService.ProcessOrdersAsync();

            // Assert
            Assert.True(result.Success);

            // Verify that alerts are only sent for delivered items
            _alertServiceMock.Verify(
                alert => alert.SendDeliveryAlertAsync("123", It.Is<OrderItem>(item => item.ProductIdentifier == "A1")), Times.Once);
            _alertServiceMock.Verify(
                alert => alert.SendDeliveryAlertAsync("456", It.Is<OrderItem>(item => item.ProductIdentifier == "B1")), Times.Once);

            // Ensure no alerts were sent for pending items
            _alertServiceMock.Verify(
                alert => alert.SendDeliveryAlertAsync("123", It.Is<OrderItem>(item => item.ProductIdentifier == "A2")), Times.Never);
            _alertServiceMock.Verify(
                alert => alert.SendDeliveryAlertAsync("456", It.Is<OrderItem>(item => item.ProductIdentifier == "B2")), Times.Never);
            
        }

        [Fact]
        public async Task ProcessOrdersAsync_ShouldReturnFailure_WhenFetchOrdersFails()
        {
            // Arrange
            _orderApiServiceMock.Setup(api => api.FetchOrdersAsync())
                                .ReturnsAsync(OperationResult<List<Order>>.Failure("Failed to fetch orders"));

            // Act
            var result = await _orderService.ProcessOrdersAsync();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Failed to fetch orders", result.Message);

        }

        [Fact]
        public async Task ProcessOrdersAsync_ShouldHandleAlertSendFailure_ForDeliveredItems()
        {
            // Arrange
            var sampleOrders = new List<Order>
            {
                new Order
                {
                    CustomerOrderNumber = "123",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Status = OrderStatusEnum.Delivered, ProductIdentifier = "A1" },
                        new OrderItem { Status = OrderStatusEnum.Pending, ProductIdentifier = "A2" }
                    }
                },
                new Order
                {
                    CustomerOrderNumber = "456",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Status = OrderStatusEnum.Delivered, ProductIdentifier = "B1" },
                        new OrderItem { Status = OrderStatusEnum.Pending, ProductIdentifier = "B2" }
                    }
                }
            };

            _orderApiServiceMock.Setup(api => api.FetchOrdersAsync())
                                .ReturnsAsync(OperationResult<List<Order>>.SuccessResult(sampleOrders));

            // Configure SendDeliveryAlertAsync to fail for a specific item and succeed for others
            _alertServiceMock.Setup(alert => alert.SendDeliveryAlertAsync("123", It.Is<OrderItem>(item => item.ProductIdentifier == "A1")))
                             .ReturnsAsync(OperationResult<bool>.Failure("Failed to send alert for Item A1"))
                             .Verifiable();

            _alertServiceMock.Setup(alert => alert.SendDeliveryAlertAsync("456", It.Is<OrderItem>(item => item.ProductIdentifier == "B1")))
                             .ReturnsAsync(OperationResult<bool>.SuccessResult(true))
                             .Verifiable();

            // Act
            var result = await _orderService.ProcessOrdersAsync();

            // Assert
            Assert.True(result.Success);

            // Verify alert attempts only for delivered items
            _alertServiceMock.Verify(
                alert => alert.SendDeliveryAlertAsync("123", It.Is<OrderItem>(item => item.ProductIdentifier == "A1")), Times.AtLeastOnce);
            _alertServiceMock.Verify(
                alert => alert.SendDeliveryAlertAsync("456", It.Is<OrderItem>(item => item.ProductIdentifier == "B1")), Times.AtLeastOnce);

            // Ensure no alerts were sent for pending items
            _alertServiceMock.Verify(
                alert => alert.SendDeliveryAlertAsync("123", It.Is<OrderItem>(item => item.ProductIdentifier == "A2")), Times.Never);
            _alertServiceMock.Verify(
                alert => alert.SendDeliveryAlertAsync("456", It.Is<OrderItem>(item => item.ProductIdentifier == "B2")), Times.Never);
        }

        [Fact]
        public async Task ProcessOrdersAsync_ShouldCallUpdateOrder_ForEachProcessedOrder()
        {
            // Arrange
            var sampleOrders = new List<Order>
            {
                new Order
                {
                    CustomerOrderNumber = "123",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Status = OrderStatusEnum.Delivered, ProductIdentifier = "A1" },
                        new OrderItem { Status = OrderStatusEnum.Pending, ProductIdentifier = "A2" }
                    }
                },
                new Order
                {
                    CustomerOrderNumber = "456",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Status = OrderStatusEnum.Delivered, ProductIdentifier = "B1" },
                        new OrderItem { Status = OrderStatusEnum.Pending, ProductIdentifier = "B2" }
                    }
                }
            };

            _orderApiServiceMock.Setup(api => api.FetchOrdersAsync())
                                .ReturnsAsync(OperationResult<List<Order>>.SuccessResult(sampleOrders));

            _alertServiceMock.Setup(alert => alert.SendDeliveryAlertAsync(It.IsAny<string>(), It.IsAny<OrderItem>()))
                             .ReturnsAsync(OperationResult<bool>.SuccessResult(true))
                             .Verifiable();

            _orderApiServiceMock.Setup(api => api.UpdateOrderAsync(It.IsAny<Order>()))
                                .ReturnsAsync(OperationResult<bool>.SuccessResult(true))
                                .Verifiable();

            // Act
            var result = await _orderService.ProcessOrdersAsync();

            // Assert
            Assert.True(result.Success);

            // Verify that UpdateOrderAsync is called for each processed order
            _orderApiServiceMock.Verify(api => api.UpdateOrderAsync(It.Is<Order>(order => order.CustomerOrderNumber == "123")), Times.Once);
            _orderApiServiceMock.Verify(api => api.UpdateOrderAsync(It.Is<Order>(order => order.CustomerOrderNumber == "456")), Times.Once);
            
        }

        [Fact]
        public async Task ProcessOrdersAsync_ShouldReturnFailure_WhenFetchOrdersThrowsException()
        {
            // Arrange
            _orderApiServiceMock.Setup(api => api.FetchOrdersAsync())
                                .ThrowsAsync(new Exception("API fetch error"));

            // Act
            var result = await _orderService.ProcessOrdersAsync();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("API fetch error", result.Message);
           
        }
    }
}
