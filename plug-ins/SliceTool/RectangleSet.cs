using System;
using System.Collections;

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

    public void WriteHTML()
    {
      _set.Sort();

      Console.WriteLine("<tr>");
      Rectangle prev = this[0];
      prev.WriteHTML(0);
      for (int i = 1; i < _set.Count; i++)
	{
	if (this[i].Top.Index != prev.Top.Index)
	  {
	  Console.WriteLine("/<tr>");
	  Console.WriteLine("");
	  Console.WriteLine("<tr>");
	  }
	prev = this[i];
	prev.WriteHTML(i);
	}
      Console.WriteLine("</tr>");
    }
  }
  }
