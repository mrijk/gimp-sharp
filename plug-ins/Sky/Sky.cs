// The Sky plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// Sky.cs
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
using System.Collections.Generic;

using Gtk;

namespace Gimp.Sky
{
  class Sky : PluginWithPreview
  {
    [SaveAttribute("tilt")]
    double _tilt;
    [SaveAttribute("rotation")]
    double _rotation;
    [SaveAttribute("seed")]
    int _seed;
    [SaveAttribute("sun_show")]
    bool _sunShow = true;
    [SaveAttribute("sun_x")]
    double _sunX = 0.2;
    [SaveAttribute("sun_y")]
    double _sunY = 0.2;
    [SaveAttribute("time")]
    double _time;
    [SaveAttribute("horizon_color")]
    RGB _horizonColor = new RGB(0.31, 0.35, 0.40);
    [SaveAttribute("sky_color")]
    RGB _skyColor = new RGB(0.01, 0.04, 0.18);
    [SaveAttribute("sun_color")]
    RGB _sunColor = new RGB(0.995, 0.90, 0.83);
    [SaveAttribute("cloud_color")]
    RGB _cloudColor = new RGB(1.0, 1.0, 1.0);
    [SaveAttribute("shadow_color")]
    RGB _shadowColor = new RGB(0, 0, 0);

    double _cameraDistance;
    const double _planetRadius = 6375.0;
    const double _cloudHeight = 5.0;
    int _width, _height;
    TMatrix _transform;
    Vector3 _cameraLocation;
    Perlin3D _clouds;

    RGB _horizonColor2, _skyColor2;

    static void Main(string[] args)
    {
      new Sky(args);
    }

    Sky(string[] args) : base(args, "Sky")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      ParamDefList inParams = new ParamDefList() {
	new ParamDef("tilt", 0.0, typeof(double), 
		     _("Camera tilt angle (0.0 - 90.0)")),
	new ParamDef("rotation", 0.0, typeof(double), 
		     _("Camera rotation angle (0.0 - 90.0)")),
	new ParamDef("seed", -1, typeof(int), 
		     _("Random seed, -1 to use current time")),
	new ParamDef("sun_show", -1, typeof(int), 
		     _("Show sun? (bool)")),
	new ParamDef("sun_x", 0.2, typeof(double), 
		     _("Sun's x coordinate (0.0 - 1.0)")),
	new ParamDef("sun_y", 0.2, typeof(double), 
		     _("Sun's y coordinate (0.0 - 1.0)"))
      };

