
using MockShop.Domain.Entities;

namespace MockShop.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateAsync(Order order);
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
        Task UpdateOrderStatusAsync(int orderId, string status);
    }
}
