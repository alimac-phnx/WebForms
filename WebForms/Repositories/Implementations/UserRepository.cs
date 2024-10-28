using Microsoft.EntityFrameworkCore;
using WebForms.Data;
using WebForms.Models;
using WebForms.Repositories.Interfaces;

namespace WebForms.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetUsersWithTemplatesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.CreatedTemplates.Count != 0)
                .ToListAsync(cancellationToken);
        }
    }
}
