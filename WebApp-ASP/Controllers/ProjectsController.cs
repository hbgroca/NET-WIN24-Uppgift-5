using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using WebApp_ASP.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp_ASP.Controllers
{
    [Authorize]
    public class ProjectsController(IImageService imageService ,IWebHostEnvironment webHostEnvironment, IProjectService projectService, IMemberService memberService, IClientService clientService) : Controller
    {
        private readonly IProjectService _projectService = projectService;
        private readonly IMemberService _memberService = memberService;
        private readonly IClientService _clientService = clientService;
        private readonly IWebHostEnvironment _env = webHostEnvironment;
        private readonly IImageService _imageService = imageService;

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
                                var member = await _memberService.GetMemberAsync(x => x.Id == id.ToString());
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
                        Debug.WriteLine($"{error.Key}: {string.Join(", ", error.Value!)}");
                    }

                    return BadRequest(new { success = false, errors });
                }

                // Handle image if present
                if (form.ProjectImage != null && form.ProjectImage.Length > 0)
                {
                    // Save image
                    form.ImageName = await _imageService.Create(form.ProjectImage, "uploaded/projects");
                }
                else
                    form.ImageName = $"/images/defaultmember.png";

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
                        // Deserialize the members json to GUID list
                        var memberIds = JsonSerializer.Deserialize<List<Guid>>(form.MembersJson);

                        if (memberIds != null)
                        {
                            foreach (var id in memberIds)
                            {
                                var member = await _memberService.GetMemberAsync(x => x.Id == id.ToString());
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

                    return BadRequest(new { success = false, errors });
                }

                // Handle image if present
                if (form.ProjectImage != null && form.ProjectImage.Length > 0)
                {
                    // Save image
                    form.ImageName = await _imageService.Create(form.ProjectImage, "uploaded/projects");
                }

                // Save the project
                var result = await _projectService.Update(form);
                if (result)
                    return Ok(new { success = true });
                else
                    return BadRequest(new { success = false, error = "Failed to update project" });
            }
            catch (Exception ex)
            {
                var error = $"Error while updating the project: {ex.Message}";
                Debug.WriteLine(error);
                Debug.WriteLine(ex.StackTrace);
                return BadRequest(new { success = false, error });
            }
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
