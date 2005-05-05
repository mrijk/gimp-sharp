// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2005 Maurits Rijk
//
// RGB.cs
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
using System.Runtime.Serialization;

namespace Gimp
  {
    [Serializable]
    public class RGB
    {
      GimpRGB _rgb = new GimpRGB();

      public RGB(double red, double green, double blue)
      {
	gimp_rgb_set(ref _rgb, red, green, blue);
      }

      public RGB(byte red, byte green, byte blue)
      {
	gimp_rgb_set_uchar(ref _rgb, red, green, blue);
      }

      public RGB(GimpRGB rgb)
      {
	_rgb = rgb;
      }

      public GimpRGB GimpRGB
      {
	get {return _rgb;}
      }

      public void GetUChar(out byte red, out byte green, out byte blue)
      {
	gimp_rgb_get_uchar(ref _rgb, out red, out green, out blue);
      }

      [DllImport("libgimpcolor-2.0.so")]
      static extern void gimp_rgb_set(ref GimpRGB rgb,
				      double red,
				      double green,
				      double blue);
      [DllImport("libgimp-2.0.so")]
      static extern void gimp_rgb_set_uchar (ref GimpRGB rgb,
					     byte red,
					     byte green,
					     byte blue);
      [DllImport("libgimp-2.0.so")]
      static extern void gimp_rgb_get_uchar (ref GimpRGB rgb,
					     out byte red,
					     out byte green,
					     out byte blue);
    }
  }
