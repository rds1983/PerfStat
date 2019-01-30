﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using FontStashSharp;
using NvgSharp;

namespace PerfStat.Samples.Demo
{
	public class Demo
	{
		private static readonly string ICON_SEARCH = Char.ConvertFromUtf32(0x1F50D);
		private static readonly string ICON_CIRCLED_CROSS = Char.ConvertFromUtf32(0x2716);
		private static readonly string ICON_CHEVRON_RIGHT = Char.ConvertFromUtf32(0xE75E);
		private static readonly string ICON_CHECK = Char.ConvertFromUtf32(0x2713);
		private static readonly string ICON_LOGIN = Char.ConvertFromUtf32(0xE740);
		private static readonly string ICON_TRASH = Char.ConvertFromUtf32(0xE729);

		int fontNormal, fontBold, fontIcons;
		int[] images = new int[12];

		//static float minf(float a, float b) { return a < b ? a : b; }
		static float maxf(float a, float b)
		{
			return a > b ? a : b;
		}
		//static float absf(float a) { return a >= 0.0f ? a : -a; }
		static float clampf(float a, float mn, float mx)
		{
			return a < mn ? mn : (a > mx ? mx : a);
		}

		// Returns 1 if col.rgba is 0.0f,0.0f,0.0f,0.0f, 0 otherwise
		public static bool isBlack(Color col)
		{
			return col == Color.Transparent;
		}

		static int mini(int a, int b)
		{
			return a < b ? a : b;
		}


		public static void drawWindow(NvgContext vg, string title, float x, float y, float w, float h)
		{
			float cornerRadius = 3.0f;
			Paint shadowPaint;
			Paint headerPaint;

			vg.Save();
			//	ClearState(vg);

			// Window
			vg.BeginPath();
			vg.RoundedRect(x, y, w, h, cornerRadius);
			vg.FillColor(new Color(28, 30, 34, 192));
			//	vg.FillColor(new Color(0,0,0,128));
			vg.Fill();

			// Drop shadow
			shadowPaint = vg.BoxGradient(x, y + 2, w, h, cornerRadius * 2, 10, new Color(0, 0, 0, 128), new Color(0, 0, 0, 0));
			vg.BeginPath();
			vg.Rect(x - 10, y - 10, w + 20, h + 30);
			vg.RoundedRect(x, y, w, h, cornerRadius);
			vg.PathWinding(Solidity.Hole);
			vg.FillPaint(shadowPaint);
			vg.Fill();

			// Header
			headerPaint = vg.LinearGradient(x, y, x, y + 15, new Color(255, 255, 255, 8), new Color(0, 0, 0, 16));
			vg.BeginPath();
			vg.RoundedRect(x + 1, y + 1, w - 2, 30, cornerRadius - 1);
			vg.FillPaint(headerPaint);
			vg.Fill();
			vg.BeginPath();
			vg.MoveTo(x + 0.5f, y + 0.5f + 30);
			vg.LineTo(x + 0.5f + w - 1, y + 0.5f + 30);
			vg.StrokeColor(new Color(0, 0, 0, 32));
			vg.Stroke();

			vg.FontSize(18.0f);
			vg.FontFace("sans-bold");
			vg.TextAlign(Alignment.Center | Alignment.Middle);

			vg.FontBlur(2);
			vg.FillColor(new Color(0, 0, 0, 128));
			vg.Text(x + w / 2, y + 16 + 1, title);

			vg.FontBlur(0);
			vg.FillColor(new Color(220, 220, 220, 160));
			vg.Text(x + w / 2, y + 16, title);

			vg.Restore();
		}

		public static void drawSearchBox(NvgContext vg, string text, float x, float y, float w, float h)
		{
			Paint bg;
			float cornerRadius = h / 2 - 1;

			// Edit
			bg = vg.BoxGradient(x, y + 1.5f, w, h, h / 2, 5, new Color(0, 0, 0, 16), new Color(0, 0, 0, 92));
			vg.BeginPath();
			vg.RoundedRect(x, y, w, h, cornerRadius);
			vg.FillPaint(bg);
			vg.Fill();

			/*	vg.BeginPath();
				vg.RoundedRect(x+0.5f,y+0.5f, w-1,h-1, cornerRadius-0.5f);
				vg.StrokeColor(new Color(0,0,0,48));
				vg.Stroke();*/

			vg.FontSize(h * 1.3f);
			vg.FontFace("icons");
			vg.FillColor(new Color(255, 255, 255, 64));
			vg.TextAlign(Alignment.Center | Alignment.Middle);
			vg.Text(x + h * 0.55f, y + h * 0.55f, ICON_SEARCH);


			vg.FontSize(20.0f);
			vg.FontFace("sans");
			vg.FillColor(new Color(255, 255, 255, 32));

			vg.TextAlign(Alignment.Left | Alignment.Middle);
			vg.Text(x + h * 1.05f, y + h * 0.5f, text);

			vg.FontSize(h * 1.3f);
			vg.FontFace("icons");
			vg.FillColor(new Color(255, 255, 255, 32));
			vg.TextAlign(Alignment.Center | Alignment.Middle);
			vg.Text(x + w - h * 0.55f, y + h * 0.55f, ICON_CIRCLED_CROSS);
		}

