using System;
using System.Runtime.InteropServices;

using Gtk;
using GtkSharp;

namespace Gimp
  {
    public class Progress
    {
      public Progress(string message)
      {
	gimp_progress_init (message);
      }

      public void Update(double percentage)
      {
	gimp_progress_update (percentage);
      }

      [DllImport("libgimp-2.0.so")]
      public static extern bool gimp_progress_init (string message);
      [DllImport("libgimp-2.0.so")]
      public static extern bool gimp_progress_update(double percentage);

    }
  }
