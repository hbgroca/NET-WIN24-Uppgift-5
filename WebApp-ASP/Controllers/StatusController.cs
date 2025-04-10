using Business.Dtos;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp_ASP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatusController(IStatusServices statusServices) : Controller
    {
        private readonly IStatusServices _statusServices = statusServices;

        [HttpPost]
        public async Task<IActionResult> AddClientStatus(ClientStatusFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Status", "Admin");
            }
            // Add status to the database
            await _statusServices.AddStatus(form);

            return RedirectToAction("Status", "Admin");
        }

        [HttpPost]
        [Route("Status/RemoveClientStatus/{statusId}")]
        public async Task<IActionResult> RemoveClientStatus(int statusId)
        {
            var result = await _statusServices.RemoveClientStatus(statusId);

            if (result)
                return Ok();

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddMemberStatus(MemberStatusFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Status", "Admin");
            }
            // Add status to the database
            await _statusServices.AddStatus(form);

            return RedirectToAction("Status", "Admin");
        }

        [HttpPost]
        [Route("Status/RemoveMemberStatus/{statusId}")]
        public async Task<IActionResult> RemoveMemberStatus(int statusId)
        {
            var result = await _statusServices.RemoveMemberStatus(statusId);

            if (result)
                return Ok();

            return NotFound();
        }
    }
}
