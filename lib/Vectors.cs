// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// Vectors.cs
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
  public class Vectors
  {
    readonly protected Int32 _ID;

    public Vectors(Image image, string name)
    {
      _ID = gimp_vectors_new(image.ID, name);
    }

    public Image Image
    {
      get {return new Image(gimp_vectors_get_image(_ID));}
    }

    public bool Linked
    {
      get {return gimp_vectors_get_linked(_ID);}
      set 
	{
	  if (!gimp_vectors_set_linked(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public string Name
    {
      get {return gimp_vectors_get_name(_ID);}
      set
	{
	  if (!gimp_vectors_set_name(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public Tattoo Tattoo
    {
      get {return new Tattoo(gimp_vectors_get_tattoo(_ID));}
      set
	{
	  if (!gimp_vectors_set_tattoo(_ID, value.ID))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public bool Visible
    {
      get {return gimp_vectors_get_visible(_ID);}
      set
	{
	  if (!gimp_vectors_set_visible(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public void ParasiteAttach(Parasite parasite)
    {
      if (!gimp_vectors_parasite_attach(_ID, parasite.Ptr))
        {
	  throw new GimpSharpException();
        }
    }

    public void ParasiteDetach(Parasite parasite)
    {
      if (!gimp_vectors_parasite_detach(_ID, parasite.Name))
        {
	  throw new GimpSharpException();
        }
    }

    public Parasite ParasiteFind(string name)
    {
      return new Parasite(gimp_vectors_parasite_find(_ID, name));
    }

    [DllImport("libgimp-2.0-0.dll")]
    extern static Int32 gimp_vectors_get_image(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_get_linked(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_vectors_get_name(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static int gimp_vectors_get_tattoo(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_get_visible(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static Int32 gimp_vectors_new(Int32 image_ID, string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_set_linked(Int32 vectors_ID, bool linked);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_set_name(Int32 vectors_ID, string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_set_tattoo(Int32 vectors_ID, int tattoo);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_set_visible(Int32 vectors_ID, 
						bool visible);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_parasite_attach(Int32 vectors_ID,
						    IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_parasite_detach(Int32 vectors_ID,
						    string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_vectors_parasite_find(Int32 drawable_ID,
						    string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_vectors_parasite_list(Int32 drawable_ID,
						  out int num_parasites,
						  ref IntPtr parasites);
  }
}
