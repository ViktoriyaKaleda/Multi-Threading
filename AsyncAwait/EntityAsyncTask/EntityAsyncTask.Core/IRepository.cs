using System.Threading.Tasks;

namespace EntityAsyncTask.Core
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
