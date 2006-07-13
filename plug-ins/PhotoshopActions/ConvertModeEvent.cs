// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ConvertModeEvent.cs
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
using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class ConvertModeEvent : ActionEvent
  {
    [Parameter("T")]
    Parameter _type;

    public override bool IsExecutable
    {
      get 
	{
	  if (_type != null && (_type is TypeParameter))
	    {
	      TypeParameter type = _type as TypeParameter;
	      if (type.Value == "RGBM" || type.Value == "Grys")
		{
		  return true;
		}
	    }
	  return false;
	}
    }

    protected override IEnumerable ListParameters()
    {
      if (_type is TypeParameter)
	{
	  string type = (_type as TypeParameter).Value;
	  string mode;

	  switch (type)
	    {
	    case "RGBM":
	      mode = "RGB color mode";
	      break;
	    default:
	      mode = type;
	      break;
	    }


	  yield return "To: " + mode;
	}
    }

    override public bool Execute()
    {
      if (_type is TypeParameter)
	{
	  TypeParameter type = _type as TypeParameter;

	  switch (type.Value)
	    {
	    case "RGBM":
	      ActiveImage.ConvertRgb();
	      break;
	    case "Grys":
	      ActiveImage.ConvertGrayscale();
	      break;
	    default:
	      Console.WriteLine("ConvertModeEvent: can't convert: " + 
				type.Value);
	      break;
	    }
	}
      else
	{
	  Console.WriteLine("ConvertModeEvent: " + _type);
	}
      return true;
    }
  }
}
