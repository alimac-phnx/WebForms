using WebForms.Data;
using WebForms.Models;
using WebForms.Repositories.Interfaces;

namespace WebForms.Repositories.Implementations
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}