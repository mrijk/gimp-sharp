using System;

namespace Gimp.PicturePackage
{
  abstract public class ImageProvider
  {
    public ImageProvider()
    {
    }

    abstract public Image GetImage();
    virtual public void Release() {}
  }
  }
