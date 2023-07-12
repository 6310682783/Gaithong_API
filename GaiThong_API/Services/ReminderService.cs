using GaiThong_API.Models;
using GaiThong_API.Repositories;

namespace GaiThong_API.Services
{
    public interface IReminderService
    {
        Task<IEnumerable<Reminder>> GetAll();

        Task<(IEnumerable<Reminder>, int)> GetAllFromNow();

        Task<Reminder> GetById(int Id);
        Task<bool> Add(Reminder reminder);
        Task<bool> Update(Reminder reminder);
        Task<bool> Delete(int id);
    }
    public class ReminderService : IReminderService
    {
        private readonly IReminderRepository reminderRepository;
        public ReminderService(IReminderRepository reminderRepository)
        {
            this.reminderRepository = reminderRepository;
        }

        public async Task<bool> Add(Reminder reminder)
        {
            reminder.RemindDate = reminder.RemindDate;
            reminder.RemindTime = reminder.RemindTime;
            reminder.Description = reminder.Description;
            reminder.CreateDate = DateTime.Now;
            var result = await reminderRepository.Add(reminder);
            return result>1;
        }

        public async Task<bool> Delete(int Id)
        {
            var result = await reminderRepository.Delete(Id);
            return result > 0;
        }

        public async Task<IEnumerable<Reminder>> GetAll()
        {
            var result = await reminderRepository.GetAll();
            return result;
        }

        public async Task<(IEnumerable<Reminder>, int)> GetAllFromNow()
        {
            var result = await reminderRepository.GetAllFromNow();
            return result;
        }

        public async Task<Reminder> GetById(int Id)
        {
            var result = await reminderRepository.GetById(Id);
            return result;
        }

        public async Task<bool> Update(Reminder reminder)
        {
            var model = await reminderRepository.GetById(reminder.Id);
            model.RemindDate = reminder.RemindDate;
            model.RemindTime = reminder.RemindTime;
            model.Description = reminder.Description;
            var result = await reminderRepository.Update(model);
            return (result > 0);
        }
    }
}
