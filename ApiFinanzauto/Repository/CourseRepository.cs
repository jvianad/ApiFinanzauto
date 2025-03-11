using ApiFinanzauto.Data;
using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;

namespace ApiFinanzauto.Repository
{
    public class CourseRepository : Repository<Course>, IRepository<Course>
    {
        public CourseRepository(ApplicationDbContext context):base(context)
        {
            
        }
    }
}
