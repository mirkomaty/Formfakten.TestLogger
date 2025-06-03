using Microsoft.Extensions.Logging;

namespace Formfakten.TestLogger
{
	class LoggerFactory : ILoggerFactory
	{
		ILoggerProvider? provider;

		public void AddProvider( ILoggerProvider provider )
		{
			if (provider is LoggerProvider lp)
				this.provider = lp;
			else
				throw new Exception( $"Logger provider of type '{provider.GetType().FullName}' is not supported." );
		}

		public ILogger CreateLogger( string categoryName )
		{
			if (this.provider == null)
				throw new Exception( "No logging provider defined" );

			return provider.CreateLogger( categoryName );
		}

		public void Dispose()
		{
		}
	}
}
