using System;
using System.Collections;
using System.IO;

namespace Gimp.SliceTool
{
  public class RectangleSet: IEnumerable
  {
    ArrayList _set = new ArrayList();
    Rectangle _selected;
    bool _changed = false;

    public RectangleSet()
    {
    }

    public IEnumerator GetEnumerator()
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
      get {return (Rectangle) _set[index];}
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
      foreach (Rectangle rectangle in _set)
	{
	if (rectangle.IsInside(x, y))
	  {
	  return rectangle;
	  }
	}
      return null;
    }

    public Rectangle Select(int x, int y)
    {
      _selected = Find(x, y);
      return _selected;
    }

    public void WriteHTML(StreamWriter w, string name, string extension)
    {
      _set.Sort();

      w.WriteLine("<tr>");
      Rectangle prev = this[0];
      prev.WriteHTML(w, name, extension, 0);
      for (int i = 1; i < _set.Count; i++)
	{
	if (this[i].Top.Index != prev.Top.Index)
	  {
	  w.WriteLine("</tr>");
	  w.WriteLine("");
	  w.WriteLine("<tr>");
	  }
	prev = this[i];
	prev.WriteHTML(w, name, extension, i);
	}
      w.WriteLine("</tr>");
    }

    public void WriteSlices(Image image, string path, string name, 
			    string extension)
    {
      foreach (Rectangle rectangle in _set)
	{
	rectangle.WriteSlice(image, path, name, extension);
	}
    }

    public void Save(StreamWriter w)
    {
      foreach (Rectangle rectangle in _set)
	{
	rectangle.Save(w);
	}
      _changed = false;
    }

    public void Resolve(SliceSet hslices, SliceSet vslices)
    {
      foreach (Rectangle rectangle in _set)
	{
	rectangle.Resolve(hslices, vslices);
	}
    }
  }
  }