		public static void drawDropDown(NvgContext vg, string text, float x, float y, float w, float h)
		{
			Paint bg;
			float cornerRadius = 4.0f;

			bg = vg.LinearGradient(x, y, x, y + h, new Color(255, 255, 255, 16), new Color(0, 0, 0, 16));
			vg.BeginPath();
			vg.RoundedRect(x + 1, y + 1, w - 2, h - 2, cornerRadius - 1);
			vg.FillPaint(bg);
			vg.Fill();

			vg.BeginPath();
			vg.RoundedRect(x + 0.5f, y + 0.5f, w - 1, h - 1, cornerRadius - 0.5f);
			vg.StrokeColor(new Color(0, 0, 0, 48));
			vg.Stroke();

			vg.FontSize(20.0f);
			vg.FontFace("sans");
			vg.FillColor(new Color(255, 255, 255, 160));
			vg.TextAlign(Alignment.Left | Alignment.Middle);
			vg.Text(x + h * 0.3f, y + h * 0.5f, text);

			vg.FontSize(h * 1.3f);
			vg.FontFace("icons");
			vg.FillColor(new Color(255, 255, 255, 64));
			vg.TextAlign(Alignment.Center | Alignment.Middle);
			vg.Text(x + w - h * 0.5f, y + h * 0.5f, ICON_CHEVRON_RIGHT);
		}

		public static void drawLabel(NvgContext vg, string text, float x, float y, float w, float h)
		{
			vg.FontSize(18.0f);
			vg.FontFace("sans");
			vg.FillColor(new Color(255, 255, 255, 128));

			vg.TextAlign(Alignment.Left | Alignment.Middle);
			vg.Text(x, y + h * 0.5f, text);
		}

		public static void drawEditBoxBase(NvgContext vg, float x, float y, float w, float h)
		{
			Paint bg;
			// Edit
			bg = vg.BoxGradient(x + 1, y + 1 + 1.5f, w - 2, h - 2, 3, 4, new Color(255, 255, 255, 32), new Color(32, 32, 32, 32));
			vg.BeginPath();
			vg.RoundedRect(x + 1, y + 1, w - 2, h - 2, 4 - 1);
			vg.FillPaint(bg);
			vg.Fill();

			vg.BeginPath();
			vg.RoundedRect(x + 0.5f, y + 0.5f, w - 1, h - 1, 4 - 0.5f);
			vg.StrokeColor(new Color(0, 0, 0, 48));
			vg.Stroke();
		}

		public static void drawEditBox(NvgContext vg, string text, float x, float y, float w, float h)
		{

			drawEditBoxBase(vg, x, y, w, h);

			vg.FontSize(20.0f);
			vg.FontFace("sans");
			vg.FillColor(new Color(255, 255, 255, 64));
			vg.TextAlign(Alignment.Left | Alignment.Middle);
			vg.Text(x + h * 0.3f, y + h * 0.5f, text);
		}

		public static void drawEditBoxNum(NvgContext vg,
							string text, string units, float x, float y, float w, float h)
		{
			float uw;

			drawEditBoxBase(vg, x, y, w, h);

			Bounds bounds = new Bounds();
			uw = vg.TextBounds(0, 0, units, ref bounds);

			vg.FontSize(18.0f);
			vg.FontFace("sans");
			vg.FillColor(new Color(255, 255, 255, 64));
			vg.TextAlign(Alignment.Right | Alignment.Middle);
			vg.Text(x + w - h * 0.3f, y + h * 0.5f, units);

			vg.FontSize(20.0f);
			vg.FontFace("sans");
			vg.FillColor(new Color(255, 255, 255, 128));
			vg.TextAlign(Alignment.Right | Alignment.Middle);
			vg.Text(x + w - uw - h * 0.5f, y + h * 0.5f, text);
		}

		public static void drawCheckBox(NvgContext vg, string text, float x, float y, float w, float h)
		{
			Paint bg;

			vg.FontSize(18.0f);
			vg.FontFace("sans");
			vg.FillColor(new Color(255, 255, 255, 160));

			vg.TextAlign(Alignment.Left | Alignment.Middle);
			vg.Text(x + 28, y + h * 0.5f, text);

			bg = vg.BoxGradient(x + 1, y + (int)(h * 0.5f) - 9 + 1, 18, 18, 3, 3, new Color(0, 0, 0, 32), new Color(0, 0, 0, 92));
			vg.BeginPath();
			vg.RoundedRect(x + 1, y + (int)(h * 0.5f) - 9, 18, 18, 3);
			vg.FillPaint(bg);
			vg.Fill();

			vg.FontSize(40);
			vg.FontFace("icons");
			vg.FillColor(new Color(255, 255, 255, 128));
			vg.TextAlign(Alignment.Center | Alignment.Middle);
			vg.Text(x + 9 + 2, y + h * 0.5f, ICON_CHECK);
		}

		public static void drawButton(NvgContext vg, string preicon, string text, float x, float y, float w, float h, Color col)
		{
			Paint bg;
			float cornerRadius = 4.0f;
			float tw = 0, iw = 0;

			bg = vg.LinearGradient(x, y, x, y + h, new Color(255, 255, 255, isBlack(col) ? 16 : 32), new Color(0, 0, 0, isBlack(col) ? 16 : 32));
			vg.BeginPath();
			vg.RoundedRect(x + 1, y + 1, w - 2, h - 2, cornerRadius - 1);
			if (!isBlack(col))
			{
				vg.FillColor(col);
				vg.Fill();
			}
			vg.FillPaint(bg);
			vg.Fill();

			vg.BeginPath();
			vg.RoundedRect(x + 0.5f, y + 0.5f, w - 1, h - 1, cornerRadius - 0.5f);
			vg.StrokeColor(new Color(0, 0, 0, 48));
			vg.Stroke();

			vg.FontSize(20.0f);
			vg.FontFace("sans-bold");
			Bounds bounds = new Bounds();
			tw = vg.TextBounds(0, 0, text, ref bounds);
			if (!string.IsNullOrEmpty(preicon))
			{
				vg.FontSize(h * 1.3f);
				vg.FontFace("icons");
				iw = vg.TextBounds(0, 0, preicon, ref bounds);
				iw += h * 0.15f;
			}

			if (!string.IsNullOrEmpty(preicon))
			{
				vg.FontSize(h * 1.3f);
				vg.FontFace("icons");
				vg.FillColor(new Color(255, 255, 255, 96));
				vg.TextAlign(Alignment.Left | Alignment.Middle);
				vg.Text(x + w * 0.5f - tw * 0.5f - iw * 0.75f, y + h * 0.5f, preicon);
			}

			vg.FontSize(20.0f);
			vg.FontFace("sans-bold");
			vg.TextAlign(Alignment.Left | Alignment.Middle);
			vg.FillColor(new Color(0, 0, 0, 160));
			vg.Text(x + w * 0.5f - tw * 0.5f + iw * 0.25f, y + h * 0.5f - 1, text);
			vg.FillColor(new Color(255, 255, 255, 160));
			vg.Text(x + w * 0.5f - tw * 0.5f + iw * 0.25f, y + h * 0.5f, text);
		}

