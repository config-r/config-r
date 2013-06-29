#r "ConfigR.Testing.Service.exe"

using ConfigR.Testing.Service;

Configurator
    .Add("settings", new Settings { Greeting = "hello world", Valediction = "goodbye world" });