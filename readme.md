# Formfakten.TestLogger

Smallest possible implementation of ILogger, ILoggerFactory, and ILoggerProvider.

The log entries are stored in-memory and can be retrieved by user code. There are some methods to get filtered subsets of the logs entries and a method to clear the log store.

That way you can use this implementation for test purposes.

## Using the TestLogger

Firstly you need a host implementation with DI:

```
var builder = Host.CreateDefaultBuilder();
builder.ConfigureServices( services =>
{
	services.AddLogging( b =>
	{
		b.ClearProviders();
		b.AddFormfaktenLogger();
	} );
} );

var host = builder.Build();
this.globalServiceProvider = host.Services;
```

Note the call of `b.AddFormfaktenLogger()`. In test projects it might be useful to keep the ServiceProvider in a global variable, so that the instance is kept in memory.

After initialization you can use the logger:

```
// Assume your current class is called LoggerTests
var logger = globalServiceProvider.GetService<ILogger<LoggerTests>>();
logger.LogInformation( "This is an info log" );
var logs = Logger.FindLogsWith( "This is an info log" );
Assert.That( logs.Count > 0 );
var logEntry = logs[0];
Assert.That( logEntry.LogLevel == LogLevel.Information );
Assert.That( logEntry.ScopeId == 0 );
```

You can log into scopes:

```
logger.BeginScope( "Scope 1" );
logger.LogError( "This is an error log" );
var logs = Logger.FindLogsWith( "This is an error log" );
Assert.That( logs.Count > 0 );
var errorLogs = 
var logEntry = logs[0];
Assert.That( logEntry.LogLevel == LogLevel.Error );
Assert.That( logEntry.ScopeId == 1 );
Assert.That( logEntry.ScopeState == "Scope 1" );

logs = Logger.FindLogsWithScope(1);
Assert.That( logs.Count == 1 );

logs = Logger.FindLogsWithScopeState( "Scope 1" );
Assert.That( logs.Count == 1 );

logs = Logger.FindLogsWithLevel( LogLevel.Error );
Assert.That( logs.Count == 1 );

Logger.ClearLogs();

logs = Logger.FindLogsWithScopeState( "Scope 1" );
Assert.That( logs.Count == 0 );
```

See the TestLogger.Tests project in the [github repo](https://github.com/mirkomaty/Formfakten.TestLogger).
