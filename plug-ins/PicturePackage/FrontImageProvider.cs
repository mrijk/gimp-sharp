using System;

namespace Gimp.PicturePackage
{
  public class FrontImageProvider : ImageProvider
  {
    Image _image;

    public FrontImageProvider(Image image)
    {
      _image = image;
    }

    override public Image GetImage()
    {
      return _image;
    }
  }
  }
