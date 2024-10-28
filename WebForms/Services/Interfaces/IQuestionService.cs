using WebForms.Models;

namespace WebForms.Services.Interfaces
{
    public interface IQuestionService
    {
        Task CreateQuestionsAsync(List<Question> questions, CancellationToken cancellationToken = default);

        bool IsQuestionNew(int id);

        Task SolveQuestionsAsync(List<Question> questions, Template template, CancellationToken cancellationToken = default);

        void UpdateQuestion(Question question, Template template);

        Task DeleteQuestionsAsync(List<Question> allQuestions, List<Question> templateQuestions, CancellationToken cancellationToken = default);

        Task<List<Question>> GetVisibleQuestions(int templateId, CancellationToken cancellationToken = default);

        Task<List<string>> GetAllQuestionTextsAsync(CancellationToken cancellationToken = default);
    }
}
