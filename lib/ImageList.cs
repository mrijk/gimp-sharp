// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// ImageList.cs
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
using System.Runtime.InteropServices;

namespace Gimp
{
  public class ImageList : IEnumerable<Image>
  {
    List<Image> _list = new List<Image>();

    public ImageList()
    {
      int num_images;
      IntPtr list = gimp_image_list(out num_images);

      int[] dest = new int[num_images];
      Marshal.Copy(list, dest, 0, num_images);

      foreach (int imageID in dest)
	{
	  _list.Add(new Image(imageID));
	}
    }

    IEnumerator<Image> IEnumerable<Image>.GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_image_list(out int num_images);
  }
}
