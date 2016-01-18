// The PicturePackage plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// PageSize.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;

namespace Gimp.PicturePackage
{
  public class PageSize : IComparable
  {
    public double Width {get;}
    public double Height {get;}

    public PageSize(double width, double height)
    {
      Width = width;
      Height = height;
    }

    public Dimensions ToDimensions() => new Dimensions((int) Width, (int) Height);

    public int CompareTo(object obj)
    {
      var size = obj as PageSize;
      int result = Width.CompareTo(size.Width);
      return (result == 0) ? Height.CompareTo(size.Height) : result;
    }
  }
}
