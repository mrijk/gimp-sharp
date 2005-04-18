using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
  public class Selection
    {
    Int32 _imageID;

    public Selection(Image image) : this(image.ID)
      {
      }

    public Selection(Int32 imageID)
      {
      _imageID = imageID;
      }

    public void Bounds(out bool non_empty,
                       out int x1, out int y1,
                       out int x2, out int y2)
      {
      if (!gimp_selection_bounds (_imageID,
                                  out non_empty,
                                  out x1, out y1, out x2, out y2))
        {
        throw new Exception();
        }
      }

    public void All()
      {
      if (!gimp_selection_all (_imageID))
        {
        throw new Exception();
        }
      }

    public void None()
      {
      if (!gimp_selection_none (_imageID))
        {
        throw new Exception();
        }
      }

    public void Clear()
      {
      if (!gimp_selection_clear (_imageID))
        {
        throw new Exception();
        }
      }

    public bool Empty
      {
      get {return gimp_selection_is_empty (_imageID);}
      }

    public Layer Float(Drawable drawable, int offx, int offy)
      {
      return new Layer(gimp_selection_float (_imageID,
                                             drawable.ID, offx, offy));
      }

    public void Load()
      {
      if (!gimp_selection_load (_imageID))
        {
        throw new Exception();
        }
      }

    public Channel Save()
      {
      return new Channel(gimp_selection_save (_imageID));
      }

    public int Value(int x, int y)
      {
      return gimp_selection_value (_imageID, x, y);
      }

    public int this [int x, int y]
      {
      get {return gimp_selection_value (_imageID, x, y);}
      }

    public void Grow(int steps)
      {
      if (!gimp_selection_grow (_imageID, steps))
        {
        throw new Exception();
        }
      }

    public void Shrink(int radius)
      {
      if (!gimp_selection_shrink (_imageID, radius))
        {
        throw new Exception();
        }
      }

    public void Invert()
      {
      if (!gimp_selection_invert (_imageID))
        {
        throw new Exception();
        }
      }

    public void Feather(int radius)
      {
      if (!gimp_selection_feather (_imageID, radius))
        {
        throw new Exception();
        }
      }

    public void Sharpen()
      {
      if (!gimp_selection_sharpen (_imageID))
        {
        throw new Exception();
        }
      }

    public void Border(int radius)
      {
      if (!gimp_selection_border (_imageID, radius))
        {
        throw new Exception();
        }
      }

    public void Translate(int offx, int offy)
      {
      if (!gimp_selection_translate (_imageID, offx, offy))
        {
        throw new Exception();
        }
      }

    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_bounds (Int32 image_ID,
                                              out bool non_empty,
                                              out int x1,
                                              out int y1,
                                              out int x2,
                                              out int y2);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_all (Int32 image_ID);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_none (Int32 image_ID);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_clear (Int32 image_ID);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_is_empty (Int32 image_ID);
    [DllImport("libgimp-2.0.so")]
    extern static Int32 gimp_selection_float (Int32 image_ID,
                                              Int32 drawable_ID,
                                              int offx, int offy);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_load (Int32 channel_ID);
    [DllImport("libgimp-2.0.so")]
    extern static Int32 gimp_selection_save (Int32 image_ID);
    [DllImport("libgimp-2.0.so")]
    extern static int gimp_selection_value (Int32 image_ID,
                                            int x, int y);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_grow (Int32 image_ID,
                                            int steps);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_shrink (Int32 image_ID,
                                              int radius);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_invert (Int32 image_ID);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_feather (Int32 image_ID,
                                               double radius);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_sharpen (Int32 image_ID);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_border (Int32 image_ID,
                                              int radius);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_translate (Int32 image_ID,
                                                 int offx, int offy);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_layer_alpha (Int32 layer_ID);
    [DllImport("libgimp-2.0.so")]
    extern static bool gimp_selection_combine (Int32 channel_ID,
                                               ChannelOps operation); 
    }
  }
