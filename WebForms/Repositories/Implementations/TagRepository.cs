using WebForms.Data;
using WebForms.Models;
using WebForms.Repositories.Interfaces;

namespace WebForms.Repositories.Implementations
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context) { }
    }
}
