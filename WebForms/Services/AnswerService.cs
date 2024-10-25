using WebForms.Interfaces;
using WebForms.Models;

namespace WebForms.Services
{
    public class AnswerService
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task AddAnswersAsync(List<Answer> answers)
        {
            await _answerRepository.AddRangeAsync(answers);
        }
    }
}
