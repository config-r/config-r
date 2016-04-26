#r "ConfigR.Tests.Smoke.Service.exe"

using ConfigR.Tests.Smoke.Service;

Add("settings", new Settings { Greeting = "hello world", Valediction = "goodbye world" });