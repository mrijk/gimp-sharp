// The Splitter plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// Splitter.cs
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

namespace Gimp.Splitter
{
  public class Splitter : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<string>("formula", _("Formula for splitting image"), ""),
	new Variable<int>("translate_1_x", _("Translation in x first layer"), 0),
	new Variable<int>("translate_1_y", _("Translation in y first layer"), 0),
	new Variable<int>("rotate_1", _("Rotation first layer"), 0),
	new Variable<int>("translate_2_x", _("Translation in x second layer"), 0),
	new Variable<int>("translate_2_y", _("Translation in y second layer"), 0),
	new Variable<int>("rotate_2", _("Rotation second layer"), 0),
	new Variable<int>("keep_layer", 
			  _("Keep first (0), second (1) or both (2) layer(s)"), 0),
	new Variable<bool>("merge", _("Merge layers after splitting"), true),
	new Variable<UInt32>("seed", _("Value for random seed"), 0),
	new Variable<bool>("random_seed", _("Use specified random seed"), false)
      };
      GimpMain<Splitter>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_splitter",
			   _("Splits an image."),
			   _("Splits an image in separate parts using a formula of the form f(x, y) = 0"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "1999 - 2016",
			   _("Splitter..."),
			   "RGB*",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Image>/Filters/Generic",
	  IconFile = "Splitter.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("splitter", true);
      return new Dialog(Variables);
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(image, drawable);
    }
  }
}
