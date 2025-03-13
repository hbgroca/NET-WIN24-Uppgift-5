using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using WebApp_ASP.Models;

namespace WebApp_ASP.Controllers
{
    public class TeamController(IWebHostEnvironment webHostEnvironment, IMemberService memberService) : Controller
    {
        private readonly IWebHostEnvironment _env = webHostEnvironment;
        private readonly IMemberService _memberService = memberService;

        public TeamMembersViewModel _viewModel= new();

        public async Task<IActionResult> TeamMembers()
        {
            ViewData["Title"] = "Team";
           
            _viewModel = new TeamMembersViewModel
            {
                Members = await _memberService.GetAllMembersAsync(),
                MemberRegistrationForm = new MemberRegistrationFormModel()
            };

            return View(_viewModel);
        }


        // Create member
        [HttpPost]
        public async Task<IActionResult> TeamMembers(MemberRegistrationFormModel form)
        {
            if (!ModelState.IsValid || form.ProfilePicture == null || form.ProfilePicture.Length == 0)
            {
                _viewModel = new TeamMembersViewModel
                {
                    Members = await _memberService.GetAllMembersAsync(),
                    MemberRegistrationForm = form,
                    RegistrationFormInvalid = true
                };

                return View(_viewModel);
            }

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
            await _memberService.CreateMemberAsync(form);

            return RedirectToAction(nameof(TeamMembers));
        }


        // Update member
        [HttpPost]
        public async Task<IActionResult> UpdateTeamMember(Guid id)
        {
            var member = await _memberService.GetMemberAsync(x => x.Id == id);

            var teamMembersViewModel = _memberService.CreateRegistrationUpdateForm(member);


            _viewModel = new TeamMembersViewModel
            {
                Members = await _memberService.GetAllMembersAsync(),
                MemberRegistrationForm = teamMembersViewModel,
                IsUpdate = true
            };

            return View("TeamMembers", _viewModel);
        }

        // Save Updated member
        [HttpPost]
        public async Task<IActionResult> SaveUpdatedTeamMember(MemberRegistrationFormModel form)
        {

            //if (!ModelState.IsValid || form.ProfilePicture == null || form.ProfilePicture.Length == 0)
            //{
            //    _viewModel = new TeamMembersViewModel
            //    {
            //        Members = await _memberService.GetAllMembersAsync(),
            //        MemberRegistrationForm = form,
            //        RegistrationFormInvalid = true
            //    };

            //    return View(_viewModel);
            //}

            //var uploadFolder = Path.Combine(_env.WebRootPath, "uploaded/members");
            //// If folder does not exist it will be created
            //Directory.CreateDirectory(uploadFolder);
            //var fileName = Path.Combine(Path.GetFileName($"{Guid.NewGuid()}_{form.ProfilePicture.FileName}"));
            //var filePath = Path.Combine(uploadFolder, fileName);

            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await form.ProfilePicture.CopyToAsync(stream);
            //}

            //// Set file path
            //form.ImageName = $"/uploaded/members/{fileName}";
            //await _memberService.CreateRegistrationUpdateForm(form);

            return View("TeamMembers");
        }
    }
}
