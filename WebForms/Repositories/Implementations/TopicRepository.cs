using WebForms.Data;
using WebForms.Models;
using WebForms.Repositories.Interfaces;

namespace WebForms.Repositories.Implementations
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public TopicRepository(ApplicationDbContext context) : base(context) { }
    }
}
