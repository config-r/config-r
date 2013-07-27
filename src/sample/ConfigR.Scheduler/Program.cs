// <copyright file="Program.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scheduler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Common.Logging;
    using ConfigR;
    using Topshelf;

    public static class Program
    {
        public static void Main(string[] args)
        {
            // NOTE (Adam): I purposely ommitted some stuff which ought to be in here to avoid cluttering the sample, including:
            // * thread safety between schedule execution and timer disposal
            // * initialization/schedule execution overlapping with next dueTime - currently an exception would be thrown due to negative dueTime
            HostFactory.Run(h => h.Service<Schedule[]>(s =>
            {
                s.ConstructUsing(() => Configurator.Get<Schedule[]>("Schedules"));

                Dictionary<Schedule, Timer> timers = null;
                s.WhenStarted(schedules => timers = schedules.ToDictionary(
                    schedule => schedule,
                    schedule => new Timer(
                        state =>
                        {
                            try
                            {
                                schedule.Action();
                            }
                            catch (Exception ex)
                            {
                                LogManager.GetCurrentClassLogger().Error("Error executing schedule", ex);
                            }

                            timers[schedule].Change((schedule.NextRun += schedule.RepeatInterval) - DateTime.Now, TimeSpan.FromMilliseconds(-1));
                        },
                        null,
                        schedule.NextRun - DateTime.Now,
                        TimeSpan.FromMilliseconds(-1))));

                s.WhenStopped(schedules => timers.Values.ToList().ForEach(timer => timer.Dispose()));
            }));
        }
    }
}
