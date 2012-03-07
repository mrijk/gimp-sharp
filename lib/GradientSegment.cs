// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// GradientSegment.cs
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

namespace Gimp
{
  public sealed class GradientSegment
  {
    readonly Gradient _gradient;
    readonly int _segment;

    internal GradientSegment(Gradient gradient, int segment)
    {
      _gradient = gradient;
      _segment = segment;
    }

    public void SetLeftColor(RGB color, double opacity)
    {
      _gradient.SegmentSetLeftColor(_segment, color, opacity);
    }

    public RGB GetLeftColor(RGB color, out double opacity)
    {
      return _gradient.SegmentGetLeftColor(_segment, out opacity);
    }

    public void SetRightColor(RGB color, double opacity)
    {
      _gradient.SegmentSetRightColor(_segment, color, opacity);
    }

    public RGB GetRightColor(RGB color, out double opacity)
    {
      return _gradient.SegmentGetRightColor(_segment, out opacity);
    }

    public double GetLeftPosition()
    {
      return _gradient.SegmentGetLeftPosition(_segment);
    }

    public double SetLeftPosition(double position)
    {
      return _gradient.SegmentSetLeftPosition(_segment, position);
    }

    public double GetMiddlePosition()
    {
      return _gradient.SegmentGetMiddlePosition(_segment);
    }

    public double SetMiddlePosition(double position)
    {
      return _gradient.SegmentSetMiddlePosition(_segment, position);
    }

    public double GetRightPosition()
    {
      return _gradient.SegmentGetRightPosition(_segment);
    }

    public double SetRightPosition(double position)
    {
      return _gradient.SegmentSetRightPosition(_segment, position);
    }

    public GradientSegmentType BlendingFunction
    {
      get {return _gradient.SegmentGetBlendingFunction(_segment);}
    }

    public GradientSegmentColor BlendingColoringType
    {
      get {return _gradient.SegmentGetBlendingColoringType(_segment);}
    }
  }
}
