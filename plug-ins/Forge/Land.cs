// The Forge plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// Land.cs
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

namespace Gimp.Forge
{
  class Land
  {
    static byte [,] _pgnd = new byte[99,3] {
      {206, 205, 0}, {208, 207, 0}, {211, 208, 0},
      {214, 208, 0}, {217, 208, 0}, {220, 208, 0},
      {222, 207, 0}, {225, 205, 0}, {227, 204, 0},
      {229, 202, 0}, {231, 199, 0}, {232, 197, 0},
      {233, 194, 0}, {234, 191, 0}, {234, 188, 0},
      {233, 185, 0}, {232, 183, 0}, {231, 180, 0},
      {229, 178, 0}, {227, 176, 0}, {225, 174, 0},
      {223, 172, 0}, {221, 170, 0}, {219, 168, 0},
      {216, 166, 0}, {214, 164, 0}, {212, 162, 0},
      {210, 161, 0}, {207, 159, 0}, {205, 157, 0},
      {203, 156, 0}, {200, 154, 0}, {198, 152, 0},
      {195, 151, 0}, {193, 149, 0}, {190, 148, 0},
      {188, 147, 0}, {185, 145, 0}, {183, 144, 0},
      {180, 143, 0}, {177, 141, 0}, {175, 140, 0},
      {172, 139, 0}, {169, 138, 0}, {167, 137, 0},
      {164, 136, 0}, {161, 135, 0}, {158, 134, 0},
      {156, 133, 0}, {153, 132, 0}, {150, 132, 0},
      {147, 131, 0}, {145, 130, 0}, {142, 130, 0},
      {139, 129, 0}, {136, 128, 0}, {133, 128, 0},
      {130, 127, 0}, {127, 127, 0}, {125, 127, 0},
      {122, 127, 0}, {119, 127, 0}, {116, 127, 0},
      {113, 127, 0}, {110, 128, 0}, {107, 128, 0},
      {104, 128, 0}, {102, 127, 0}, { 99, 126, 0},
      { 97, 124, 0}, { 95, 122, 0}, { 93, 120, 0},
      { 92, 117, 0}, { 92, 114, 0}, { 92, 111, 0},
      { 93, 108, 0}, { 94, 106, 0}, { 96, 104, 0},
      { 98, 102, 0}, {100, 100, 0}, {103,  99, 0},
      {106,  99, 0}, {109,  99, 0}, {111, 100, 0},
      {114, 101, 0}, {117, 102, 0}, {120, 103, 0},
      {123, 102, 0}, {125, 102, 0}, {128, 100, 0},
      {130,  98, 0}, {132,  96, 0}, {133,  94, 0},
      {134,  91, 0}, {134,  88, 0}, {134,  85, 0},
      {133,  82, 0}, {131,  80, 0}, {129,  78, 0}
    };

    public static RGB GetLand(double r)
    {
      //#define ELEMENTS(array) (sizeof(array)/sizeof((array)[0]))
      //int ix = ((r - 128) * (ELEMENTS(pgnd) - 1)) / 127;
      int ix = (int)((r - 128) * (99 - 1)) / 127;
      
      /* Land area.  Look up colour based on elevation from
	 precomputed colour map table. */
      byte ir = _pgnd[ix, 0];
      byte ig = _pgnd[ix, 1];
      byte ib = _pgnd[ix, 2];

      return new RGB(ir, ig, ib);
    }
  }
}
