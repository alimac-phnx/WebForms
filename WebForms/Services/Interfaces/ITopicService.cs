using WebForms.Models;

namespace WebForms.Services.Interfaces
{
    public interface ITopicService
    {
        Task<Topic> GetTopicByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<Topic>> GetAllTopicsAsync(CancellationToken cancellationToken = default);

        Task<string> GetTopicNameAsync(int id, CancellationToken cancellationToken = default);
    }
}