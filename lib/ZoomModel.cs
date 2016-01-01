// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2016 Maurits Rijk
//
// ZoomModel.cs
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

using Gtk;

namespace Gimp
{
  public enum ZoomType
  {
    In,
    Out,
    InMore,
    OutMore,
    InMax,
    OutMax,
    To 
  }

  public class ZoomModel
  {
    readonly IntPtr _model;
    internal IntPtr Ptr => _model;

    public double Factor => gimp_zoom_model_get_factor(_model);

    public ZoomModel()
    {
      _model = gimp_zoom_model_new();
    }

    internal ZoomModel(IntPtr model)
    {
      _model = model;
    }

    public void SetRange(double min, double max)
    {
      gimp_zoom_model_set_range(_model, min, max);
    }

    public void Zoom(ZoomType zoomType, double scale)
    {
      gimp_zoom_model_zoom(_model, zoomType, scale);
    }

    public void GetFraction(out int numerator, out int denominator)
    {
      gimp_zoom_model_get_fraction(_model, out numerator, out denominator);
    }

    // Fix me: implement ZoomButton. Probably in separate class

    public static double ZoomStep(ZoomType zoomType, double scale)
    {
      return gimp_zoom_model_zoom_step(zoomType, scale);
    }

    [DllImport("libgimpui-2.0-0.dll")]
      extern static IntPtr gimp_zoom_model_new();
    [DllImport("libgimpui-2.0-0.dll")]
      extern static void gimp_zoom_model_set_range(IntPtr model,
						   double min,
						   double max);
    [DllImport("libgimpui-2.0-0.dll")]
      extern static void gimp_zoom_model_zoom(IntPtr model,
					      ZoomType zoom_type,
					      double scale);
    [DllImport("libgimpui-2.0-0.dll")]
      extern static double gimp_zoom_model_get_factor(IntPtr model);
    [DllImport("libgimpui-2.0-0.dll")]
      extern static void gimp_zoom_model_get_fraction(IntPtr model,
						  out int numerator,
						  out int denominator);
    [DllImport("libgimpui-2.0-0.dll")]
      extern static IntPtr gimp_zoom_button_new(IntPtr model,
						ZoomType zoom_type,
						IconSize icon_size);
    [DllImport("libgimpui-2.0-0.dll")]
      extern static double gimp_zoom_model_zoom_step(ZoomType zoom_type,
						     double scale);
  }
}
