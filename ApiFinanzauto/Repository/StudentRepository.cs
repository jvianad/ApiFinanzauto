using ApiFinanzauto.Data;
using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;

namespace ApiFinanzauto.Repository
{
    public class StudentRepository: Repository<Student>,IRepository<Student>
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}