		public static void drawSlider(NvgContext vg, float pos, float x, float y, float w, float h)
		{
			Paint bg, knob;
			float cy = y + (int)(h * 0.5f);
			float kr = (int)(h * 0.25f);

			vg.Save();
			//	ClearState(vg);

			// Slot
			bg = vg.BoxGradient(x, cy - 2 + 1, w, 4, 2, 2, new Color(0, 0, 0, 32), new Color(0, 0, 0, 128));
			vg.BeginPath();
			vg.RoundedRect(x, cy - 2, w, 4, 2);
			vg.FillPaint(bg);
			vg.Fill();

			// Knob Shadow
			bg = vg.RadialGradient(x + (int)(pos * w), cy + 1, kr - 3, kr + 3, new Color(0, 0, 0, 64), new Color(0, 0, 0, 0));
			vg.BeginPath();
			vg.Rect(x + (int)(pos * w) - kr - 5, cy - kr - 5, kr * 2 + 5 + 5, kr * 2 + 5 + 5 + 3);
			vg.Circle(x + (int)(pos * w), cy, kr);
			vg.PathWinding(Solidity.Hole);
			vg.FillPaint(bg);
			vg.Fill();

			// Knob
			knob = vg.LinearGradient(x, cy - kr, x, cy + kr, new Color(255, 255, 255, 16), new Color(0, 0, 0, 16));
			vg.BeginPath();
			vg.Circle(x + (int)(pos * w), cy, kr - 1);
			vg.FillColor(new Color(40, 43, 48, 255));
			vg.Fill();
			vg.FillPaint(knob);
			vg.Fill();

			vg.BeginPath();
			vg.Circle(x + (int)(pos * w), cy, kr - 0.5f);
			vg.StrokeColor(new Color(0, 0, 0, 92));
			vg.Stroke();

			vg.Restore();
		}

		public static void drawEyes(NvgContext vg, float x, float y, float w, float h, float mx, float my, float t)
		{
			Paint gloss, bg;
			float ex = w * 0.23f;
			float ey = h * 0.5f;
			float lx = x + ex;
			float ly = y + ey;
			float rx = x + w - ex;
			float ry = y + ey;
			float dx, dy, d;
			float br = (ex < ey ? ex : ey) * 0.5f;
			float blink = 1 - (float)(Math.Pow(Math.Sin(t * 0.5f), 200) * 0.8f);

			bg = vg.LinearGradient(x, y + h * 0.5f, x + w * 0.1f, y + h, new Color(0, 0, 0, 32), new Color(0, 0, 0, 16));
			vg.BeginPath();
			vg.Ellipse(lx + 3.0f, ly + 16.0f, ex, ey);
			vg.Ellipse(rx + 3.0f, ry + 16.0f, ex, ey);
			vg.FillPaint(bg);
			vg.Fill();

			bg = vg.LinearGradient(x, y + h * 0.25f, x + w * 0.1f, y + h, new Color(220, 220, 220, 255), new Color(128, 128, 128, 255));
			vg.BeginPath();
			vg.Ellipse(lx, ly, ex, ey);
			vg.Ellipse(rx, ry, ex, ey);
			vg.FillPaint(bg);
			vg.Fill();

			dx = (mx - rx) / (ex * 10);
			dy = (my - ry) / (ey * 10);
			d = (float)Math.Sqrt(dx * dx + dy * dy);
			if (d > 1.0f)
			{
				dx /= d;
				dy /= d;
			}
			dx *= ex * 0.4f;
			dy *= ey * 0.5f;
			vg.BeginPath();
			vg.Ellipse(lx + dx, ly + dy + ey * 0.25f * (1 - blink), br, br * blink);
			vg.FillColor(new Color(32, 32, 32, 255));
			vg.Fill();

			dx = (mx - rx) / (ex * 10);
			dy = (my - ry) / (ey * 10);
			d = (float)Math.Sqrt(dx * dx + dy * dy);
			if (d > 1.0f)
			{
				dx /= d;
				dy /= d;
			}
			dx *= ex * 0.4f;
			dy *= ey * 0.5f;
			vg.BeginPath();
			vg.Ellipse(rx + dx, ry + dy + ey * 0.25f * (1 - blink), br, br * blink);
			vg.FillColor(new Color(32, 32, 32, 255));
			vg.Fill();

			gloss = vg.RadialGradient(lx - ex * 0.25f, ly - ey * 0.5f, ex * 0.1f, ex * 0.75f, new Color(255, 255, 255, 128), new Color(255, 255, 255, 0));
			vg.BeginPath();
			vg.Ellipse(lx, ly, ex, ey);
			vg.FillPaint(gloss);
			vg.Fill();

			gloss = vg.RadialGradient(rx - ex * 0.25f, ry - ey * 0.5f, ex * 0.1f, ex * 0.75f, new Color(255, 255, 255, 128), new Color(255, 255, 255, 0));
			vg.BeginPath();
			vg.Ellipse(rx, ry, ex, ey);
			vg.FillPaint(gloss);
			vg.Fill();
		}

