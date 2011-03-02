// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// Buffer.cs
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
  public class Buffer
  {
    string _name;

    public Buffer(string name)
    {
      _name = name;
    }

    public void Delete()
    {
      if (!gimp_buffer_delete(_name))
        {
	  throw new GimpSharpException();
        }
    }

    static public List<Buffer> GetBuffers(string filter)
    {
      int numBuffers;
      IntPtr ptr = gimp_buffers_get_list(filter, out numBuffers);
      return Util.ToList<Buffer>(ptr, numBuffers, (s) => new Buffer(s));
    }

    public string Name
    {
      get {return _name;}
      set {_name = gimp_buffer_rename(_name, value);}
    }

    public int Width
    {
      get {return gimp_buffer_get_width(Name);}
    }

    public int Height
    {
      get {return gimp_buffer_get_height(Name);}
    }

    public int Bytes
    {
      get {return gimp_buffer_get_bytes(Name);}
    }

    public ImageBaseType ImageType
    {
      get {return gimp_buffer_get_image_type(Name);}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_buffers_get_list(string filter, 
					       out int num_buffers);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_buffer_rename(string buffer_name, 
					    string new_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_buffer_delete(string buffer_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_buffer_get_width(string buffer_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_buffer_get_height(string buffer_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_buffer_get_bytes(string buffer_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern ImageBaseType gimp_buffer_get_image_type(string buffer_name);
  }
}
