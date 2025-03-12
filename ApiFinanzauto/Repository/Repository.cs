using ApiFinanzauto.Data;
using ApiFinanzauto.Filters;
using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanzauto.Repository
{
    public class Repository<T>: IRepository<T> where T : class
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

        public async Task<List<T>> GetAllAsync(object filter = null)
        {
            IQueryable<T> query = _dbSet;

            // Incluir relaciones según el tipo de entidad
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

            // Aplicar filtros dinámicamente
            if (filter != null)
            {
                if (typeof(T) == typeof(Professor) && filter is ProfessorFilter professorFilter)
                {
                    if (!string.IsNullOrEmpty(professorFilter.FirstName))
                        query = query.Where(p => (p as Professor)!.FirstName.Contains(professorFilter.FirstName));
                    if (!string.IsNullOrEmpty(professorFilter.LastName))
                        query = query.Where(p => (p as Professor)!.LastName.Contains(professorFilter.LastName));
                    if (!string.IsNullOrEmpty(professorFilter.Email))
                        query = query.Where(p => (p as Professor)!.Email.Contains(professorFilter.Email));
                }
                else if (typeof(T) == typeof(Student) && filter is StudentFilter studentFilter)
                {
                    if (!string.IsNullOrEmpty(studentFilter.FirstName))
                        query = query.Where(s => (s as Student)!.Name.Contains(studentFilter.FirstName));
                    if (!string.IsNullOrEmpty(studentFilter.LastName))
                        query = query.Where(s => (s as Student)!.LastName.Contains(studentFilter.LastName));
                    if (!string.IsNullOrEmpty(studentFilter.Email))
                        query = query.Where(s => (s as Student)!.Email.Contains(studentFilter.Email));
                }
                else if (typeof(T) == typeof(Course) && filter is CourseFilter courseFilter)
                {
                    if (!string.IsNullOrEmpty(courseFilter.Name))
                        query = query.Where(c => (c as Course)!.Name.Contains(courseFilter.Name));
                    if (courseFilter.ProfessorId.HasValue)
                        query = query.Where(c => (c as Course)!.ProfessorId == courseFilter.ProfessorId.Value);
                }
                else if (typeof(T) == typeof(Grade) && filter is GradeFilter gradeFilter)
                {
                    if (gradeFilter.StudentId.HasValue)
                        query = query.Where(g => (g as Grade)!.StudentId == gradeFilter.StudentId.Value);
                    if (gradeFilter.CourseId.HasValue)
                        query = query.Where(g => (g as Grade)!.CourseId == gradeFilter.CourseId.Value);
                    if (gradeFilter.GradeValueMin.HasValue)
                        query = query.Where(g => (g as Grade)!.GradeValue >= gradeFilter.GradeValueMin.Value);
                    if (gradeFilter.GradeValueMax.HasValue)
                        query = query.Where(g => (g as Grade)!.GradeValue <= gradeFilter.GradeValueMax.Value);
                }
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
