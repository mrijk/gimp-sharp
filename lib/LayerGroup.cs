// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
//
// LayerGroup.cs
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
  public class LayerGroup : Layer
  {
    public LayerGroup(Image image) : base(gimp_layer_group_new(image.ID))
    {
    }

    internal LayerGroup(Int32 layerID) : base(layerID)
    {
    }

    public void Insert(Layer layer, int position)
    {
      Image.InsertLayer(layer, this, position);
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_layer_group_new(Int32 image_ID);
  }
}
