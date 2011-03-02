// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// ParasiteList.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Gimp
{
  public sealed class ParasiteList
  {
    readonly List<Parasite> _list;

    public ParasiteList()
    {
    }

    internal delegate bool GetParasitesFunc(Int32 ID, 
					    out int num_parasites,
					    out IntPtr parasties);
    internal delegate IntPtr ParasiteFindFunc(Int32 ID, string name);

    internal ParasiteList(Int32 ID, GetParasitesFunc getParasites,
			  ParasiteFindFunc parasiteFind)
    {
      int numParasites;
      IntPtr parasites;
      if (getParasites(ID, out numParasites, out parasites))
	{
	  _list = Util.ToList<Parasite>(parasites, numParasites,
					(s) => ParasiteFind(ID, parasiteFind,
							    s));
	}
      else
	{
	  throw new GimpSharpException();
	}
    }

    Parasite ParasiteFind(Int32 ID, ParasiteFindFunc parasiteFind, string name)
    {
      IntPtr found = parasiteFind(ID, name);
      return (found == IntPtr.Zero) ? null : new Parasite(found);
    }

    public IEnumerator<Parasite> GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    public void Add(Parasite parasite)
    {
      _list.Add(parasite);
    }

    public void ForEach(Action<Parasite> action)
    {
      _list.ForEach(action);
    }

    public int Count
    {
      get {return _list.Count;}
    }

    public Parasite this[int index]
    {
      get {return _list[index];}
    }
  }
}