		public static void drawGraph(NvgContext vg, float x, float y, float w, float h, float t)
		{
			Paint bg;
			float[] samples = new float[6];
			float[] sx = new float[6], sy = new float[6];
			float dx = w / 5.0f;
			int i;

			samples[0] = (1 + (float)Math.Sin(t * 1.2345f + (float)Math.Cos(t * 0.33457f) * 0.44f)) * 0.5f;
			samples[1] = (1 + (float)Math.Sin(t * 0.68363f + (float)Math.Cos(t * 1.3f) * 1.55f)) * 0.5f;
			samples[2] = (1 + (float)Math.Sin(t * 1.1642f + (float)Math.Cos(t * 0.33457) * 1.24f)) * 0.5f;
			samples[3] = (1 + (float)Math.Sin(t * 0.56345f + (float)Math.Cos(t * 1.63f) * 0.14f)) * 0.5f;
			samples[4] = (1 + (float)Math.Sin(t * 1.6245f + (float)Math.Cos(t * 0.254f) * 0.3f)) * 0.5f;
			samples[5] = (1 + (float)Math.Sin(t * 0.345f + (float)Math.Cos(t * 0.03f) * 0.6f)) * 0.5f;

			for (i = 0; i < 6; i++)
			{
				sx[i] = x + i * dx;
				sy[i] = y + h * samples[i] * 0.8f;
			}

			// Graph background
			bg = vg.LinearGradient(x, y, x, y + h, new Color(0, 160, 192, 0), new Color(0, 160, 192, 64));
			vg.BeginPath();
			vg.MoveTo(sx[0], sy[0]);
			for (i = 1; i < 6; i++)
				vg.BezierTo(sx[i - 1] + dx * 0.5f, sy[i - 1], sx[i] - dx * 0.5f, sy[i], sx[i], sy[i]);
			vg.LineTo(x + w, y + h);
			vg.LineTo(x, y + h);
			vg.FillPaint(bg);
			vg.Fill();

			// Graph line
			vg.BeginPath();
			vg.MoveTo(sx[0], sy[0] + 2);
			for (i = 1; i < 6; i++)
				vg.BezierTo(sx[i - 1] + dx * 0.5f, sy[i - 1] + 2, sx[i] - dx * 0.5f, sy[i] + 2, sx[i], sy[i] + 2);
			vg.StrokeColor(new Color(0, 0, 0, 32));
			vg.StrokeWidth(3.0f);
			vg.Stroke();

			vg.BeginPath();
			vg.MoveTo(sx[0], sy[0]);
			for (i = 1; i < 6; i++)
				vg.BezierTo(sx[i - 1] + dx * 0.5f, sy[i - 1], sx[i] - dx * 0.5f, sy[i], sx[i], sy[i]);
			vg.StrokeColor(new Color(0, 160, 192, 255));
			vg.StrokeWidth(3.0f);
			vg.Stroke();

			// Graph sample pos
			for (i = 0; i < 6; i++)
			{
				bg = vg.RadialGradient(sx[i], sy[i] + 2, 3.0f, 8.0f, new Color(0, 0, 0, 32), new Color(0, 0, 0, 0));
				vg.BeginPath();
				vg.Rect(sx[i] - 10, sy[i] - 10 + 2, 20, 20);
				vg.FillPaint(bg);
				vg.Fill();
			}

			vg.BeginPath();
			for (i = 0; i < 6; i++)
				vg.Circle(sx[i], sy[i], 4.0f);
			vg.FillColor(new Color(0, 160, 192, 255));
			vg.Fill();
			vg.BeginPath();
			for (i = 0; i < 6; i++)
				vg.Circle(sx[i], sy[i], 2.0f);
			vg.FillColor(new Color(220, 220, 220, 255));
			vg.Fill();

			vg.StrokeWidth(1.0f);
		}

		public static void drawSpinner(NvgContext vg, float cx, float cy, float r, float t)
		{
			float a0 = 0.0f + t * 6;
			float a1 = (float)Math.PI + t * 6;
			float r0 = r;
			float r1 = r * 0.75f;
			float ax, ay, bx, by;
			Paint paint;

			vg.Save();

			vg.BeginPath();
			vg.Arc(cx, cy, r0, a0, a1, Winding.ClockWise);
			vg.Arc(cx, cy, r1, a1, a0, Winding.CounterClockWise);
			vg.ClosePath();
			ax = cx + (float)Math.Cos(a0) * (r0 + r1) * 0.5f;
			ay = cy + (float)Math.Sin(a0) * (r0 + r1) * 0.5f;
			bx = cx + (float)Math.Cos(a1) * (r0 + r1) * 0.5f;
			by = cy + (float)Math.Sin(a1) * (r0 + r1) * 0.5f;
			paint = vg.LinearGradient(ax, ay, bx, by, new Color(0, 0, 0, 0), new Color(0, 0, 0, 128));
			vg.FillPaint(paint);
			vg.Fill();

			vg.Restore();
		}

