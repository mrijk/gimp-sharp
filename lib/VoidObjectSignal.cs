// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// VoidObjectSignal.cs
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

namespace Gimp
  {
  using System;
  using System.Runtime.InteropServices;

    internal delegate void voidObjectDelegate(IntPtr arg0, int key);

    internal class voidObjectSignal : GLib.SignalCallback 
    {

      private static voidObjectDelegate _Delegate;

      private static void voidObjectCallback(IntPtr arg0, int key)
      {
	if (!_Instances.Contains(key))
	  throw new Exception("Unexpected signal key " + key);

	voidObjectSignal inst = (voidObjectSignal) _Instances[key];
	EventHandler h = (EventHandler) inst._handler;
	h (inst._obj, new EventArgs ());
      }

      public voidObjectSignal(GLib.Object obj, string name, Delegate eh, 
			      Type argstype, int connect_flags) : 
	base(obj, eh, argstype)
      {
	if (_Delegate == null) 
	  {
	  _Delegate = new voidObjectDelegate(voidObjectCallback);
	  }
	Connect (name, _Delegate, connect_flags);
      }

      protected override void Dispose (bool disposing)
      {
	_Instances.Remove(_key);
	if(_Instances.Count == 0)
	  _Delegate = null;

	Disconnect ();
	base.Dispose (disposing);
      }
    }
  }
