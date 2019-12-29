using KC.Template.Domain;
using KC.Template.IRepository;
using Microsoft.EntityFrameworkCore;

namespace KC.Template.Repository
{
    /// <summary>
    /// Repository基类
    /// </summary>
    public class BaseRepository<T, TDBContext> : DefaultRepositoryImpl<T, TDBContext>, IBaseRepository<T>
        where T : class
        where TDBContext : DbContext
    {
        public BaseRepository(TDBContext dbContext)
            : base(dbContext)
        {

        }
    }
}
