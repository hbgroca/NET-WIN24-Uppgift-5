using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Business.Dtos;

namespace WebApp_ASP.Controllers
{
    [Authorize]
    public class ClientsController(IClientService clientService) : Controller
    {
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClient(Guid clientId)
        {
            var result = await _clientService.Delete(clientId);
            if (result is false)
                Debug.WriteLine( "Error while deleting the client");

            return Redirect("~/clients");
        }
    }
}
