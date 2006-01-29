// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// Procedure.cs
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
  public class Procedure
  {
    string _name;
    string _blurb;
    string _help;
    string _author;
    string _copyright;
    string _date;
    string _menu_path;
    string _image_types;

    ParamDefList _in_params;
    ParamDefList _return_vals;

    public Procedure(string name, string blurb, string help, 
		     string author, string copyright, 
		     string date, string menu_path, 
		     string image_types,
		     ParamDefList in_params)
    {
      _name = name;
      _blurb = blurb;
      _help = help;
      _author = author;
      _copyright = copyright;
      _date = date;
      _menu_path = menu_path;
      _image_types = image_types;
      _in_params = in_params;
    }

    public void Install(bool usesImage, bool usesDrawable)
    {
      GimpParamDef[] args = _in_params.GetGimpParamDef(usesImage, 
						       usesDrawable);
      
      gimp_install_procedure(_name, _blurb, _help, _author, _copyright, _date, 
			     _menu_path, _image_types, PDBProcType.PLUGIN, 
			     args.Length, 0, args, null);
    }

    public void MenuRegister(string menu_path)
    {
      if (!gimp_plugin_menu_register(_name, menu_path))
	{
	  throw new Exception();
	}
    }

    public string Name
    {
      get {return _name;}
      set {_name = value;}
    }

    [DllImport("libgimp-2.0-0.dll")]
    public static extern void gimp_install_procedure(
      string name,
      string blurb,
      string help,
      string author,
      string copyright,
      string date,
      string menu_path,
      string image_types,
      PDBProcType type,
      int    n_params,
      int    n_return_vals,
      GimpParamDef[] _params,
      GimpParamDef[] return_vals);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_plugin_menu_register(string procedure_name,
							string menu_path);
  }
}