		public static void drawThumbnails(NvgContext vg, float x, float y, float w, float h, int[] images, float t)
		{
			float cornerRadius = 3.0f;
			Paint shadowPaint, imgPaint, fadePaint;
			float ix, iy, iw, ih;
			float thumb = 60.0f;
			float arry = 30.5f;
			int imgw, imgh;
			float stackh = (images.Length / 2) * (thumb + 10) + 10;
			int i;
			float u = (1 + (float)Math.Cos(t * 0.5f)) * 0.5f;
			float u2 = (1 - (float)Math.Cos(t * 0.2f)) * 0.5f;
			float scrollh, dv;

			vg.Save();
			//	ClearState(vg);

			// Drop shadow
			shadowPaint = vg.BoxGradient(x, y + 4, w, h, cornerRadius * 2, 20, new Color(0, 0, 0, 128), new Color(0, 0, 0, 0));
			vg.BeginPath();
			vg.Rect(x - 10, y - 10, w + 20, h + 30);
			vg.RoundedRect(x, y, w, h, cornerRadius);
			vg.PathWinding(Solidity.Hole);
			vg.FillPaint(shadowPaint);
			vg.Fill();

			// Window
			vg.BeginPath();
			vg.RoundedRect(x, y, w, h, cornerRadius);
			vg.MoveTo(x - 10, y + arry);
			vg.LineTo(x + 1, y + arry - 11);
			vg.LineTo(x + 1, y + arry + 11);
			vg.FillColor(new Color(200, 200, 200, 255));
			vg.Fill();

			vg.Save();
			vg.Scissor(x, y, w, h);
			vg.Translate(0, -(stackh - h) * u);

			dv = 1.0f / (float)(images.Length - 1);

			for (i = 0; i < images.Length; i++)
			{
				float tx, ty, v, a;
				tx = x + 10;
				ty = y + 10;
				tx += (i % 2) * (thumb + 10);
				ty += (i / 2) * (thumb + 10);
				vg.ImageSize(images[i], out imgw, out imgh);
				if (imgw < imgh)
				{
					iw = thumb;
					ih = iw * (float)imgh / (float)imgw;
					ix = 0;
					iy = -(ih - thumb) * 0.5f;
				}
				else
				{
					ih = thumb;
					iw = ih * (float)imgw / (float)imgh;
					ix = -(iw - thumb) * 0.5f;
					iy = 0;
				}

				v = i * dv;
				a = clampf((u2 - v) / dv, 0, 1);

				if (a < 1.0f)
					drawSpinner(vg, tx + thumb / 2, ty + thumb / 2, thumb * 0.25f, t);

				imgPaint = vg.ImagePattern(tx + ix, ty + iy, iw, ih, 0.0f / 180.0f * (float)Math.PI, images[i], a);
				vg.BeginPath();
				vg.RoundedRect(tx, ty, thumb, thumb, 5);
				vg.FillPaint(imgPaint);
				vg.Fill();

				shadowPaint = vg.BoxGradient(tx - 1, ty, thumb + 2, thumb + 2, 5, 3, new Color(0, 0, 0, 128), new Color(0, 0, 0, 0));
				vg.BeginPath();
				vg.Rect(tx - 5, ty - 5, thumb + 10, thumb + 10);
				vg.RoundedRect(tx, ty, thumb, thumb, 6);
				vg.PathWinding(Solidity.Hole);
				vg.FillPaint(shadowPaint);
				vg.Fill();

				vg.BeginPath();
				vg.RoundedRect(tx + 0.5f, ty + 0.5f, thumb - 1, thumb - 1, 4 - 0.5f);
				vg.StrokeWidth(1.0f);
				vg.StrokeColor(new Color(255, 255, 255, 192));
				vg.Stroke();
			}
			vg.Restore();

			// Hide fades
			fadePaint = vg.LinearGradient(x, y, x, y + 6, new Color(200, 200, 200, 255), new Color(200, 200, 200, 0));
			vg.BeginPath();
			vg.Rect(x + 4, y, w - 8, 6);
			vg.FillPaint(fadePaint);
			vg.Fill();

			fadePaint = vg.LinearGradient(x, y + h, x, y + h - 6, new Color(200, 200, 200, 255), new Color(200, 200, 200, 0));
			vg.BeginPath();
			vg.Rect(x + 4, y + h - 6, w - 8, 6);
			vg.FillPaint(fadePaint);
			vg.Fill();

			// Scroll bar
			shadowPaint = vg.BoxGradient(x + w - 12 + 1, y + 4 + 1, 8, h - 8, 3, 4, new Color(0, 0, 0, 32), new Color(0, 0, 0, 92));
			vg.BeginPath();
			vg.RoundedRect(x + w - 12, y + 4, 8, h - 8, 3);
			vg.FillPaint(shadowPaint);
			//	vg.FillColor(new Color(255,0,0,128));
			vg.Fill();

			scrollh = (h / stackh) * (h - 8);
			shadowPaint = vg.BoxGradient(x + w - 12 - 1, y + 4 + (h - 8 - scrollh) * u - 1, 8, scrollh, 3, 4, new Color(220, 220, 220, 255), new Color(128, 128, 128, 255));
			vg.BeginPath();
			vg.RoundedRect(x + w - 12 + 1, y + 4 + 1 + (h - 8 - scrollh) * u, 8 - 2, scrollh - 2, 2);
			vg.FillPaint(shadowPaint);
			//	vg.FillColor(new Color(0,0,0,128));
			vg.Fill();

			vg.Restore();
		}

