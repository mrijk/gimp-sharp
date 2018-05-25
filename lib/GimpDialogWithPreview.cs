// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
//
// GimpDialog.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;
using Gtk;

namespace Gimp
{
  public abstract class GimpDialogWithPreview : GimpDialog 
  {
    protected Drawable Drawable {get;}
    protected GimpPreview Preview {get;}
    protected VBox Vbox {get;}

    public GimpDialogWithPreview(string title, Drawable drawable, 
				 VariableSet variables, 
				 Func<GimpPreview> factory) : 
      base(title, variables)
    {
      Drawable = drawable;

      Vbox = new VBox(false, 0) {BorderWidth = 12};
      VBox.PackStart(Vbox, true, true, 0);

      Preview = factory();

      Preview.Invalidated += delegate {UpdatePreview(Preview);};

      Vbox.PackStart(Preview, true, true, 0);

      variables.ValueChanged += delegate {InvalidatePreview();};
    }

    protected void InvalidatePreview()
    {
      Preview.Invalidate();
    }

    virtual protected void UpdatePreview(GimpPreview preview) {}
  }
}
