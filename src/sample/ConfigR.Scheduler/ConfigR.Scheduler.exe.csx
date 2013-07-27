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
                Action = () =>
                    {
                        Console.WriteLine("{0}: The 1st schedule is sending some emails!", DateTime.Now.ToString("o"));
                        // send some emails
                    },
                NextRun = DateTime.Now.AddSeconds(4),
                RepeatInterval = TimeSpan.FromSeconds(8),
            },
            new Schedule
            {
                Action = () =>
                    {
                        Console.WriteLine("{0}: The 2nd schedule is downloading some reports!", DateTime.Now.ToString("o"));
                        // download some reports
                    },
                NextRun = DateTime.Now.AddSeconds(6),
                RepeatInterval = TimeSpan.FromSeconds(8),
            },
            new Schedule
            {
                Action = () =>
                    {
                        Console.WriteLine("{0}: The 3rd schedule is performing some housekeeping!", DateTime.Now.ToString("o"));
                        // perform some housekeeping
                    },
                NextRun = DateTime.Now.AddSeconds(8),
                RepeatInterval = TimeSpan.FromSeconds(8),
            }
        });