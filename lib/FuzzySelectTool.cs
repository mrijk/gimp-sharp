// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// FuzzySelectTool.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Fuzzy Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Fuzzy Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;
using System.Runtime.InteropServices;

namespace Gimp
{
  public sealed class FuzzySelectTool
  {
    Int32 _imageID;

    public FuzzySelectTool(Image image)
    {
      _imageID = image.ID; 
    }

    public void Select(Coordinate<double> coordinate, int threshold, 
		       ChannelOps operation, bool antialias, bool feather, 
		       double featherRadius, bool sampleMerged)
    {
      if (!gimp_fuzzy_select(_imageID, coordinate.X, coordinate.Y, threshold,
			     operation, antialias, feather, featherRadius, 
			     sampleMerged))
	{
	  throw new GimpSharpException();
	}
    }

    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_fuzzy_select(Int32 image_ID,
					 double x,
					 double y,
					 int threshold,
					 ChannelOps operation,
					 bool antialias,
					 bool feather,
					 double feather_radius,
					 bool sample_merged);
  }
}
