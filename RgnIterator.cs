using System;
using System.Runtime.InteropServices;

namespace Gimp
{
	public class RgnIterator
	{
		public delegate void IterFuncSrc(byte[] from);
		public delegate void IterFuncSrcDest(byte[] from, ref byte[] to);

		Drawable _drawable;
		int x1, y1, x2, y2;

		public RgnIterator(Drawable drawable)
		{
			_drawable = drawable;
			drawable.MaskBounds(out x1, out y1, out x2, out y2);
		}

		public void Iterate(IterFuncSrc func)
		{
			byte[] from = new Byte[_drawable.Bpp];

			PixelRgn srcPR = new PixelRgn(_drawable, x1, y1, x2 - x1, y2 - y1, false, false);
			for (IntPtr pr = PixelRgn.Register(srcPR); pr != IntPtr.Zero; pr = PixelRgn.Process(pr))
			{
				IntPtr src = srcPR.Data;
				for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
				{
					IntPtr s = src;
					for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
					{
						Marshal.Copy(s, from, 0, srcPR.Bpp);
						func(from);
						s = (IntPtr) ((int) s + srcPR.Bpp);
					}
					src = (IntPtr) ((int) src + srcPR.Rowstride);
				}
			}
		}

		public void Iterate(IterFuncSrcDest func)
		{
			byte[] from = new Byte[_drawable.Bpp];
			byte[] to = new Byte[_drawable.Bpp];

			PixelRgn srcPR = new PixelRgn(_drawable, x1, y1, x2 - x1, y2 - y1, false, false);
			PixelRgn destPR = new PixelRgn(_drawable, x1, y1, x2 - x1, y2 - y1, true, true);

			int bpp = srcPR.Bpp;

			for (IntPtr pr = PixelRgn.Register(srcPR, destPR); pr != IntPtr.Zero; pr = PixelRgn.Process(pr))
			{
				IntPtr src = srcPR.Data;
				IntPtr dest = destPR.Data;

				for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
				{
					IntPtr s = src;
					IntPtr d = dest;

					for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
					{
						Marshal.Copy(s, from, 0, bpp);
						func(from, ref to);
						Marshal.Copy(to, 0, d, bpp);
						s = (IntPtr) ((int) s + bpp);
						d = (IntPtr) ((int) d + bpp);
					}
					src = (IntPtr) ((int) src + srcPR.Rowstride);
					dest = (IntPtr) ((int) dest + srcPR.Rowstride);
				}				
			}
			_drawable.Flush();
			_drawable.MergeShadow(true);
			_drawable.Update(x1, y1, x2 - x1, y2 - y1);
		}
	}
}
