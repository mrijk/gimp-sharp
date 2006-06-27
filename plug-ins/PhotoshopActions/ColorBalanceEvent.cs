// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ColorBalanceEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class ColorBalanceEvent : ActionEvent
  {
    [Parameter("ShdL")]
    ListParameter _shadows;
    [Parameter("MdtL")]
    ListParameter _midtones;
    [Parameter("HghL")]
    ListParameter _highlights;
    [Parameter("PrsL")]
    bool _preserveLuminosity;

    protected override void FillParameters(TreeStore store, TreeIter iter)
    {
      // TODO: add actual values
      store.AppendValues(iter, "Shadow Levels: ");
      store.AppendValues(iter, "Midtone Levels: ");
      store.AppendValues(iter, "Highlight Levels: ");
      store.AppendValues(iter, ((_preserveLuminosity) ? "With" : "Without") +
			 " Preserve Luminosity");
    }

    void SetBalance(ListParameter parameter, TransferMode transferMode)
    {
      int cyanRed = (parameter[0] as LongParameter).Value;
      int magentaGreen = (parameter[1] as LongParameter).Value;
      int yellowBlue = (parameter[2] as LongParameter).Value;

      if (cyanRed != 0 || magentaGreen != 0 || yellowBlue != 0)
	{
	  ActiveDrawable.ColorBalance(transferMode, _preserveLuminosity,
				      cyanRed, magentaGreen, yellowBlue);
	}
    }

    override public bool Execute()
    {
      SetBalance(_shadows, TransferMode.Shadows);
      SetBalance(_midtones, TransferMode.Midtones);
      SetBalance(_highlights, TransferMode.Highlights);

      return true;
    }
  }
}
