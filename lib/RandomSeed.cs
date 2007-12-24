// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// RandomSeed.cs
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

using Gtk;

namespace Gimp
  {
    public class RandomSeed : HBox
    {
      public RandomSeed(ref UInt32 seed, ref bool random_seed) : 
	base(gimp_random_seed_new (ref seed, ref random_seed))
      {
      }

      // Warning: next few functions could break in future GIMP versions!

      public SpinButton SpinButton
      {
	get {return Children[0] as SpinButton;}
      }

      public ToggleButton Toggle
      {
	get {return Children[2] as ToggleButton;}
      }

      [DllImport("libgimpwidgets-2.0-0.dll")]
      extern static IntPtr gimp_random_seed_new (ref UInt32 seed,
						 ref bool random_seed);
    }
  }
