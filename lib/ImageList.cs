using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class ImageList : IEnumerable
    {
      ArrayList _list = new ArrayList();

      public ImageList()
      {
	int num_images;
	IntPtr list = gimp_image_list(out num_images);

	int[] dest = new int[num_images];
	Marshal.Copy(list, dest, 0, num_images);

	foreach (int imageID in dest)
	  {
	  _list.Add(new Image(imageID));
	  }
      }

      public virtual IEnumerator GetEnumerator()
      {
	return _list.GetEnumerator();
      }

      [DllImport("libgimp-2.0.so")]
      static extern IntPtr gimp_image_list(out int num_images);
    }
  }
