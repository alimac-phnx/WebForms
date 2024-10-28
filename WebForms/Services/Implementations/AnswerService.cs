using WebForms.Models;
using WebForms.Repositories.Interfaces;
using WebForms.Services.Interfaces;

namespace WebForms.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task AddAnswersAsync(List<Answer> answers, CancellationToken cancellationToken = default)
        {
            await _answerRepository.AddRangeAsync(answers, cancellationToken);
        }
    }
}
