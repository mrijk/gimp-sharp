// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// Gradient.cs
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
  public class Gradient
    {
    string _name;

    public Gradient(string name)
      {
      _name = gimp_gradient_new(name);
      }

    internal Gradient(string name, bool unused)
      {
      _name = name;
      }

    public Gradient(Gradient gradient)
      {
      _name = gimp_gradient_duplicate(gradient._name);
      }

    public string Rename(string new_name)
      {
      _name = gimp_gradient_rename(_name, new_name);
      return _name;
      }

    public string Name
      {
      get {return _name;}
      set {Rename(value);}
      }

    public void Delete()
      {
      if (!gimp_gradient_delete(_name))
        {
        throw new Exception();
        }
      }

    public bool Editable
      {
      get {return gimp_gradient_is_editable(_name);}
      }

    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_gradient_new(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_gradient_duplicate(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_gradient_rename(string name, string new_name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_delete(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_is_editable(string name);
    }
  }
