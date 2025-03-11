using ApiFinanzauto.Data;
using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;

namespace ApiFinanzauto.Repository
{
    public class GradeRepository : Repository<Grade>, IRepository<Grade>
    {
        public GradeRepository(ApplicationDbContext context):base(context)
        {
            
        }
    }
}
