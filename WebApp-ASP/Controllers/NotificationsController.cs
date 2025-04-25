using Business.Hubs;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WebApp_ASP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(INotificationSerivces notificationSerivces, IHubContext<NotificationHub> hubContext) : ControllerBase
    {
        private readonly INotificationSerivces _notificationServices = notificationSerivces;
        private readonly IHubContext<NotificationHub> _notificationHub = hubContext;

        [HttpPost]
        public async Task<IActionResult> CreateNotification(NotificationEntity entity)
        {
            if (entity == null)
                return BadRequest("Notification entity cannot be null");

            await _notificationServices.AddNotificationAsync(entity);

            return Ok(new { success =true, message = "Notification created successfully" });
        }



        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
            if(string.IsNullOrEmpty(userId) || userId == "anonymous")
                return Unauthorized("User not authenticated");

            // Get the last 100 notifications for the user
            var notifications = await _notificationServices.GetNotificationsAsync(userId, 100);

            return Ok(notifications);
        }



        [HttpPost("dismiss/{notificationId}")]
        public async Task<IActionResult> DismissNotification(string notificationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
            if (string.IsNullOrEmpty(userId) || userId == "anonymous")
                return Unauthorized("User not authenticated");

            // Remove the notification from the database
            await _notificationServices.DismissNotificationAsync(userId, notificationId);
            // Remove the notification from the user's view
            await _notificationHub.Clients.User(userId).SendAsync("NotificationDismissed", notificationId);

            return Ok( new { success = true, });
        }

        [HttpPost("dismissall/{userName}")]
        public async Task<IActionResult> DismissAllNotifications(string userName)
        {
            if (string.IsNullOrEmpty(userName) || userName == "anonymous")
                return Unauthorized("User not authenticated");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";

            // Remove the notifications from the database
            await _notificationServices.DismissAllNotificationsAsync(userName);
            // Remove the notifications from the user's view
            await _notificationHub.Clients.User(userId).SendAsync("AllNotificationsDismissed");
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
        public async Task<IActionResult> Test()
        {
            string Message = $"All user notification test!";

            var notification = new NotificationEntity
            {
                Message = Message,
                Created = DateTime.Now,
                Image = "",
                TargetGroupId = 1, // All users
                NotificationTypeId = 1, // Client
            };
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
            await _notificationServices.AddNotificationAsync(notification, userId);

            return Ok(new { success = true });
        }
        [HttpPost("sendadmintest")]
        public async Task<IActionResult> Testadmin()
        {
            string Message = $"Admin notification test!";

            var notification = new NotificationEntity
            {
                Message = Message,
                Created = DateTime.Now,
                Image = "",
                TargetGroupId = 2, // Admins only
                NotificationTypeId = 2, // Project
            };
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
            await _notificationServices.AddNotificationAsync(notification, userId);

            return Ok(new { success = true });
        }
    }
}
