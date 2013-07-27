// <copyright file="Schedule.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scheduler
{
    using System;

    public class Schedule
    {
        public Action Action { get; set; }

        public DateTime NextRun { get; set; }

        public TimeSpan RepeatInterval { get; set; }
    }
}
