using System;
using System.Collections;
using System.IO;

namespace Gimp.SliceTool
{
  public class RectangleSet: IEnumerable
  {
    ArrayList _set = new ArrayList();

    public RectangleSet()
    {
    }

    public IEnumerator GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    public void Add(Rectangle rectangle)
    {
      _set.Add(rectangle);
    }

    Rectangle this[int index]
    {
      get {return (Rectangle) _set[index];}
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
	_set.Add(rectangle);
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

    public void WriteSlices(Image image, string name, string extension)
    {
      foreach (Rectangle rectangle in _set)
	{
	rectangle.WriteSlice(image, name, extension);
	}
    }

    public bool Remove(int x, int y, Slice slice)
    {
      Rectangle found = null;
      bool merged = false;

      foreach (Rectangle rectangle in _set)
	{
	Slice piece = rectangle.Contains(x, y, slice);
	if (piece != null)
	  {
	  if (found == null)
	    {
	    Console.WriteLine("Found first slicepiece");
	    found = rectangle;
	    }
	  else
	    {
	    Console.WriteLine("Found second slicepiece!");
	    if (piece.IsPartOf(found))
	      {
	      Console.WriteLine("... and it's the same!");
	      rectangle.Merge(found);
	      merged = true;
	      break;
	      }
	    }
	  }
	}
      if (merged)
	{
	_set.Remove(found);
	}

      return merged;
    }
  }
  }
