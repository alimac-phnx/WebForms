using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebForms.Data;
using WebForms.Interfaces;
using WebForms.Models;

namespace WebForms.Repositories
{
    public class FormRepository : Repository<Form>, IFormRepository
    {
        public FormRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Form> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Forms
                .Include(f => f.Answers)
                .SingleOrDefaultAsync(f => f.Id == id, cancellationToken);
        }

        public override async Task<List<Form>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Forms
                .Include(f => f.Answers)
                .ToListAsync(cancellationToken);
        }
    }
}