      yield return new Procedure("plug_in_sky",
				 _("Sky"),
				 _("Sky"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2007",
				 _("Sky..."),
				 "RGB*",
				 inParams)
	{
	  MenuPath = "<Image>/Filters/Media/",
	  IconFile = "Sky.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Sky", true);

      GimpDialog dialog = DialogNew("Sky", "Sky", IntPtr.Zero, 0,
				    Gimp.StandardHelpFunc, "Sky");

      VBox vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      // Sun parameters
      GimpFrame frame = new GimpFrame(_("Sun"));
      vbox.Add(frame);

      GimpTable table = new GimpTable(3, 2, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      frame.Add(table);
      ScaleEntry sunX = new ScaleEntry(table, 0, 1, _("_X:"), 150, 3, 
				       _sunX, 0.0, 1.0, 0.01, 0.1, 2,
				       true, 0, 0, null, null);
      ScaleEntry sunY = new ScaleEntry(table, 0, 2, _("_Y:"), 150, 3, 
				       _sunY, 0.0, 1.0, 0.01, 0.1, 2,
				       true, 0, 0, null, null);
      CheckButton sunShow = new CheckButton(_("_Show sun"));
      table.Attach(sunShow, 0, 2, 3, 4);

      // Camera parameters
      frame = new GimpFrame(_("Camera"));
      vbox.Add(frame);
      table = new GimpTable(2, 1, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      frame.Add(table);
      ScaleEntry rotation = new ScaleEntry(table, 0, 1, _("_Rotation angle:"), 
					   150, 3, _rotation, 0.0, 90.0, 
					   1.0, 8.0, 0, true, 0, 0, 
					   null, null);
      ScaleEntry tilt = new ScaleEntry(table, 0, 2, _("_Tilt angle:"), 
				       150, 3, _tilt, 0.0, 90.0, 1.0, 8.0, 0,
				       true, 0, 0, null, null);

      // Colors
      frame = new GimpFrame(_("Colors"));
      vbox.Add(frame);
      table = new GimpTable(5, 2, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      frame.Add(table);

      GimpColorButton colorButton = new GimpColorButton("", 16, 16, 
							_horizonColor, 
							ColorAreaType.Flat);
      colorButton.Update = true;
      table.AttachAligned(0, 0, _("_Horizon:"), 0.0, 0.5, colorButton, 1, 
			  true);

      colorButton = new GimpColorButton("", 16, 16, _skyColor,
					ColorAreaType.Flat);
      colorButton.Update = true;
      table.AttachAligned(0, 1, _("S_ky:"), 0.0, 0.5, colorButton, 1, 
			  true);

      colorButton = new GimpColorButton("", 16, 16, _sunColor,
					ColorAreaType.Flat);
      colorButton.Update = true;
      table.AttachAligned(0, 2, _("S_un:"), 0.0, 0.5, colorButton, 1, 
			  true);

      colorButton = new GimpColorButton("", 16, 16, _cloudColor,
					ColorAreaType.Flat);
      colorButton.Update = true;
      table.AttachAligned(0, 3, _("C_loud:"), 0.0, 0.5, colorButton, 1, 
			  true);

      colorButton = new GimpColorButton("", 16, 16, _shadowColor,
					ColorAreaType.Flat);
      colorButton.Update = true;
      table.AttachAligned(0, 4, _("Sh_adow:"), 0.0, 0.5, colorButton, 1, 
			  true);

      return dialog;
    }

    override protected void UpdatePreview(AspectPreview preview)
    {
      Initialize(_drawable);
      preview.Update(DoSky);
    }

    void Initialize(Drawable drawable)
    {
      const double lensAngle = 70.0;
      const double earthRadius = 6375.0;
      double[] amplitudes = new double[]{1.0, 0.5, 0.25, 0.125, 0.0625,
					 0.03125, 0.05, 0.05, 0.04, 
					 0.0300};

      _width = drawable.Width;
      _height = drawable.Height;
      _clouds = new Perlin3D(10, 16.0, amplitudes, _seed);
      _cameraDistance = _width * 0.5 / Math.Tan(lensAngle * Math.PI / 180.0);
      TMatrix temp1 = new TMatrix(_tilt, 1);
      TMatrix temp2 = new TMatrix(_rotation, 2);
      _transform = TMatrix.Combine(temp1, temp2);
      _cameraLocation = new Vector3(0.0, earthRadius + 0.2, 0.0);

      _horizonColor2 = FromScreen(_horizonColor);
      _skyColor2 = FromScreen(_skyColor);
    }

    override protected void Render(Drawable drawable)
    {
      Initialize(drawable);
      RgnIterator iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.Progress = new Progress("Sky");
      iter.IterateDest(DoSky);

      Display.DisplaysFlush();
    }

    Pixel DoSky(int x, int y)
    {
      double scale = 1.0 / 100.0;
      Vector3 ray = new Vector3(x - _width * 0.5, _height - 1 - y, 
				_cameraDistance);
      Vector3 worldRay = _transform.Transform(ray);
      worldRay.Normalize();

      double skyDistance, dummy;
      Vector3 intersection, intersection2;
      bool result1 = SphereIntersect(_planetRadius, worldRay, 
				     out intersection, out skyDistance);
      bool result2 = SphereIntersect(_planetRadius + _cloudHeight * 0.9,
				     worldRay, out intersection2, out dummy);

      double value = (intersection.Y - _cameraLocation.Y) /
	(_planetRadius + _cloudHeight - _cameraLocation.Y);

      RGB rgb = Interpolate(Math.Pow(value, 200.0), _horizonColor2, 
			    _skyColor2);

      if (_sunShow)
	{
	  // Fix me: draw sun
	}

      if (result1 && result2)
	{
	  double offset = _planetRadius + _cloudHeight;
	  DrawCloudPoint(rgb, Math.Pow(value, 100.0), x, y,
			 (intersection.X + offset) * scale,
			 (intersection.Z + offset) * scale,
			 (intersection2.X + offset) * scale,
			 (intersection2.Z + offset) * scale);
	}

      return new Pixel(255, 0, 0);
    }

    RGB Interpolate(double value, RGB rgb1, RGB rgb2)
    {
      double r = value * rgb2.R + (1.0 - value) * rgb1.R;
      double g = value * rgb2.G + (1.0 - value) * rgb1.G;
      double b = value * rgb2.B + (1.0 - value) * rgb1.B;

      return new RGB(r, g, b);
    }

    void DrawCloudPoint(RGB rgb, double value, int x, int y,
			double cloudX, double cloudY, 
			double shadowX, double shadowY)
    {
      double offsetX = _time * 0.25;
      double offsetY = _time * 0.33;

      // double point1 = 
    }

    RGB FromScreen(RGB rgb)
    {
      const double gamma = 1.5;

      return new RGB(Math.Pow(rgb.R, gamma),
		     Math.Pow(rgb.G, gamma),
		     Math.Pow(rgb.B, gamma));
    }

    bool SphereIntersect(double radius, Vector3 cameraRay, 
			 out Vector3 intersection, out double dist)
    {
      dist = 0.0;
      intersection = null;

      // Sphere is the center of the world
      Vector3 point = new Vector3(-_cameraLocation.X, -_cameraLocation.Y,
				  -_cameraLocation.Z);
      double distance = cameraRay.InnerProduct(point);
      
      double halfChordSqr = radius * radius + distance * distance
	- point.InnerProduct(point);
      if (halfChordSqr < 0.0001)
	return false;

      double minDist = distance + Math.Sqrt(halfChordSqr);
      if (minDist < 0.0)
	return false;

      dist = minDist;

      intersection = new Vector3(_cameraLocation.X + cameraRay.X * minDist,
				 _cameraLocation.Y + cameraRay.Y * minDist,
				 _cameraLocation.Z + cameraRay.Z * minDist);

      return true;
    }
  }
}
