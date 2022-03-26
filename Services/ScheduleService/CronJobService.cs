using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ScheduleService
{
    public class CronScheduleService : IHostedService
    {
        public CronScheduleService()
        {
            CornJobRunnerFactory.RegisterAllJobs();
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            CornJobRunnerFactory.RunAllJobs();
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
