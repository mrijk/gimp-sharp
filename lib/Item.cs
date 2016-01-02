// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2016 Maurits Rijk
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

    internal int ID => _ID;

    public Item(int ID)
    {
      _ID = ID;
    }

    // Fix me: figure out how to get rid of this
    internal Item()
    {
    }

    static protected Item Instantiate(int ID)
    {
      if (ID == -1)
	return null;

      var item = new Item(ID);
      // The order matters!
      if (item.IsSelection)
	return null; // new Selection(ID);
      else if (item.IsChannel)
	return new Channel(ID);
      else if (item.IsTextLayer)
	return new TextLayer(ID);
      else if (item.IsLayerMask)
	return new Mask(ID);
      else if (item.IsGroup)
	return new LayerGroup(ID);
      else if (item.IsVectors)
	return new Vectors(ID);
      else if (item.IsLayer)
	return new Layer(ID);
      return null;
    }

    public bool IsValid => gimp_item_is_valid(ID);

    public void Delete()
    {
      if (!gimp_item_delete(ID))
	{
	  throw new GimpSharpException();
	}
    }

    public Image Image => new Image(gimp_item_get_image(ID));

    public bool IsDrawable => gimp_item_is_drawable(ID);
    public bool IsLayer => gimp_item_is_layer(ID);
    public bool IsTextLayer => gimp_item_is_text_layer(ID);
    public bool IsChannel => gimp_item_is_channel(ID);
    public bool IsLayerMask => gimp_item_is_layer_mask(ID);
    public bool IsSelection => gimp_item_is_selection(ID);
    public bool IsVectors => gimp_item_is_vectors(ID);
    public bool IsGroup => gimp_item_is_group(ID);

    public Item Parent => Instantiate(gimp_item_get_parent(ID));

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
	    Array.ForEach(dest, childID => children.Add(Instantiate(childID)));
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

    public ParasiteList ParasiteList => 
      new ParasiteList(ID, gimp_item_parasite_list, gimp_item_parasite_find);

    public void Lower()
    {
      Image.LowerItem(this);
    }

    public void Raise()
    {
      Image.RaiseItem(this);
    }

    public int Position => Image.GetItemPosition(this);

    public Item TransformFlipSimple(OrientationType flipType, bool autoCenter, 
				    double axis)
    {
      return new Item(gimp_item_transform_flip_simple(ID, flipType, autoCenter,
						      axis));
    }

    public Item TransformFlip(double x0, double y0, double x1, double y1)
    {
      return new Item(gimp_item_transform_flip (ID, x0, y0, x1, y1));
    }

    public Item TransformPerspective(double x0, double y0, double x1, double y1,
				     double x2, double y2, double x3, double y3)
    {
      return new Item(gimp_item_transform_perspective(ID, x0, y0, x1, y1, x2, y2, x3, y3));
    }

    public Item TransformRotateSimple(RotationType rotate_type, bool auto_center,
				      int centerX, int centerY)
    {
      return new Item(gimp_item_transform_rotate_simple(ID, rotate_type, auto_center, 
							centerX, centerY));
    }

    public Item TransformRotate(double angle, bool auto_center,
				int centerX, int centerY)
    {
      return new Item(gimp_item_transform_rotate(ID, angle, auto_center, 
						 centerX, centerY));
    }
    public Item TransformScale(double x0, double y0, double x1, double y1)
    {
      return new Item(gimp_item_transform_scale(ID, x0, y0, x1, y1));
    }

    public Item Transform2d(double sourceX, double sourceY,
			    double scaleX, double scaleY,
			    double angle, double destX, double destY)
    {
      return new Item(gimp_item_transform_2d(ID, sourceX, sourceY, scaleX, scaleY,
					     angle, destX, destY));
    }

    public Item TransformMatrix(double coeff_0_0, double coeff_0_1, double coeff_0_2, 
				double coeff_1_0, double coeff_1_1, double coeff_1_2,
				double coeff_2_0, double coeff_2_1, double coeff_2_2)
    {
      return new Item(gimp_item_transform_matrix(ID, 
						 coeff_0_0, coeff_0_1, coeff_0_2, 
						 coeff_1_0, coeff_1_1, coeff_1_2, 
						 coeff_2_0, coeff_2_1, coeff_2_2));
    }

    public override bool Equals(object o)
    {
      if (o is Item)
	{
	  return ID == (o as Item).ID;
	}
      return false;
    }

    public override int GetHashCode()
    {
      return ID.GetHashCode();
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
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_item_transform_flip_simple(Int32 item_ID,
							OrientationType flip_type,
							bool auto_center,
							double axis);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_item_transform_flip(Int32 item_ID,
						 double x0, double y0,
						 double x1, double y1);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_item_transform_perspective(Int32 item_ID,
							double x0,
							double y0,
							double x1,
							double y1,
							double x2,
							double y2,
							double x3,
							double y3);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_item_transform_rotate_simple(Int32 item_ID,
							  RotationType rotate_type,
							  bool auto_center,
							  int center_x,
							  int center_y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_item_transform_rotate(Int32 item_ID,
						   double angle,
						   bool auto_center,
						   int center_x,
						   int center_y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_item_transform_scale(Int32 item_ID,
						  double x0,
						  double y0,
						  double x1,
						  double y1);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_item_transform_shear(Int32 item_ID,
						  OrientationType shear_type,
						  double magnitude);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_item_transform_2d(Int32 item_ID,
					       double source_x,
					       double source_y,
					       double scale_x,
					       double scale_y,
					       double angle,
					       double dest_x,
					       double dest_y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_item_transform_matrix(Int32 item_ID,
						   double coeff_0_0,
						   double coeff_0_1,
						   double coeff_0_2,
						   double coeff_1_0,
						   double coeff_1_1,
						   double coeff_1_2,
						   double coeff_2_0,
						   double coeff_2_1,
						   double coeff_2_2);
  }
}
