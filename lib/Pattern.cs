// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// Pattern.cs
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
  public sealed class Pattern : DataObject
  {
    public Pattern(string name) : base(name)
    {
    }

    internal Pattern(string name, bool unused) : base(name)
    {
    }

    protected override string TryRename(string newName)
    {
      throw new GimpSharpException("Rename for patterns not supported by GIMP");
    }

    public PatternInfo Info
    {
      get
	{
	  int width, height, bpp;
	  if (!gimp_pattern_get_info(Name, out width, out height, out bpp))
	    {
	      throw new GimpSharpException();
	    }
	  return new PatternInfo(width, height, bpp);
	}
    }

    public Pixel[] GetPixels(out int width, out int height, out int bpp,
			     out int numColorBytes)
    {
      IntPtr colorBytes;
      if (!gimp_pattern_get_pixels(Name, out width, out height,
				   out bpp, out numColorBytes,
				   out colorBytes))
        {
	  throw new GimpSharpException();
        }
      return null; // Fix me: fill array with colors
    }

    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_pattern_get_info(string name,
                                             out int width, out int height, 
					     out int bpp);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_pattern_get_pixels(string name,
					       out int width, out int height, 
					       out int bpp, 
					       out int num_color_bytes,
					       out IntPtr color_bytes);
  }
}
