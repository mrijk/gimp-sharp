using System;

using Gtk;

namespace Gimp.SliceTool
{
	public class SliceTool : Plugin
	{
		Gdk.GC _gc;
		// AspectPreview _preview;
		PreviewArea _preview;
		bool _toggle = true;

		[STAThread]
		static void Main(string[] args)
		{
			new SliceTool(args);
		}

		
		public SliceTool(string[] args) : base(args)
		{
		}

		override protected void Query()
		{
			InstallProcedure("plug_in_slice_tool",
				"Slice Tool",
				"Slice Tool",
				"Maurits Rijk",
				"(C) Maurits Rijk",
				"2005",
				"Slice Tool...",
				"RGB*, GRAY*",
				null);

			MenuRegister("<Image>/Filters/Web");
		}

		override protected bool CreateDialog()
		{
			gimp_ui_init("SliceTool", true);

			Dialog dialog = DialogNew("Slice Tool", "SliceTool",
				IntPtr.Zero, 0, null, "SliceTool");

			VBox vbox = new VBox(false, 12);
			vbox.BorderWidth = 12;
			dialog.VBox.PackStart(vbox, true, true, 0);

			_preview = new PreviewArea();
			_preview.WidthRequest = _drawable.Width;
			_preview.HeightRequest = _drawable.Height;
			_preview.ExposeEvent += new ExposeEventHandler(OnExposed);
			_preview.Realized += new EventHandler(OnRealized);
			vbox.PackStart(_preview, true, true, 0);

			dialog.ShowAll();
			return DialogRun();
		}

		void OnExposed (object o, ExposeEventArgs args)
		{
			int width = _drawable.Width;
			int height = _drawable.Height;

			PixelRgn rgn = new PixelRgn(_drawable, 0, 0, width, height, false, false);
			
			byte[] buf = rgn.GetRect(0, 0, width, height);
			_preview.Draw(0, 0, width, height, ImageType.RGB, buf, width * _drawable.Bpp);
			_preview.GdkWindow.DrawRectangle(_gc, true, new Gdk.Rectangle(0, 0, 100, 100));
		}

		void OnRealized (object o, EventArgs args)
		{
			_gc = new Gdk.GC(_preview.GdkWindow);
		}

		override protected void DoSomething(Drawable drawable)
		{
		}
	}
}
