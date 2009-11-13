// The Forge plug-in
// Copyright (C) 2006-2009 Maurits Rijk
//
// Mesh.cs
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
  class Mesh
  {
    readonly double[] _data;
    readonly int _meshsize;

    double _rmin, _rmax;

    public Mesh(double[] data, int meshsize)
    {
      _data = data;
      _meshsize = meshsize;
    }

    public void ApplyPowerLawScaling(double powscale)
    {
      for (int i = 0; i < _meshsize; i++) 
	{
	  for (int j = 0; j < _meshsize; j++) 
	    {
	      double r = GetReal(i, j);
	      
	      if (r > 0) 
		{
		  SetReal(i, j, Math.Pow(r, powscale));
		}
	    }
	}
    }

    public void AutoScale()
    {
      ComputeExtrema();
      Rescale();
    }

    void ComputeExtrema()
    {
      _rmin = double.MaxValue;
      _rmax = double.MinValue;
      
      for (int i = 0; i < _meshsize; i++) 
	{
	  for (int j = 0; j < _meshsize; j++) 
	    {
	      double r = GetReal(i, j);
	      
	      _rmin = Math.Min(_rmin, r);
	      _rmax = Math.Max(_rmax, r);
	    }
	}
    }

    void Rescale()
    {
      double rmean = (_rmin + _rmax) / 2;
      double rrange = (_rmax - _rmin) / 2;
      
      for (int i = 0; i < _meshsize; i++) 
	{
	  for (int j = 0; j < _meshsize; j++) 
	    {
	      SetReal(i, j, (GetReal(i, j) - rmean) / rrange);
	    }
	}
    }

    public double GetReal(int i, int j)
    {
      return _data[1 + (i * _meshsize + j) * 2];
    }

    void SetReal(int i, int j, double r)
    {
      _data[1 + (i * _meshsize + j) * 2] = r;
    }
  }
}
