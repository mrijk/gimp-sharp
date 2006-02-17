// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// LayerList.cs
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
  public sealed class LayerList : IEnumerable<Layer>
  {
    List<Layer> _list = new List<Layer>();

    public LayerList(Image image)
    {
      int num_layers;
      IntPtr list = gimp_image_get_layers(image.ID, out num_layers);

      int[] dest = new int[num_layers];
      Marshal.Copy(list, dest, 0, num_layers);

      foreach (int layerID in dest)
        {
	  _list.Add(new Layer(layerID));
        }
    }

    IEnumerator<Layer> IEnumerable<Layer>.GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    public int Count
    {
      get {return _list.Count;}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_image_get_layers(Int32 image_ID, 
                                               out int num_layers);
  }
}
