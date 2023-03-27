using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;

namespace iTravelerServer.DAL.Repositories;

public class TicketDetailRepository : IBaseRepository<TicketDetail>
{
    private readonly ApplicationDbContext _db;
    
    public TicketDetailRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task Create(TicketDetail entity)
    {
        _db.TicketDetail.Add(entity);
        _db.SaveChanges();
    }
    
    public IQueryable<TicketDetail> GetAll()
    {
        return _db.TicketDetail;
    }

    public TicketDetail Get(TicketDetail entity)
    {
        return _db.TicketDetail.FirstOrDefault(x => 
            x.Ticket_id == entity.Ticket_id && 
            x.Direction == entity.Direction );
    }

    public TicketDetail Get(int id)
    {
        return _db.TicketDetail.Find(id);
    }

    public async Task Delete(TicketDetail entity)
    {
        _db.TicketDetail.Remove(entity);
         _db.SaveChanges();
    }
    
    public async Task<TicketDetail> Update(TicketDetail entity)
    {
        _db.TicketDetail.Update(entity);
        await _db.SaveChangesAsync();
    
        return entity;
    }

    public TicketDetail UpdateSync(TicketDetail entity)
    {
        _db.TicketDetail.Update(entity);
        _db.SaveChanges();
    
        return entity;
    }
  
}