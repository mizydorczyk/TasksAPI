using TasksAPI.Entities;
using TasksAPI.Exceptions;

namespace TasksAPI
{
    public class BlacklistDrainer
    {
        private readonly TasksDbContext _dbContext;
        public BlacklistDrainer(TasksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Clear()
        {
            var blacklist = _dbContext.Blacklist;
            if(blacklist is null)
            {
                throw new NotFoundException("Blacklist not found");
            }
            var expired = blacklist.Where(x => x.ExpDate < DateTime.Now).ToList();
            _dbContext.Blacklist.RemoveRange(expired);
            _dbContext.SaveChanges();
        }
    }
}
