using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class Gimp
    {
      [DllImport("libgimpbase-2.0-0.dll")]
      public static extern IntPtr gimp_directory();

      static public string Directory()
      {
	IntPtr tmp = gimp_directory();
	return null;
      }
    }
  }
