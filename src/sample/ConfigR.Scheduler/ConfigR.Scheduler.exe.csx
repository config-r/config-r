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
                Action = () => Console.WriteLine("First schedule is running!"),
                NextRun = DateTime.Now.AddSeconds(1),
                RepeatInterval = TimeSpan.FromSeconds(4),
            },
            new Schedule
            {
                Action = () => Console.WriteLine("Second schedule is running!"),
                NextRun = DateTime.Now.AddSeconds(2),
                RepeatInterval = TimeSpan.FromSeconds(4),
            },
            new Schedule
            {
                Action = () => Console.WriteLine("Third schedule is running!"),
                NextRun = DateTime.Now.AddSeconds(2),
                RepeatInterval = TimeSpan.FromSeconds(4),
            },
        });