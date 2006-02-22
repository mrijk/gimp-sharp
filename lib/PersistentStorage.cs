// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
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

    public PersistentStorage(Plugin plugin)
    {
      _plugin = plugin;
      _name = plugin.Name;
    }

    BinaryFormatter _formatter = new BinaryFormatter();

    public void SetData()
    {
      MemoryStream memoryStream = new MemoryStream();
      Type type = _plugin.GetType();

      foreach (FieldInfo field 
	       in type.GetFields(BindingFlags.Instance |  
				 BindingFlags.NonPublic | 
				 BindingFlags.Public))
	{
	  foreach (object attribute in field.GetCustomAttributes(true))
	    {
	      if (attribute is SaveAttribute)
		{
		  _formatter.Serialize(memoryStream, field.GetValue(_plugin));
		}
	    }
	}
      gimp_procedural_db_set_data(_name, memoryStream.GetBuffer(),
				  (int) memoryStream.Length);		    
    }


    public void GetData()
    {
      int size = gimp_procedural_db_get_data_size(_name);
      if (size > 0)
	{
	  byte[] data = new byte[size];
	  gimp_procedural_db_get_data(_name, data);

	  MemoryStream memoryStream = new MemoryStream(data);
	  Type type = _plugin.GetType();

	  foreach (FieldInfo field 
		   in type.GetFields(BindingFlags.Instance |  
				     BindingFlags.NonPublic | 
				     BindingFlags.Public))
	    {
	      foreach (object attribute in field.GetCustomAttributes(true))
		{
		  if (attribute is SaveAttribute)
		    {
		      field.SetValue(_plugin, 
				     _formatter.Deserialize(memoryStream));
		    }
		}
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
