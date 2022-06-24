using FontStashSharp;
using Microsoft.Xna.Framework;
using NvgSharp;

namespace PerfStat
{
	public class PerfGraphWidget
	{
		private float[] _values = new float[100];
		private int _head;
		private readonly FontSystem _fontSystem;

		public PerfGraphStyle Style { get; set; }
		public string Name { get; set; }

		public PerfGraphWidget(PerfGraphStyle style = PerfGraphStyle.Fps, string name = "Frame Time")
		{
			Style = style;
			Name = name;

			_fontSystem = new FontSystem();
			_fontSystem.AddFont(Resources.CreateTtfSystemByteArray());
		}

		public void Update(double frameTime)
		{
			_head = (_head + 1) % _values.Length;
			_values[_head] = (float)frameTime;
		}

		public float GetAverage()
		{
			float avg = 0;
			for (var i = 0; i < _values.Length; i++)
			{
				avg += _values[i];
			}
			return avg / _values.Length;
		}

		public void Render(NvgContext vg, int x, int y)
		{
			int i;
			float avg, w, h;
			string str;

			avg = GetAverage();

			w = 200;
			h = 35;

			vg.BeginPath();
			vg.Rect(x, y, w, h);
			vg.FillColor(new Color(0, 0, 0, 128));
			vg.Fill();

			vg.BeginPath();
			vg.MoveTo(x, y + h);
			if (Style == PerfGraphStyle.Fps)
			{
				for (i = 0; i < _values.Length; i++)
				{
					float v = 1.0f / (0.00001f + _values[(_head + i) % _values.Length]);
					float vx, vy;
					if (v > 80.0f)
						v = 80.0f;
					vx = x + (float)i / (_values.Length - 1) * w;
					vy = y + h - ((v / 80.0f) * h);
					vg.LineTo(vx, vy);
				}
			}
			else if (Style == PerfGraphStyle.Percent)
			{
				for (i = 0; i < _values.Length; i++)
				{
					float v = _values[(_head + i) % _values.Length] * 1.0f;
					float vx, vy;
					if (v > 100.0f)
						v = 100.0f;
					vx = x + ((float)i / (_values.Length - 1)) * w;
					vy = y + h - ((v / 100.0f) * h);
					vg.LineTo(vx, vy);
				}
			}
			else
			{
				for (i = 0; i < _values.Length; i++)
				{
					float v = _values[(_head + i) % _values.Length] * 1000.0f;
					float vx, vy;
					if (v > 20.0f)
						v = 20.0f;
					vx = x + ((float)i / (_values.Length - 1)) * w;
					vy = y + h - ((v / 20.0f) * h);
					vg.LineTo(vx, vy);
				}
			}
			vg.LineTo(x + w, y + h);
			vg.FillColor(new Color(255, 192, 0, 128));
			vg.Fill();


			if (!string.IsNullOrEmpty(Name))
			{
				var font = _fontSystem.GetFont(14);
				vg.FillColor(new Color(240, 240, 240, 192));
				vg.Text(font, Name, x + 3, y + 1);
			}

			if (Style == PerfGraphStyle.Fps)
			{
				var font = _fontSystem.GetFont(18);
				vg.FillColor(new Color(240, 240, 240, 255));
				str = string.Format("{0:0.00} FPS", 1.0f / avg);
				vg.Text(font, str, x + w - 3, y + 1, TextHorizontalAlignment.Right);

				font = _fontSystem.GetFont(15);
				vg.FillColor(new Color(240, 240, 240, 160));
				str = string.Format("{0:0.00} ms", avg * 1000.0f);
				vg.Text(font, str, x + w - 3, y + h - 1, TextHorizontalAlignment.Right, TextVerticalAlignment.Bottom);
			}
			else if (Style == PerfGraphStyle.Percent)
			{
				var font = _fontSystem.GetFont(18);
				vg.FillColor(new Color(240, 240, 240, 255));
				str = string.Format("{0:0.00} %%", avg);
				vg.Text(font, str, x + w - 3, y + 1, TextHorizontalAlignment.Right);
			}
			else
			{
				var font = _fontSystem.GetFont(18);
				vg.FillColor(new Color(240, 240, 240, 255));
				str = string.Format("{0:0.00} ms", avg * 1000.0f);
				vg.Text(font, str, x + w - 3, y + 1, TextHorizontalAlignment.Right);
			}
		}
	}
}