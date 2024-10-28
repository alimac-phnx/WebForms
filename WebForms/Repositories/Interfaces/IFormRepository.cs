using WebForms.Models;

namespace WebForms.Repositories.Interfaces
{
    public interface IFormRepository : IRepository<Form>
    {
        Task<Form> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<Form>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}