using Microsoft.AspNetCore.Http;

namespace WebApp_ASP.Services;

public class ImageService(IWebHostEnvironment webHostEnvironment) : IImageService
{
    private readonly IWebHostEnvironment _env = webHostEnvironment;

    public async Task<string> Create(IFormFile image, string saveFolder)
    {
        var projectImage = image;
        var uploadFolder = Path.Combine(_env.WebRootPath, saveFolder);
        // If folder does not exist it will be created
        Directory.CreateDirectory(uploadFolder);
        var fileName = Path.Combine(Path.GetFileName($"{Guid.NewGuid()}_{projectImage.FileName}"));
        var filePath = Path.Combine(uploadFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await projectImage.CopyToAsync(stream);
        }

        return $"/{saveFolder}/{fileName}";
    }
}
