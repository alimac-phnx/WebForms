using WebForms.Interfaces;
using WebForms.Models;

namespace WebForms.Services
{
    public class TopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<Topic> GetTopicByIdAsync(int id)
        {
            return await _topicRepository.GetByIdAsync(id);
        }

        public async Task<List<Topic>> GetAllTopicsAsync()
        {
            return await _topicRepository.GetAllAsync();
        }

        public async Task<string> GetTopicNameAsync(int id)
        {
            return (await _topicRepository.GetByIdAsync(id)).Name;
        }
    }
}