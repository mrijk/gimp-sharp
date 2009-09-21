// The Sky plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// TMatrix.cs
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

namespace Gimp.Sky
{
  class TMatrix
  {
    double[,] _data = new double[4, 4];

    public TMatrix()
    {
    }

    public TMatrix(double angle, int axis)
    {
      _data[axis - 1, axis - 1] = 1.0;
      _data[3, 3] = 1.0;
      
      int m1 = axis % 3;
      int m2 = (m1 + 1) % 3;
   
      angle *= Math.PI / 180.0;
      double c = Math.Cos(angle);
      double s = Math.Sin(angle);
      
      _data[m1, m1] = c;
      _data[m1, m2] = s;
      _data[m2, m2] = c;
      _data[m2, m1] = -s;
    }

    public Vector3 Transform(Vector3 vector)
    {
      double x = 
	_data[0, 0] * vector.X +
	_data[0, 1] * vector.Y +
	_data[0, 2] * vector.Z +
	_data[0, 3];

      double y = 
	_data[1, 0] * vector.X +
	_data[1, 1] * vector.Y +
	_data[1, 2] * vector.Z +
	_data[1, 3];

      double z = 
	_data[2, 0] * vector.X +
	_data[2, 1] * vector.Y +
	_data[2, 2] * vector.Z +
	_data[2, 3];

      return new Vector3(x, y, z);
    }

    static public TMatrix Multiply(TMatrix in1, TMatrix in2)
    {
      var result = new TMatrix();

      for (int i = 0; i < 4; i++)
	{
	  for(int j = 0; j < 4; j++)
	    {
	      double sum = 0.0;

	      for (int k = 0; k < 4; k++)
		{
		  sum += in1._data[i, k] * in2._data[k, j];
		}
	      result._data[i, j] = sum;
	    }
	}
      return result;
    }

    static public TMatrix Combine(TMatrix in1, TMatrix in2)
    {
      var result = Multiply(in1, in2);

      for (int i = 0; i < 3; i++)
	{
	  result._data[i, 3] = in1._data[i, 3] + in2._data[i, 3];
	}
      return result;
    }
  }
}
