using WebForms.Models;
using WebForms.ViewModels;

namespace WebForms.Services.Interfaces
{
    public interface IFormService
    {
        Task<Form> GetFormByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<FormTableViewModel>> GetFormsInfoAsync(int userId, CancellationToken cancellationToken = default);

        Task CreateForm(List<Answer> answers, int templateId, int userId, CancellationToken cancellationToken = default);

        Task UpdateFormAsync(List<Answer> answers, Form form, CancellationToken cancellationToken = default);

        Task DeleteFormAsync(int formId, CancellationToken cancellationToken = default);

        Task<List<Form>> GetAllByTemplateAsync(int templateId, CancellationToken cancellationToken = default);
    }
}