// The Forge plug-in
// Copyright (C) 2006-2016 Maurits Rijk (maurits.rijk@gmail.com)
//
// Complex.cs
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

using static System.Math;

namespace Gimp.Forge
{
  struct Complex
  {
    public double Real {get; set;}
    public double Imag {get; set;}

    public Complex(double real, double imag) : this()
    {
      Real = real;
      Imag = imag;
    }

    static public Complex FromRadiusPhase(double radius, double phase) =>
      new Complex(radius * Cos(phase), radius * Sin(phase));

    public Complex Conjugate => new Complex(Real, -Imag);
  }
}
