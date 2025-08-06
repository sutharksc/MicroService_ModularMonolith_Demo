using OrderModule.Application.Features.Order;
using SharedModule.Protos;
namespace OrderModule.Application.Abstractions
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(CreateOrderRequest request);
        Task<GetOrderByIdResult?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
