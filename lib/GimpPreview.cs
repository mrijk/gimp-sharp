using System;
using System.Runtime.InteropServices;

using Gdk;
using Gtk;

namespace Gimp
  {
    public class GimpPreview : VBox
    {
      public GimpPreview(IntPtr ptr) : base(ptr)
      {
      }

      public bool Update
      {
	get {return gimp_preview_get_update(Handle);}
	set {gimp_preview_set_update(Handle, value);}
      }

      public void SetBounds(int xmin, int ymin, int xmax, int ymax)
      {
	gimp_preview_set_bounds(Handle, xmin, ymin, xmax, ymax);
      }

      public void GetPosition(out int x, out int y)
      {
	gimp_preview_get_position(Handle, out x, out y);
      }

      public void GetSize(out int width, out int height)
      {
	gimp_preview_get_size(Handle, out width, out height);
      }

      public void Draw()
      {
	gimp_preview_draw(Handle);
      }

      public void DrawBuffer(byte[] buffer, int rowstride)
      {
	gimp_preview_draw_buffer(Handle, buffer, rowstride);
      }

      public void Invalidate()
      {
	gimp_preview_invalidate(Handle);
      }

      public Cursor DefaultCursor
      {
	set {gimp_preview_set_default_cursor(Handle, value);}
      }

      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_preview_set_update (IntPtr preview,
						  bool update);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static bool gimp_preview_get_update (IntPtr preview);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_preview_set_bounds (IntPtr preview,
						  int xmin, int ymin, 
						  int xmax, int ymax);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_preview_get_position(IntPtr preview,
						   out int x, out int y);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_preview_get_size(IntPtr preview,
					       out int width, out int height);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_preview_draw(IntPtr preview);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_preview_draw_buffer(IntPtr preview,
						  byte[] buffer, 
						  int rowstride);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_preview_invalidate(IntPtr preview);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_preview_set_default_cursor(IntPtr preview,
							 Cursor cursor);
    }
  }
