using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;

namespace iTravelerServer.DAL.Repositories;


public class PlaceRepository : IBaseRepository<Place>
{
    private readonly ApplicationDbContext _db;

    public PlaceRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public IQueryable<Place> GetAll()
    {
        return _db.Place;
    }

    public Place Get(Place entity)
    {
        throw new NotImplementedException();
    }

    public Place Get(int id)
    {
        return _db.Place.Find(id);    }

    public async Task Delete(Place entity)
    {
        _db.Place.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Create(Place entity)
    {
        await _db.Place.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<Place> Update(Place entity)
    {
        _db.Place.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public Place UpdateSync(Place entity)
    {
        _db.Place.Update(entity);
        _db.SaveChanges();

        return entity;
    }
}