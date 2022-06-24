using NvgSharp;
using System.IO;

namespace PerfStat
{
	internal static class Resources
	{
		public static byte[] CreateTtfSystemByteArray()
		{
			var assembly = typeof(Resources).Assembly;

			var path = "PerfStat.Resources.Roboto-Regular.ttf";

			var ms = new MemoryStream();
			using (var stream = assembly.GetManifestResourceStream(path))
			{
				stream.CopyTo(ms);
			}

			return ms.ToArray();
		}
	}
}
