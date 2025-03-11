using ApiFinanzauto.Data;
using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;

namespace ApiFinanzauto.Repository
{
    public class ProfessorRepository : Repository<Professor>, IRepository<Professor>
    {
        public ProfessorRepository(ApplicationDbContext context): base(context)
        {
            
        }
    }
}
