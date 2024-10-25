using Microsoft.IdentityModel.Tokens;
using WebForms.Interfaces;
using WebForms.Models;

namespace WebForms.Services
{
    public class TagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _tagRepository.GetAllAsync();
        }

        public async Task<List<string>> GetAllTagNamesAsync()
        {
            var tags = await _tagRepository.GetAllAsync();

            return tags.Select(t => t.Name).ToList();
        }

        public async Task<List<Tag>> GetExistingTagsAsync(List<string> tagNames)
        {
            return await _tagRepository.FindAsync(tag => tagNames.Contains(tag.Name));
        }

        public async Task<List<Tag>> CreateNewTagsAsync(List<string> newTagNames)
        {
            var newTags = new List<Tag>();

            foreach (var tagName in newTagNames)
            {
                newTags.Add(new Tag { Name = tagName });
            }

            await _tagRepository.AddRangeAsync(newTags);

            return newTags;
        }

        public List<string> GetNewTagNames(List<string> tagNames, List<Tag> existingTags)
        {
            return tagNames.Where(tagName => !tagName.IsNullOrEmpty() && !existingTags.Any(etag => etag.Name == tagName)).ToList();
        }

        public async Task AddTagsToTemplateAsync(List<string> tagNames, Template template)
        {
            var existingTags = await GetExistingTagsAsync(tagNames);

            foreach (var existingTag in existingTags)
            {
                template.Tags.Add(existingTag);
            }

            var newTagNames = GetNewTagNames(tagNames, existingTags);

            template.Tags.AddRange(await CreateNewTagsAsync(newTagNames));
        }
    }
}
