using KC.Template.Domain;
using KC.Template.Domain.Entities;
using KC.Template.IRepository;

namespace KC.Template.Repository
{
    public class UserRepository : BaseRepository<User, ThisDBContext>, IUserRepository
    {
        public UserRepository(ThisDBContext dbContext)
            : base(dbContext)
        {

        }
    }
}
