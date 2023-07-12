using GaiThong_API.Models;
using GaiThong_API.Models.ViewModels;
using GaiThong_API.Repositories;

namespace GaiThong_API.Services
{
    public interface IHomeService
    {
        Task<ReminderModel> GetAllToday();

    }
    public class HomeService : IHomeService 
    {
        private readonly IReminderRepository reminderRepository;
        public HomeService(IReminderRepository reminderRepository)
        {
            this.reminderRepository = reminderRepository;
        }

        public async Task<ReminderModel> GetAllToday()
        {
            var reminders = await reminderRepository.GetAll();
            var reminder = new ReminderModel();
            reminder.Count = reminders.Where((m) => m.RemindDate.Value.Date == DateTime.Now.Date).Count();
            return reminder;
        }
    }
}
