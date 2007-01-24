// The Forge plug-in
// Copyright (C) 2006-2007 Massimo Perga (massimo.perga@gmail.com)
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

using System;

namespace Gimp.Forge
{
  struct Complex
  {
    double _real;
    double _imag;

    public Complex(double real, double imag)
    {
      _real = real;
      _imag = imag;
    }

    static public Complex FromRadiusPhase(double radius, double phase)
    {
      return new Complex(radius * Math.Cos(phase), radius * Math.Sin(phase));
    }

    public double Real
    {
      get {return _real;}
      set {_real = value;}
    }

    public double Imag
    {
      get {return _imag;}
      set {_imag = value;}
    }

    public Complex Conjugate
    {
      get {return new Complex(_real, -_imag);}
    }
  }
}
