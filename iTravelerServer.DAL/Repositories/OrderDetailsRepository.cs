using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;

namespace iTravelerServer.DAL.Repositories;

public class OrderDetailsRepository : IBaseRepository<OrderDetails>
{
    private readonly ApplicationDbContext _db;
    
    public OrderDetailsRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task Create(OrderDetails entity)
    {
        _db.OrderDetails.Add(entity);
        _db.SaveChanges();
    }
    
    public IQueryable<OrderDetails> GetAll()
    {
        return _db.OrderDetails;
    }

    public OrderDetails Get(OrderDetails entity)
    {
        return _db.OrderDetails.FirstOrDefault(x => 
            x.Order_id == entity.Order_id && 
            x.Direction == entity.Direction );
    }

    public OrderDetails Get(int id)
    {
        return _db.OrderDetails.Find(id);
    }

    public async Task Delete(OrderDetails entity)
    {
        _db.OrderDetails.Remove(entity);
         _db.SaveChanges();
    }
    
    public async Task<OrderDetails> Update(OrderDetails entity)
    {
        _db.OrderDetails.Update(entity);
        await _db.SaveChangesAsync();
    
        return entity;
    }

    public OrderDetails UpdateSync(OrderDetails entity)
    {
        _db.OrderDetails.Update(entity);
        _db.SaveChanges();
    
        return entity;
    }
  
}