		public static void drawColorwheel(NvgContext vg, float x, float y, float w, float h, float t)
		{
			int i;
			float r0, r1, ax, ay, bx, by, cx, cy, aeps, r;
			float hue = (float)Math.Sin(t * 0.12f);
			Paint paint;

			vg.Save();

			/*	vg.BeginPath();
				vg.Rect(x,y,w,h);
				vg.FillColor(new Color(255,0,0,128));
				vg.Fill();*/

			cx = x + w * 0.5f;
			cy = y + h * 0.5f;
			r1 = (w < h ? w : h) * 0.5f - 5.0f;
			r0 = r1 - 20.0f;
			aeps = 0.5f / r1;   // half a pixel arc length in radians (2pi cancels out).

			for (i = 0; i < 6; i++)
			{
				float a0 = (float)i / 6.0f * (float)Math.PI * 2.0f - aeps;
				float a1 = (float)(i + 1.0f) / 6.0f * (float)Math.PI * 2.0f + aeps;
				vg.BeginPath();
				vg.Arc(cx, cy, r0, a0, a1, Winding.ClockWise);
				vg.Arc(cx, cy, r1, a1, a0, Winding.CounterClockWise);
				vg.ClosePath();
				ax = cx + (float)Math.Cos(a0) * (r0 + r1) * 0.5f;
				ay = cy + (float)Math.Sin(a0) * (r0 + r1) * 0.5f;
				bx = cx + (float)Math.Cos(a1) * (r0 + r1) * 0.5f;
				by = cy + (float)Math.Sin(a1) * (r0 + r1) * 0.5f;
				paint = vg.LinearGradient(ax, ay, bx, by, NvgUtility.HSLA(a0 / ((float)Math.PI * 2), 1.0f, 0.55f, 255), NvgUtility.HSLA(a1 / ((float)Math.PI * 2), 1.0f, 0.55f, 255));
				vg.FillPaint(paint);
				vg.Fill();
			}

			vg.BeginPath();
			vg.Circle(cx, cy, r0 - 0.5f);
			vg.Circle(cx, cy, r1 + 0.5f);
			vg.StrokeColor(new Color(0, 0, 0, 64));
			vg.StrokeWidth(1.0f);
			vg.Stroke();

			// Selector
			vg.Save();
			vg.Translate(cx, cy);
			vg.Rotate(hue * (float)Math.PI * 2);

			// Marker on
			vg.StrokeWidth(2.0f);
			vg.BeginPath();
			vg.Rect(r0 - 1, -3, r1 - r0 + 2, 6);
			vg.StrokeColor(new Color(255, 255, 255, 192));
			vg.Stroke();

			paint = vg.BoxGradient(r0 - 3, -5, r1 - r0 + 6, 10, 2, 4, new Color(0, 0, 0, 128), new Color(0, 0, 0, 0));
			vg.BeginPath();
			vg.Rect(r0 - 2 - 10, -4 - 10, r1 - r0 + 4 + 20, 8 + 20);
			vg.Rect(r0 - 2, -4, r1 - r0 + 4, 8);
			vg.PathWinding(Solidity.Hole);
			vg.FillPaint(paint);
			vg.Fill();

			// Center triangle
			r = r0 - 6;
			ax = (float)Math.Cos(120.0f / 180.0f * (float)Math.PI) * r;
			ay = (float)Math.Sin(120.0f / 180.0f * (float)Math.PI) * r;
			bx = (float)Math.Cos(-120.0f / 180.0f * (float)Math.PI) * r;
			by = (float)Math.Sin(-120.0f / 180.0f * (float)Math.PI) * r;
			vg.BeginPath();
			vg.MoveTo(r, 0);
			vg.LineTo(ax, ay);
			vg.LineTo(bx, by);
			vg.ClosePath();
			paint = vg.LinearGradient(r, 0, ax, ay, NvgUtility.HSLA(hue, 1.0f, 0.5f, 255), new Color(255, 255, 255, 255));
			vg.FillPaint(paint);
			vg.Fill();
			paint = vg.LinearGradient((r + ax) * 0.5f, (0 + ay) * 0.5f, bx, by, new Color(0, 0, 0, 0), new Color(0, 0, 0, 255));
			vg.FillPaint(paint);
			vg.Fill();
			vg.StrokeColor(new Color(0, 0, 0, 64));
			vg.Stroke();

			// Select circle on triangle
			ax = (float)Math.Cos(120.0f / 180.0f * (float)Math.PI) * r * 0.3f;
			ay = (float)Math.Sin(120.0f / 180.0f * (float)Math.PI) * r * 0.4f;
			vg.StrokeWidth(2.0f);
			vg.BeginPath();
			vg.Circle(ax, ay, 5);
			vg.StrokeColor(new Color(255, 255, 255, 192));
			vg.Stroke();

			paint = vg.RadialGradient(ax, ay, 7, 9, new Color(0, 0, 0, 64), new Color(0, 0, 0, 0));
			vg.BeginPath();
			vg.Rect(ax - 20, ay - 20, 40, 40);
			vg.Circle(ax, ay, 7);
			vg.PathWinding(Solidity.Hole);
			vg.FillPaint(paint);
			vg.Fill();

			vg.Restore();

			vg.Restore();
		}

		public static void drawLines(NvgContext vg, float x, float y, float w, float h, float t)
		{
			int i, j;
			float pad = 5.0f, s = w / 9.0f - pad * 2;
			float[] pts = new float[4 * 2];
			float fx, fy;
			LineCap[] joins = new LineCap[] { LineCap.Miter, LineCap.Round, LineCap.Bevel };
			LineCap[] caps = new LineCap[] { LineCap.Butt, LineCap.Round, LineCap.Square };

			vg.Save();
			pts[0] = -s * 0.25f + (float)Math.Cos(t * 0.3f) * s * 0.5f;
			pts[1] = (float)Math.Sin(t * 0.3f) * s * 0.5f;
			pts[2] = -s * 0.25f;
			pts[3] = 0;
			pts[4] = s * 0.25f;
			pts[5] = 0;
			pts[6] = s * 0.25f + (float)Math.Cos(-t * 0.3f) * s * 0.5f;
			pts[7] = (float)Math.Sin(-t * 0.3f) * s * 0.5f;

			for (i = 0; i < 3; i++)
			{
				for (j = 0; j < 3; j++)
				{
					fx = x + s * 0.5f + (i * 3 + j) / 9.0f * w + pad;
					fy = y - s * 0.5f + pad;

					vg.LineCap(caps[i]);
					vg.LineJoin(joins[j]);

					vg.StrokeWidth(s * 0.3f);
					vg.StrokeColor(new Color(0, 0, 0, 160));
					vg.BeginPath();
					vg.MoveTo(fx + pts[0], fy + pts[1]);
					vg.LineTo(fx + pts[2], fy + pts[3]);
					vg.LineTo(fx + pts[4], fy + pts[5]);
					vg.LineTo(fx + pts[6], fy + pts[7]);
					vg.Stroke();

					vg.LineCap(LineCap.Butt);
					vg.LineJoin(LineCap.Bevel);

					vg.StrokeWidth(1.0f);
					vg.StrokeColor(new Color(0, 192, 255, 255));
					vg.BeginPath();
					vg.MoveTo(fx + pts[0], fy + pts[1]);
					vg.LineTo(fx + pts[2], fy + pts[3]);
					vg.LineTo(fx + pts[4], fy + pts[5]);
					vg.LineTo(fx + pts[6], fy + pts[7]);
					vg.Stroke();
				}
			}

			vg.Restore();
		}

