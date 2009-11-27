// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// PersistentStorage.cs
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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace Gimp
{
  public sealed class PersistentStorage
  {
    readonly Plugin _plugin;
    readonly string _name;
    BinaryFormatter _formatter = new BinaryFormatter();

    public PersistentStorage(Plugin plugin)
    {
      _plugin = plugin;
      _name = plugin.Name;
    }

    public void SetData()
    {
      var memoryStream = new MemoryStream();

      var type = _plugin.GetType();

      foreach (var attribute in new SaveAttributeSet(type))
	{
	  var field = attribute.Field;
	  _formatter.Serialize(memoryStream, field.GetValue(_plugin));
	}

      int length = (int) memoryStream.Length;
      if (length != 0)
	{
	  gimp_procedural_db_set_data(_name, memoryStream.GetBuffer(), length);
	}
    }

    public void GetData()
    {
      int size = gimp_procedural_db_get_data_size(_name);
      if (size > 0)
	{
	  var data = new byte[size];
	  gimp_procedural_db_get_data(_name, data);

	  var memoryStream = new MemoryStream(data);
	  var type = _plugin.GetType();

	  foreach (var attribute in new SaveAttributeSet(type))
	    {
	      var field = attribute.Field;
	      field.SetValue(_plugin, _formatter.Deserialize(memoryStream));
	    }
	}
    }

    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_procedural_db_set_data(string identifier, 
							  byte[] data,
							  int bytes);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_procedural_db_get_data(string identifier, 
							  byte[] data);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern int gimp_procedural_db_get_data_size(string identifier);
  }
}
