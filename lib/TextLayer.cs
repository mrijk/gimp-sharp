// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// TextLayer.cs
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
  public class TextLayer : Layer
  {    
    public TextLayer(Image image, double x, double y, string text, int border,
		     bool antialias, double size, SizeType sizeType,
		     string fontname) :
      base(gimp_text_fontname(image.ID, -1, x, y, text, border, antialias,
			      size, sizeType, fontname))
    {    
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_text_fontname(Int32 image_ID,
					   Int32 drawable_ID,
					   double x,
					   double y,
					   string text,
					   int border,
					   bool antialias,
					   double size,
					   SizeType size_type,
					   string fontname);
  }
}
