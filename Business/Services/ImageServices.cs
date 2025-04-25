using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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
            // CoPilot (Claude 3.5) was used to figure out how to resize the image in the best possible way
            using var imageStream = image.OpenReadStream();
            using var img = await Image.LoadAsync(imageStream);
            // Resize the image to a width and height of 256 pixels
            img.Mutate(x => x.Resize(256, 256));
            // Save the image to the specified path
            await img.SaveAsync(filePath);

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
