using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
  public class Parasite
    {
    IntPtr _parasite;

    public const int PERSISTENT = 1;
    public const int UNDOABLE = 2;
    public const int ATTACH_PARENT = 0x80 << 8;
    public const int PARENT_PERSISTENT = PERSISTENT << 8;
    public const int PARENT_UNDOABLE = UNDOABLE << 8;
    public const int ATTACH_GRANDPARENT = 0x80 << 16;
    public const int GRANDPARENT_PERSISTENT = PERSISTENT << 16;
    public const int GRANDPARENT_UNDOABLE = UNDOABLE << 16;

    public Parasite(string name, UInt32 flags, UInt32 size, object data)
      {
      _parasite = gimp_parasite_new (name, flags, size, data);
      }

    public Parasite(Parasite parasite)
      {
      _parasite = gimp_parasite_copy(parasite._parasite);
      }

    public Parasite(IntPtr parasite)
      {
      _parasite = parasite;
      }

    public void Free()
      {
      gimp_parasite_free(_parasite);
      }

    public bool IsType(string name)
      {
      return gimp_parasite_is_type(_parasite, name);
      }

    public bool IsPersistent()
      {
      return gimp_parasite_is_persistent(_parasite);
      }

    public bool IsUndoable()
      {
      return gimp_parasite_is_undoable(_parasite);
      }

    public bool HasFlag(ulong flag)
      {
      return gimp_parasite_has_flag(_parasite, flag);
      }

    public ulong Flags
      {
      get {return gimp_parasite_flags(_parasite);}
      }

    public string Name
      {
      get {return gimp_parasite_name(_parasite);}
      }

    public object Data
      {
      get {return gimp_parasite_data(_parasite);}
      }

    public long DataSize
      {
      get {return gimp_parasite_data_size(_parasite);}
      }

    public IntPtr Ptr
      {
      get {return _parasite;}
      }

    [DllImport("libgimp-2.0.so")]
    static extern IntPtr gimp_parasite_new (string name, UInt32 flags, UInt32 size, object data);
    [DllImport("libgimp-2.0.so")]
    static extern void gimp_parasite_free (IntPtr parasite);
    [DllImport("libgimp-2.0.so")]
    static extern IntPtr gimp_parasite_copy (IntPtr parasite);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_parasite_compare(IntPtr a, IntPtr b);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_parasite_is_type(IntPtr parasite, string name);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_parasite_is_persistent(IntPtr parasite);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_parasite_is_undoable(IntPtr parasite);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_parasite_has_flag(IntPtr parasite, ulong flag);
    [DllImport("libgimp-2.0.so")]
    static extern ulong gimp_parasite_flags(IntPtr parasite);
    [DllImport("libgimp-2.0.so")]
    static extern string gimp_parasite_name(IntPtr parasite);
    [DllImport("libgimp-2.0.so")]
    static extern object gimp_parasite_data(IntPtr parasite);
    [DllImport("libgimp-2.0.so")]
    static extern long gimp_parasite_data_size(IntPtr parasite);
    }
  }
