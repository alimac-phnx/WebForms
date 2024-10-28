using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using WebForms.Models;
using WebForms.Services.Interfaces;

namespace WebForms.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public ImageService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
        }

        public async Task<string> UploadImageToCloudAsync(IFormFile imageFile, CancellationToken cancellationToken = default)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("File is missing");

            using var stream = imageFile.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, stream),
                Folder = "TemplateImages",
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Image upload failed");

            return uploadResult.SecureUrl.ToString();
        }

        public async Task AddImageToTemplateAsync(IFormFile imageFile, Template template, CancellationToken cancellationToken = default)
        {
            if (imageFile != null)
            {
                template.ImageUrl = await UploadImageToCloudAsync(imageFile, cancellationToken);
            }
        }
    }
}
