// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
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
      _map["AmbB"] = "Ambience";
      _map["AmbC"] = "Ambient Color";
      _map["AmMn"] = "Amplitude min";
      _map["AmMx"] = "Amplitude max";
      _map["Ang1"] = "Channel 1";
      _map["Ang2"] = "Channel 2";
      _map["Ang3"] = "Channel 3";
      _map["Ang4"] = "Channel 4";
      _map["AntA"] = "Anti-alias";
      _map["AdRg"] = "right edges";
      _map["AdTp"] = "top edges";
      _map["Amnt"] = "Amount";
      _map["Angl"] = "angle";
      _map["Anno"] = "none";
      _map["Bcbc"] = "bicubic";
      _map["BckC"] = "background color";
      _map["Bckg"] = "background";
      _map["BlcB"] = "black body";
      _map["Blck"] = "black";
      _map["BlcL"] = "Foreground Level";
      _map["Bl"]   = "blue";
      _map["Blks"] = "Blocks";
      _map["BlrM"] = "Method";
      _map["BlrQ"] = "Quality";
      _map["Blst"] = "Blast";
      _map["BmpA"] = "Height";
      _map["bottomRightPixelColor"] = "bottom right pixel color";
      _map["BrbW"] = "brush wide blurry";
      _map["Brgh"] = "Brightness";
      _map["BrsD"] = "Brush Detail";
      _map["BrSm"] = "brush simple";
      _map["BrsS"] = "Brush Size";
      _map["BtmM"] = "bitmap mode";
      _map["Btom"] = "Bottom";
      _map["CBrn"] = "color burn";
      _map["CDdg"] = "color dodge";
      _map["ChlA"] = "Chalk Area";
      _map["Chnl"] = "Channel";
      _map["ChrA"] = "Charcoal Area";
      _map["Clcl"] = "calculation";
      _map["Clr"]  = "color";
      _map["Clrz"] = "Colorize";
      _map["ClSz"] = "Cell Size";
      _map["Cmps"] = "composite channel";
      _map["CMYM"] = "CMYK color mode";
      _map["Cntg"] = "Contiguous";
      _map["Cntn"] = "Continue";
      _map["Cntr"] = "Contrast";
      _map["CrnH"] = "Current History State";
      _map["CrnL"] = "Current Light";
      _map["CrrL"] = "current layer";
      _map["Crtl"] = "Interpolation";
      _map["Cyn" ] = "cyan";
      _map["Dfnt"] = "Definition";
      _map["Dfrn"] = "difference";
      _map["Dmtr"] = "diameter";
      _map["Dnst"] = "Density";
      _map["Dplc"] = "Duplicate";
      _map["DrcB"] = "Direction Balance";
      _map["Drct"] = "Direction";
      _map["Drft"] = "draft";
      _map["DrkI"] = "Dark Intensity";
      _map["Drkn"] = "darken";
      _map["Dstr"] = "Distribution";
      _map["Dtl"]  = "Detail";
      _map["Edg"]  = "Edge";
      _map["ElmO"] = "Odd Fields";
      _map["Elps"] = "Ellipse";
      _map["Exps"] = "Exposure";
      _map["ExtD"] = "Depth";
      _map["ExtF"] = "Solid Front Faces";
      _map["ExtM"] = "Mask Incomplete Blocks";
      _map["ExtR"] = "Random";
      _map["ExtS"] = "Size";
      _map["ExtT"] = "Type";
      _map["FllD"] = "full document";
      _map["FrgC"] = "foreground color";
      _map["FrmW"] = "Frame Width";
      _map["Frnt"] = "front";
      _map["Frst"] = "first";
      _map["FtOn"] = "Fit On Screen";
      _map["Gd"]   = "good";
      _map["Glos"] = "Gloss";
      _map["GlwE"] = "glowing edges";
      _map["Grn"]  = "green";
      _map["GrnE"] = "Enlarged";
      _map["GrnH"] = "Horizontal";
      _map["GrnR"] = "Regular";
      _map["Grns"] = "Graininess";
      _map["Grnt"] = "Grain Type";
      _map["GrSf"] = "Soft";
      _map["GrtW"] = "Grout Width";
      _map["Gry"]  = "gray";
      _map["Grys"] = "grayscale mode";
      _map["Gsn"]  = "gaussian";
      _map["HlSz"] = "Size";
      _map["HrdL"] = "hard light";
      _map["Hrdn"] = "hardness";
      _map["Hrzn"] = "Horizontal";
      _map["HrzO"] = "Horizontal Only";
      _map["ImgB"] = "Image Balance";
      _map["IndC"] = "indexed color mode";
      _map["Insd"] = "inside";
      _map["IntC"] = "Create";
      _map["IntE"] = "Eliminate";
      _map["Intn"] = "Intensity";
      _map["Intr"] = "Interpolation";
      _map["Invr"] = "Invert Source";
      _map["InvT"] = "Invert Texture";
      _map["LbCM"] = "Lab color mode";
      _map["LDBL"] = "Bottom Left";
      _map["LDBt"] = "Bottom";
      _map["LDTp"] = "Top";
      _map["LDTL"] = "Top Left";
      _map["LDTR"] = "Top Right";
      _map["Left"] = "Left";
      _map["Lft"]  = "Left";
      _map["LgDr"] = "Light/Dark Balance";
      _map["LghD"] = "Light Direction";
      _map["LghG"] = "Lighten Grout";
      _map["LghI"] = "Light Intensity";
      _map["Lghn"] = "lighten";
      _map["Lght"] = "lightness";
      _map["Lmns"] = "luminosity";
      _map["LngL"] = "long lines";
      _map["Lnr"]  = "linear";
      _map["Lns"]  = "Lens";
      _map["LngS"] = "long strokes";
      _map["LPBt"] = "light position bottom";
      _map["LPLf"] = "light position left";
      _map["LPRg"] = "light position right";
      _map["LPTL"] = "Top Left";
      _map["LPTp"] = "light position top";
      _map["Lrg"]  = "Large";
      _map["Lst"]  = "last";
      _map["Lvl"]  = "Level";
      _map["LvlB"] = "Level-based";
      _map["Lwr"]  = "lower";
      _map["Mdm"]  = "medium";
      _map["Mgnt"] = "magenta";
      _map["Mltp"] = "multiply";
      _map["Mnch"] = "Monochromatic";
      _map["Md"]   = "Mode";
      _map["Msge"] = "Message";
      _map["Msk"]  = "mask";
      _map["Mtrl"] = "Material";
      _map["N"]    = "no";
      _map["Nkn"]  = "35mm Prime";
      _map["Nkn1"] = "105mm Prime";
      _map["Nm"]   = "Name";
      _map["NmbG"] = "Number of Generators";
      _map["NmbR"] = "Ridges";
      _map["Nrml"] = "normal";
      _map["Nxt"]  = "next";
      _map["Orng"] = "orange";
      _map["Ornt"] = "Orientation";
      _map["Otsd"] = "outside";
      _map["Ovrl"] = "overlay";
      _map["Pht3"] = "Photoshop";
      _map["Phtc"] = "Photocopy";
      _map["PhtP"] = "Photoshop PDF";
      _map["Plgn"] = "polygon";
      _map["PlrR"] = "Polar to Rectangular";
      _map["Pncl"] = "Pencil Width";
      _map["PndR"] = "Pond Ripples";
      _map["PntD"] = "paint daubs";
      _map["PprB"] = "Paper Brightness";
      _map["PrnS"] = "Print Size";
      _map["PrsL"] = "Preserve Luminosity";
      _map["Prvs"] = "previous";
      _map["PrsT"] = "Preserve Transparency";
      _map["Pstn"] = "Position";
      _map["Ptrn"] = "pattern";
      _map["Pyrm"] = "pyramids";
      _map["Rd"]   = "red";
      _map["Rds"]  = "Radius";
      _map["Rght"] = "Right";
      _map["Rlf"]  = "Relief";
      _map["Rlg"]  = "Relief";
      _map["Rndm"] = "random";
      _map["Rndn"] = "roundness";
      _map["RctP"] = "Rectangular to Polar";
      _map["RGBC"] = "RGB color";
      _map["RGBM"] = "RGB color mode";
      _map["RndS"] = "Random Seed";
      _map["RplM"] = "Ripple Magnitude";
      _map["RplS"] = "Ripple Size";
      _map["Rpt"]  = "repeat";
      _map["RptE"] = "repeat edge pixels";
      _map["Rtcl"] = "reticulation";
      _map["SBME"] = "Edge Only";
      _map["SBMN"] = "smart blur mode normal";
      _map["SBQH"] = "smart blur quality high";
      _map["SBQL"] = "smart blur quality low";
      _map["SBQM"] = "smart blur quality medium";
      _map["Scl"]  = "Scale";
      _map["SclH"] = "Scale horizontal";
      _map["Scln"] = "Scaling";
      _map["SclV"] = "Scale vertical";
      _map["ScrL"] = "Line";
      _map["Scrn"] = "screen";
      _map["ScrT"] = "Pattern Type";
      _map["SDir"] = "Stroke Direction";
      _map["SDLD"] = "Left Diagonal";
      _map["SDRD"] = "Right Diagonal";
      _map["SftL"] = "soft light";
      _map["Sftn"] = "Softness";
      _map["ShdI"] = "Shadow Intensity";
      _map["Shdw"] = "shadow";
      _map["Shrp"] = "Sharpness";
      _map["Sml"]  = "small";
      _map["Smth"] = "Smoothness";
      _map["Spcn"] = "spacing";
      _map["SprR"] = "Spray Radius";
      _map["SqrS"] = "Square Size";
      _map["StDt"] = "Stroke Detail";
      _map["Stgr"] = "Stagger";
      _map["StrD"] = "Stroke Detail";
      _map["Strg"] = "Strength";
      _map["StrL"] = "Stroke Length";
      _map["StrP"] = "Stroke Pressure";
      _map["StrS"] = "Stroke Size";
      _map["StrW"] = "Stroke Width";
      _map["Sz"]   = "Size";
      _map["trimBasedOn"] = "Based on";
      _map["Tlrn"] = "Tolerance";
      _map["TlSz"] = "Tile Size";
      _map["Top"]  = "Top";
      _map["Trgt"] = "current";
      _map["Trns"] = "transparent";
      _map["Trsp"] = "transparency";
      _map["TxBl"] = "Blocks";
      _map["TxBr"] = "Brick";
      _map["TxCa"] = "Canvas";
      _map["TxFr"] = "Frosted";
      _map["TxSt"] = "Sandstone";
      _map["TxtC"] = "Texture Coverage";
      _map["Txtr"] = "Texture";
      _map["TxtT"] = "Texture Type";
      _map["TxTL"] = "Tiny Lens";
      _map["UndA"] = "Undefined Area";
      _map["Unfr"] = "uniform";
      _map["Usng"] = "Using";
      _map["Vrtc"] = "Vertical";
      _map["Wdth"] = "Width";
      _map["WhHi"] = "White is High";
      _map["Wht"]  = "white";
      _map["WhtL"] = "Background Level";
      _map["WLMn"] = "Wave length min";
      _map["WLMx"] = "Wave length max";
      _map["Wnd"]  = "Wind";
      _map["WndM"] = "Method";
      _map["Wrp"]  = "wrap";
      _map["WrpA"] = "wrap around";
      _map["WvSn"] = "wave sine";
      _map["WvSq"] = "wave square";
      _map["Wvtp"] = "Wave Type";
      _map["Xclu"] = "exclusion";
      _map["Yllw"] = "yellow";
      _map["Zm"]   = "zoom";
      _map["ZZTy"] = "Style";
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

