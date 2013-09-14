#r "ConfigR.ConsoleApplication.exe"

using ConfigR.ConsoleApplication;

Configurator
    .Add("Foo", new Foo { Bar = "Baz" }); // the Foo class is defined in the applicaton!
