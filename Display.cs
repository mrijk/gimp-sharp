using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class Display
    {
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_display_new(Int32 image_ID);

      Int32 _displayID;

      public Display(Image image)
      {
	_displayID = gimp_display_new(image.ID);
      }

      [DllImport("libgimp-2.0.so")]
      static extern void gimp_displays_flush();

      public static void DisplaysFlush()
      {
	gimp_displays_flush();
      }

      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_display_delete(Int32 display_ID);

      public void Delete()
      {
	if (!gimp_display_delete(_displayID))
	  {
	  // Fix me: throw exception
	  }
      }
    }
  }
