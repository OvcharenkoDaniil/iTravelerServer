using System.Linq;
using System.Threading.Tasks;

using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.ViewModels.AccountVM;
using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.DAL.Repositories
{
    public class AccountRepository : IBaseRepository<Account>
    {
        private readonly ApplicationDbContext _db;

        public AccountRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<Account> GetAll()
        {
            return _db.Account;
        }
        
        public async Task<Account?> Get(Account entity)
        {
            return await _db.Account.FirstOrDefaultAsync(x => x.Email == entity.Email);
        }

        public async Task Delete(Account entity)
        {
            _db.Account.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(Account entity)
        {
            await _db.Account.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Account> Update(Account entity)
        {
            _db.Account.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}