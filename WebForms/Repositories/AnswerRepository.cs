using WebForms.Data;
using WebForms.Interfaces;
using WebForms.Models;

namespace WebForms.Repositories
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
