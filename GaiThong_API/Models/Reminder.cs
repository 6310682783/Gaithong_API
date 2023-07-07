using Microsoft.VisualBasic;

namespace GaiThong_API.Models
{
    public class Reminder
    {
        public int Id { get; set; }

        public DateTime? RemindDate { get; set; }

        public DateTime? RemindTime { get; set; }

        public string CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string Description { get; set; }

    }
}
