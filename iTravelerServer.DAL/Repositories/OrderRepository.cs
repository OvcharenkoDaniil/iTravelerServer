using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;

namespace iTravelerServer.DAL.Repositories;


public class OrderRepository : IBaseRepository<Order>
{
    private readonly ApplicationDbContext _db;

    public OrderRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public IQueryable<Order> GetAll()
    {
        return _db.Order;
    }

    public Task<Order?> Get(Order entity)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Order entity)
    {
        _db.Order.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Create(Order entity)
    {
        await _db.Order.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<Order> Update(Order entity)
    {
        _db.Order.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }
}