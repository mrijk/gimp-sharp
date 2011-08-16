// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
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
      // Inner helper class
      class RandomSeedInfo
      {
	public IntPtr Handle {get; set;}
	
	public RandomSeedInfo(Variable<UInt32> seed, Variable<bool> randomSeed)
	{
	  UInt32 seedRef = seed.Value;
	  bool randomSeedRef = randomSeed.Value;
	  Handle = gimp_random_seed_new(ref seedRef, ref randomSeedRef);
	}
      }
      
      public RandomSeed(ref UInt32 seed, ref bool randomSeed) : 
	base(gimp_random_seed_new(ref seed, ref randomSeed))
      {
      }
 
      RandomSeed(Variable<UInt32> seed, Variable<bool> randomSeed,
		 RandomSeedInfo info) : base(info.Handle)
      {
	SpinButton.ValueChanged += delegate 
	  {seed.Value = (UInt32) SpinButton.ValueAsInt;};

	Toggle.Toggled += delegate {randomSeed.Value = Toggle.Active;};

	seed.ValueChanged += delegate {SpinButton.Value = seed.Value;};
	randomSeed.ValueChanged += delegate {Toggle.Active = randomSeed.Value;};
      }
      
      public RandomSeed(Variable<UInt32> seed, Variable<bool> randomSeed) :
	this(seed, randomSeed, new RandomSeedInfo(seed, randomSeed))
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
