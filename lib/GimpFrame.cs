using System;
using System.Runtime.InteropServices;

using Gtk;

namespace Gimp
  {
    public class GimpFrame : Frame
    {
      [DllImport("libgimpwidgets-2.0.so")]
      extern static IntPtr gimp_frame_new(string label);

      public GimpFrame(string label) : base(gimp_frame_new(label))
      {
      }
    }
  }
