using SimpleBroker.Demo;

Console.WriteLine("SimpleBroker demo – .NET 8\n");

var broker = new SimpleBroker.Broker.SimpleBroker();

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) => { e.Cancel = true; cts.Cancel(); };

DemoSetup.Run(broker, cts);

try { await Task.Delay(Timeout.Infinite, cts.Token); }
catch (OperationCanceledException) { }

Console.WriteLine("Encerrando... bye!\n");
