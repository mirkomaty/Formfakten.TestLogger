using System;

namespace Formfakten.TestLogger
{
	public class LogScope : IDisposable
	{
		static int globalCounter = 0;
		int instanceCount;
		public int InstanceCount => instanceCount;
		public string? State { get; private set; }

		public LogScope(string? state)
		{
			this.instanceCount = Interlocked.Increment( ref globalCounter );
			this.State = state;
		}

		public void Dispose()
		{
		}
	}
}
