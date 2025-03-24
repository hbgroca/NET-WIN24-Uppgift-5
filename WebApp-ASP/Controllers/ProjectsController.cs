using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp_ASP.Controllers
{
    public class ProjectsController(IWebHostEnvironment webHostEnvironment, IProjectService projectService, IMemberService memberService, IClientService clientService) : Controller
    {
        private readonly IProjectService _projectService = projectService;
        private readonly IMemberService _memberService = memberService;
        private readonly IClientService _clientService = clientService;
        private readonly IWebHostEnvironment _env = webHostEnvironment;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProject(AddProjectFormModel form)
        {
            try
            {
                // Update Client
                var client = await _clientService.GetClientAsync(c => c.Id == form.ClientId);
                if (client != null)
                    form.Client = client;
                else
                    ModelState.AddModelError("ClientId", " ");


                // Add members
                if (string.IsNullOrEmpty(form.MembersJson) || form.MembersJson == "[]")
                    ModelState.AddModelError("MembersJson", "");
                else
                {
                    try
                    {
                        var memberIds = JsonSerializer.Deserialize<List<Guid>>(form.MembersJson);

                        if (memberIds != null)
                        {
                            foreach (var id in memberIds)
                            {
                                var member = await _memberService.GetMemberAsync(x => x.Id == id);
                                if (member != null)
                                    form.Members.Add(member);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("MembersJson", " ");
                        Debug.WriteLine($"Error deserializing members: {ex.Message}");
                    }
                }

                // Check if field are valid
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    foreach (var error in errors)
                    {
                        Debug.WriteLine($"{error.Key}: {string.Join(", ", error.Value)}");
                    }

                    return BadRequest(new { success = false, errors });
                }

                // Handle image if present
                if (form.ProjectImage != null && form.ProjectImage.Length > 0)
                {
                    var projectImage = form.ProjectImage;
                    var uploadFolder = Path.Combine(_env.WebRootPath, "uploaded/projects");
                    // If folder does not exist it will be created
                    Directory.CreateDirectory(uploadFolder);
                    var fileName = Path.Combine(Path.GetFileName($"{Guid.NewGuid()}_{projectImage.FileName}"));
                    var filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await projectImage.CopyToAsync(stream);
                    }

                    // Set file path
                    form.ImageName = $"/uploaded/projects/{fileName}";
                }
                else
                    form.ImageName = $"/images/defaultprofile.png";

                // Save the project
                var result = await _projectService.CreateProjectAsync(form);
                if (result != null)
                    return Ok(new { success = true });
                else
                    return BadRequest(new { success = false, error = "Failed to create project" });
            }
            catch (Exception ex)
            {
                var error = $"Error while creating the project: {ex.Message}";
                Debug.WriteLine(error);
                Debug.WriteLine(ex.StackTrace);
                return BadRequest(new { success = false, error });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(EditProjectFormModel form)
        {

            // Check if field are valid
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                foreach (var error in errors)
                {
                    Debug.WriteLine($"{error.Key}: {string.Join(", ", error.Value)}");
                }

                return BadRequest(new { success = false, errors });
            }

            return Ok(new { success = true });
        }


            // Get project by id
            [HttpGet]
        [Route("getproject/{id}")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            var project = await _projectService.GetProjectAsync(i => i.Id == id);
            if (project == null)
                return NotFound();

            return Ok(project);
        }


        [HttpPost]
        [Route("deleteproject/{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var result = await _projectService.Delete(id);

            // Navigate to member list page
            return RedirectToAction("Projects", "Admin");
        }
    }
}
