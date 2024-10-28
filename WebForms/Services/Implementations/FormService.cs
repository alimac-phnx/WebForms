using WebForms.Models;
using WebForms.Repositories.Interfaces;
using WebForms.Services.Interfaces;
using WebForms.ViewModels;

namespace WebForms.Services.Implementations
{
    public class FormService : IFormService
    {
        public readonly IFormRepository _formRepository;

        public readonly ITemplateService _templateService;

        public readonly IAnswerService _answerService;

        public FormService(IFormRepository formRepository, ITemplateService templateService, IAnswerService answerService)
        {
            _formRepository = formRepository;
            _templateService = templateService;
            _answerService = answerService;
        }

        public async Task<Form> GetFormByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _formRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<FormTableViewModel>> GetFormsInfoAsync(int userId, CancellationToken cancellationToken = default)
        {
            var forms = (from form in await _formRepository.GetAllAsync(cancellationToken)
                         join template in await _templateService.GetAllTemplatesAsync(cancellationToken)
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

        public async Task CreateForm(List<Answer> answers, int templateId, int userId, CancellationToken cancellationToken = default)
        {
            var form = new Form()
            {
                TemplateId = templateId,
                AssignedByUserId = userId,
                AssignedAt = DateTime.Now
            };

            await _formRepository.AddAsync(form, cancellationToken);
            form.Answers = answers;

            await _answerService.AddAnswersAsync(answers, cancellationToken);
            await _formRepository.UpdateAsync(form, cancellationToken);
        }

        public async Task UpdateFormAsync(List<Answer> answers, Form form, CancellationToken cancellationToken = default)
        {
            form.Answers = answers;

            form.AssignedAt = DateTime.Now;

            await _formRepository.UpdateAsync(form, cancellationToken);
        }

        public async Task DeleteFormAsync(int formId, CancellationToken cancellation = default)
        {
            await _formRepository.DeleteAsync(formId, cancellation);
        }

        public async Task<List<Form>> GetAllByTemplateAsync(int templateId, CancellationToken cancellationToken = default)
        {
            var forms = await _formRepository.GetAllAsync(cancellationToken);
            return forms.Where(f => f.TemplateId == templateId).ToList();
        }
    }
}