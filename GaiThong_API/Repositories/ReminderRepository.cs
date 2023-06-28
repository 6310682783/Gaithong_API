using Dapper;
using Microsoft.Data.SqlClient;
using GaiThong_API.Models;

namespace GaiThong_API.Repositories
{
    public interface IReminderRepository
    {
        Task<IEnumerable<Reminder>> GetAll();
        Task<Reminder> GetById(int Id);
        Task<int> Add(Reminder remindner);
        Task<int> Update(Reminder reminder);
        Task<int> Delete(int Id);
    }
    public class ReminderRepository : GenericRepository<Reminder>, IReminderRepository
    {
        public ReminderRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public override async Task<int> Add(Reminder model)
        {
            var sqlCommand = string.Format(@"INSERT INTO [dbo].[Reminder]([RemindDate],[RemindTime],[CreateBy],[CreateDate],[Description])
                                            VALUES (@RemindDate,@RemindTime,@CreateBy,@CreateDate,@Description)");
            using (var db = new SqlConnection(connectionString))
            {
                return await db.ExecuteAsync(sqlCommand, MappingParameter(model));
            }
        }

        public async override Task<int> Delete(int Id)
        {
            var sqlCommand = string.Format(@"DELETE FROM [dbo].[Reminder] WHERE [Id] = @Id");
            using (var db = new SqlConnection(connectionString))
            {
                return await db.ExecuteAsync(sqlCommand, new { Id = Id });
            }
        }

        public async override Task<int> Update(Reminder model)
        {
            var sqlCommand = string.Format(@"UPDATE [dbo].[Reminder]
                                               SET [RemindDate] = @RemindDate 
                                                ,[RemindTime] = @RemindTime
                                                ,[CreateBy] = @CreateBy
                                                ,[CreateDate] = @CreateDate
                                                ,[Description] = @Description
                                                 WHERE [Id] = @Id");
            using (var db = new SqlConnection(connectionString))
            {
                return await db.ExecuteAsync(sqlCommand, MappingParameter(model));
            }
        }

        private object MappingParameter(Reminder model)
        {
            return new
            {
                Id = model.Id,
                RemindDate = model.RemindDate,
                RemindTime = model.RemindTime,
                CreateBy = model.CreateBy,
                CreateDate = model.CreateDate,
                Description = model.Description,
            };
        }
    }
}
