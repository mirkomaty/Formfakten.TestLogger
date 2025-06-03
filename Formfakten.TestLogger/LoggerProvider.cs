using Microsoft.Extensions.Logging;

namespace Formfakten.TestLogger
{
	public class LoggerProvider : ILoggerProvider
	{
		public ILogger CreateLogger( string categoryName )
		{
			return new Logger( categoryName );
		}

		public void Dispose()
		{			
		}
	}
}
