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

    public Rectangle Find(double x, double y)
    {
      foreach (Rectangle rectangle in _set)
	{
	if (rectangle.Inside(x, y))
	  return rectangle;
	}
      return null;
    }

    public void Render(ProviderFactory factory, Renderer renderer)
    {
      factory.Reset();
      foreach (Rectangle rectangle in _set)
	{
	ImageProvider provider = rectangle.Provider;

	if (provider == null)
	  {
	  provider = factory.Provide();
	  if (provider == null)
	    {
	    break;
	    }
	  Image image = provider.GetImage();
	  if (image == null)
	    {
	    Console.WriteLine("Couldn't load image!");
	    }
	  else
	    {
	    rectangle.Render(image, renderer);
	    }
	  factory.Cleanup(provider);
	  }
	else
	  {
	  rectangle.Render(provider.GetImage(), renderer);
	  provider.Release();
	  }
	}
      factory.Cleanup();
      renderer.Cleanup();
    }

    public int Count
    {
      get {return _set.Count;}
    }
  }
  }
