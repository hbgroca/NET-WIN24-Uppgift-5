using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Business.Services;

public class ImageServices : IImageServices
{
    public async Task<string> Create(IFormFile image, string saveFolder)
    {
        var projectImage = image;
        var uploadFolder = $"{Environment.CurrentDirectory}\\wwwroot\\uploaded\\{saveFolder}";
        // If folder does not exist it will be created
        Directory.CreateDirectory(uploadFolder);
        var fileName = Path.Combine(Path.GetFileName($"{Guid.NewGuid()}_{projectImage.FileName}"));
        var filePath = Path.Combine(uploadFolder, fileName);

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await projectImage.CopyToAsync(stream);
            }

            return $"/uploaded/{saveFolder}/{fileName}";
        }
        catch(Exception ex)
        {
            Debug.WriteLine($"An error occurred while saving the image: {ex.Message}");
            if(saveFolder == "projects")
            {
                return $"/images/defaultproject.png";
            }
            return $"/images/defaultmember.png";
        }
    }

    
    public async Task<string> Update(IFormFile image, string saveFolder, string oldImageUrl)
    {
        var result = await Create(image, saveFolder);
        if(string.IsNullOrEmpty(result))
        {
            Debug.WriteLine("An error occurred while updating the image");
            return oldImageUrl;
        }

        // Delete the old image
        Delete(oldImageUrl);
        return result;
    }

    public bool Delete(string imageUrl)
    {
        if(imageUrl.Contains("defaultmember.png"))
            return false;

        string imageToRemove = imageUrl.Replace("/", "\\");
        var cutString = $"{Environment.CurrentDirectory}\\wwwroot{imageToRemove}";
        if (File.Exists(cutString))
        {
            Debug.WriteLine($"!!! - Trying to remove file: {cutString}");
            File.Delete(cutString);
            return true;
        }
        return false;
    }
}
