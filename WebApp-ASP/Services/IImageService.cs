
namespace WebApp_ASP.Services
{
    public interface IImageService
    {
        Task<string> Create(IFormFile image, string saveFolder);
    }
}