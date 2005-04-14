using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
  public class Parasite
    {
    IntPtr _parasite;

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

    public IntPtr Ptr
      {
      get {return _parasite;}
      }

    [DllImport("libgimp-2.0.so")]
    static extern IntPtr gimp_parasite_new (string name, UInt32 flags, 
                                            UInt32 size, object data);
    [DllImport("libgimp-2.0.so")]
    static extern void gimp_parasite_free (IntPtr parasite);
    [DllImport("libgimp-2.0.so")]
    static extern IntPtr gimp_parasite_copy (IntPtr parasite);
    }
  }
