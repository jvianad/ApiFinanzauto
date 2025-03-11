using ApiFinanzauto.Data;
using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanzauto.Repository
{
    public class Repository<T>:IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task PostAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IQueryable<T> query = _dbSet;

            if (typeof(T) == typeof(Professor))
            {
                query = query.Include(p => (p as Professor)!.Courses);
            }
            else if (typeof(T) == typeof(Student))
            {
                query = query.Include(s => (s as Student)!.Grades);
            }
            else if (typeof(T) == typeof(Course))
            {
                query = query.Include(c => (c as Course)!.Grades)
                             .Include(c => (c as Course)!.Professor);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            IQueryable<T> query = _dbSet;

            if (typeof(T) == typeof(Professor))
            {
                query = query.Include(p => (p as Professor)!.Courses);
            }
            else if (typeof(T) == typeof(Student))
            {
                query = query.Include(s => (s as Student)!.Grades);
            }
            else if (typeof(T) == typeof(Course))
            {
                query = query.Include(c => (c as Course)!.Grades)
                             .Include(c => (c as Course)!.Professor);
            }

            return await query.FirstOrDefaultAsync(p => EF.Property<int>(p, "Id") == id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }
}
