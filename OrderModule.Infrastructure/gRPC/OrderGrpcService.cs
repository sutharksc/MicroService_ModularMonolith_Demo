using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OrderModule.Application.Abstractions;
using SharedModule.Protos;

namespace OrderModule.Infrastructure.gRPC
{
    public class OrderGrpcService(IOrderService orderService) : OrderService.OrderServiceBase
    {
        public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            var orderId = await orderService.CreateOrderAsync(request);
            return new CreateOrderResponse
            {
                IsSuccess = true,
                Data = Any.Pack(new StringValue { Value = orderId.ToString() })
            };
        }
    }
}
