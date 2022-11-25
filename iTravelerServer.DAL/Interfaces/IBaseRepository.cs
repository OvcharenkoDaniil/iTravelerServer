using System.Linq;
using System.Threading.Tasks;

namespace iTravelerServer.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Create(T entity);

        IQueryable<T> GetAll();
        Task<T?> Get(T entity);

        Task Delete(T entity);

        Task<T> Update(T entity);
    }
}