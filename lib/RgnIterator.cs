using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class RgnIterator
    {
      public delegate void IterFuncSrc(byte[] src);
      public delegate void IterFuncDest(int x, int y, ref byte[] dest);
      public delegate void IterFuncSrcDest(int x, int y, byte[] src, 
					   ref byte[] dest);

      Drawable _drawable;
      int x1, y1, x2, y2;
      RunMode _runmode;
      int _bpp;

      [DllImport("libgimp-2.0.so")]
      public static extern bool gimp_progress_update(double percentage);

      public RgnIterator(Drawable drawable, RunMode runmode)
      {
	_drawable = drawable;
	_runmode = runmode;
	_bpp = drawable.Bpp;
	drawable.MaskBounds(out x1, out y1, out x2, out y2);
      }

      public void Iterate(IterFuncSrc func)
      {
	byte[] from = new Byte[_bpp];

	PixelRgn srcPR = new PixelRgn(_drawable, x1, y1, x2 - x1, y2 - y1, 
				      false, false);
	for (IntPtr pr = PixelRgn.Register(srcPR); pr != IntPtr.Zero; 
	     pr = PixelRgn.Process(pr))
	  {
	  IntPtr src = srcPR.Data;
	  for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
	    {
	    IntPtr s = src;
	    for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
	      {
	      Marshal.Copy(s, from, 0, _bpp);
	      func(from);
	      s = (IntPtr) ((int) s + _bpp);
	      }
	    src = (IntPtr) ((int) src + srcPR.Rowstride);
	    }
	  }
      }

      public void Iterate(IterFuncDest func)
      {
	int total_area;
	int area_so_far;

	total_area = (x2 - x1) * (y2 - y1);
	area_so_far = 0;

	byte[] to = new Byte[_bpp];

	PixelRgn destPR = new PixelRgn(_drawable, x1, y1, x2 - x1, y2 - y1, 
				       true, true);
	for (IntPtr pr = PixelRgn.Register(destPR); pr != IntPtr.Zero; 
	     pr = PixelRgn.Process(pr))
	  {
	  IntPtr dest = destPR.Data;
	  for (int y = destPR.Y; y < destPR.Y + destPR.H; y++)
	    {
	    IntPtr d = dest;
	    for (int x = destPR.X; x < destPR.X + destPR.W; x++)
	      {
	      func(x, y, ref to);
	      Marshal.Copy(to, 0, d, _bpp);
	      d = (IntPtr) ((int) d + _bpp);
	      }
	    dest = (IntPtr) ((int) dest + destPR.Rowstride);
	    }
	  if (_runmode != RunMode.NONINTERACTIVE)
	    {
	    area_so_far += destPR.W * destPR.H;
	    gimp_progress_update ((double) area_so_far / (double) total_area);
	    }
	  }
	_drawable.Flush();
	_drawable.MergeShadow(true);
	_drawable.Update(x1, y1, x2 - x1, y2 - y1);
      }

      public void Iterate(IterFuncSrcDest func)
      {
	byte[] from = new Byte[_bpp];
	byte[] to = new Byte[_bpp];

	PixelRgn srcPR = new PixelRgn(_drawable, x1, y1, x2 - x1, y2 - y1, 
				      false, false);
	PixelRgn destPR = new PixelRgn(_drawable, x1, y1, x2 - x1, y2 - y1, 
				       true, true);

	for (IntPtr pr = PixelRgn.Register(srcPR, destPR); pr != IntPtr.Zero; 
	     pr = PixelRgn.Process(pr))
	  {
	  IntPtr src = srcPR.Data;
	  IntPtr dest = destPR.Data;

	  for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
	    {
	    IntPtr s = src;
	    IntPtr d = dest;

	    for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
	      {
	      Marshal.Copy(s, from, 0, _bpp);
	      func(x, y, from, ref to);
	      Marshal.Copy(to, 0, d, _bpp);
	      s = (IntPtr) ((int) s + _bpp);
	      d = (IntPtr) ((int) d + _bpp);
	      }
	    src = (IntPtr) ((int) src + srcPR.Rowstride);
	    dest = (IntPtr) ((int) dest + srcPR.Rowstride);
	    }				
	  }
	_drawable.Flush();
	_drawable.MergeShadow(true);
	_drawable.Update(x1, y1, x2 - x1, y2 - y1);
      }
    }
  }
