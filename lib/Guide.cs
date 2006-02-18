// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// Guide.cs
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
  public class Guide
  {
    protected Int32 _imageID;
    protected Int32 _guideID;

    public Guide(Image image, Int32 guideID)
    {
      _imageID = image.ID;
      _guideID = guideID;
    }

    Guide(Int32 imageID, Int32 guideID)
    {
      _imageID = imageID;
      _guideID = guideID;
    }

    public void Delete()
    {
      if (!gimp_image_delete_guide(_imageID, _guideID))
        {
	  throw new Exception();
        }
    }

    public int Position
    {
      get {return gimp_image_get_guide_position(_imageID, _guideID);}
    }

    public Guide FindNext()
    {
      Int32 next = gimp_image_find_next_guide(_imageID, _guideID);
      return (next == 0) ? null : new Guide(_imageID, next);
    }

    public OrientationType Orientation
    {
      get {return gimp_image_get_guide_orientation(_imageID, _guideID);}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_delete_guide(Int32 image_ID, Int32 guide_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_image_get_guide_position (Int32 image_ID,
                                                     Int32 guide_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_find_next_guide (Int32 image_ID,
                                                    Int32 guide_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern OrientationType 
    gimp_image_get_guide_orientation (Int32 image_ID, Int32 guide_ID);
  }
}
