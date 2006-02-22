// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// Path.cs
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
  public class Path
  {
    internal Int32 _imageID;
    internal string _name;

    public Path(Image image, string name)
    {
      _imageID = image.ID;
      _name = name;
    }

    public void Delete()
    {
      if (!gimp_path_delete(_imageID, _name))
        {
	  throw new Exception();
        }
    }

    public bool Locked
    {
      get {return gimp_path_get_locked(_imageID, _name);}
      set
	{
          if (!gimp_path_set_locked(_imageID, _name, value))
            {
	      throw new Exception();
            }
	}
    }

    public Tattoo Tattoo
    {
      get {return new Tattoo(gimp_path_get_tattoo(_imageID, _name));}
      set
	{
          if (!gimp_path_set_tattoo(_imageID, _name, value.ID))
            {
	      throw new Exception();
            }
	}
    }

    public void ToSelection(ChannelOps op, bool antialias,
                            bool feather, double feather_radius_x, 
			    double feather_radius_y)
    {
      if (!gimp_path_to_selection(_imageID, _name, op, antialias, 
                                  feather, feather_radius_x, feather_radius_y))
        {
	  throw new Exception();
        }
    }

    public void Import(string filename, bool merge, bool scale)
    {
      if (!gimp_path_import(_imageID, filename, merge, scale))
        {
	  throw new Exception();
        }
    }

    public void ImportString(string importString, int length, bool merge,
                             bool scale)
    {
      if (!gimp_path_import_string(_imageID, importString, length, merge, 
				   scale))
        {
	  throw new Exception();
        }
    }

    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_path_delete(Int32 image_ID,
                                        string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static int gimp_path_get_tattoo(Int32 image_ID,
                                           string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_path_set_tattoo(Int32 image_ID,
                                            string name, int tattoovalue);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_path_get_locked(Int32 image_ID,
                                            string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_path_set_locked(Int32 image_ID,
                                            string name, bool locked);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_path_to_selection(Int32 image_ID,
                                              string name, ChannelOps op, 
					      bool antialias,
                                              bool feather,
                                              double feather_radius_x,
                                              double feather_radius_y);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_path_import(Int32 image_ID, string filename, 
					bool merge, bool scale);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_path_import_string(Int32 image_ID, 
					       string importString, int length,
					       bool merge, bool scale);
  }
}