		private static int LoadFont(NvgContext vg, string name, string path)
		{
			byte[] data;
			var ms = new MemoryStream();
			using (var stream = TitleContainer.OpenStream(path))
			{
				stream.CopyTo(ms);

				data = ms.ToArray();
			}

			return vg.CreateFontMem(name, data);
		}

		public int loadDemoData(GraphicsDevice device, NvgContext vg)
		{
			int i;

			if (vg == null)
				return -1;

			for (i = 0; i < 12; i++)
			{
				var path = "Assets/images/image" + (i + 1).ToString() + ".jpg";

				int width, height;
				byte[] data;
				using (var stream = File.OpenRead(path))
				{
					var texture = Texture2D.FromStream(device, stream);

					width = texture.Width;
					height = texture.Height;
					data = new byte[texture.Width * texture.Height * 4];
					texture.GetData(data);
				}

				images[i] = vg.CreateImageRGBA(width, height, 0, data);
			}

			fontIcons = LoadFont(vg, "icons", "Assets/entypo.ttf");
			fontNormal = LoadFont(vg, "sans", "Assets/Roboto-Regular.ttf");
			fontBold = LoadFont(vg, "sans-bold", "Assets/Roboto-Bold.ttf");

			return 0;
		}

		public static void drawParagraph(NvgContext vg, float x, float y, float width, float height, float mx, float my)
		{
			TextRow[] rows = new TextRow[3];
			GlyphPosition[] glyphs = new GlyphPosition[100];
			string text = "This is longer chunk of text.\n  \n  Would have used lorem ipsum but she    was busy jumping over the lazy dog with the fox and all the men who came to the aid of the party.";
			StringSegment start;
			int nrows, i, nglyphs, j, lnum = 0;
			float lineh;
			float caretx, px;
			Bounds bounds = new Bounds();
			float a;
			float gx = 0, gy = 0;
			int gutter = 0;

			for (i = 0; i < rows.Length; ++i)
			{
				rows[i] = new TextRow();
			}

			vg.Save();

			vg.FontSize(18.0f);
			vg.FontFace("sans");
			vg.TextAlign(Alignment.Left | Alignment.Top);

			float ascender, descender;
			vg.TextMetrics(out ascender, out descender, out lineh);

			// The text break API can be used to fill a large buffer of rows,
			// or to iterate over the text just few lines (or just one) at a time.
			// The "next" variable of the last returned item tells where to continue.
			start = text;
			while (true)
			{
				nrows = vg.TextBreakLines(start, width, rows, out start);

				if (nrows <= 0)
				{
					break;
				}
				for (i = 0; i < nrows; i++)
				{
					TextRow row = rows[i];
					var hit = mx > x && mx < (x + width) && my >= y && my < (y + lineh);

					vg.BeginPath();
					vg.FillColor(new Color(255, 255, 255, hit ? 64 : 16));
					vg.Rect(x, y, row.Width, lineh);
					vg.Fill();

					vg.FillColor(new Color(255, 255, 255, 255));
					vg.Text(x, y, row.Str);

					if (hit)
					{
						caretx = (mx < x + row.Width / 2) ? x : x + row.Width;
						px = x;
						nglyphs = vg.TextGlyphPositions(x, y, row.Str, glyphs);
						for (j = 0; j < nglyphs; j++)
						{
							float x0 = glyphs[j].X;
							float x1 = (j + 1 < nglyphs) ? glyphs[j + 1].X : x + row.Width;
							float gx2 = x0 * 0.3f + x1 * 0.7f;
							if (mx >= px && mx < gx2)
								caretx = glyphs[j].X;
							px = gx2;
						}
						vg.BeginPath();
						vg.FillColor(new Color(255, 192, 0, 255));
						vg.Rect(caretx, y, 1, lineh);
						vg.Fill();

						gutter = lnum + 1;
						gx = x - 10;
						gy = y + lineh / 2;
					}
					lnum++;
					y += lineh;
				}
			}

			if (gutter > 0)
			{
				string txt = gutter.ToString();
				vg.FontSize(13.0f);
				vg.TextAlign(Alignment.Right | Alignment.Middle);

				vg.TextBounds(gx, gy, txt, ref bounds);

				vg.BeginPath();
				vg.FillColor(new Color(255, 192, 0, 255));
				vg.RoundedRect((int)bounds.b1 - 4, (int)bounds.b2 - 2,
					(int)(bounds.b3 - bounds.b1) + 8,
					(int)(bounds.b4 - bounds.b2) + 4,
					((int)(bounds.b4 - bounds.b2) + 4) / 2 - 1);
				vg.Fill();

				vg.FillColor(new Color(32, 32, 32, 255));
				vg.Text(gx, gy, txt);
			}

			y += 20.0f;

			vg.FontSize(13.0f);
			vg.TextAlign(Alignment.Left | Alignment.Top);
			vg.TextLineHeight(1.2f);

			vg.TextBoxBounds(x, y, 150, "Hover your mouse over the text to see calculated caret position.", ref bounds);

			// Fade the tooltip out when close to it.
			gx = (float)Math.Abs((mx - (bounds.b1 + bounds.b3) * 0.5f) / (bounds.b1 - bounds.b3));
			gy = (float)Math.Abs((my - (bounds.b2 + bounds.b4) * 0.5f) / (bounds.b2 - bounds.b4));
			a = maxf(gx, gy) - 0.5f;
			a = clampf(a, 0, 1);
			vg.GlobalAlpha(a);

			vg.BeginPath();
			vg.FillColor(new Color(220, 220, 220, 255));
			vg.RoundedRect(bounds.b1 - 2, bounds.b2 - 2, (int)(bounds.b3 - bounds.b1) + 4, (int)(bounds.b4 - bounds.b2) + 4, 3);
			px = (int)((bounds.b3 + bounds.b1) / 2);
			vg.MoveTo(px, bounds.b2 - 10);
			vg.LineTo(px + 7, bounds.b2 + 1);
			vg.LineTo(px - 7, bounds.b2 + 1);
			vg.Fill();

			vg.FillColor(new Color(0, 0, 0, 220));
			vg.TextBox(x, y, 150, "Hover your mouse over the text to see calculated caret position.");

			vg.Restore();
		}

