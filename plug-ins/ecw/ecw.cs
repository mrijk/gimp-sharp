// The ecw plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ecw.cs
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
  class ecw : FilePlugin
  {
    [STAThread]
    static void Main(string[] args)
    {
      new ecw(args);
    }

    public ecw(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      set.Add(FileLoadProcedure("file_ecw_load",
				"Loads ecw images",
				"This plug-in loads ECW images.",
				"Maurits Rijk",
				"(C) Maurits Rijk",
				"2006",
				"ecw Image"));

      set.Add(FileSaveProcedure("file_ecw_save",
				"Saves ecw images",
				"This plug-in saves ECW images.",
				"Maurits Rijk",
				"(C) Maurits Rijk",
				"2006",
				"ecw Image",
				"RGB*"));
      return set;
    }

    override protected void Query()
    {
      base.Query();
      RegisterLoadHandler("ecw", "");
      RegisterSaveHandler("ecw", "");
    }

    override protected Image Load(string filename)
    {
      if (File.Exists(filename))
	{
	  BinaryReader reader = new BinaryReader(File.Open(filename, 
							   FileMode.Open));

	  ecw_wrapper_init();

	  IntPtr fileView;
	  int error = NCScbmOpenFileView(filename, out fileView, IntPtr.Zero);
	  
	  IntPtr foo = NCSGetErrorText(error);
	  Console.WriteLine(Marshal.PtrToStringAuto(foo));
	  
	  IntPtr tmp;
	  error = NCScbmGetViewFileInfo(fileView, out tmp);
	  // Console.WriteLine(NCSGetErrorText(error));
	  
	  NCSFileViewFileInfo info = (NCSFileViewFileInfo) Marshal.PtrToStructure(tmp, typeof(NCSFileViewFileInfo));
	  Console.WriteLine("SizeX: {0}, SizeY: {1}, Bands: {2}, CompressionRate: {3}, Datum: {4}",
			    info.SizeX, info.SizeY, info.Bands, info.CompressionRate, info.Datum);
	  
	  error = NCScbmCloseFileView(fileView);
	  // Console.WriteLine(NCSGetErrorText(error));
	  
	  reader.Close();

	  return null;	// image;
	}
      return null;
    }

    override protected bool Save(Image image, Drawable drawable, 
				 string filename)
    {
      BinaryWriter writer = new BinaryWriter(File.Open(filename, 
						       FileMode.Create));
      writer.Close();

      return true;
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

    // TODO: fix mappings from .so to .dll
  [DllImport("ecwwrapper.so")]
  static extern void ecw_wrapper_init();

  [DllImport("NCSEcw.so")]
  static extern int NCScbmOpenFileView(string Path, out IntPtr FileView, 
				       IntPtr RefreshCallback);
  [DllImport("libNCSEcw.so")]
  static extern int NCScbmCloseFileView(IntPtr FileView);
  [DllImport("libNCSEcw.so")]
  static extern int NCScbmGetViewFileInfo(IntPtr FileView, 
					  out IntPtr NCSFileViewFileInfo);
  [DllImport("libNCSUtil.so")]
  static extern IntPtr NCSGetErrorText(int error);
  }
}
