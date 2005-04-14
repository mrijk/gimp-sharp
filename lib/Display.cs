using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Gimp
  {
  public class Display
    {
    Int32 _displayID = -1;

    public Display(Image image)
      {
      Debug.Assert(image != null);
      _displayID = gimp_display_new(image.ID);
      }

    public void Delete()
      {
      Debug.Assert(_displayID != -1);
      if (!gimp_display_delete(_displayID))
        {
        throw new Exception();
        }
      }

    public static void DisplaysFlush()
      {
      gimp_displays_flush();
      }

    public static bool DisplaysReconnect(
      Image oldImage, Image newImage)
      {
      Debug.Assert(oldImage != null && newImage != null);
      return gimp_displays_reconnect(oldImage.ID,
                                     newImage.ID);
      }

    [DllImport("libgimp-2.0.so")]
    static extern Int32 gimp_display_new(Int32 image_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_display_delete(Int32 display_ID);
    [DllImport("libgimp-2.0.so")]
    static extern void gimp_displays_flush();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_displays_reconnect(Int32 old_image_ID,
                                               Int32 new_image_ID);
    }
  }
