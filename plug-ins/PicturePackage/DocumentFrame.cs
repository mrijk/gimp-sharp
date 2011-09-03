// The PicturePackage plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// DocumentFrame.cs
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

namespace Gimp.PicturePackage
{
  public class DocumentFrame : PicturePackageFrame
  {
    ComboBox _size;
    ComboBox _layout;

    LayoutSet _layoutSet;
    PageSizeSet _sizes;

    public DocumentFrame(VariableSet variables, LayoutSet layoutSet) : 
      base(5, 3, "Document")
    {
      _layoutSet = layoutSet;

      CreatePageSizeWidget(variables, layoutSet);
      CreateLayoutWidget(layoutSet);
      CreateResolutionWidget(variables);
      CreateUnitsWidget(variables);
      CreateColorModeWidget(variables);
      CreateFlattenWidget(variables);
    }

    void CreatePageSizeWidget(VariableSet variables, LayoutSet layoutSet)
    {
      int resolution = variables.GetValue<int>("resolution");

      _size = ComboBox.NewText();
      FillPageSizeMenu(layoutSet, resolution);
      _size.Changed += delegate
	{
	  _layoutSet = layoutSet.GetLayouts(_sizes[_size.Active], resolution);
	  FillLayoutMenu(_layoutSet);
	};
      AttachAligned(0, 0, _("_Page Size:"), 0.0, 0.5, _size, 2, false);
    }

    void FillPageSizeMenu(LayoutSet layoutSet, int resolution)
    {
      (_size.Model as ListStore).Clear();

      _sizes = layoutSet.GetPageSizeSet(resolution);
      _sizes.ForEach(size => AppendPageSizeEntry(size));
      _size.Active = 0;
    }

    void CreateLayoutWidget(LayoutSet layoutSet)
    {
      _layout = ComboBox.NewText();
      FillLayoutMenu(_layoutSet);
      _layout.Changed += delegate
	{
	  if (_layout.Active >= 0)
	  {
	    layoutSet.Selected = _layoutSet[_layout.Active];
	  }
	};
      AttachAligned(0, 1, _("_Layout:"), 0.0, 0.5, _layout, 2, false);
    }

    void CreateResolutionWidget(VariableSet variables)
    {
      var resolution = variables.Get<int>("resolution");
      var button = new GimpSpinButton (0, 1200, 1, resolution);
      AttachAligned(0, 2, _("_Resolution:"), 0.0, 0.5, button, 1, true);
    }

    void CreateUnitsWidget(VariableSet variables)
    {
      Attach(new GimpComboBox(variables.Get<int>("resolution"),
			      new string[]{"pixels/inch", "pixels/cm", 
					   "pixels/mm"}), 2, 3, 2, 3);	
    }

    void CreateColorModeWidget(VariableSet variables)
    {
      var mode = new GimpComboBox(variables.Get<int>("color_mode"),
				  new string[]{_("Grayscale"), _("RGB Color")});
      AttachAligned(0, 3, _("_Mode:"), 0.0, 0.5, mode, 2, false);
    }

    void CreateFlattenWidget(VariableSet variables)
    {
      var flatten = variables.Get<bool>("flatten");
      Attach(new GimpCheckButton(_("Flatten All Layers"), flatten), 0, 2, 4, 5);
    }

    void AppendPageSizeEntry(PageSize size)
    {
      _size.AppendText(String.Format("{0,1:f1} x {1,1:f1} inches", 
				     size.Width, size.Height));
    }

    void FillLayoutMenu(LayoutSet layoutSet)
    {
      (_layout.Model as ListStore).Clear();
      layoutSet.ForEach(layout => _layout.AppendText(layout.Name));
      _layout.Active = 0;
    }
  }
}
