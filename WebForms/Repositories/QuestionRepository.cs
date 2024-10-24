using WebForms.Data;
using WebForms.Interfaces;
using WebForms.Models;

namespace WebForms.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
