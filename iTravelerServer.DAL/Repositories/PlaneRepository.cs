using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;

namespace iTravelerServer.DAL.Repositories;


public class PlaneRepository : IBaseRepository<Plane>
{
    private readonly ApplicationDbContext _db;

    public PlaneRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public IQueryable<Plane> GetAll()
    {
        return _db.Plane;
    }

    public Task<Plane?> Get(Plane entity)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Plane entity)
    {
        _db.Plane.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Create(Plane entity)
    {
        await _db.Plane.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<Plane> Update(Plane entity)
    {
        _db.Plane.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }
}