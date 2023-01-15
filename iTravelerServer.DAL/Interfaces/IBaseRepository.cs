using System.Linq;
using System.Threading.Tasks;

namespace iTravelerServer.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Create(T entity);
        IQueryable<T> GetAll();
        T Get(T entity);
        T Get(int id);
        Task Delete(T entity);
        Task<T> Update(T entity);
        T UpdateSync(T entity);
        
    }
}