using KC.Template.Infrastructure;
using KC.Template.IService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KC.Template.BgService
{
    public class UserHS : BackgroundService
    {
        private readonly IUserService _userService;
        private readonly CustomSettings _customSettings;
        private readonly ILogger<UserHS> _logger;

        public UserHS(IOptions<CustomSettings> customSettingsOption,
            ILogger<UserHS> logger,
            IUserService userService)
        {
            _customSettings = customSettingsOption.Value;
            _logger = logger;
            _userService = userService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TaskFactory factory = new TaskFactory();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await factory.StartNew(() => _userService.Excute());
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "程序异常");
                    Thread.Sleep(_customSettings.SleepTime);
                }
            }
        }
    }
}
