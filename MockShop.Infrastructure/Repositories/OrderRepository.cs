using Microsoft.EntityFrameworkCore;
using MockShop.Application.Interfaces;
using MockShop.Domain.Entities;
using MockShop.Infrastructure.Persistance;

namespace MockShop.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
            => await _context.Orders.Where(o => o.Status == "Paid").ToListAsync();

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
    }
}
