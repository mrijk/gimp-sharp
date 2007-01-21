// The Colorize plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// Ported from http://registry.gimp.org/plugin?id=5479
// copyright 2005 Christopher Lais
//
// UmfPack.cs
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
using System.Runtime.InteropServices;

namespace Gimp.Colorize
{
  class UmfPack
  {
    const int UMFPACK_CONTROL = 20;
    const int UMFPACK_INFO = 90;
    const int UMFPACK_A = 0;

    double[] _control;
    double[] _info;

    IntPtr _symbolic;
    IntPtr _numeric;

    public UmfPack()
    {
      umfpack_wrapper_init();
    }

    public void Defaults()
    {
      _control = new double[UMFPACK_CONTROL];
      _info = new double[UMFPACK_INFO];
      
      umfpack_di_defaults(_control);
    }

    public void TripletToCol(int nRow, int nCol, int nz, int[] Ti, int[] Tj,
			     double[,] Tx, int[] Ap, int[] Ai, double[,] Ax,
			     int[] Map)
    {
      int status = umfpack_di_triplet_to_col(nRow, nCol, nz, Ti, Tj, Tx, Ap, 
					     Ai, Ax, Map);
      if (status != 0)
	{
	  throw new GimpSharpException();
	}
    }

    public void Symbolic(int nRow, int nCol, int[] Ap, int[] Ai, double[,] Ax)
    {
      int status = umfpack_di_symbolic(nRow, nCol, Ap, Ai, Ax, out _symbolic, 
				       _control, _info);
      if (status != 0)
	{
	  throw new GimpSharpException();
	}
    }

    public void FreeSymbolic()
    {
      umfpack_di_free_symbolic(ref _symbolic);
    }

    public void Numeric(int[] Ap, int[] Ai, double[,] Ax)
    {
      int status = umfpack_di_numeric(Ap, Ai, Ax, _symbolic, out _numeric, 
				      _control, _info);
      if (status != 0)
	{
	  throw new GimpSharpException();
	}
    }

    public void FreeNumeric()
    {
      umfpack_di_free_numeric(ref _numeric);
    }


    public void Solve(int[] Ap, int[] Ai, double[,] Ax, double[,] X,
		      double[,] B)
    {
      int status = umfpack_di_solve(UMFPACK_A, Ap, Ai, Ax, X, B, _numeric, 
				    _control, _info);
      if (status != 0)
	{
	  throw new GimpSharpException();
	}
    }

    // TODO: fix mappings from .so to .dll
    [DllImport("umfpackwrapper.so")]
    static extern void umfpack_wrapper_init();

    [DllImport("libumfpack.dll")]
    static extern void umfpack_di_defaults(double[] control);
    [DllImport("libumfpack.dll")]
    static extern int umfpack_di_triplet_to_col(int n_row,
						int n_col,
						int nz,
						int[] Ti,
						int[] Tj,
						double[,] Tx,
						int[] Ap,
						int[] Ai,
						double[,] Ax,
						int[] Map);
    [DllImport("libumfpack.dll")]
    static extern int umfpack_di_symbolic(int n_row,
					  int n_col,
					  int[] Ap,
					  int[] Ai,
					  double[,] Ax,
					  out IntPtr Symbolic,
					  double[] Control,
					  double[] Info);
    [DllImport("libumfpack.dll")]
    static extern int umfpack_di_numeric(int[] Ap,
					 int[] Ai,
					 double[,] Ax,
					 IntPtr Symbolic,
					 out IntPtr Numeric,
					 double[] Control,
					 double[] Info);
    [DllImport("libumfpack.dll")]
    static extern int umfpack_di_solve(int sys,
				       int[] Ap,
				       int[] Ai,
				       double[,] Ax,
				       double[,] X,
				       double[,] B,
				       IntPtr Numeric,
				       double[] Control,
				       double[] Info);
    [DllImport("libumfpack.dll")]
    static extern void umfpack_di_free_symbolic(ref IntPtr Symbolic);
    [DllImport("libumfpack.dll")]
    static extern void umfpack_di_free_numeric(ref IntPtr Numeric);
  }
}
