using WebForms.Interfaces;
using WebForms.Models;

namespace WebForms.Services
{
    public class QuestionService
    {
        public readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository) 
        { 
            _questionRepository = questionRepository;
        }

        public async Task CreateQuestions(List<Question> questions)
        {
            await _questionRepository.AddRangeAsync(questions);
        }

        public bool IsQuestionNew(int id)
        {
            if (id == 0) return true;
            else return false;
        }

        public async Task SolveQuestions(List<Question> questions, Template template)
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

            await CreateQuestions(newQuestions);
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

        public async Task DeleteQuestions(List<Question> allQuestions, List<Question> templateQuestions)
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
    }
}
