using System;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
	public class Preview : PreviewArea
	{
		Gdk.GC _gc;
		Drawable _drawable;
		SliceTool _parent;
		PreviewRenderer _renderer;

		public Preview(Drawable drawable, SliceTool parent)
		{
			_drawable = drawable;
			_parent = parent;

			ExposeEvent += new ExposeEventHandler(OnExposed);
			Realized += new EventHandler(OnRealized);

			Events = EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | 
				EventMask.PointerMotionHintMask | EventMask.PointerMotionMask;
		}

		void OnExposed (object o, ExposeEventArgs args)
		{	
			_parent.Redraw(_renderer);
		}

		void OnRealized (object o, EventArgs args)
		{
			_gc = new Gdk.GC(GdkWindow);

			int width = _drawable.Width;
			int height = _drawable.Height;

			_renderer = new PreviewRenderer(this, _gc, width, height);

			PixelRgn rgn = new PixelRgn(_drawable, 0, 0, width, height, 
				false, false);
      
			byte[] buf = rgn.GetRect(0, 0, width, height);
			Draw(0, 0, width, height, ImageType.RGB, buf, width * _drawable.Bpp);
		}

		public PreviewRenderer Renderer
		{
			get {return _renderer;}
		}
	}
}