// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2005 Maurits Rijk
//
// Drawable.cs
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
  public class Drawable
    {
    IntPtr _drawable;
    protected Int32 _ID;

    public Drawable(Int32 drawableID)
      {
      _ID = drawableID;
      _drawable = gimp_drawable_get (_ID);
      }

    public Drawable(IntPtr drawable)
      {
      _drawable = drawable;
      }

    public void Detach()
      {
      gimp_drawable_detach (_drawable);
      }

    public void Flush()
      {
      gimp_drawable_flush (_drawable);
      }

    public bool Delete()
      {
      return gimp_drawable_delete (_ID);
      }

    public string Name
      {
      get {return gimp_drawable_get_name(_ID);}
      set {gimp_drawable_set_name(_ID, value);}
      }

    public bool Visible
      {
      get {return gimp_drawable_get_visible(_ID);}
      set {gimp_drawable_set_visible(_ID, value);}
      }

    public bool Linked
      {
      get {return gimp_drawable_get_linked(_ID);}
      set {gimp_drawable_set_linked(_ID, value);}
      }

    public int Tattoo
      {
      get {return gimp_drawable_get_tattoo(_ID);}
      set {gimp_drawable_set_tattoo(_ID, value);}
      }

    public Tile GetTile(bool shadow, int row, int col)
      {
      return new Tile(gimp_drawable_get_tile(_ID, shadow, row, col));
      }

    public Tile GetTile2(bool shadow, int x, int y)
      {
      return new Tile(gimp_drawable_get_tile2(_ID, shadow, x, y));
      }

    public byte[] GetThumbnailData(ref int width, ref int height, out int bpp)
      {
      IntPtr src = gimp_drawable_get_thumbnail_data(_ID, ref width,
                                                    ref height, out bpp);
      byte[] dest = new byte[width * height * bpp];
      Marshal.Copy(src, dest, 0, width * height * bpp);
      // Fix me: free src
      return dest;
      }

    public void Fill(FillType fill_type)
      {
      if (!gimp_drawable_fill(_ID, fill_type))
        {
        throw new Exception();
        }
      }

    public void Update(int x, int y, int width, int height)
      {
      if (!gimp_drawable_update(_ID, x, y, width, height))
        {
        throw new Exception();
        }
      }

    public void MaskBounds(out int x1, out int y1, out int x2, out int y2)
      {
      if (!gimp_drawable_mask_bounds(_ID, out x1, out y1, 
                                     out x2, out y2))
        {
        throw new Exception();
        }
      }

    public void MaskIntersect(out int x1, out int y1, out int x2, out int y2)
      {
      if (!gimp_drawable_mask_intersect(_ID, out x1, out y1, 
                                        out x2, out y2))
        {
        throw new Exception();
        }
      }

    public Image Image
      {
      get {return new Image(gimp_drawable_get_image(_ID));}
      }

    public bool MergeShadow(bool undo)
      {
      return gimp_drawable_merge_shadow(_ID, undo);
      }

    public bool HasAlpha()
      {
      return gimp_drawable_has_alpha(_ID);
      }

    public bool IsRGB
      {
      get {return gimp_drawable_is_rgb(_ID);}
      }

    public bool IsGray
      {
      get {return gimp_drawable_is_gray(_ID);}
      }

    public bool IsIndexed
      {
      get {return gimp_drawable_is_indexed(_ID);}
      }

    public int Bpp
      {
      get {return gimp_drawable_bpp(_ID);}
      }

    public int Width
      {
      get {return gimp_drawable_width(_ID);}
      }

    public int Height
      {
      get {return gimp_drawable_height(_ID);}
      }

    public void Offsets(out int offset_x, out int offset_y)
      {
      if (!(gimp_drawable_offsets(_ID, out offset_x, out offset_y)))
        {
        throw new Exception();
        }
      }

    public bool IsLayer()
      {
      return gimp_drawable_is_layer(_ID);
      }

    public bool IsLayerMask()
      {
      return gimp_drawable_is_layer_mask(_ID);
      }

    public bool IsChannel()
      {
      return gimp_drawable_is_channel(_ID);
      }

    public void Offset(bool wrap_around, OffsetType fill_type,
                       int offset_x, int offset_y)
      {
      if (!gimp_drawable_offset(_ID, wrap_around, fill_type, offset_x, 
                                offset_y))
        {
        throw new Exception();
        }
      }

    public Parasite ParasiteFind(string name)
      {
      return new Parasite(gimp_drawable_parasite_find(_ID, name));
      }

    public void ParasiteAttach(Parasite parasite)
      {
      if (!gimp_drawable_parasite_attach(_ID, parasite.Ptr))
        {
        throw new Exception();
        }
      }

    public void ParasiteDetach(string name)
      {
      if (!gimp_drawable_parasite_detach(_ID, name))
        {
        throw new Exception();
        }
      }

    public void AttachNewParasite(string name, int flags, int size, 
                                  object data)
      {
      if (!gimp_drawable_attach_new_parasite(_ID, name, flags, size, data))
        {
        throw new Exception();
        }
      }

    public void Desaturate()
      {
      if (!gimp_desaturate(_ID))
        {
        throw new Exception();
        }
      }

    public void FuzzySelect(double x, double y, int threshold, 
                            ChannelOps operation, bool antialias,
                            bool feather, double feather_radius,
                            bool sample_merged)
      {
      if (!gimp_fuzzy_select(_ID, x, y, threshold, operation,
                             antialias, feather, feather_radius, 
                             sample_merged))
        {
        throw new Exception();
        }
      }

    public Int32 ID
      {
      get {return _ID;}
      }

    public IntPtr Ptr
      {
      get {return _drawable;}
      }

    [DllImport("libgimp-2.0.so")]
    static extern IntPtr gimp_drawable_get(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern void gimp_drawable_detach(IntPtr drawable);
    [DllImport("libgimp-2.0.so")]
    static extern void gimp_drawable_flush(IntPtr drawable);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_delete(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern string gimp_drawable_get_name(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_set_name(Int32 drawable_ID, 
                                              string name);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_get_visible(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_set_visible(Int32 drawable_ID,
                                                 bool visible);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_get_linked(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_set_linked(Int32 drawable_ID,
                                                bool linked);
    [DllImport("libgimp-2.0.so")]
    static extern int gimp_drawable_get_tattoo(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_set_tattoo(Int32 drawable_ID, 
                                                int tattoo);
    [DllImport("libgimp-2.0.so")]
    static extern IntPtr gimp_drawable_get_tile(Int32 drawable_ID,
                                                bool shadow, int row, int col);
    [DllImport("libgimp-2.0.so")]
    static extern IntPtr gimp_drawable_get_tile2(Int32 drawable_ID,
                                                 bool shadow, int x, int y);
    [DllImport("libgimp-2.0.so")]
    static extern IntPtr gimp_drawable_get_thumbnail_data(Int32 drawable_ID,
                                                          ref int width, 
                                                          ref int height, 
                                                          out int bpp);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_merge_shadow(Int32 drawable_ID,
                                                  bool undo);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_fill(Int32 drawable_ID,
                                          FillType fill_type);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_update(Int32 drawable_ID,
                                            int x,
                                            int y,
                                            int width,
                                            int height);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_mask_bounds(Int32 drawable_ID,
                                                 out int x1,
                                                 out int y1,
                                                 out int x2,
                                                 out int y2);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_mask_intersect(Int32 drawable_ID,
                                                    out int x1,
                                                    out int y1,
                                                    out int x2,
                                                    out int y2);
    [DllImport("libgimp-2.0.so")]
    static extern Int32 gimp_drawable_get_image(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_has_alpha (Int32 drawable_ID);      
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_is_rgb (Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_is_gray (Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_is_indexed (Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern int gimp_drawable_bpp (Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern int gimp_drawable_width(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern int gimp_drawable_height(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_offsets(Int32 drawable_ID,
                                             out int offset_x,
                                             out int offset_y);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_is_layer(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_is_layer_mask(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_is_channel(Int32 drawable_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_offset(Int32 drawable_ID,
                                            bool wrap_around,
                                            OffsetType fill_type,
                                            int offset_x,
                                            int offset_y);
    [DllImport("libgimp-2.0.so")]
    static extern IntPtr gimp_drawable_parasite_find(Int32 drawable_ID,
                                                     string name);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_parasite_attach(Int32 drawable_ID,
                                                     IntPtr parasite);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_parasite_detach(Int32 drawable_ID,
                                                     string name);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_drawable_attach_new_parasite(Int32 drawable_ID,
                                                         string name, 
                                                         int flags,
                                                         int size,
                                                         object data);

    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_desaturate (Int32 drawable_ID);

    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_fuzzy_select (Int32 drawable_ID,
                                          double x,
                                          double y,
                                          int threshold,
                                          ChannelOps operation,
                                          bool antialias,
                                          bool feather,
                                          double feather_radius,
                                          bool sample_merged);
    }
  }
