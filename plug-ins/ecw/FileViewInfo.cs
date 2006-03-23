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
  public class FileViewInfo
  {
    NCSFileViewFileInfo _info;

    internal FileViewInfo(IntPtr fileView)
    {
      IntPtr tmp;
      int error = NCScbmGetViewFileInfo(fileView, out tmp);
      
      _info = (NCSFileViewFileInfo) 
	Marshal.PtrToStructure(tmp, typeof(NCSFileViewFileInfo));
    }

    public UInt32 SizeX
    {
      get {return _info.SizeX;}
    }

    public UInt32 SizeY
    {
      get {return _info.SizeY;}
    }

    public UInt32 Bands
    {
      get {return _info.Bands;}
    }

    public UInt16 CompressionRate
    {
      get {return _info.CompressionRate;}
    }

    public string Datum
    {
      get {return _info.Datum;}
    }

    public enum CellSizeUnits
    {
      Invalid = 0,
      Meters = 1,
      Degrees = 2,
      Feet = 3,
      Unknown = 4
    };

    [StructLayout(LayoutKind.Sequential)]
    struct NCSFileViewFileInfo
    {
      public UInt32	SizeX;
      public UInt32	SizeY;			
      public UInt16	Bands;
      public UInt16	CompressionRate;
      public CellSizeUnits CellSizeUnits;
      public IEEE8	CellIncrementX;
      public IEEE8	CellIncrementY;
      public IEEE8	CellOriginX;
      public IEEE8	CellOriginY;
      public string	Datum;
      public string	Projection;
    };

    [DllImport("libNCSEcw.so")]
    static extern int NCScbmGetViewFileInfo(IntPtr FileView, 
					    out IntPtr NCSFileViewFileInfo);
  }
}
