using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class Gimp
    {
      static public string Directory()
      {
	IntPtr tmp = gimp_directory();
	return Marshal.PtrToStringAuto(tmp);
      }

      [DllImport("libgimpbase-2.0.so")]
      public static extern IntPtr gimp_directory();
    }
  }
