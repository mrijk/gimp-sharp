using System;
using System.Runtime.InteropServices;

using Gtk;

namespace Gimp
  {
    public class RandomSeed : Widget
    {
      [DllImport("libgimpwidgets-2.0.so")]
      extern static IntPtr gimp_random_seed_new (ref UInt32 seed,
						 ref bool random_seed);

      public RandomSeed(ref UInt32 seed, ref bool random_seed) : 
	base(gimp_random_seed_new (ref seed, ref random_seed))
      {
      }
    }
  }
