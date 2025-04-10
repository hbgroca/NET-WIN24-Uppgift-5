using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApp_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(INotificationSerivces notificationSerivces) : ControllerBase
    {
        private readonly INotificationSerivces _notificationServices = notificationSerivces;

        [HttpPost]
        public async Task<IActionResult> CreateNotification(NotificationEntity entity)
        {
            if (entity == null)
                return BadRequest("Notification entity cannot be null");

            await _notificationServices.AddNotificationAsync(entity.NotificationTypeId, entity.Message, "Anonomous", entity.Image, entity.TargetGroupId);
            
            return Ok(new { success =true, message = "Notification created successfully" });
        }



        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
            if(string.IsNullOrEmpty(userId) || userId == "anonymous")
                return Unauthorized("User not authenticated");

            var notifications = await _notificationServices.GetNotificationsAsync(userId, 100);
            if (notifications == null || !notifications.Any())
                return NotFound("No notifications found");
            return Ok(notifications);
        }



        [HttpPost("dismiss/{notificationId}")]
        public async Task<IActionResult> DismissNotification(string notificationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
            if (string.IsNullOrEmpty(userId) || userId == "anonymous")
                return Unauthorized("User not authenticated");

            await _notificationServices.DismissNotificationAsync(userId, notificationId);
            return Ok( new { success = true, });
        }

        [HttpPost("dismissall/{userName}")]
        public async Task<IActionResult> DismissAllNotifications(string userName)
        {
            if (string.IsNullOrEmpty(userName) || userName == "anonymous")
                return Unauthorized("User not authenticated");

            await _notificationServices.DismissAllNotificationsAsync(userName);
            return Ok(new { success = true, });
        }

        [HttpPost("isuseradmin")]
        public IActionResult GetRole()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Admin")
            {
                return Ok(new { success = true });
            }
            else
            {
                return Ok(new { success = false });
            }
        }

        [HttpPost("sendtest")]
        public async Task<IActionResult> test()
        {
            await _notificationServices.AddNotificationAsync(1, "Test notifcation", "Anonomous", "/images/defaultmember.png", 1);
            return Ok(new { success = true });
        }
        [HttpPost("sendadmintest")]
        public async Task<IActionResult> testadmin()
        {
            await _notificationServices.AddNotificationAsync(2, "Admin test notification", "Anonomous", "/images/defaultmember.png", 2);
            return Ok(new { success = true });
        }
    }
}
