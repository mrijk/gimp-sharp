// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// ColorHalftoneEvent.cs
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
  public class ColorHalftoneEvent : ActionEvent
  {
    [Parameter("Rds")]
    int _radius;
    [Parameter("Ang1")]
    int _channel1;
    [Parameter("Ang2")]
    int _channel2;
    [Parameter("Ang3")]
    int _channel3;
    [Parameter("Ang4")]
    int _channel4;

    public override bool IsExecutable
    {
      get {return false;}
    }

    override public bool Execute()
    {
      return false;
    }
  }
}
