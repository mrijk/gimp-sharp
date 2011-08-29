// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// GimpComboBox.cs
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

using Gtk;

namespace Gimp
{
  public class GimpComboBox : ComboBox
  {
    public GimpComboBox(Variable<int> variable, string[] entries) : 
      this((new ComboBox(entries)).Handle, variable)
    {
    }

    GimpComboBox(IntPtr handle, Variable<int> variable) : base(handle)
    {
      Active = variable.Value;

      Changed += delegate {variable.Value = Active;};
      variable.ValueChanged += delegate {Active = variable.Value;};
    }

    public static ComboBox NewText(Variable<int> variable)
    {
      return new GimpComboBox(ComboBox.NewText().Handle, variable);
    }
  }
}
