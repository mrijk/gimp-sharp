// The PicturePackage plug-in
// Copyright (C) 2004-2011 Maurits Rijk, Massimo Perga
//
// PicturePackage.cs
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

namespace Gimp.PicturePackage
{
  public enum DialogStateType
  {
    SrcImgValid,  	 // Source combo, Image selected, No image
    SrcImgInvalid,	 // Source combo, Image selected, With image
    SrcFileValid, 	 // Source combo, File selected, No file
    SrcFileInvalid   // Source combo, File selected, With file 
  };

  public class PicturePackage : Plugin
  {
    LayoutSet _layoutSet = new LayoutSet();
    
    Variable<ProviderFactory> _loader = new Variable<ProviderFactory>(null);
    Variable<Layout> _layout = new Variable<Layout>(null);

    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<bool>("flatten", "", false),
	new Variable<int>("resolution", "", 72),
	new Variable<int>("units", "", 0),
	new Variable<int>("color_mode", "", 1),
	new Variable<string>("label", "", ""),
	new Variable<int>("position", "", 0)
      };
      GimpMain<PicturePackage>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_picture_package",
			   _("Picture package"),
			   _("Picture package"),
			   "Maurits Rijk, Massimo Perga",
			   "Maurits Rijk, Massimo Perga",
			   "2004-2011",
			   _("Picture Package..."),
			   "",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Toolbox>/Xtns/Extensions",
	  IconFile = "PicturePackage.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("PicturePackage", true);
      return new Dialog(Variables, _layoutSet, _layout, _loader);
    }

    override protected void Render()
    {
      var renderer = new Renderer(Variables);
      renderer.Render(_layout.Value, _loader.Value);
    }
  }
}

