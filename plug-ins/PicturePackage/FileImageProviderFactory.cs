using System;

namespace Gimp.PicturePackage
{
  public class FileImageProviderFactory : ProviderFactory
  {
    ImageProvider _provider;

    public FileImageProviderFactory(string filename)
    {
      _provider = new FileImageProvider(filename);
    }

    public override ImageProvider Provide()
    {
      return _provider;
    }

    public override void Cleanup()
    {
      Console.WriteLine("Cleanup: " + _provider);
      _provider.Release();
    }
  }
  }
