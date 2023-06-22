using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;

namespace iTravelerServer.DAL.Repositories;

public class FlightDetailsRepository : IBaseRepository<FlightDetails>
{
    private readonly ApplicationDbContext _db;
    
    public FlightDetailsRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task Create(FlightDetails entity)
    {
        _db.FlightDetails.Add(entity);
        _db.SaveChanges();
    }
    
    public IQueryable<FlightDetails> GetAll()
    {
        return _db.FlightDetails;
    }

    public FlightDetails Get(FlightDetails entity)
    {
        return _db.FlightDetails.FirstOrDefault(x => 
            x.FlightDetail_id == entity.FlightDetail_id);
    }

    public FlightDetails Get(int id)
    {
        return _db.FlightDetails.Find(id);
    }

    public async Task Delete(FlightDetails entity)
    {
        _db.FlightDetails.Remove(entity);
         _db.SaveChanges();
    }
    
    public async Task<FlightDetails> Update(FlightDetails entity)
    {
        _db.FlightDetails.Update(entity);
        await _db.SaveChangesAsync();
    
        return entity;
    }

    public FlightDetails UpdateSync(FlightDetails entity)
    {
        _db.FlightDetails.Update(entity);
        _db.SaveChanges();
    
        return entity;
    }
  
}