// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2016 Maurits Rijk
//
// Parasite.cs
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

namespace Gimp
{
  public class Parasite
  {
    readonly IntPtr _parasite;

    public ulong Flags => gimp_parasite_flags(_parasite);
    public string Name => gimp_parasite_name(_parasite);

    // Fix me: doesn't work yet!
    public object Data => gimp_parasite_data(_parasite);
    public UInt32 DataSize => gimp_parasite_data_size(_parasite);
    public bool IsPersistent => gimp_parasite_is_persistent(_parasite);
    public bool IsUndoable => gimp_parasite_is_undoable(_parasite);

    internal IntPtr Ptr => _parasite;

    public const int Persistent = 1;
    public const int Undoable = 2;
    public const int AttachParent = 0x80 << 8;
    public const int ParentPersistent = Persistent << 8;
    public const int ParentUndoable = Undoable << 8;
    public const int AttachGrandparent = 0x80 << 16;
    public const int GrandparentPersistent = Persistent << 16;
    public const int GrandparentUndoable = Undoable << 16;

    public Parasite(string name, UInt32 flags, object data)
    {
      // Fix me: this doesn't work for data == string yet!
      int rawsize = Marshal.SizeOf(data);
      IntPtr buffer = Marshal.AllocHGlobal(rawsize);
      Marshal.StructureToPtr(data, buffer, false);

      _parasite = gimp_parasite_new(name, flags, (UInt32) rawsize, buffer);
    }

    public Parasite(Parasite parasite)
    {
      _parasite = gimp_parasite_copy(parasite._parasite);
    }

    public Parasite(IntPtr parasite)
    {
      _parasite = parasite;
    }

    public override bool Equals(object o)
    {
      if (o is Parasite)
	{
	  return gimp_parasite_compare(_parasite, (o as Parasite)._parasite);
	}
      return false;
    }

    public override int GetHashCode() => _parasite.GetHashCode();

    public static bool operator==(Parasite parasite1, Parasite parasite2)
    {
      if (ReferenceEquals(parasite1, parasite2))
	{
	  return true;
	}

      if (((object) parasite1 == null) || ((object) parasite2 == null))
	{
	  return false;
	}

      return parasite1._parasite.Equals(parasite2._parasite);
    }

    public static bool operator!=(Parasite parasite1, Parasite parasite2) =>
      !(parasite1 == parasite2);

    public void Free()
    {
      gimp_parasite_free(_parasite);
    }

    public bool IsType(string name) => gimp_parasite_is_type(_parasite, name);

    public bool HasFlag(ulong flag) => gimp_parasite_has_flag(_parasite, flag);

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_parasite_new (string name, UInt32 flags, 
					    UInt32 size, IntPtr data);
    //					    UInt32 size, object data);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_parasite_free (IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_parasite_copy (IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_parasite_compare(IntPtr a, IntPtr b);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_parasite_is_type(IntPtr parasite, string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_parasite_is_persistent(IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_parasite_is_undoable(IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_parasite_has_flag(IntPtr parasite, ulong flag);
    [DllImport("libgimp-2.0-0.dll")]
    static extern ulong gimp_parasite_flags(IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_parasite_name(IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    static extern object gimp_parasite_data(IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    static extern UInt32 gimp_parasite_data_size(IntPtr parasite);
  }
}
