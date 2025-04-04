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
    }
}
