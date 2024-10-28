using WebForms.Models;
using WebForms.ViewModels;

namespace WebForms.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<List<Template>> GetAllTemplatesAsync(CancellationToken cancellationToken = default);

        Task<List<Template>> GetUserTemplatesAsync(int userId, CancellationToken cancellationToken = default);

        Task CreateTemplateAsync(TemplateCreateViewModel model, int userId, CancellationToken cancellationToken = default);

        Task<Template> GetTemplateByIdAsync(int id, CancellationToken cancellationToken = default);

        Task UpdateQuestionsInTemplateAsync(List<Question> questions, Template template, CancellationToken cancellationToken = default);

        Task UpdateTemplateAsync(List<string> tagNames, List<Question> questions, Template template, CancellationToken cancellationToken = default);

        Task DeleteTemplateAsync(int id, CancellationToken cancellationToken = default);

        Task<string> GetTemplateAuthorNameAsync(int userId, CancellationToken cancellationToken = default);

        Task<List<Template>> SearchTemplatesByQueryAsync(string query, CancellationToken cancellationToken = default);

        List<TemplateDisplayViewModel> PrepareTemplatesToDisplay(List<Template> templates);
    }
}