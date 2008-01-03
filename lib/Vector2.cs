// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// Vector2.cs
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

using System.Runtime.InteropServices;

namespace Gimp
{
  public class Vector2
  {
    GimpVector2 _vector;

    public Vector2(double x, double y)
    {
      _vector = gimp_vector2_new(x, y);
    }

    public Vector2() : this(0.0, 0.0)
    {
    }

    Vector2(Vector2 vector) : this(vector.X, vector.Y)
    {
    }

    public void Set(double x, double y)
    {
      gimp_vector2_set(ref _vector, x, y);
    }

    public double Length
    {
      get {return gimp_vector2_length(ref _vector);}
    }

    public double X
    {
      get {return _vector.x;}
      set {_vector.x = value;}
    }

    public double Y
    {
      get {return _vector.y;}
      set {_vector.y = value;}
    }

    public override bool Equals(object o)
    {
      if (o is Vector2)
	{
	  Vector2 vector = o as Vector2;
	  return vector.X == X && vector.Y == Y;
	}
      return false;
    }

    public override int GetHashCode()
    {
      return X.GetHashCode() + Y.GetHashCode();
    }

    public void Mul(double factor)
    {
      gimp_vector2_mul(ref _vector, factor);
    }

    public void Normalize()
    {
      gimp_vector2_normalize(ref _vector);
    }

    public void Neg()
    {
      gimp_vector2_neg(ref _vector);
    }

    public Vector2 Add(Vector2 vector)
    {
      gimp_vector2_add(ref _vector, ref _vector, ref vector._vector);
      return this;
    }

    public Vector2 Sub(Vector2 vector)
    {
      gimp_vector2_sub(ref _vector, ref _vector, ref vector._vector);
      return this;
    }

    public static Vector2 operator + (Vector2 v1, Vector2 v2)
    {
      return (new Vector2(v1)).Add(v2);
    }

    public static Vector2 operator - (Vector2 v1, Vector2 v2)
    {
      return (new Vector2(v1)).Sub(v2);
    }

    public static Vector2 operator - (Vector2 vector)
    {
      Vector2 v = new Vector2(vector);
      v.Neg();
      return v;
    }

    public static Vector2 operator * (Vector2 vector, double factor)
    {
      Vector2 v = new Vector2(vector);
      v.Mul(factor);
      return v;
    }

    public double InnerProduct(Vector2 vector)
    {
      return gimp_vector2_inner_product(ref _vector, ref vector._vector);
    }

    public double CrossProduct(Vector2 vector)
    {
      return gimp_vector2_cross_product(ref _vector, ref vector._vector);
    }

    public void Rotate(double alpha, double beta, double gamma)
    {
      gimp_vector2_rotate(ref _vector, alpha, beta, gamma);
    }

    public override string ToString()
    {
      return string.Format("({0}, {1})", X, Y);
    }

    [DllImport("libgimpmath-2.0-0.dll")]
    static extern GimpVector2 gimp_vector2_new(double x,
					       double y);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector2_set(ref GimpVector2 vector,
					double x,
					double y);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern double gimp_vector2_length(ref GimpVector2 vector);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector2_mul(ref GimpVector2 vector,
					double factor);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector2_normalize(ref GimpVector2 vector);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector2_neg(ref GimpVector2 vector);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector2_add(ref GimpVector2 result,
					ref GimpVector2 vector1,
					ref GimpVector2 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector2_sub(ref GimpVector2 result,
					ref GimpVector2 vector1,
					ref GimpVector2 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern double gimp_vector2_inner_product(ref GimpVector2 vector1,
						    ref GimpVector2 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern double gimp_vector2_cross_product(ref GimpVector2 vector1,
						    ref GimpVector2 vector2);
    [DllImport("libgimpmath-2.0-0.dll")]
    static extern void gimp_vector2_rotate(ref GimpVector2 vector,
					   double alpha,
					   double beta,
					   double gamma);
  }

  [StructLayout(LayoutKind.Sequential)]
  internal struct GimpVector2
  {
    public double x, y;
  }
}
