using System;

using Gdk;

namespace Gimp.PicturePackage
{
  abstract public class Renderer
  {
    public Renderer()
    {
    }
    
    abstract public void Render(Image image, double x, double y, 
				double w, double h);
  }
  }
