using Microsoft.AspNetCore.Http;

namespace Business.Interfaces;
public interface IImageServices
{
    Task<string> Create(IFormFile image, string saveFolder);
    bool Delete(string imageUrl);
    Task<string> Update(IFormFile image, string saveFolder, string oldImageUrl);
}