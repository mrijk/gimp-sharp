// The SliceTool plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// RectangleSet.cs
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
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Gimp.SliceTool
{
  public class RectangleSet
  {
    readonly List<Rectangle> _set = new List<Rectangle>();
    public Rectangle Selected {get; private set;}
    public bool Changed {get; set;}
    
    public RectangleSet()
    {
    }

    public IEnumerator<Rectangle> GetEnumerator()
    {
      return _set.GetEnumerator();
    }
    
    public void Add(Rectangle rectangle)
    {
      Changed = true;
      _set.Add(rectangle);
      if (Selected == null)
	{
	  Selected = rectangle;
	}
    }
    
    public void Remove(Rectangle rectangle)
    {
      Changed = true;
      _set.Remove(rectangle);
    }
    
    public Rectangle this[int index]
    {
      get {return _set[index];}
    }
    
    public void ForEach(Action<Rectangle> action)
    {
      _set.ForEach(action);
    }

    public void Clear()
    {
      Changed = true;
      _set.Clear();
    }
    
    public void Slice(Slice slice)
    {
      RectangleSet created = new RectangleSet();

      ForEach(rectangle => 
	{
	  if (rectangle.IntersectsWith(slice))
	  {
	    created.Add(rectangle.Slice(slice));
	  }
	});

      created.ForEach(rectangle => Add(rectangle));
    }
    
    public Rectangle Find(Coordinate<int> c)
    {
      return _set.Find(rectangle => rectangle.IsInside(c));
    }
    
    public Rectangle Select(Coordinate<int> c)
    {
      Selected = Find(c);
      return Selected;
    }
    
    public void WriteHTML(StreamWriter w, string name, bool useGlobalExtension)
    {
      _set.Sort();
      
      w.WriteLine("<tr>");
      Rectangle prev = this[0];
      prev.WriteHTML(w, name, useGlobalExtension, 0);
      for (int i = 1; i < _set.Count; i++)
	{
	  if (this[i].Top.Index != prev.Top.Index)
	    {
	      w.WriteLine("</tr>");
	      w.WriteLine("");
	      w.WriteLine("<tr>");
	    }
	  prev = this[i];
	  prev.WriteHTML(w, name, useGlobalExtension, i);
	}
      w.WriteLine("</tr>");
    }
    
    public void WriteSlices(Image image, string path, string name, 
			    bool useGlobalExtension)
    {
      ForEach(rectangle => rectangle.WriteSlice(image, path, name, 
						useGlobalExtension));
    }
    
    public void Save(StreamWriter w)
    {
      ForEach(rectangle => rectangle.Save(w));
      Changed = false;
    }
    
    public void Resolve(SliceSet hslices, SliceSet vslices)
    {
      ForEach(rectangle => rectangle.Resolve(hslices, vslices));
    }
  }
}
