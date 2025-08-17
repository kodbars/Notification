using Microsoft.AspNetCore.Mvc;
using NotificationsDb.Data;
using NotificationsDb.Repository.IRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNotifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _notification;

        public NotificationsController(INotificationRepository notification)
        {
            _notification = notification;
        }

        //POST api/<NotificationsController>
        [HttpPost]
        public async Task AddNotifications([FromBody] Notification notification)
        {
            await _notification.Create(notification);
        }
        //Get api/<NotificationsController>
        [HttpGet]
        public async Task<IEnumerable<Notification>> GetAll() 
        {
            return await _notification.GetAll();
        }
        //Delete api/<NotificationsController>/id
        [HttpDelete("{id:int}")]
        public async Task DeleteNotifications(int id) 
        {
            await _notification.Delete(id);
        }

    }
}
