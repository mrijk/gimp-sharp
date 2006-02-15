// The Ministeck plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// MinisteckPalette.cs
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

namespace Gimp.Ministeck
{
  public class MinisteckPalette
  {
    Palette _palette;

    public MinisteckPalette()
    {
      _palette = new Palette("Ministeck");
#if true
      _palette.AddEntry("first", new RGB(253, 254, 253));
      _palette.AddEntry("", new RGB(206, 153,  50));
      _palette.AddEntry("", new RGB(155, 101,  52));
      _palette.AddEntry("", new RGB( 50,  50,  50));
      _palette.AddEntry("", new RGB(  4,   3,  98));
      _palette.AddEntry("", new RGB(  2, 102,  54));
      
      _palette.AddEntry("", new RGB(  2,  50, 154));
      _palette.AddEntry("", new RGB(254,  50, 102));
      _palette.AddEntry("", new RGB(206, 154, 102));
      _palette.AddEntry("", new RGB(254, 254,  50));
      _palette.AddEntry("", new RGB(250,  90,   6));
      _palette.AddEntry("", new RGB( 55, 101,  53));
      
      _palette.AddEntry("", new RGB(103, 102, 101));
      _palette.AddEntry("", new RGB(206,   2,  50));
      _palette.AddEntry("", new RGB(254, 154,  54));
      _palette.AddEntry("", new RGB(102,  50,  50));
      _palette.AddEntry("", new RGB(253, 154, 154));
      _palette.AddEntry("", new RGB( 50, 102, 206));
      
      _palette.AddEntry("", new RGB(  3,  50,  56));
      _palette.AddEntry("", new RGB( 50,   2, 102));
      _palette.AddEntry("", new RGB(251, 155, 101));
      _palette.AddEntry("", new RGB(254, 254, 202));
      _palette.AddEntry("", new RGB(  4,   2,   2));
      _palette.AddEntry("last", new RGB(206, 206, 206));
#else
      _palette.AddEntry("black"     , new RGB(  7,   7,   7));
      _palette.AddEntry("dark blue" , new RGB( 64,  10, 121));
      _palette.AddEntry("light blue", new RGB( 59, 138, 207));
      _palette.AddEntry("light blue", new RGB( 59, 138, 207));
#endif 
    }

    public void Delete()
    {
      _palette.Delete();
    }
  }
}
