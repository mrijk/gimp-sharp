using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
  public class Tile
    {
    IntPtr _tile;

    public Tile(IntPtr tile)
      {
      _tile = tile;
      }

    public void Ref()
      {
      gimp_tile_ref(_tile);
      }

    public void RefZero()
      {
      gimp_tile_ref_zero(_tile);
      }

    public void Unref(bool dirty)
      {
      gimp_tile_unref(_tile, dirty);
      }

    public void Flush()
      {
      gimp_tile_flush(_tile);
      }

    static public void CacheSize(ulong kilobytes)
      {
      gimp_tile_cache_size(kilobytes);
      }

    static public void CacheNtiles(ulong ntiles)
      {
      gimp_tile_cache_ntiles(ntiles);
      }

    [DllImport("libgimp-2.0.so")]
    static extern void gimp_tile_ref(IntPtr tile);
    [DllImport("libgimp-2.0.so")]
    static extern void gimp_tile_ref_zero(IntPtr tile);
    [DllImport("libgimp-2.0.so")]
    static extern void gimp_tile_unref(IntPtr tile,
                                       bool dirty);
    [DllImport("libgimp-2.0.so")]
    static extern void gimp_tile_flush(IntPtr tile);
    [DllImport("libgimp-2.0.so")]
    static extern void gimp_tile_cache_size(ulong kilobytes);
    [DllImport("libgimp-2.0.so")]
    static extern void gimp_tile_cache_ntiles(ulong ntiles);
    }
  }
