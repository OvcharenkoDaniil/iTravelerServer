using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;

namespace iTravelerServer.DAL.Repositories;

public class TicketRepository : IBaseRepository<Ticket>
{
    private readonly ApplicationDbContext _db;
    
    public TicketRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task Create(Ticket entity)
    {
         _db.Ticket.Add(entity);
         _db.SaveChanges();
    }
    
    public IQueryable<Ticket> GetAll()
    {
        return _db.Ticket;
    }

    public Ticket Get(Ticket entity)
    {
        return _db.Ticket.FirstOrDefault(x => 
            x.Price == entity.Price && 
            x.BwFlight_id == entity.BwFlight_id && 
            x.FwFlight_id == entity.FwFlight_id);
    }

    public Ticket Get(int id)
    {
        return _db.Ticket.Find(id);
    }

    public async Task Delete(Ticket entity)
    {
        _db.Ticket.Remove(entity);
        await _db.SaveChangesAsync();
    }
    
    public async Task<Ticket> Update(Ticket entity)
    {
        _db.Ticket.Update(entity);
        await _db.SaveChangesAsync();
    
        return entity;
    }

    public Ticket UpdateSync(Ticket entity)
    {
        _db.Ticket.Update(entity);
        _db.SaveChangesAsync();

        return entity;
    }
}