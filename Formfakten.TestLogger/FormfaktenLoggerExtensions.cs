using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Formfakten.TestLogger
{
	public static class FormfaktenLoggerExtensions
	{
		public static ILoggingBuilder AddFormfaktenLogger( this ILoggingBuilder builder )
		{
			builder.Services.TryAddEnumerable( ServiceDescriptor.Singleton<ILoggerProvider, LoggerProvider>() );

			return builder;
		}
	}
}
