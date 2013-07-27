#r "ConfigR.Scheduler.exe"

using System;
using ConfigR.Scheduler;

Configurator
    .Add(
        "Schedules",
        new[]
        {
            new Schedule
            {
                Action = () => Console.WriteLine("{0}: 1st schedule is running!", DateTime.Now.ToString("o")),
                NextRun = DateTime.Now.AddSeconds(1),
                RepeatInterval = TimeSpan.FromSeconds(4),
            },
            new Schedule
            {
                Action = () => Console.WriteLine("{0}: 2nd schedule is running!", DateTime.Now.ToString("o")),
                NextRun = DateTime.Now.AddSeconds(2),
                RepeatInterval = TimeSpan.FromSeconds(4),
            },
            new Schedule
            {
                Action = () => Console.WriteLine("{0}: 3rd schedule is running!", DateTime.Now.ToString("o")),
                NextRun = DateTime.Now.AddSeconds(3),
                RepeatInterval = TimeSpan.FromSeconds(4),
            },
        });