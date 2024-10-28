using WebForms.Models;

namespace WebForms.Repositories.Interfaces
{
    public interface ITemplateRepository : IRepository<Template>
    {
        Task<Template> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<Template>> GetTemplatesByUserAsync(int userId, CancellationToken cancellationToken = default);

        Task<List<Template>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}