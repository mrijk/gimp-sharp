using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public enum FillType
      {
	FOREGROUND_FILL,
	BACKGROUND_FILL,
	WHITE_FILL,
	TRANSPARENT_FILL,
	PATTERN_FILL
      };

    public class Drawable
    {
      [DllImport("libgimp-2.0.so")]
      static extern IntPtr gimp_drawable_get(Int32 drawable_ID);

      [DllImport("libgimp-2.0.so")]
      static extern string gimp_drawable_get_name(Int32 drawable_ID);

      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_drawable_set_name(Int32 drawable_ID, 
						string name);

      [DllImport("libgimp-2.0.so")]
      static extern void gimp_drawable_flush(IntPtr drawable);

      [DllImport("libgimp-2.0.so")]
      static extern void gimp_drawable_detach(IntPtr drawable);

      [DllImport("libgimp-2.0.so")]
      static extern int gimp_drawable_bpp (Int32 drawable_ID);

      [DllImport("libgimp-2.0.so")]
      static extern int gimp_drawable_width(Int32 drawable_ID);

      [DllImport("libgimp-2.0.so")]
      static extern int gimp_drawable_height(Int32 drawable_ID);

      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_drawable_get_image(Int32 drawable_ID);

      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_drawable_fill(Int32 drawable_ID,
					    FillType fill_type);

      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_drawable_mask_bounds(Int32 drawable_ID,
						   out int x1,
						   out int y1,
						   out int x2,
						   out int y2);

      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_drawable_merge_shadow(Int32 drawable_ID,
						    bool undo);

      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_drawable_update(Int32 drawable_ID,
					      int x,
					      int y,
					      int width,
					      int height);

      IntPtr _drawable;
      Int32 _drawableID;

      public Drawable(Int32 drawableID)
      {
	_drawableID = drawableID;
	_drawable = gimp_drawable_get(_drawableID);
      }

      public bool Fill(FillType fill_type)
      {
	return gimp_drawable_fill(_drawableID, fill_type);
      }

      public bool MaskBounds(out int x1, out int y1, out int x2, out int y2)
      {
	return gimp_drawable_mask_bounds(_drawableID, out x1, out y1, 
					 out x2, out y2);
      }

      public bool MergeShadow(bool undo)
      {
	return gimp_drawable_merge_shadow(_drawableID, undo);
      }

      public bool Update(int x, int y, int width, int height)
      {
	return gimp_drawable_update(_drawableID, x, y, width, height);
      }

      public void Flush()
      {
	gimp_drawable_flush(_drawable);
      }

      public void Detach()
      {
	gimp_drawable_detach(_drawable);
      }

      public string Name
      {
	get {return gimp_drawable_get_name(_drawableID);}
	set {gimp_drawable_set_name(_drawableID, "foo");}
      }

      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_drawable_has_alpha (Int32 drawable_ID);
      
      public bool HasAlpha()
      {
	return gimp_drawable_has_alpha(_drawableID);
      }

      public int Bpp
      {
	get {return gimp_drawable_bpp(_drawableID);}
      }

      public int Height
      {
	get {return gimp_drawable_height(_drawableID);}
      }

      public int Width
      {
	get {return gimp_drawable_width(_drawableID);}
      }

      public Image Image
      {
	get {return new Image(gimp_drawable_get_image(_drawableID));}
      }

      public Int32 ID
      {
	get {return _drawableID;}
      }

      public IntPtr Ptr
      {
	get {return _drawable;}
      }
    }
  }
