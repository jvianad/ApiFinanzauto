
using System.Linq.Expressions;

namespace ApiFinanzauto.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(object filter = null);
        Task<T> GetByIdAsync(int id);
        Task PostAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
