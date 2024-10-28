using WebForms.Models;

namespace WebForms.Services.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default);

        Task<List<string>> GetAllTagNamesAsync(CancellationToken cancellationToken = default);

        Task<List<Tag>> GetExistingTagsAsync(List<string> tagNames, CancellationToken cancellationToken = default);

        Task<List<Tag>> CreateNewTagsAsync(List<string> newTagNames, CancellationToken cancellationToken = default);

        List<string> GetNewTagNames(List<string> tagNames, List<Tag> existingTags);

        Task AddTagsToTemplateAsync(List<string> tagNames, Template template, CancellationToken cancellationToken = default);
    }
}