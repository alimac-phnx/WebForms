using WebForms.Models;

namespace WebForms.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageToCloudAsync(IFormFile imageFile, CancellationToken cancellationToken = default);

        Task AddImageToTemplateAsync(IFormFile imageFile, Template template, CancellationToken cancellationToken = default);
    }
}
