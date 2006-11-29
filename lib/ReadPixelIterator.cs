// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// ReadPixelIterator.cs
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
using System.Collections;
using System.Collections.Generic;

namespace Gimp
{
  public class ReadPixelIterator
  {
    readonly int _x1, _y1, _x2, _y2;

    readonly Drawable _drawable;
    readonly RunMode _runmode;


    public ReadPixelIterator(Drawable drawable, RunMode runmode)
    {
      _drawable = drawable;
      _runmode = runmode;
      drawable.MaskBounds(out _x1, out _y1, out _x2, out _y2);
    }
	
    public IEnumerator<Pixel> GetEnumerator()
    {
      PixelRgn srcPR = new PixelRgn(_drawable, _x1, _y1, _x2 - _x1, _y2 - _y1, 
				    false, false);
      for (IntPtr pr = PixelRgn.Register(srcPR); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
	    {
	      for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
		{
		  Pixel pixel = srcPR[y, x];
		  pixel.X = x;
		  pixel.Y = y;
		  yield return pixel;
		}
	    }
	}
    }
  }
}
