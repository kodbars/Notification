using Microsoft.EntityFrameworkCore;
using NotificationsDb.Data;
using NotificationsDb.Repository.IRepository;

namespace NotificationsDb.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationsDbContext _db;

        public NotificationRepository(NotificationsDbContext db)
        {
            _db = db;
        }

        public async Task Create(Notification obj)
        {
            _db.Notifications.Add(obj);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var obj = await _db.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            if (obj != null) 
            {
                _db.Notifications.Remove(obj);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Notification>> GetAll()
        {
            return await _db.Notifications.ToListAsync();
        }
    }
}
