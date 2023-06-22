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
             _db.Flight.Add(entity);
             _db.SaveChanges();
        }
    
        // public void CreateSync(Flight entity)
        // {
        //      _db.Flight.Add(entity);
        //      _db.SaveChanges();
        // }
        public IQueryable<Flight> GetAll()
        {
            return _db.Flight;
        }

        public Flight Get(Flight entity)
        {
            throw new NotImplementedException();
        }

        public Flight Get(int id)
        {
            return _db.Flight.Find(id);
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

        public Flight UpdateSync(Flight entity)
        {
            _db.Flight.Update(entity);
            _db.SaveChanges();
    
            return entity;
        }
    }
}