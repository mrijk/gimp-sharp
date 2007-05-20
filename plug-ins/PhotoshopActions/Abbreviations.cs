// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// Abbreviations.cs
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

using System.Collections;
using System.Collections.Generic;

namespace Gimp.PhotoshopActions
{
  static class Abbreviations
  {
    static readonly Dictionary<string, string> _map = 
      new Dictionary<string, string>();

    static Abbreviations()
    {
      _map["ActP"] = "Actual Pixels";
      _map["AdBt"] = "bottom edges";
      _map["AdCH"] = "horizontal centers";
      _map["AdCV"] = "vertical centers";
      _map["AdLf"] = "left edges";
      _map["AdRg"] = "right edges";
      _map["AdTp"] = "top edges";
      _map["Anno"] = "none";
      _map["BckC"] = "background color";
      _map["Bckg"] = "background";
      _map["Blck"] = "black";
      _map["Bl"]   = "blue";
      _map["Blks"] = "Blocks";
      _map["BrbW"] = "brush wide blurry";
      _map["BrsD"] = "Brush Detail";
      _map["BrSm"] = "brush simple";
      _map["BrsS"] = "Brush Size";
      _map["BtmM"] = "bitmap mode";
      _map["Btom"] = "Bottom";
      _map["CBrn"] = "color burn";
      _map["Clcl"] = "calculation";
      _map["Clr"]  = "color";
      _map["Clrz"] = "Colorize";
      _map["ClSz"] = "Cell Size";
      _map["CMYM"] = "CMYK color mode";
      _map["Cntn"] = "Continue";
      _map["Cntr"] = "center";
      _map["CrnH"] = "Current History State";
      _map["CrrL"] = "current layer";
      _map["Cyn" ] = "cyan";
      _map["Dplc"] = "Duplicate";
      _map["DrcB"] = "Direction Balance";
      _map["Drkn"] = "darken";
      _map["Edg"]  = "Edge";
      _map["Elps"] = "Ellipse";
      _map["ExtF"] = "Solid Front Faces";
      _map["ExtM"] = "Mask Incomplete Blocks";
      _map["ExtR"] = "Random";
      _map["ExtT"] = "Type";
      _map["FllD"] = "full document";
      _map["FrgC"] = "foreground color";
      _map["Frnt"] = "front";
      _map["Frst"] = "first";
      _map["FtOn"] = "Fit On Screen";
      _map["GlwE"] = "glowing edges";
      _map["Grn"]  = "green";
      _map["GrnE"] = "grain enlarged";
      _map["GrnH"] = "grain horizontal";
      _map["GrnR"] = "grain regular";
      _map["GrtW"] = "Grout Width";
      _map["Gry"]  = "gray";
      _map["Grys"] = "grayscale mode";
      _map["Gsn"]  = "gaussian";
      _map["HrdL"] = "hard light";
      _map["Hrzn"] = "Horizontal";
      _map["HrzO"] = "Horizontal Only";
      _map["Insd"] = "inside";
      _map["Invr"] = "Invert Source";
      _map["InvT"] = "Invert Texture";
      _map["LbCM"] = "Lab color mode";
      _map["LDBL"] = "Bottom Left";
      _map["LDTp"] = "Top";
      _map["LDTR"] = "Top Right";
      _map["Left"] = "Left";
      _map["LgDr"] = "Light/Dark Balance";
      _map["LghD"] = "Light Direction";
      _map["LghG"] = "Lighten Grout";
      _map["Lghn"] = "lighten";
      _map["Lght"] = "lightness";
      _map["Lmns"] = "luminosity";
      _map["LngL"] = "long lines";
      _map["Lnr"]  = "linear";
      _map["LngS"] = "long strokes";
      _map["LPBt"] = "light position bottom";
      _map["LPLf"] = "light position left";
      _map["LPRg"] = "light position right";
      _map["LPTp"] = "light position top";
      _map["Lvl"]  = "Level";
      _map["LvlB"] = "Level-based";
      _map["Lwr"]  = "lower";
      _map["Mgnt"] = "magenta";
      _map["Mltp"] = "multiply";
      _map["Mnch"] = "Monochromatic";
      _map["Msge"] = "Message";
      _map["Nkn1"] = "105mm Prime";
      _map["Nrml"] = "normal";
      _map["N"]    = "no";
      _map["Otsd"] = "outside";
      _map["Ovrl"] = "overlay";
      _map["Phtc"] = "Photocopy";
      _map["Pncl"] = "Pencil Width";
      _map["PntD"] = "paint daubs";
      _map["PprB"] = "Paper Brightness";
      _map["PrnS"] = "Print Size";
      _map["Prvs"] = "previous";
      _map["PrsT"] = "Preserve Transparency";
      _map["Ptrn"] = "pattern";
      _map["Pyrm"] = "pyramids";
      _map["Rd"]   = "red";
      _map["Rds"]  = "Radius";
      _map["Rght"] = "Right";
      _map["Rlf"]  = "Relief";
      _map["Rndm"] = "random";
      _map["RctP"] = "Rectangular to Polar";
      _map["RGBM"] = "RGB color mode";
      _map["RptE"] = "repeat edge pixels";
      _map["Rtcl"] = "reticulation";
      _map["SBMN"] = "smart blur mode normal";
      _map["SBQH"] = "smart blur quality high";
      _map["SBQH"] = "smart blur quality low";
      _map["SBQH"] = "smart blur quality medium";
      _map["Scln"] = "Scaling";
      _map["Scrn"] = "screen";
      _map["SDir"] = "Stroke Direction";
      _map["SDLD"] = "Left Diagonal";
      _map["SDLD"] = "Right Diagonal";
      _map["SftL"] = "soft light";
      _map["Sftn"] = "Softness";
      _map["ShdI"] = "Shadow Intensity";
      _map["Shdw"] = "shadow";
      _map["Shrp"] = "Sharpness";
      _map["Sml"]  = "small";
      _map["SprR"] = "Spray Radius";
      _map["StrD"] = "Stroke Detail";
      _map["StrL"] = "Stroke Length";
      _map["StrP"] = "Stroke Pressure";
      _map["StrS"] = "Stroke Size";
      _map["TlSz"] = "Tile Size";
      _map["Top"]  = "Top";
      _map["Trns"] = "transparent";
      _map["Trsp"] = "transparency";
      _map["TxCa"] = "Canvas";
      _map["TxSt"] = "Sandstone";
      _map["Txtr"] = "Texture";
      _map["TxtT"] = "Texture Type";
      _map["Unfr"] = "uniform";
      _map["Vrtc"] = "Vertical";
      _map["Wdth"] = "Width";
      _map["Wht"]  = "white";
      _map["Wnd"]  = "Wind";
      _map["Wrp"]  = "wrap";
      _map["WrpA"] = "wrap around";
      _map["WvSn"] = "wave sine";
      _map["WvSq"] = "wave square";
      _map["Yllw"] = "yellow";
      _map["Zm"]   = "zoom";
    }

    public static string Get(string key)
    {
      string fullString;

      if (_map.TryGetValue(key, out fullString))
	{
	  return fullString;
	}
      else
	{
	  return key;
	}
    }
  }
}

