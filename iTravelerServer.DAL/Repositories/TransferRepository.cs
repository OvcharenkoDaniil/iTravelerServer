using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;

namespace iTravelerServer.DAL.Repositories;

public class TransferRepository : IBaseRepository<Transfer>
{
    private readonly ApplicationDbContext _db;

    public TransferRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public IQueryable<Transfer> GetAll()
    {
        return _db.Transfer;
    }

    public Task<Transfer?> Get(Transfer entity)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Transfer entity)
    {
        _db.Transfer.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Create(Transfer entity)
    {
        await _db.Transfer.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<Transfer> Update(Transfer entity)
    {
        _db.Transfer.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }
}