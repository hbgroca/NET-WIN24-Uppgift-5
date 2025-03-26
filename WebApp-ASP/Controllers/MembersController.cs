using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Business.Models;

namespace WebApp_ASP.Controllers
{
    public class MembersController(IWebHostEnvironment webHostEnvironment, IMemberService memberService) : Controller
    {
        private readonly IWebHostEnvironment _env = webHostEnvironment;
        private readonly IMemberService _memberService = memberService;
        

        // Add member
        public async Task<IActionResult> AddMember(AddMemberFormModel form)
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
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploaded/members");
                // If folder does not exist it will be created
                Directory.CreateDirectory(uploadFolder);
                var fileName = Path.Combine(Path.GetFileName($"{Guid.NewGuid()}_{form.ProfilePicture.FileName}"));
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await form.ProfilePicture.CopyToAsync(stream);
                }

                // Set file path
                form.ImageName = $"/uploaded/members/{fileName}";
            }
            else
                form.ImageName = $"/images/defaultmember.png";

                // Send data to service
                var result = await _memberService.CreateMemberAsync(form);

            if (result is null)
            {
                var error = "Error while creating the client";
                return BadRequest(new { success = false, error });
            }

            return Ok(new { success = true });
        }


        // Edit member
        public async Task<IActionResult> EditMember(EditMemberFormModel form)
        {
            Debug.WriteLine($"EditMember called");

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
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploaded/members");
                // If folder does not exist it will be created
                Directory.CreateDirectory(uploadFolder);
                var fileName = Path.Combine(Path.GetFileName($"{form.Id}_{form.ProfilePicture.FileName}"));
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await form.ProfilePicture.CopyToAsync(stream);
                }

                // Set file path
                form.ImageName = $"/uploaded/members/{fileName}";
            }

            //Send data to service
            var result = await _memberService.UpdateMember(form);

            if (result is false)
            {
                var error = "Error while creating the client";
                return BadRequest(new { success = false, error });
            }

            return Ok(new { success = true });
        }


        // Get member by id
        [HttpGet]
        [Route("getmembers/{id}")]
        public async Task<IActionResult> GetMember(Guid id)
        {
            var client = await _memberService.GetMemberAsync(i => i.Id == id.ToString());
            if (client == null)
                return NotFound();

            return Ok(client);
        }


        [HttpPost]
        [Route("deletemember/{id}")]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var result = await _memberService.Delete(id);

            // Navigate to member list page
            return RedirectToAction("Members", "Admin");
        }
    }
}
