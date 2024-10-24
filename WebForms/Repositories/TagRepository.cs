using WebForms.Data;
using WebForms.Interfaces;
using WebForms.Models;

namespace WebForms.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
