using Microsoft.AspNetCore.Http;
using Optional;

namespace Application.Services.ImageService
{
    public interface IImageService
    {
        Task<Option<string>> SaveImageFromFileAsync(string path, IFormFile image, string? oldImagePath);
    }
}
