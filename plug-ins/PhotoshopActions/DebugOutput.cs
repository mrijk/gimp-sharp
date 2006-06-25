// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// DebugOutput.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;

namespace Gimp.PhotoshopActions
{
  public class DebugOutput
  {
    static int _level;
    static bool _quiet;

    public static void Dump(string format, params object[] list)
    {
      if (!_quiet)
	{
	  for (int i = 0; i < _level; i++)
	    {
	      Console.Write("\t");
	    }
	  Console.WriteLine(format, list);
	}
    }

    public static bool Quiet
    {
      set {_quiet = value;}
    }

    public static int Level
    {
      get {return _level;}
      set {_level = value;}
    }
  }
}
