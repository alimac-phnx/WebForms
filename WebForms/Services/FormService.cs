using WebForms.Interfaces;
using WebForms.Models;
using WebForms.ViewModels;

namespace WebForms.Services
{
    public class FormService
    {
        public readonly IFormRepository _formRepository;

        public readonly TemplateService _templateService;

        public readonly AnswerService _answerService;

        public FormService(IFormRepository formRepository, TemplateService templateService, AnswerService answerService)
        {
            _formRepository = formRepository;
            _templateService = templateService;
            _answerService = answerService;
        }

        public async Task<Form> GetFormByIdAsync(int id)
        {
            return await _formRepository.GetByIdAsync(id);
        }

        public async Task<List<FormTableViewModel>> GetFormsInfoAsync(int userId)
        {
            var forms = (from form in await _formRepository.GetAllAsync()
                               join template in await _templateService.GetAllTemplatesAsync()
                               on form.TemplateId equals template.Id
                               where form.AssignedByUserId == userId
                               select new FormTableViewModel
                               {
                                   Id = form.Id,
                                   Name = template.Name,
                                   Description = template.Description,
                                   AssignedAt = form.AssignedAt,
                                   AnswerCount = form.Answers.Count
                               }).ToList();

            return forms;
        }

        public async Task CreateForm(List<Answer> answers, int templateId, int userId)
        {
            var form = new Form()
            {
                TemplateId = templateId,
                AssignedByUserId = userId,
                AssignedAt = DateTime.Now
            };

            await _formRepository.AddAsync(form);
            form.Answers = answers;

            await _answerService.AddAnswersAsync(answers);
            await _formRepository.UpdateAsync(form);
        }

        public async Task UpdateFormAsync(List<Answer> answers, Form form)
        {
            form.Answers = answers;

            form.AssignedAt = DateTime.Now;

            await _formRepository.UpdateAsync(form);
        }

        public async Task DeleteFormAsync(int formId)
        {
            await _formRepository.DeleteAsync(formId);
        }
    }
}
