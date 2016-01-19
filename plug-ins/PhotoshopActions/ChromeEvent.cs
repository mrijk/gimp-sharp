// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// ChromeEvent.cs
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

using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class ChromeEvent : ActionEvent
  {
    [Parameter("Dtl")]
    int _detail;
    [Parameter("Smth")]
    int _smoothness;

    public override bool IsExecutable => false;

    protected override IEnumerable ListParameters()
    {
      yield return "Detail: " + _detail;
      yield return "Smoothness: " + _smoothness;
    }

    override public bool Execute() => false;
  }
}
