using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class FloatingSelection
    {
      Int32 _ID;

      public FloatingSelection(Int32 floatingSelID)
      {
	_ID = floatingSelID;
      }

      public void Remove()
      {
	if (!gimp_floating_sel_remove (_ID))
	  {
	  throw new Exception();
	  }
      }

      public void Anchor()
      {
	if (!gimp_floating_sel_anchor (_ID))
	  {
	  throw new Exception();
	  }
      }

      public void Rigor(bool undo)
      {
	if (!gimp_floating_sel_rigor (_ID, undo))
	  {
	  throw new Exception();
	  }
      }

      public void Relax(bool undo)
      {
	if (!gimp_floating_sel_relax (_ID, undo))
	  {
	  throw new Exception();
	  }
      }

      [DllImport("libgimp-2.0.so")]
      extern static bool gimp_floating_sel_remove (Int32 floating_sel_ID);
      [DllImport("libgimp-2.0.so")]
      extern static bool gimp_floating_sel_anchor (Int32 floating_sel_ID);
      [DllImport("libgimp-2.0.so")]
      extern static bool gimp_floating_sel_to_layer (Int32 floating_sel_ID);
      [DllImport("libgimp-2.0.so")]
      extern static bool gimp_floating_sel_attach (Int32 floating_sel_ID,
						   Int32 drawable_ID);
      [DllImport("libgimp-2.0.so")]
      extern static bool gimp_floating_sel_rigor (Int32 floating_sel_ID,
						  bool undo);
      [DllImport("libgimp-2.0.so")]
      extern static bool gimp_floating_sel_relax (Int32 floating_sel_ID,
						  bool undo);
    }
  }
