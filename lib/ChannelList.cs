using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gimp
  {
  public class ChannelList : IEnumerable
    {
    ArrayList _list = new ArrayList();

    public ChannelList(Image image)
      {
      int num_channels;
      IntPtr list = gimp_image_get_channels(image.ID, out num_channels);

      int[] dest = new int[num_channels];
      Marshal.Copy(list, dest, 0, num_channels);

      foreach (int ChannelID in dest)
        {
        _list.Add(new Channel(ChannelID));
        }
      }

    public virtual IEnumerator GetEnumerator()
      {
      return _list.GetEnumerator();
      }

    public int Count
      {
      get {return _list.Count;}
      }

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_image_get_channels(Int32 image_ID, 
                                                 out int num_channels);
    }
  }
