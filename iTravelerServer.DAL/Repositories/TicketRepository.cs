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
        await _db.Ticket.AddAsync(entity);
        await _db.SaveChangesAsync();
    }
    
    public IQueryable<Ticket> GetAll()
    {
        return _db.Ticket;
    }

    public Task<Ticket?> Get(Ticket entity)
    {
        throw new NotImplementedException();
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
}