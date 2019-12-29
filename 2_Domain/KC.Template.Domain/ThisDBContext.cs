using KC.Template.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KC.Template.Domain
{
    public class ThisDBContext: DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ThisDBContext(DbContextOptions<ThisDBContext> options) : base(options)
        {

        }

        #region DbSet设置
        public DbSet<User> User { get; set; }
        #endregion

        /// <summary>
        /// 数据库表映射配置
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User")
                .HasKey(x => x.Id); 
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
