// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// RgnIterator.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;

namespace Gimp
{
  public sealed class RgnIterator
  {
    public delegate void IterFuncSrc(byte[] src);
    public delegate void IterFuncSrcFull(int x, int y, byte[] src);
    public delegate byte[] IterFuncDest();
    public delegate byte[] IterFuncDestFull(int x, int y);
    public delegate byte[] IterFuncSrcDest(byte[] src);
    public delegate byte[] IterFuncSrcDestFull(int x, int y, byte[] src);

    readonly int x1, y1, x2, y2;

    readonly Drawable _drawable;
    readonly RunMode _runmode;
    Progress _progress;

    public RgnIterator(Drawable drawable, RunMode runmode)
    {
      _drawable = drawable;
      _runmode = runmode;
      drawable.MaskBounds(out x1, out y1, out x2, out y2);
    }

    public Progress Progress
    {
      set {_progress = value;}
    }

    public void IterateSrc(IterFuncSrc func)
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

    public void IterateSrc(IterFuncSrcFull func)
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
		  func(x, y, srcPR[y, x]);
		}
	    }
	}
    }

    public void IterateDest(IterFuncDest func)
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
		  destPR[y, x] = func();
		}
	    }
	  if (_runmode != RunMode.Noninteractive)
	    {
	      area_so_far += destPR.W * destPR.H;
	      _progress.Update ((double) area_so_far / (double) total_area);
	    }
	}
      _drawable.Flush();
      _drawable.MergeShadow(true);
      _drawable.Update(x1, y1, x2 - x1, y2 - y1);
    }

    public void IterateDest(IterFuncDestFull func)
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
	  if (_runmode != RunMode.Noninteractive)
	    {
	      area_so_far += destPR.W * destPR.H;
	      _progress.Update ((double) area_so_far / (double) total_area);
	    }
	}
      _drawable.Flush();
      _drawable.MergeShadow(true);
      _drawable.Update(x1, y1, x2 - x1, y2 - y1);
    }

    public void IterateSrcDest(IterFuncSrcDest func)
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
		  destPR[y, x] = func(srcPR[y, x]);
		}
	    }				
	}
      _drawable.Flush();
      _drawable.MergeShadow(true);
      _drawable.Update(x1, y1, x2 - x1, y2 - y1);
    }

    public void IterateSrcDest(IterFuncSrcDestFull func)
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
  }
}
