using Formfakten.TestLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace TestLogger.Tests
{
	[TestFixture]
	public class LoggerTests
	{
		IServiceProvider globalServiceProvider;

		public LoggerTests()
		{
			Build();
			// This code avoids warning 8616.
			if (globalServiceProvider == null)
				throw new Exception( "No Service Provider" );
		}

		void Build()
		{
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
		}

		[Test]
		public void TestIfDiIsAvailable()
		{
			var logger = globalServiceProvider.GetService<ILogger<LoggerTests>>();
			Assert.That( logger != null );
		}

		[Test]
		public void CanLogWithLogger()
		{
			var logger = globalServiceProvider.GetService<ILogger<LoggerTests>>();
			Assert.That( logger != null );
			logger!.LogInformation( "This is an info log" );
			var logs = Logger.FindLogsWith( "This is an info log" );
			Assert.That( logs.Count > 0 );
			var logEntry = logs[0];
			Assert.That( logEntry.LogLevel == LogLevel.Information );
			Assert.That( logEntry.ScopeId == 0 );
		}

		[Test]
		public void CanLogWithScope()
		{
			var logger = globalServiceProvider.GetService<ILogger<LoggerTests>>();
			logger!.BeginScope( "Scope 1" );
			Assert.That( logger != null );
			logger!.LogError( "This is an error log" );
			var logs = Logger.FindLogsWith( "This is an error log" );
			Assert.That( logs.Count > 0 );
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

			logs = Logger.FindLogsWithScope( 2 );
			Assert.That( logs.Count == 0 );

			logs = Logger.FindLogsWithScopeState( "Scope 2" );
			Assert.That( logs.Count == 0 );

			Logger.ClearLogs();

			logs = Logger.FindLogsWithScope( 1 );
			Assert.That( logs.Count == 0 );

			logs = Logger.FindLogsWithScopeState( "Scope 1" );
			Assert.That( logs.Count == 0 );
		}
	}
}
