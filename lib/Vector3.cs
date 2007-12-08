// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// Vector3.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;
using System.Runtime.InteropServices;

namespace Gimp
{
  public class Vector3
  {
    GimpVector3 _vector;

    public Vector3(double x, double y, double z)
    {
      _vector = gimp_vector3_new(x, y, z);
    }

    Vector3(GimpVector3 vector)
    {
      _vector = vector;
    }

    public void Set(double x, double y, double z)
    {
      gimp_vector3_set(ref _vector, x, y, z);
    }

    public double Length
    {
      get {return gimp_vector3_length(ref _vector);}
    }

    public double X
    {
      get {return _vector.x;}
    }

    public double Y
    {
      get {return _vector.y;}
    }

    public double Z
    {
      get {return _vector.z;}
    }

    public void Mul(double factor)
    {
      gimp_vector3_mul(ref _vector, factor);
    }

    public void Normalize()
    {
      gimp_vector3_normalize(ref _vector);
    }

    public void Neg()
    {
      gimp_vector3_neg(ref _vector);
    }

    public Vector3 Add(Vector3 vector)
    {
      gimp_vector3_add(ref _vector, ref _vector, ref vector._vector);
      return this;
    }

    public Vector3 Sub(Vector3 vector)
    {
      gimp_vector3_sub(ref _vector, ref _vector, ref vector._vector);
      return this;
    }

    public double InnerProduct(Vector3 vector)
    {
      return gimp_vector3_inner_product(ref _vector, ref vector._vector);
    }

    public double CrossProduct(Vector3 vector)
    {
      return gimp_vector3_cross_product(ref _vector, ref vector._vector);
    }

    public void Rotate(double alpha, double beta, double gamma)
    {
      gimp_vector3_rotate(ref _vector, alpha, beta, gamma);
    }

    public override string ToString()
    {
      return string.Format("({0}, {1}, {2})", X, Y, Z);
    }

    [DllImport("libgimpmath-2.0-0.dll")]
    static extern GimpVector3 gimp_vector3_new(double x,
					       double y,
					       double z);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_set(ref GimpVector3 vector,
					double x,
					double y,
					double z);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern double gimp_vector3_length(ref GimpVector3 vector);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_mul(ref GimpVector3 vector,
					double factor);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_normalize(ref GimpVector3 vector);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_neg(ref GimpVector3 vector);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_add(ref GimpVector3 result,
					ref GimpVector3 vector1,
					ref GimpVector3 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_sub(ref GimpVector3 result,
					ref GimpVector3 vector1,
					ref GimpVector3 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern double gimp_vector3_inner_product(ref GimpVector3 vector1,
						    ref GimpVector3 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern double gimp_vector3_cross_product(ref GimpVector3 vector1,
						    ref GimpVector3 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector3_rotate(ref GimpVector3 vector,
					   double alpha,
					   double beta,
					   double gamma);
  }

  [StructLayout(LayoutKind.Sequential)]
  internal struct GimpVector3
  {
    public double x, y, z;
  }
}
