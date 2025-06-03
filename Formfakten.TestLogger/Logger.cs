using Microsoft.Extensions.Logging;

namespace Formfakten.TestLogger
{
	public class LogEntry
	{
		public string Text { get; set; } = String.Empty;
		public LogLevel LogLevel { get; set; }
		public int ScopeId { get; set; }

		public string ScopeState { get; set; } = String.Empty;
	}
	public class Logger : ILogger
	{
		private static List<LogEntry> testLogs = new List<LogEntry>();
		private readonly string cat;
		private LogScope? scope;

		public Logger(string cat)
		{
			this.cat = cat;
		}

		public List<LogEntry>? TestLogs
		{
			get
			{
				return testLogs;
			}
		}

		public void Log<TState>( LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter )
		{
			int scopeId = 0;
			string scopeState = String.Empty;
			if (this.scope != null)
			{
				scopeId = this.scope.InstanceCount;
				scopeState = scope.State ?? String.Empty;
			}

			var prefix = $"{DateTime.Now.ToString()} [{Thread.CurrentThread.ManagedThreadId}|{scopeId}] {logLevel.ToString().ToUpper()} {cat} - ";

			testLogs.Add( new LogEntry { Text = $"{prefix}{formatter( state, exception )}", LogLevel = logLevel, ScopeId = scopeId, ScopeState = scopeState } );
		}

		public static void ClearLogs()
		{
			testLogs = new List<LogEntry>();
		}

		public static List<LogEntry> FindLogsWith( string text )
		{
			return testLogs.FindAll( l => -1 < l.Text.IndexOf( text ) );
		}

		public static List<LogEntry> FindTestLogsWith( LogLevel logLevel )
		{
			return testLogs.FindAll( l => l.LogLevel == logLevel );
		}

		public static List<LogEntry> FindLogsWithScope( int scopeId )
		{
			return testLogs.FindAll( l => l.ScopeId == scopeId );
		}

		public static List<LogEntry> FindLogsWithScopeState( string scopeState )
		{
			return testLogs.FindAll( l => l.ScopeState == scopeState );
		}

		public static List<LogEntry> FindLogsWithLevel( LogLevel logLevel )
		{
			return testLogs.FindAll( l => l.LogLevel == logLevel );
		}

		public bool IsEnabled( LogLevel logLevel )
		{
			return true;
		}

		public IDisposable? BeginScope<TState>( TState state ) where TState : notnull
		{
			this.scope = new LogScope( state?.ToString() );
			return this.scope;
		}
	}
}
