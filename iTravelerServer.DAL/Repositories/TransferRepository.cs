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

    public Transfer Get(Transfer entity)
    {
        throw new NotImplementedException();
    }

    public Transfer Get(int id)
    {
        return _db.Transfer.Find(id);
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

    public Transfer UpdateSync(Transfer entity)
    {
        _db.Transfer.Update(entity);
        _db.SaveChanges();
    
        return entity;
    }
}