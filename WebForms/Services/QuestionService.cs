using WebForms.Interfaces;
using WebForms.Models;
using WebForms.Repositories;

namespace WebForms.Services
{
    public class QuestionService
    {
        public readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository) 
        { 
            _questionRepository = questionRepository;
        }

        public async Task CreateQuestionsAsync(List<Question> questions)
        {
            await _questionRepository.AddRangeAsync(questions);
        }

        public bool IsQuestionNew(int id)
        {
            if (id == 0) return true;
            else return false;
        }

        public async Task SolveQuestionsAsync(List<Question> questions, Template template)
        {
            var newQuestions = new List<Question>();

            foreach (var question in questions)
            {
                if (IsQuestionNew(question.Id))
                {
                    question.TemplateId = template.Id;
                    newQuestions.Add(question);
                }
                else
                {
                    UpdateQuestion(question, template);
                }
            }

            await CreateQuestionsAsync(newQuestions);
        }

        public void UpdateQuestion(Question question, Template template)
        {
            var existingQuestion = template.Questions.SingleOrDefault(q => q.Id == question.Id);

            if (existingQuestion != null)
            {
                existingQuestion.Text = question.Text;
                existingQuestion.Type = question.Type;
                existingQuestion.IsVisible = question.IsVisible;
            }
        }

        public async Task DeleteQuestionsAsync(List<Question> allQuestions, List<Question> templateQuestions)
        {
            var questionsToDelete = new List<Question>();

            foreach (var templateQuestion in templateQuestions)
            {
                if (!allQuestions.Any(q => q.Id == templateQuestion.Id))
                {
                    questionsToDelete.Add(templateQuestion);
                }
            }

            await _questionRepository.DeleteRangeAsync(questionsToDelete);
        }

        public async Task<List<Question>> GetVisibleQuestions(int templateId)
        {
            return await _questionRepository.FindAsync(q => q.TemplateId == templateId && q.IsVisible);
        }

        public async Task<List<string>> GetAllQuestionTextsAsync()
        {
            var questions = await _questionRepository.GetAllAsync();

            return questions.Select(q => q.Text).ToList();
        }
    }
}
