// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// AddMaskEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class AddMaskEvent : ActionEvent
  {
    public AddMaskEvent(ActionEvent srcEvent) : base(srcEvent) 
    {
    }

    public override string EventForDisplay => base.EventForDisplay + " mask";

    override public bool Execute()
    {
      // Fix me, this is an ugly construction
      var layer = new Layer(ActiveDrawable);
      if (!layer.HasAlpha)
	{
	  layer.AddAlpha();
	}
      var mask = layer.CreateMask(AddMaskType.White);
      layer.Mask = mask;
      ActiveDrawable = mask;

      return true;
    }
  }
}
