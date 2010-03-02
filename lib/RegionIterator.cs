// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// RegionIterator.cs
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
using System.Runtime.InteropServices;

namespace Gimp
{
  public sealed class RegionIterator
  {
    readonly PixelRgn[] _regions;

    public RegionIterator(params PixelRgn[] regions)
    {
      _regions = regions;
    }

    // Fix me: there could be some overhead in reading from PixelRgn's that
    // are write-only! This could be solved by introducing Read/Write/ReadWrite
    // regions

    public void ForEach(Action<Pixel> func)
    {
      var rgn = _regions[0];
      for (IntPtr pr = PixelRgn.Register(rgn); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = rgn.Y; y < rgn.Y + rgn.H; y++)
	    {
	      for (int x = rgn.X; x < rgn.X + rgn.W; x++)
		{
		  func(rgn[y, x]);
		}
	    }
	}
    }

    public void ForEach(Action<Pixel, Pixel> func)
    {
      var rgn1 = _regions[0];
      var rgn2 = _regions[1];
      for (IntPtr pr = PixelRgn.Register(rgn1, rgn2); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y1 = rgn1.Y, y2 = rgn2.Y; y1 < rgn1.Y + rgn1.H; y1++, y2++)
	    {
	      for (int x1 = rgn1.X, x2 = rgn2.X; x1 < rgn1.X + rgn1.W; 
		   x1++, x2++)
		{
		  func(rgn1[y1, x1], rgn2[y2, x2]);
		}
	    }
	}
    }

    public void ForEach(Action<Pixel, Pixel, Pixel> func)
    {
      var rgn1 = _regions[0];
      var rgn2 = _regions[1];
      var rgn3 = _regions[2];

      for (IntPtr pr = PixelRgn.Register(rgn1, rgn2, rgn3); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y1 = rgn1.Y, y2 = rgn2.Y, y3 = rgn3.Y;
	       y1 < rgn1.Y + rgn1.H; y1++, y2++, y3++)
	    {
	      for (int x1 = rgn1.X, x2 = rgn2.X, x3 = rgn3.X; 
		   x1 < rgn1.X + rgn1.W; x1++, x2++, x3++)
		{
		  func(rgn1[y1, x1], rgn2[y2, x2], rgn3[y3, x3]);
		}
	    }
	}
    }
  }
}
