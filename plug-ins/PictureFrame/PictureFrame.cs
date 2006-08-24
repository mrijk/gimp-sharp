// The PictureFrame plug-in
// Copyright (C) 2006 Oded Coster
//
// PictureFrame.cs
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
using Gtk;
using Mono.Unix;

namespace Gimp.PictureFrame
{
  public class PictureFrame : Plugin
  {
  
  	private string pictureFrameImagePath;
  
    static void Main(string[] args)
    {
      string localeDir = Gimp.LocaleDirectory;
      Catalog.Init("PictureFrame", localeDir);
      new PictureFrame(args);
    }

    public PictureFrame(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      Procedure procedure = new Procedure("plug_in_picture_frame",
          Catalog.GetString("Picture frame"),
          Catalog.GetString("Picture frame"),
          "Oded Coster",
          "(C) Oded Coster",
          "2006",
          Catalog.GetString("Picture frame..."),
          "RGB*, GRAY*");
      procedure.MenuPath = "<Image>/Filters/Render";
      //procedure.IconFile("PictureFrame.png");

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
    	gimp_ui_init("Picture Frame", true);
    	
    	Dialog dialog = DialogNew("Picture Frame", "Picture Frame", IntPtr.Zero, 0,
				Gimp.StandardHelpFunc, "Picture Frame");
		dialog.Modal = false;

		VBox vbox = new VBox(false, 12);
		vbox.BorderWidth = 12;
		dialog.VBox.PackStart(vbox, true, true, 0);
      
    	FileEntry entry = new FileEntry("Load Frame...", "PictureFrame.svg",false, true);
    	entry.FilenameChanged += GetNewFileName;

		vbox.PackStart(entry, false, false, 0);

		dialog.ShowAll();
				
		return DialogRun();

    }
    
    private void GetNewFileName(object sender, EventArgs e)
    {
    	FileEntry frameFile = sender as FileEntry;
    	
    	if(frameFile != null)
    	{
    		pictureFrameImagePath = frameFile.FileName;
    	}
    }
    
    override protected void Render(Image image, Drawable original_drawable)
    {
    
   	  try{

		
	  	Image frame = Image.Load(RunMode.Interactive, pictureFrameImagePath, pictureFrameImagePath);
	    Layer new_layer = new Layer(frame.ActiveLayer, image);
	    
        new_layer.Visible = true;
          
        image.UndoGroupStart();

        image.AddLayer(new_layer, -1); 
        image.ActiveLayer = new_layer;

        image.UndoGroupEnd();
        Display.DisplaysFlush();

		frame.Delete();
	    
	  }
	  catch(Exception ex) {
	
		throw new GimpSharpException();
	  	
	  }
	
    }
  
  }
}
