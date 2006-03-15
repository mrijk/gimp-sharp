// The SliceTool plug-in
// Copyright (C) 2004-2006 Maurits Rijk
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
    List<Rectangle> _set = new List<Rectangle>();
    Rectangle _selected;
    bool _changed = false;
    
    public RectangleSet()
    {
    }

    public IEnumerator<Rectangle> GetEnumerator()
    {
      return _set.GetEnumerator();
    }
    
    public void Add(Rectangle rectangle)
    {
      _changed = true;
      _set.Add(rectangle);
      if (_selected == null)
	{
	  _selected = rectangle;
	}
    }
    
    public void Remove(Rectangle rectangle)
    {
      _changed = true;
      _set.Remove(rectangle);
    }
    
    public Rectangle this[int index]
    {
      get {return _set[index];}
    }
    
    public void Clear()
    {
      _changed = true;
      _set.Clear();
    }
    
    public Rectangle Selected
    {
      get {return _selected;}
    }
    
    public bool Changed
    {
      get {return _changed;}
      set {_changed = value;}
    }
    
    public void Slice(Slice slice)
    {
      RectangleSet created = new RectangleSet();
      
      foreach (Rectangle rectangle in _set)
	{
	  if (rectangle.IntersectsWith(slice))
	    {
	      created.Add(rectangle.Slice(slice));
	    }
	}
      
      foreach (Rectangle rectangle in created)
	{
	  Add(rectangle);
	}
    }
    
    public Rectangle Find(int x, int y)
    {
      return _set.Find(delegate(Rectangle rectangle) 
      {return rectangle.IsInside(x, y);});
    }
    
    public Rectangle Select(int x, int y)
    {
      _selected = Find(x, y);
      return _selected;
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
      foreach (Rectangle rectangle in _set)
	{
	  rectangle.WriteSlice(image, path, name, useGlobalExtension);
	}
    }
    
    public void Save(StreamWriter w)
    {
      _set.ForEach(delegate(Rectangle rectangle) {rectangle.Save(w);}); 
      _changed = false;
    }
    
    public void Resolve(SliceSet hslices, SliceSet vslices)
    {
      _set.ForEach(delegate(Rectangle rectangle) 
      {rectangle.Resolve(hslices, vslices);});
    }
  }
}
