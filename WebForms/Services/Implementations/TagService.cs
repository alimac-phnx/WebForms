using Microsoft.IdentityModel.Tokens;
using WebForms.Models;
using WebForms.Repositories.Interfaces;
using WebForms.Services.Interfaces;

namespace WebForms.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<List<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default)
        {
            return await _tagRepository.GetAllAsync(cancellationToken);
        }

        public async Task<List<string>> GetAllTagNamesAsync(CancellationToken cancellationToken = default)
        {
            var tags = await _tagRepository.GetAllAsync(cancellationToken);

            return tags.Select(t => t.Name.ToLower()).ToList();
        }

        public async Task<List<Tag>> GetExistingTagsAsync(List<string> tagNames, CancellationToken cancellationToken = default)
        {
            return await _tagRepository.FindAsync(tag => tagNames.Contains(tag.Name), cancellationToken);
        }

        public async Task<List<Tag>> CreateNewTagsAsync(List<string> newTagNames, CancellationToken cancellationToken = default)
        {
            var newTags = new List<Tag>();

            foreach (var tagName in newTagNames)
            {
                newTags.Add(new Tag { Name = tagName });
            }

            await _tagRepository.AddRangeAsync(newTags, cancellationToken);

            return newTags;
        }

        public List<string> GetNewTagNames(List<string> tagNames, List<Tag> existingTags)
        {
            return tagNames.Where(tagName => !tagName.IsNullOrEmpty() && !existingTags.Any(etag => etag.Name == tagName)).ToList();
        }

        public async Task AddTagsToTemplateAsync(List<string> tagNames, Template template, CancellationToken cancellationToken = default)
        {
            var existingTags = await GetExistingTagsAsync(tagNames, cancellationToken);

            foreach (var existingTag in existingTags)
            {
                template.Tags.Add(existingTag);
            }

            var newTagNames = GetNewTagNames(tagNames, existingTags);

            template.Tags.AddRange(await CreateNewTagsAsync(newTagNames, cancellationToken));
        }
    }
}
