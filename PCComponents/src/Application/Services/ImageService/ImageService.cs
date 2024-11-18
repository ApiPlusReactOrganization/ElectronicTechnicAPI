using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Optional;

namespace Application.Services.ImageService
{
    public class ImageService(IWebHostEnvironment webHostEnvironment) : IImageService
    {
        public async Task<Option<string>> SaveImageFromFileAsync(string path, IFormFile image, string? oldImagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(oldImagePath))
                {
                    var fullOldPath = Path.Combine(webHostEnvironment.ContentRootPath, path, oldImagePath);
                    if (File.Exists(fullOldPath))
                    {
                        File.Delete(fullOldPath);
                    }
                }

                var types = image.ContentType.Split('/');

                if (types[0] != "image")
                {
                    return Option.None<string>();
                }

                var root = webHostEnvironment.ContentRootPath;
                var imageName = $"{Guid.NewGuid()}.{types[1]}";
                var filePath = Path.Combine(root, path, imageName);

                using (var stream = File.OpenWrite(filePath))
                {
                    using (var imageStream = image.OpenReadStream())
                    {
                        await imageStream.CopyToAsync(stream);
                    }
                }

                return Option.Some(imageName);
            }
            catch (Exception ex)
            {
                return Option.None<string>();
            }
        }
    }
}