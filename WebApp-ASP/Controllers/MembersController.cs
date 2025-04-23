using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Business.Dtos;

namespace WebApp_ASP.Controllers
{
    [Authorize]
    public class MembersController(IMemberService memberService) : Controller
    {
        private readonly IMemberService _memberService = memberService;


        //// Add member
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

            // Send data to service
            var result = await _memberService.CreateMemberAsync(form);

            if (result is null)
            {
                var error = "Error while creating the member";
                return BadRequest(new { success = false, error });
            }

            return Ok(new { success = true });
        }

        // Profile edit
        public async Task<IActionResult> ProfileEdit(Guid id)
        {
            var user = await _memberService.GetMemberAsync(x => x.Id == id.ToString());

            EditMemberFormModel form = new EditMemberFormModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Title = user.Title,
                Street = user.Address.Street,
                ZipCode = user.Address.ZipCode,
                City = user.Address.City,
                Country = user.Address.Country,
                Day = user.BirthDate.Day,
                Month = user.BirthDate.Month,
                Year = user.BirthDate.Year,
                ImageName = user.ImageUrl,
                Status = user.Status,
            };

            return View(form);
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

            //Send data to service
            var result = await _memberService.UpdateMember(form);

            if (result is false)
            {
                var error = "Error while editing the member";
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
            return RedirectToAction("Members", "Admin");
        }
    }
}
