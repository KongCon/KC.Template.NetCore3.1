using Autofac;
using KC.Template.Domain;
using KC.Template.Repository;
using KC.Template.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace KC.Template.BgService.Extensions
{
    public class AutofacModule : Module
    {
        private HostBuilderContext _hostBuilderContext { get; }
        public AutofacModule(HostBuilderContext hostBuilderContext)
        {
            _hostBuilderContext = hostBuilderContext;
        }

        protected override void Load(ContainerBuilder builder)
        {
            #region 注入数据库上下文
            var dbConnection = _hostBuilderContext.Configuration.GetConnectionString("ThisDBConnection");
            builder.Register(x =>
            {
                return new DbContextOptionsBuilder<ThisDBContext>()
                .UseLazyLoadingProxies()//EFCore默认不开启延迟加载，此处开启延迟加载（即使用导航属性）
                .UseSqlServer(dbConnection)
                 .Options;
                // 如果使用SQL Server 2008数据库，请添加UseRowNumberForPaging的选项
                // 参考资料:https://github.com/aspnet/EntityFrameworkCore/issues/4616
                //.UseSqlServer(dbConnection, o=>o.UseRowNumberForPaging())

            }).InstancePerLifetimeScope();

            builder.RegisterType<ThisDBContext>().AsSelf().InstancePerLifetimeScope();
            #endregion

            #region 注入Service层
            builder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
            #endregion

            #region 注入Repository层
            //builder.RegisterAssemblyTypes(typeof(BaseRepository<>).Assembly)
            //        .AsImplementedInterfaces()
            //        .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(BaseRepository<,>).Assembly)
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
            #endregion

            #region 添加后台服务
            builder.RegisterType<UserHS>().As<IHostedService>().SingleInstance();
            #endregion
        }
    }
}
