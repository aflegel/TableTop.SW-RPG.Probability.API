using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Visualizer.Extensions
{
	[DefaultStatusCode(DefaultStatusCode)]
	public class ServiceUnavailableResult : StatusCodeResult
	{
		private const int DefaultStatusCode = StatusCodes.Status503ServiceUnavailable;

		/// <summary>
		/// Creates a new <see cref="ServiceUnavailableResult"/> instance.
		/// </summary>
		public ServiceUnavailableResult() : base(DefaultStatusCode)
		{
		}
	}
}
