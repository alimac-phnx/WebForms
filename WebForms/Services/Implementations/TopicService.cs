using WebForms.Models;
using WebForms.Repositories.Interfaces;
using WebForms.Services.Interfaces;

namespace WebForms.Services.Implementations
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<Topic> GetTopicByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _topicRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<Topic>> GetAllTopicsAsync(CancellationToken cancellationToken = default)
        {
            return await _topicRepository.GetAllAsync(cancellationToken);
        }

        public async Task<string> GetTopicNameAsync(int id, CancellationToken cancellationToken = default)
        {
            return (await _topicRepository.GetByIdAsync(id, cancellationToken)).Name;
        }
    }
}