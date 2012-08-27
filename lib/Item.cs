// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
//
// Item.cs
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
  public /* abstract */ class Item
  {
    protected Int32 _ID;

    public Item(int ID)
    {
      _ID = ID;
    }

    // Fix me: figure out how to get rid of this
    internal Item()
    {
    }

    public bool IsValid
    {
      get {return gimp_item_is_valid(ID);}
    }

    public void Delete()
    {
      if (!gimp_item_delete(ID))
	{
	  throw new GimpSharpException();
	}
    }

    public Image Image
    {
      get {return new Image(gimp_item_get_image(ID));}
    }

    public bool IsDrawable
    {
      get {return gimp_item_is_drawable(ID);}
    }

    public bool IsLayer
    {
      get {return gimp_item_is_layer(ID);}
    }

    public bool IsTextLayer
    {
      get {return gimp_item_is_text_layer(ID);}
    }

    public bool IsChannel
    {
      get {return gimp_item_is_channel(ID);}
    }

    public bool IsLayerMask
    {
      get {return gimp_item_is_layer_mask(ID);}
    }

    public bool IsSelection
    {
      get {return gimp_item_is_selection(ID);}
    }

    public bool IsVectors
    {
      get {return gimp_item_is_vectors(ID);}
    }

    public bool IsGroup
    {
      get {return gimp_item_is_group(ID);}
    }

    public Item Parent
    {
      // Fix me!
      get {return null;}
      // get {return new Item(gimp_item_get_parent(ID));}
    }

    public List<Item> Children
    {
      get 
      {
	var children = new List<Item>();

	int numChildren;
	IntPtr ptr = gimp_item_get_children(ID, out numChildren);

	if (numChildren > 0)
	  {
	    var dest = new int[numChildren];
	    Marshal.Copy(ptr, dest, 0, numChildren);
	    // Fix me: instantiate concrete objects based on type iso Item objects
	    Array.ForEach(dest, childID => children.Add(new Item(childID)));
	  }
	return children;
      }
    }

    public string Name
    {
      get {return gimp_item_get_name(ID);}
      set
	{
	  if (!gimp_item_set_name(ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public bool Visible
    {
      get {return gimp_item_get_visible(_ID);}
      set
	{
	  if (!gimp_item_set_visible(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public bool Linked
    {
      get {return gimp_item_get_linked(ID);}
      set 
	{
	  if (!gimp_item_set_linked(ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public bool LockContent
    {
      get {return gimp_item_get_lock_content(ID);}
      set 
	{
	  if (!gimp_item_set_lock_content(ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public Tattoo Tattoo
    {
      get {return new Tattoo(gimp_item_get_tattoo(ID));}
      set
	{
	  if (!gimp_item_set_tattoo(ID, value.ID))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public void AttachParasite(Parasite parasite)
    {
      if (!gimp_item_attach_parasite(ID, parasite.Ptr))
        {
	  throw new GimpSharpException();
        }
    }

    public void DetachParasite(Parasite parasite)
    {
      if (!gimp_item_detach_parasite(ID, parasite.Name))
        {
	  throw new GimpSharpException();
        }
    }

    public Parasite ParasiteFind(string name)
    {
      IntPtr found = gimp_item_parasite_find(ID, name);
      return (found == IntPtr.Zero) ? null : new Parasite(found);
    }

    public ParasiteList ParasiteList
    {
      get
	{
	  return new ParasiteList(ID, gimp_item_parasite_list,
				  gimp_item_parasite_find);
	}
    }

    internal int ID
    {
      get {return _ID;}
    }

    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_item_is_valid(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static Int32 gimp_item_get_image(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_item_delete(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_item_is_drawable(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_item_is_layer(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_item_is_text_layer(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_item_is_channel(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_item_is_layer_mask(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_item_is_selection(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_item_is_vectors(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_item_is_group(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_item_get_parent(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_item_get_children(Int32 item_ID,
						out int num_children);
    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_item_get_name(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_item_set_name(Int32 item_ID, string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_item_get_visible(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_item_set_visible(Int32 item_ID, bool visible);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_item_get_linked(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_item_set_linked(Int32 item_ID, bool linked);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_item_get_lock_content(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_item_set_lock_content(Int32 item_ID, bool lock_content);
    [DllImport("libgimp-2.0-0.dll")]
    extern static int gimp_item_get_tattoo(Int32 item_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_item_set_tattoo(Int32 item_ID, int tattoo);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_item_attach_parasite(Int32 item_ID, IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_item_detach_parasite(Int32 item_ID, string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_item_parasite_find(Int32 item_ID, string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_item_parasite_list(Int32 item_ID,
					       out int num_parasites,
					       out IntPtr parasites);
  }
}
