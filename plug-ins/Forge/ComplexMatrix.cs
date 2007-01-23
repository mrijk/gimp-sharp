// The Forge plug-in
// Copyright (C) 2006-2007 Massimo Perga (massimo.perga@gmail.com)
//
// ComplexMatrix.cs
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
  class ComplexMatrix
  {
    readonly int _size;
    readonly Complex[,] _matrix;

    public ComplexMatrix(int size)
    {
      _size = size;
      _matrix = new Complex[size, size];
    }

    public Complex this[int row, int col]
    {
      get {return _matrix[row, col];}
      set {_matrix[row, col] = value;}
    }
    
    public double[] ToFlatArray()
    {
      double[] array = new double[(1 + _size * _size) * 2];

      int i = 2;	// Skip first 2!
      for (int row = 0; row < _size; row++)
	{
	  for (int col = 0; col < _size; col++)
	    {
	      Complex c = _matrix[row, col];
	      array[i++] = c.Real;
	      array[i++] = c.Imag;
	    }
	}

      return array;
    }
  }
}
