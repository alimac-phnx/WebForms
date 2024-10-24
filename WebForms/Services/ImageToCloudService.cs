using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace WebForms.Services
{
    public class ImageToCloudService
    {
        private readonly Cloudinary _cloudinary;

        public ImageToCloudService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
        }

        public async Task<string> UploadImageToCloud(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("File is missing");

            // Преобразование IFormFile в поток
            using var stream = imageFile.OpenReadStream();

            // Опции загрузки
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, stream),
                Folder = "TemplateImages",
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            // Загрузка изображения
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Image upload failed");

            // Возвращение ссылки на изображение
            return uploadResult.SecureUrl.ToString();
        }
    }
}
