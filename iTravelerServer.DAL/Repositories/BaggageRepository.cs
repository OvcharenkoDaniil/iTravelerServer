using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;


namespace iTravelerServer.DAL.Repositories
{
    public class BaggageRepository : IBaseRepository<Baggage>
    {
        private readonly ApplicationDbContext _db;

        public BaggageRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public IQueryable<Baggage> GetAll()
        {
            return _db.Baggage;
        }
        public Baggage Get(Baggage entity)
        {
            return _db.Baggage.FirstOrDefault(x => x.Baggage_id == entity.Baggage_id);
        }
        public Baggage Get(int id)
        {
            return _db.Baggage.FirstOrDefault(x => x.Baggage_id == id);
        }
        public async Task Delete(Baggage entity)
        {
            _db.Baggage.Remove(entity);
            await _db.SaveChangesAsync();
        }
        public async Task Create(Baggage entity)
        {
            await _db.Baggage.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task<Baggage> Update(Baggage entity)
        {
            _db.Baggage.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
        public Baggage UpdateSync(Baggage entity)
        {
            _db.Baggage.Update(entity);
            _db.SaveChangesAsync();

            return entity;
        }
    }
}