using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class RgnIterator
    {
      public delegate void IterFuncSrc(byte[] src);
      public delegate byte[] IterFuncDest(int x, int y);
      public delegate byte[] IterFuncSrcDest(int x, int y, byte[] src);

      Drawable _drawable;
      int x1, y1, x2, y2;
      RunMode _runmode;
      Progress _progress;
      int _bpp;

      public RgnIterator(Drawable drawable, RunMode runmode)
      {
	_drawable = drawable;
	_runmode = runmode;
	_bpp = drawable.Bpp;
	drawable.MaskBounds(out x1, out y1, out x2, out y2);
      }

      public Progress Progress
      {
	set {_progress = value;}
      }

      public void Iterate(IterFuncSrc func)
      {
	PixelRgn srcPR = new PixelRgn(_drawable, x1, y1, x2 - x1, y2 - y1, 
				      false, false);
	for (IntPtr pr = PixelRgn.Register(srcPR); pr != IntPtr.Zero; 
	     pr = PixelRgn.Process(pr))
	  {
	  for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
	    {
	    for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
	      {
	      func(srcPR[y, x]);
	      }
	    }
	  }
      }

      public void Iterate(IterFuncDest func)
      {
	int total_area = (x2 - x1) * (y2 - y1);
	int area_so_far = 0;

	PixelRgn destPR = new PixelRgn(_drawable, x1, y1, x2 - x1, y2 - y1, 
				       true, true);
	for (IntPtr pr = PixelRgn.Register(destPR); pr != IntPtr.Zero; 
	     pr = PixelRgn.Process(pr))
	  {
	  for (int y = destPR.Y; y < destPR.Y + destPR.H; y++)
	    {
	    for (int x = destPR.X; x < destPR.X + destPR.W; x++)
	      {
	      destPR[y, x] = func(x, y);
	      }
	    }
	  if (_runmode != RunMode.NONINTERACTIVE)
	    {
	    area_so_far += destPR.W * destPR.H;
	    _progress.Update ((double) area_so_far / (double) total_area);
	    }
	  }
	_drawable.Flush();
	_drawable.MergeShadow(true);
	_drawable.Update(x1, y1, x2 - x1, y2 - y1);
      }

      public void Iterate(IterFuncSrcDest func)
      {
	PixelRgn srcPR = new PixelRgn(_drawable, x1, y1, x2 - x1, y2 - y1, 
				      false, false);
	PixelRgn destPR = new PixelRgn(_drawable, x1, y1, x2 - x1, y2 - y1, 
				       true, true);

	for (IntPtr pr = PixelRgn.Register(srcPR, destPR); pr != IntPtr.Zero; 
	     pr = PixelRgn.Process(pr))
	  {
	  for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
	    {
	    for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
	      {
	      destPR[y, x] = func(x, y, srcPR[y, x]);
	      }
	    }				
	  }
	_drawable.Flush();
	_drawable.MergeShadow(true);
	_drawable.Update(x1, y1, x2 - x1, y2 - y1);
      }

      [DllImport("libgimp-2.0.so")]
      public static extern bool gimp_progress_update(double percentage);
    }
  }
