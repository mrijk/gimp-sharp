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
using System.Reflection;
using System.Runtime.InteropServices;

using Gdk;

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

    string _menuPath;	// Fix me: this name looks to much like _menu_path
    string _iconFile;

    ParamDefList _inParams;
    ParamDefList _return_vals;

    public Procedure(string name, string blurb, string help, 
		     string author, string copyright, 
		     string date, string menu_path, 
		     string image_types,
		     ParamDefList inParams)
    {
      _name = name;
      _blurb = blurb;
      _help = help;
      _author = author;
      _copyright = copyright;
      _date = date;
      _menu_path = menu_path;
      _image_types = image_types;
      _inParams = inParams;
    }

    public void Install(bool usesImage, bool usesDrawable)
    {
      GimpParamDef[] args = _inParams.GetGimpParamDef(usesImage, 
						      usesDrawable);
      
      gimp_install_procedure(_name, _blurb, _help, _author, _copyright, _date, 
			     _menu_path, _image_types, PDBProcType.PLUGIN, 
			     args.Length, 0, args, null);
      MenuRegister();
      IconRegister();
    }

    public void MenuRegister()
    {
      if (_menuPath != null)
	{
	  if (!gimp_plugin_menu_register(_name, _menuPath))
	    {
	      throw new Exception();
	    }
	}
    }

    public void IconRegister()
    {
      if (_iconFile != null)
	{
	  Pixbuf pixbuf = new Pixbuf(Assembly.GetEntryAssembly(), _iconFile);
	  
	  Pixdata data = new Pixdata();
	  data.FromPixbuf(pixbuf, false);
	  if (!gimp_plugin_icon_register(_name, IconType.INLINE_PIXBUF, 
					 data.Serialize()))
	    {
	      throw new Exception();
	    }
	}
    }

    public string Name
    {
      get {return _name;}
      set {_name = value;}
    }

    public string MenuPath
    {
      set {_menuPath = value;}
    }

    public string IconFile
    {
      set {_iconFile = value;}
    }

    public ParamDefList InParams
    {
      get {return _inParams;}
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
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_plugin_icon_register(string procedure_name,
							IconType icon_type, 
							byte[] icon_data);
  }
}
