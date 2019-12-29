using Autofac;
using Autofac.Extensions.DependencyInjection;
using KC.Template.BgService.Extensions;
using KC.Template.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KC.Template.BgService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                logger.Info("程序开始运行");
                await CreateHostBuilder(args).Build().RunAsync();
            }
            catch (Exception e)
            {
                logger.Error(e, "程序异常停止");
                throw e;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
            {
                //use nlog
                loggingBuilder.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
                LogManager.LoadConfiguration("nlog.config");
                //此处通过程序修改NLog的日志数据库连接变量，也可以直接在nlog.config配置文件中配置
                LogManager.Configuration.Variables["connectionString"] = hostBuilderContext.Configuration.GetConnectionString("LogDBConnectionString");
            })
            .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
            {
                configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                serviceCollection.AddOptions();
                //自定义配置加载
                serviceCollection.Configure<CustomSettings>(hostBuilderContext.Configuration.GetSection("CustomSettings"));
            })
            .ConfigureContainer<ContainerBuilder>((hostBuilderContext, _containerBuilder) =>
            {
                _containerBuilder.RegisterModule(new AutofacModule(hostBuilderContext));
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
