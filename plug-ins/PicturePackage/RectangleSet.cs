using System;
using System.Collections;

namespace Gimp.PicturePackage
{
  public class RectangleSet : IEnumerable
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

    public Rectangle this[int index]
    {
      get {return (Rectangle) _set[index];}
    }

    public Rectangle Find(int x, int y)
    {
      foreach (Rectangle rectangle in _set)
	{
	if (rectangle.Inside(x, y))
	  return rectangle;
	}
      return null;
    }

    public void Render(Image image, Renderer renderer)
    {
      foreach (Rectangle rectangle in _set)
	{
	rectangle.Render(image, renderer);
	}
    }

    public void Render(ImageProvider provider, Renderer renderer)
    {
      foreach (Rectangle rectangle in _set)
	{
	rectangle.Provider = provider;
	rectangle.Render(renderer);
	}
    }

    public void Render(Renderer renderer)
    {
      foreach (Rectangle rectangle in _set)
	{
	rectangle.Render(renderer);
	}
    }

    public int Count
    {
      get {return _set.Count;}
    }
  }
  }
