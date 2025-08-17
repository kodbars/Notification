using NotificationsDb.Data;

namespace NotificationsDb.Repository.IRepository
{
    public interface INotificationRepository
    {
        public Task Create(Notification obj);
        public Task Delete(int id);
        public Task<IEnumerable<Notification>> GetAll();
    }
}
