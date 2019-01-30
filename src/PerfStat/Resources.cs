using NvgSharp;
using System.IO;

namespace PerfStat
{
	internal static class Resources
	{
		private static int? _defaultFontId;

		public static int GetDefaultFontId(NvgContext nvgContext)
		{
			if (_defaultFontId != null)
			{
				return _defaultFontId.Value;
			}

			_defaultFontId = nvgContext.CreateFontMem("perfStat", CreateTtfSystemByteArray());

			return _defaultFontId.Value;
		}

		private static byte[] CreateTtfSystemByteArray()
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
