// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// ProceduralDb.cs
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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Gimp
{
  public sealed class ProceduralDb
  {
    public static string TempName()
    {
      return gimp_procedural_db_temp_name();
    }

    public static byte[] GetData(string identifier)
    {
      int size = GetDataSize(identifier);
      if (size > 0)
	{
	  var data = new byte[size];
	  if (!gimp_procedural_db_get_data(identifier, data))
	    {
	      throw new GimpSharpException();
	    }
	  return data;
	}
      return null;
    }

    public static void SetData(string identifier, byte[] data)
    {
      if (!gimp_procedural_db_set_data(identifier, data, data.Length))
	{
	  throw new GimpSharpException();
	}
    }

    public static void Dump(string filename)
    {
      if (!gimp_procedural_db_dump(filename))
	{
	  throw new GimpSharpException();
	}
    }

    public static List<string> Query(string name, string blurb, string help,
				     string author, string copyright, 
				     string date, string procedureType)
    {
      int numMatches;
      IntPtr ptr;
      var procedureNames = new List<string>();

      if (!gimp_procedural_db_query(name, blurb, help, author, copyright, 
				    date, procedureType, out numMatches,
				    out ptr))
	{
	  throw new GimpSharpException();
	}
      
      // TODO: put next lines in a Util class
      for (int i = 0; i < numMatches; i++)
	{
	  IntPtr tmp = (IntPtr) Marshal.PtrToStructure(ptr, typeof(IntPtr));
	  procedureNames.Add(Marshal.PtrToStringAnsi(tmp));
	  ptr = (IntPtr)((int)ptr + Marshal.SizeOf(tmp));
        }
      return procedureNames;
    }
    
    public static bool ProcExists(string name)
    {
      return gimp_procedural_db_proc_exists(name);
    }

    public static int GetDataSize(string identifier)
    {
      return gimp_procedural_db_get_data_size(identifier);
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_procedural_db_temp_name();
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_procedural_db_get_data(string identifier, 
							  byte[] data);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_procedural_db_set_data(string identifier, 
							  byte[] data,
							  int bytes);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_procedural_db_dump(string filename);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_procedural_db_query(string name,
						       string blurb,
						       string help,
						       string author,
						       string copyright,
						       string date,
						       string proc_type,
						       out int num_matches,
						       out IntPtr procedure_names);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_procedural_db_proc_exists(
						    string procedure_name);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern int gimp_procedural_db_get_data_size(string identifier);
  }
}
