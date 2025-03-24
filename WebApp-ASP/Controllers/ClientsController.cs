using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Business.Models;

namespace WebApp_ASP.Controllers
{
    public class ClientsController(IWebHostEnvironment webHostEnvironment,IClientService clientService) : Controller
    {
        private readonly IWebHostEnvironment _env = webHostEnvironment;
        private readonly IClientService _clientService = clientService;

        [HttpPost]
        public async Task<IActionResult> AddClient(AddClientFormModel form)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new { success = false, errors });
            }

            // Store image
            if (form.ProfilePicture != null && form.ProfilePicture.Length >= 0)
            {
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploaded/clients");
                // If folder does not exist it will be created
                Directory.CreateDirectory(uploadFolder);
                var fileName = Path.Combine(Path.GetFileName($"{Guid.NewGuid()}_{form.ProfilePicture.FileName}"));
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await form.ProfilePicture.CopyToAsync(stream);
                }

                // Set file path
                form.ImageName = $"/uploaded/clients/{fileName}";
            }
            else
                form.ImageName = $"/images/defaultprofile.png";

            // Send data to service
            var result = await _clientService.CreateClientAsync(form);

            if(result is null)
            {
                var error = "Error while creating the client";
                return BadRequest(new { success = false, error });
            }

            return Ok(new { success = true });
        }


        [HttpPost]
        public async Task<IActionResult> EditClient(EditClientFormModel form)
        {
            // Check if inputs is valid
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new { success = false, errors });
            }

            // Store image
            if (form.ProfilePicture != null && form.ProfilePicture.Length >= 0)
            {
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploaded/clients");
                // If folder does not exist it will be created
                Directory.CreateDirectory(uploadFolder);
                var fileName = Path.Combine(Path.GetFileName($"{form.Id}_{form.ProfilePicture.FileName}"));
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await form.ProfilePicture.CopyToAsync(stream);
                }

                // Set file path
                form.ImageName = $"/uploaded/clients/{fileName}";
            }

            //Send data to service
            var result = await _clientService.UpdateClient(form);

            if (result is false)
            {
                var error = "Error while creating the client";
                return BadRequest(new { success = false, error });
            }

            return Ok(new { success = true });
        }



        [HttpGet]
        [Route("getclients/{id}")]
        public async Task<IActionResult> GetClient(Guid id)
        {
            var client = await _clientService.GetClientAsync(i => i.Id == id);
            if (client == null)
                return NotFound();

            return Ok(client);
        }
    }
}
