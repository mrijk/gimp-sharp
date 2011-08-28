// The Sky plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Code ported from Physically Modeled Media Plug-In for The GIMP
//                  Copyright (c) 2000-2001 David A. Bartold
//
// Renderer.cs
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
  public class Renderer : BaseRenderer
  {
    readonly Drawable _drawable;

    readonly double _cameraDistance;
    const double _planetRadius = 6375.0;
    const double _cloudHeight = 5.0;
    readonly int _width, _height;
    readonly TMatrix _transform;
    readonly Vector3 _cameraLocation;
    readonly Perlin3D _clouds;
    readonly int _intSunX, _intSunY;

    readonly RGB _horizonColor2, _skyColor2, _sunColor2, _cloudColor2;
    readonly RGB _shadowColor2;

    public Renderer(VariableSet variables, Drawable drawable) : 
      base(variables)                                                   
    {
      _drawable = drawable;

      const double lensAngle = 70.0;
      const double earthRadius = 6375.0;
      var amplitudes = new double[]{1.0, 0.5, 0.25, 0.125, 0.0625,
				    0.03125, 0.05, 0.05, 0.04, 0.0300};

      _width = drawable.Width;
      _height = drawable.Height;

      _clouds = new Perlin3D(10, 16.0, amplitudes, 
			     (int) GetValue<UInt32>("seed"));
      _cameraDistance = _width * 0.5 / Math.Tan(lensAngle * Math.PI / 180.0);

      _intSunX = (int) Math.Round((_width - 1) * GetValue<double>("sun_x"));
      _intSunY = (int) Math.Round((_height - 1) * GetValue<double>("sun_y"));

      _horizonColor2 = FromScreen("horizon_color");
      _skyColor2 = FromScreen("sky_color");
      _sunColor2 = FromScreen("sun_color");
      _cloudColor2 = FromScreen("cloud_color");
      _shadowColor2 = FromScreen("shadow_color");

      var tilt = new TMatrix(GetValue<double>("tilt"), 1);
      var rotation = new TMatrix(GetValue<double>("rotation"), 2);
      _transform = TMatrix.Combine(tilt, rotation);
      _cameraLocation = new Vector3(0.0, earthRadius + 0.2, 0.0);
    }

    public void Render()
    {
      var iter = new RgnIterator(_drawable, _("Sky"));
      iter.IterateDest(DoSky);
    }

    public void Render(AspectPreview preview)
    {
      preview.Update(DoSky);
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

      if (GetValue<bool>("sun_show"))
	{
	  rgb = DrawSun(x, y, _width / 35, rgb);
	}

      if (intersection != null && intersection2 != null)
	{
	  double offset = _planetRadius + _cloudHeight;
	  return DrawCloudPoint(rgb, Math.Pow(value, 100.0),
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

    Pixel DrawCloudPoint(RGB rgb, double value,
			 double cloudX, double cloudY, 
			 double shadowX, double shadowY)
    {
      double time = GetValue<double>("time");

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
      var rgb1 = RGB.Interpolate(point2, _cloudColor2, _shadowColor2);
      rgb = RGB.Interpolate(Math.Min(value * point1, 1.0), rgb, rgb1);
      
      return SetFore(rgb);
    }
    
    RGB FromScreen(string identifier)
    {
      var rgb = GetValue<RGB>(identifier);
      const double gamma = 1.5;
      RGB result = new RGB(rgb);
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
