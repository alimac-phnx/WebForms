using Microsoft.EntityFrameworkCore;
using WebForms.Data;
using WebForms.Models;
using WebForms.Repositories.Interfaces;

namespace WebForms.Repositories.Implementations
{
    public class TemplateRepository : Repository<Template>, ITemplateRepository
    {
        public TemplateRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Template> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Templates
                .Include(t => t.Questions)
                .Include(t => t.Tags)
                .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<Template>> GetTemplatesByUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.Templates
                .AsNoTracking()
                .Include(t => t.Questions)
                .Include(t => t.Tags)
                .Where(t => t.CreatedByUserId == userId)
                .ToListAsync(cancellationToken);
        }

        public override async Task<List<Template>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Templates
                .Include(t => t.Questions)
                .Include(t => t.Tags)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
