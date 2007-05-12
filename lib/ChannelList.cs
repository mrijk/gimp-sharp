// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// ChannelList.cs
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
  public sealed class ChannelList
  {
    readonly List<Channel> _list = new List<Channel>();

    public ChannelList(Image image)
    {
      int num_channels;
      IntPtr list = gimp_image_get_channels(image.ID, out num_channels);

      if (num_channels > 0)
	{
	  int[] dest = new int[num_channels];
	  Marshal.Copy(list, dest, 0, num_channels);
	  
	  foreach (int ChannelID in dest)
	    {
	      _list.Add(new Channel(ChannelID));
	    }
	}
    }

    public Channel this[int index]
    {
      get {return _list[index];}
    }

    public Channel this[string name]
    {
      get 
	{
	  foreach (Channel channel in _list)
	    {
	      if (channel.Name == name)
		{
		  return channel;
		}
	    }
	  return null;
	}
    }

    public IEnumerator<Channel> GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    public int Count
    {
      get {return _list.Count;}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_image_get_channels(Int32 image_ID, 
						 out int num_channels);
  }
}
