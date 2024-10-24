using WebForms.Data;
using WebForms.Interfaces;
using WebForms.Models;

namespace WebForms.Repositories
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public TopicRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
