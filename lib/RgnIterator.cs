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
    public delegate void IterFuncSrc(Pixel src);
    public delegate Pixel IterFuncDest();
    public delegate Pixel IterFuncDestFull(int x, int y);
    public delegate Pixel IterFuncSrcDest(Pixel src);

    readonly Drawable _drawable;
    readonly RunMode _runmode;
    readonly Rectangle _rectangle;

    Progress _progress;

    public RgnIterator(Drawable drawable, RunMode runmode)
    {
      _drawable = drawable;
      _runmode = runmode;
      _rectangle = drawable.MaskBounds;
    }

    public Progress Progress
    {
      set {_progress = value;}
    }

    public int Count
    {
      get {return _rectangle.Area;}
    }

    public void IterateSrc(IterFuncSrc func)
    {
      foreach (Pixel pixel in new ReadPixelIterator(_drawable, _runmode))
	{
	  func(pixel);
	}
    }

    public void IterateDest(IterFuncDest func)
    {
      int total_area = _rectangle.Area;
      int area_so_far = 0;

      PixelRgn destPR = new PixelRgn(_drawable, _rectangle, true, true);
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
      _drawable.Update(_rectangle);
    }

    public void IterateDest(IterFuncDestFull func)
    {
      int total_area = _rectangle.Area;
      int area_so_far = 0;

      PixelRgn destPR = new PixelRgn(_drawable, _rectangle, true, true);
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
      _drawable.Update(_rectangle);
    }

    public void IterateDestFull(IterFuncDestFull func)
    {
      IterateDest(func);
    }

    public void IterateDestSimple(IterFuncDest func)
    {
      IterateDest(func);
    }

    public void IterateSrcDest(IterFuncSrcDest func)
    {
      PixelRgn srcPR = new PixelRgn(_drawable, _rectangle, false, false);
      PixelRgn destPR = new PixelRgn(_drawable, _rectangle, true, true);

      for (IntPtr pr = PixelRgn.Register(srcPR, destPR); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
	    {
	      for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
		{
		  Pixel pixel = srcPR[y, x];
		  pixel.X = x;
		  pixel.Y = y;
		  destPR[y, x] = func(pixel);
		}
	    }				
	}
      _drawable.Flush();
      _drawable.MergeShadow(true);
      _drawable.Update(_rectangle);
    }
  }
}
