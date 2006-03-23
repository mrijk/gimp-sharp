// The ecw plug-in
// Copyright (C) 2006 Maurits Rijk
//
// FileView.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;
using System.IO;
using System.Runtime.InteropServices;

using IEEE8 = System.Double;

namespace Gimp.ecw
{
  public class FileView
  {
    readonly IntPtr _ptr;

    public FileView(IntPtr ptr)
    {
      _ptr = ptr;
    }

    public static FileView Open(string filename)
    {
      IntPtr ptr;
      int error = NCScbmOpenFileView(filename, out ptr, IntPtr.Zero);
      // Add exception handling here.

      return new FileView(ptr);
    }

    public void Close()
    {
      int error = NCScbmCloseFileView(_ptr);
      // Add exception handling here.
    }

    public void Set(UInt32[] bands, UInt32 TLX, UInt32 TLY, UInt32 BRX,
		    UInt32 BRY, UInt32 sizeX, UInt32 sizeY)
    {
      int  error = NCScbmSetFileView(_ptr, (UInt32) bands.Length, bands, 
				     TLX, TLY, BRX, BRY, sizeX, sizeY);
    }

    public void Set()
    {
      FileViewInfo info = Info;
      UInt32[] bands = new UInt32[]{ 0, 1, 2 };

      Set(bands, 0, 0, info.SizeX - 1, info.SizeY - 1, 
	  info.SizeX - 1, info.SizeY - 1);
    }

    public void ReadLineRGB(byte[] rgb)
    {
      int error = NCScbmReadViewLineRGB(_ptr, rgb);
    }

    public FileViewInfo Info
    {
      get {return new FileViewInfo(_ptr);}
    }

    [DllImport("NCSEcw.so")]
    static extern int NCScbmOpenFileView(string Path, out IntPtr FileView, 
					 IntPtr RefreshCallback);
    [DllImport("libNCSEcw.so")]
    static extern int NCScbmCloseFileView(IntPtr FileView);
    [DllImport("libNCSEcw.so")]
    static extern int NCScbmSetFileView(IntPtr NCSFileView, 
					UInt32 Bands, UInt32[] BandList,
					UInt32 TLX, UInt32 TLY, 
					UInt32 BRX, UInt32 BRY,
					UInt32 SizeX, UInt32 SizeY);
    [DllImport("libNCSEcw.so")]
    static extern int NCScbmReadViewLineRGB(IntPtr NCSFileView, 
					    byte[] RGBTriplets);
  }
}
