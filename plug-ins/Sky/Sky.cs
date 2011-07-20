// The Sky plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Code ported from Physically Modeled Media Plug-In for The GIMP
//                  Copyright (c) 2000-2001 David A. Bartold
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
  class Sky : PluginWithPreview<AspectPreview>
  {
    Variable<double> _tilt = new Variable<double>
    ("tilt", _("Camera tilt angle (0.0 - 90.0)"), 0.0);
    Variable<double> _rotation = new Variable<double>
    ("rotation", _("Camera rotation angle (0.0 - 90.0)"), 0.0);
    Variable<UInt32> _seed = new Variable<UInt32>
    ("seed", _("Random seed, -1 to use current time"), 0);
    Variable<bool> _sunShow = new Variable<bool>
    ("sun_show", _("Show sun? (bool)"), true);
    Variable<double> _sunX = new Variable<double>
    ("sun_x", _("Sun's x coordinate (0.0 - 1.0)"), 0.2);
    Variable<double> _sunY = new Variable<double>
    ("sun_y", _("Sun's y coordinate (0.0 - 1.0)"), 0.2);
    Variable<double> _time = new Variable<double>
    ("time", _("Time in hours (0.0 - 24.0)"), 0.0);
    Variable<RGB> _horizonColor = new Variable<RGB>
    ("horizon_color", _("Horizon color"), new RGB(0.31, 0.35, 0.40));
    Variable<RGB> _skyColor = new Variable<RGB>
    ("sky_color", _("Color at highest point in the sky"), 
     new RGB(0.01, 0.04, 0.18));
    Variable<RGB> _sunColor = new Variable<RGB>
    ("sun_color", _("Sun color"), new RGB(0.995, 0.90, 0.83));
    Variable<RGB> _cloudColor = new Variable<RGB>
    ("cloud_color", _("Cloud color"), new RGB(1.0, 1.0, 1.0));
    Variable<RGB> _shadowColor = new Variable<RGB>
    ("shadow_color", _("Cloud shadow color"), new RGB(0, 0, 0));
    Variable<bool> _randomSeed = new Variable<bool>
    ("random_seed", _(""), false);

    double _cameraDistance;
    const double _planetRadius = 6375.0;
    const double _cloudHeight = 5.0;
    int _width, _height;
    TMatrix _transform;
    Vector3 _cameraLocation;
    Perlin3D _clouds;
    int _intSunX, _intSunY;

    RGB _horizonColor2, _skyColor2, _sunColor2, _cloudColor2, _shadowColor2;

    static void Main(string[] args)
    {
      GimpMain<Sky>(args);
    }

    override protected Procedure GetProcedure()
    {
      var inParams = new ParamDefList(_tilt, _rotation, _seed, _sunShow,
				      _sunX, _sunY, _time, _horizonColor,
				      _skyColor, _sunColor, _cloudColor,
				      _shadowColor);
      return new Procedure("plug_in_sky",
			   _("Sky"),
			   _("Sky"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2007-2011",
			   _("Sky..."),
			   "RGB*",
			   inParams)
	{
	  MenuPath = "<Image>/Filters/Render/",
	  IconFile = "Sky.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Sky", true);

      var dialog = DialogNew("Sky", "Sky", IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, "Sky");

      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      CreateRandomEntry(vbox);
      CreateSunParameters(vbox);
      CreateCameraParameters(vbox);
      CreateColorParameters(vbox);

      _seed.ValueChanged += delegate {InvalidatePreview();};
      _randomSeed.ValueChanged += delegate {InvalidatePreview();};
      _sunX.ValueChanged += delegate {InvalidatePreview();};
      _sunY.ValueChanged += delegate {InvalidatePreview();};
      _sunShow.ValueChanged += delegate {InvalidatePreview();};
      _rotation.ValueChanged += delegate {InvalidatePreview();};
      _tilt.ValueChanged += delegate {InvalidatePreview();};

      return dialog;
    }

    void CreateRandomEntry(VBox vbox)
    {
      var table = new GimpTable(1, 3) {ColumnSpacing = 6, RowSpacing = 6};
      vbox.Add(table);

      var seed = new RandomSeed(_seed, _randomSeed);

      table.AttachAligned(0, 0, _("Random _Seed:"), 0.0, 0.5, seed, 2, true);
    }

    void CreateSunParameters(VBox vbox)
    {
      var table = CreateFramedTable(vbox, "Sun", 3, 2);

      var sunX = new ScaleEntry(table, 0, 1, _("_X:"), 150, 4, 
				_sunX, 0.0, 1.0, 0.01, 0.1, 2);

      var sunY = new ScaleEntry(table, 0, 2, _("_Y:"), 150, 4, 
				_sunY, 0.0, 1.0, 0.01, 0.1, 2);
      _sunShow.ValueChanged += delegate
	{
	  sunX.Sensitive = sunY.Sensitive = _sunShow.Value;
	};

      var sunShow = new GimpCheckButton(_("_Show sun"), _sunShow);
      table.Attach(sunShow, 0, 2, 3, 4);

      Preview.ButtonPressEvent += delegate(object o, ButtonPressEventArgs args)
	{
	  if (_sunShow.Value)
	  {
	    var size = Preview.Size;
	    sunX.Value = args.Event.X / size.Width;
	    sunY.Value = args.Event.Y / size.Height;
	  }
	};
    }

    void CreateCameraParameters(VBox vbox)
    {
      var table = CreateFramedTable(vbox, "Camera", 2, 1);

      new ScaleEntry(table, 0, 1, _("_Rotation angle:"), 
		     150, 3, _rotation, 0.0, 90.0, 1.0, 8.0, 0);

      new ScaleEntry(table, 0, 2, _("_Tilt angle:"), 
		     150, 3, _tilt, 0.0, 90.0, 1.0, 8.0, 0);
    }

    void CreateColorParameters(VBox vbox)
    {
      var table = CreateFramedTable(vbox, "Colors", 3, 4);

      CreateColorButton(table, 0, 0, "_Horizon:", _horizonColor);
      CreateColorButton(table, 0, 1, "S_ky:", _skyColor);
      CreateColorButton(table, 0, 2, "S_un:", _sunColor);
      CreateColorButton(table, 2, 0, "C_loud:", _cloudColor);
      CreateColorButton(table, 2, 1, "Sh_adow:", _shadowColor);
    }

    GimpTable CreateFramedTable(VBox vbox, string label, 
				uint rows, uint columns)
    {
      var frame = new GimpFrame(_(label));
      vbox.Add(frame);

      var table = new GimpTable(rows, columns) 
	{ColumnSpacing = 6, RowSpacing = 6};
      frame.Add(table);

      return table;
    }

    void CreateColorButton(GimpTable table, int column, int row, string label,
			   Variable<RGB> color)
    {
      var button = new GimpColorButton("", 16, 16, color, ColorAreaType.Flat) 
	{Update = true};
      table.AttachAligned(column, row, _(label), 0.0, 0.5, button, 1, true);
    }

    override protected void UpdatePreview(GimpPreview preview)
    {
      Initialize(_drawable);
      (preview as AspectPreview).Update(DoSky);
    }

    void Initialize(Drawable drawable)
    {
      const double lensAngle = 70.0;
      const double earthRadius = 6375.0;
      var amplitudes = new double[]{1.0, 0.5, 0.25, 0.125, 0.0625,
				    0.03125, 0.05, 0.05, 0.04, 0.0300};
      _width = drawable.Width;
      _height = drawable.Height;
      _clouds = new Perlin3D(10, 16.0, amplitudes, (int) _seed.Value);
      _cameraDistance = _width * 0.5 / Math.Tan(lensAngle * Math.PI / 180.0);

      _intSunX = (int) Math.Round((_width - 1) * _sunX.Value);
      _intSunY = (int) Math.Round((_height - 1) * _sunY.Value);

      _horizonColor2 = FromScreen(_horizonColor);
      _skyColor2 = FromScreen(_skyColor);
      _sunColor2 = FromScreen(_sunColor);
      _cloudColor2 = FromScreen(_cloudColor);
      _shadowColor2 = FromScreen(_shadowColor);

      var tilt = new TMatrix(_tilt.Value, 1);
      var rotation = new TMatrix(_rotation.Value, 2);
      _transform = TMatrix.Combine(tilt, rotation);
      _cameraLocation = new Vector3(0.0, earthRadius + 0.2, 0.0);
    }

    override protected void Render(Drawable drawable)
    {
      Initialize(drawable);
      var iter = new RgnIterator(drawable, _("Sky"));
      iter.IterateDest(DoSky);
    }

    Pixel DoSky(int x, int y)
    {
      const double scale = 1.0 / 100.0;
      var ray = new Vector3(x - _width * 0.5, _height - 1 - y, _cameraDistance);
      var worldRay = _transform.Transform(ray);
      worldRay.Normalize();

      double skyDistance, dummy;
      var intersection = SphereIntersect(_planetRadius + _cloudHeight, 
					 worldRay, out skyDistance);
      var intersection2 = 
	SphereIntersect(_planetRadius + _cloudHeight * 0.9,
			worldRay, out dummy);

      double intersectionY = (intersection == null) ? 0 : intersection.Y;
      double value = (intersectionY - _cameraLocation.Y) /
	(_planetRadius + _cloudHeight - _cameraLocation.Y);

      var rgb = RGB.Interpolate(Math.Pow(value, 200.0), _horizonColor2, 
				_skyColor2);

      if (_sunShow.Value)
	{
	  rgb = DrawSun(x, y, _width / 35, rgb);
	}

      if (intersection != null && intersection2 != null)
	{
	  double offset = _planetRadius + _cloudHeight;
	  return DrawCloudPoint(rgb, Math.Pow(value, 100.0), x, y,
				(intersection.X + offset) * scale,
				(intersection.Z + offset) * scale,
				(intersection2.X + offset) * scale,
				(intersection2.Z + offset) * scale);
	}

      return new Pixel(3) {Color = rgb};
    }

    RGB DrawSun(int x, int y, double sunSize, RGB rgb)
    {
      double dist = Math.Sqrt(_width * _width + _height * _height);

      double distance = Math.Sqrt((_intSunX - x) * (_intSunX - x) +
				  (_intSunY - y) * (_intSunY - y));

      if (distance < sunSize)
	{
	  return _sunColor2;
	}
      else if (distance < dist)
	{
	  double value = 1.0 - 1.0 /
	    Math.Pow(200.0, (distance - sunSize) / (dist - sunSize));
	  return RGB.Interpolate(value, _sunColor2, rgb);
	}
      return rgb;
    }

    Pixel SetFore(RGB rgb)
    {
      /*
       * Perform some contrast adjustment and clip the results before applying
       * gamma.  We adjust the contrast because the viewer expects a photograph
       * to be more vivid than the real world.  Without the adjustment, the
       * results won't resemble a photo.  Try removing the contrast adjustment
       * and see for yourself.
       */
      RGB result = rgb * 1.6 - 0.2;
      result.Clamp();
      result.Gamma(1.5);
      return new Pixel(3){Color = result};
    }

    void Expose(RGB inoutRGB, double value, RGB rgb)
    {
      inoutRGB.R = value * rgb.R + inoutRGB.R * (1.0 - value);
      inoutRGB.G = value * rgb.G + inoutRGB.G * (1.0 - value);
      inoutRGB.B = value * rgb.B + inoutRGB.B * (1.0 - value);
    }

    Pixel DrawCloudPoint(RGB rgb, double value, int x, int y,
			 double cloudX, double cloudY, 
			 double shadowX, double shadowY)
    {
      double time = _time.Value;

      double offsetX = time * 0.25;
      double offsetY = time * 0.33;

      double point1 = _clouds.Get(cloudX + offsetX, cloudY + offsetY,
				  time * 2.5);
      double point2 = _clouds.Get(shadowX + offsetX, shadowY + offsetY,
				  time * 2.5);

      if (point1 < 0.525)
	point1 = 0.0;
      else
	point1 = Math.Pow((point1 - 0.525) / (1.0 - 0.525), 0.4);
      
      if (point2 < 0.56)
	point2 = 0.0;
      else
	point2 = Math.Pow((point2 - 0.56) / (1.0 - 0.56), 0.9);
      
      // Apply shadow 
      RGB rgb1 = RGB.Interpolate(point2, _cloudColor2, _shadowColor2);
      Expose(rgb, Math.Min(value * point1, 1.0), rgb1);
      
      return SetFore(rgb);
    }
    
    RGB FromScreen(Variable<RGB> rgb)
    {
      const double gamma = 1.5;
      RGB result = new RGB(rgb.Value);
      result.Gamma(1 / gamma);
      return result;
    }
    
    Vector3 SphereIntersect(double radius, Vector3 cameraRay, out double dist)
    {
      dist = 0.0;

      // Sphere is the center of the world
      var point = -_cameraLocation;
      double distance = cameraRay.InnerProduct(point);
      
      double halfChordSqr = radius * radius + distance * distance
	- point.InnerProduct(point);
      if (halfChordSqr < 0.0001)
	return null;

      dist = distance + Math.Sqrt(halfChordSqr);
      return (dist < 0.0) ? null : _cameraLocation + cameraRay * dist;
    }
  }
}
