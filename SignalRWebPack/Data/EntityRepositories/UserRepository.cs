using Microsoft.Extensions.Logging;
using SignalRWebPack.Models;

namespace SignalRWebPack.Data.EntityRepositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(EfContext db, 
            ILogger<Repository<User>> logger)
            : base(db, logger)
        {
        }
    }
}
