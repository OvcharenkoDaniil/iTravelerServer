using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;

namespace iTravelerServer.DAL.Repositories;


public class AirportRepository : IBaseRepository<Airport>
{
    private readonly ApplicationDbContext _db;

    public AirportRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public IQueryable<Airport> GetAll()
    {
        return _db.Airport;
    }

    public Task<Airport?> Get(Airport entity)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Airport entity)
    {
        _db.Airport.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Create(Airport entity)
    {
        await _db.Airport.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<Airport> Update(Airport entity)
    {
        _db.Airport.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }
}