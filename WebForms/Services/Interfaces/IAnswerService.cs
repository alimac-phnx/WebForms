using WebForms.Models;

namespace WebForms.Services.Interfaces
{
    public interface IAnswerService
    {
        Task AddAnswersAsync(List<Answer> answers, CancellationToken cancellationToken = default);
    }
}