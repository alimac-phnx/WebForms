using WebForms.Models;

namespace WebForms.Interfaces
{
    public interface ITemplateRepository : IRepository<Template>
    {
        Task<List<Template>> GetTemplatesByUserAsync(int userId, CancellationToken cancellationToken = default);
    }
}