		public static void drawWidths(NvgContext vg, float x, float y, float width)
		{
			int i;

			vg.Save();

			vg.StrokeColor(new Color(0, 0, 0, 255));

			for (i = 0; i < 20; i++)
			{
				float w = (i + 0.5f) * 0.1f;
				vg.StrokeWidth(w);
				vg.BeginPath();
				vg.MoveTo(x, y);
				vg.LineTo(x + width, y + width * 0.3f);
				vg.Stroke();
				y += 10;
			}

			vg.Restore();
		}

		public static void drawCaps(NvgContext vg, float x, float y, float width)
		{
			int i;
			LineCap[] caps = new[] { LineCap.Butt, LineCap.Round, LineCap.Square };
			float lineWidth = 8.0f;

			vg.Save();

			vg.BeginPath();
			vg.Rect(x - lineWidth / 2, y, width + lineWidth, 40);
			vg.FillColor(new Color(255, 255, 255, 32));
			vg.Fill();

			vg.BeginPath();
			vg.Rect(x, y, width, 40);
			vg.FillColor(new Color(255, 255, 255, 32));
			vg.Fill();

			vg.StrokeWidth(lineWidth);
			for (i = 0; i < 3; i++)
			{
				vg.LineCap(caps[i]);
				vg.StrokeColor(new Color(0, 0, 0, 255));
				vg.BeginPath();
				vg.MoveTo(x, y + i * 10 + 5);
				vg.LineTo(x + width, y + i * 10 + 5);
				vg.Stroke();
			}

			vg.Restore();
		}

		public static void drawScissor(NvgContext vg, float x, float y, float t)
		{
			vg.Save();

			// Draw first rect and set scissor to it's area.
			vg.Translate(x, y);
			vg.Rotate(NvgUtility.DegToRad(5));
			vg.BeginPath();
			vg.Rect(-20, -20, 60, 40);
			vg.FillColor(new Color(255, 0, 0, 255));
			vg.Fill();
			vg.Scissor(-20, -20, 60, 40);

			// Draw second rectangle with offset and rotation.
			vg.Translate(40, 0);
			vg.Rotate(t);

			// Draw the intended second rectangle without any scissoring.
			vg.Save();
			vg.ResetScissor();
			vg.BeginPath();
			vg.Rect(-20, -10, 60, 30);
			vg.FillColor(new Color(255, 128, 0, 64));
			vg.Fill();
			vg.Restore();

			// Draw second rectangle with combined scissoring.
			vg.IntersectScissor(-20, -10, 60, 30);
			vg.BeginPath();
			vg.Rect(-20, -10, 60, 30);
			vg.FillColor(new Color(255, 128, 0, 255));
			vg.Fill();

			vg.Restore();
		}

		public void renderDemo(NvgContext vg, float mx, float my, float width, float height,
						float t, bool blowup)
		{
			float x, y, popy;

			drawEyes(vg, width - 250, 50, 150, 100, mx, my, t);
			drawParagraph(vg, width - 450, 50, 150, 100, mx, my);
			drawGraph(vg, 0, height / 2, width, height / 2, t);
			drawColorwheel(vg, width - 300, height - 300, 250.0f, 250.0f, t);

			// Line joints
			drawLines(vg, 120, height - 50, 600, 50, t);

			// Line caps
			drawWidths(vg, 10, 50, 30);

			// Line caps
			drawCaps(vg, 10, 300, 30);

			drawScissor(vg, 50, height - 80, t);

			vg.Save();
			if (blowup)
			{
				vg.Rotate((float)Math.Sin(t * 0.3f) * 5.0f / 180.0f * (float)Math.PI);
				vg.Scale(2.0f, 2.0f);
			}

			// Widgets
			drawWindow(vg, "Widgets `n Stuff", 50, 50, 300, 400);
			x = 60;
			y = 95;
			drawSearchBox(vg, "Search", x, y, 280, 25);
			y += 40;
			drawDropDown(vg, "Effects", x, y, 280, 28);
			popy = y + 14;
			y += 45;

			// Form
			drawLabel(vg, "Login", x, y, 280, 20);
			y += 25;
			drawEditBox(vg, "Email", x, y, 280, 28);
			y += 35;
			drawEditBox(vg, "Password", x, y, 280, 28);
			y += 38;
			drawCheckBox(vg, "Remember me", x, y, 140, 28);
			drawButton(vg, ICON_LOGIN, "Sign in", x + 138, y, 140, 28, new Color(0, 96, 128, 255));
			y += 45;

			// Slider
			drawLabel(vg, "Diameter", x, y, 280, 20);
			y += 25;
			drawEditBoxNum(vg, "123.00", "px", x + 180, y, 100, 28);
			drawSlider(vg, 0.4f, x, y, 170, 28);
			y += 55;

			drawButton(vg, ICON_TRASH, "Delete", x, y, 160, 28, new Color(128, 16, 8, 255));
			drawButton(vg, null, "Cancel", x + 170, y, 110, 28, new Color(0, 0, 0, 0));

			// Thumbnails box
			drawThumbnails(vg, 365, popy - 30, 160, 300, images, t);

			vg.Restore();
		}
	}
}