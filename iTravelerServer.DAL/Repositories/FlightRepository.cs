using System.Linq;
using System.Threading.Tasks;
using iTravelerServer.DAL;
using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;


namespace Automarket.DAL.Repositories
{
    public class FlightRepository : IBaseRepository<Flight>
    {
        private readonly ApplicationDbContext _db;
    
        public FlightRepository(ApplicationDbContext db)
        {
            _db = db;
        }
    
        public async Task Create(Flight entity)
        {
            await _db.Flight.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
    
        public IQueryable<Flight> GetAll()
        {
            return _db.Flight;
        }

        public Task<Flight?> Get(Flight entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Flight entity)
        {
            _db.Flight.Remove(entity);
            await _db.SaveChangesAsync();
        }
    
        public async Task<Flight> Update(Flight entity)
        {
            _db.Flight.Update(entity);
            await _db.SaveChangesAsync();
    
            return entity;
        }
    }
}