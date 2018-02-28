using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using Quartz;
using Quartz.Impl;

namespace NightWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private IScheduler _sched;

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole2 is running");

            try
            {
                RunAsync(_cancellationTokenSource.Token).Wait();
            }
            finally
            {
                _runCompleteEvent.Set();
            }
        }


        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole2 has been started");
            ConfigureScheduler();
            return result;
        }


        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole2 is stopping");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole2 has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000, cancellationToken);
            }
        }

        private void ConfigureScheduler()
        {
            var schedFact = new StdSchedulerFactory();

            _sched = schedFact.GetScheduler().Result;
            var job = new JobDetailImpl("DailyJob", null, typeof(DailyJob));
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(2, 0)
                .InTimeZone(timeZoneInfo);
            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSchedule(cronScheduleBuilder)
                .Build();

            _sched.ScheduleJob(job, trigger);
            _sched.Start();
        }
    }
}