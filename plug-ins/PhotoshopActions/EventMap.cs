// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// EventMap.cs
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
using System.Collections.Generic;
using System.Reflection;

namespace Gimp.PhotoshopActions
{
  public class EventMap
  {
    readonly Dictionary<string, string> _map = 
      new Dictionary<string, string>();
    
    readonly Dictionary<string, int> _statistics =
      new Dictionary<string, int>();

    public EventMap()
    {
      _map["add"] = "AddEvent";
      _map["addNoise"] = "AddNoiseEvent";
      _map["addTo"] = "AddToEvent";
      _map["border"] = "BorderEvent";
      _map["brightnessEvent"] = "BrightnessEvent";
      _map["canvasSize"] = "CanvasSizeEvent";
      _map["channelMixer"] = _map["ChnM"] = "ChannelMixerEvent";
      _map["chrome"] = _map["Chrm"] = "ChromeEvent";
      _map["clearEvent"] = "ClearEvent";
      _map["close"] = "CloseEvent";
      _map["clouds"] = "CloudsEvent";
      _map["colorBalance"] = "ColorBalanceEvent";
      _map["contract"] = "ContractEvent";
      _map["convertMode"] = _map["CnvM"] = "ConvertModeEvent";
      _map["copyEvent"] = _map["copy"] = "CopyEvent";
      _map["copyMerged"] = "CopyMergedEvent";
      _map["copyToLayer"] = "CopyToLayerEvent";
      _map["crop"] = "CropEvent";
      _map["curves"] = "CurvesEvent";
      _map["cut"] = "CutEvent";
      _map["delete"] = "DeleteEvent";
      _map["desaturate"] = _map["Dstt"] = "DesaturateEvent";
      _map["differenceClouds"] = _map["DfrC"] = "DifferenceCloudsEvent";
      _map["diffuseGlow"] = "DiffuseGlowEvent";
      _map["diffuse"] = "DiffuseEvent";
      _map["duplicate"] = "DuplicateEvent";
      _map["dustAndScratches"] = "DustAndScratchesEvent";
      _map["emboss"] = "EmbossEvent";
      _map["equalize"] = "EqualizeEvent";
      _map["exchange"] = "ExchangeEvent";
      _map["expand"] = _map["Expn"] = "ExpandEvent";
      _map["facet"] = "FacetEvent";
      _map["fade"] = _map["Fade"] = "FadeEvent";
      _map["feather"] = "FeatherEvent";
      _map["fill"] = "FillEvent";
      _map["findEdges"] = "FindEdgesEvent";
      _map["flattenImage"] = "FlattenImageEvent";
      _map["flip"] = _map["Flip"] = "FlipEvent";
      _map["gaussianBlur"] = "GaussianBlurEvent";
      _map["glowingEdges"] = "GlowingEdgesEvent";
      _map["gradientClassEvent"] = _map["Grdn"] = "GradientClassEvent";
      _map["gradientMapEvent"] = "GradientMapEvent";
      _map["grain"] = "GrainEvent";
      _map["grow"] = "GrowEvent";
      _map["hide"] = "HideEvent";
      _map["hueSaturation"] = "HueSaturationEvent";
      _map["imageSize"] = _map["ImgS"] = "ImageSizeEvent";
      _map["inverse"] = "InverseEvent";
      _map["invert"] = _map["Invr"] = "InvertEvent";
      _map["lensFlare"] = "LensFlareEvent";
      _map["levels"] = _map["Lvls"] = "LevelsEvent";
      _map["link"] = _map["Lnk"] = "LinkEvent";
      _map["linkSelectedLayers"] = "LinkSelectedLayersEvent";
      _map["make"] = _map["Mk"] = "MakeEvent";
      _map["maximum"] = "MaximumEvent";
      _map["median"] = "MedianEvent";
      _map["mergeLayers"] = "MergeLayersEvent";
      _map["mergeVisible"] = "MergeVisibleEvent";
      _map["mezzotint"] = "MezzotintEvent";
      _map["minimum"] = "MinimumEvent";
      _map["mosaic"] = "MosaicEvent";
      _map["move"] = "MoveEvent";
      _map["motionBlur"] = _map["MtnB"] = "MotionBlurEvent";
      _map["offset"] = "OffsetEvent";
      _map["open"] = "OpenEvent";
      _map["paste"] = _map["past"] = "PasteEvent";
      _map["photocopy"] = "PhotocopyEvent";
      _map["play"] = "PlayEvent";
      _map["pointillize"] = "PointillizeEvent";
      _map["polar"] = _map["Plr"] = "PolarEvent";
      _map["posterization"] = "PosterizationEvent";
      _map["radialBlur"] = _map["RdlB"] = "RadialBlurEvent";
      _map["removeWhiteMatte"] = "RemoveWhiteMatteEvent";
      _map["replaceColor"] = "ReplaceColorEvent";
      _map["reset"] = "ResetEvent";
      _map["rotateEventEnum"] = _map["Rtte"] = "RotateEvent";
      _map["select"] = "SelectEvent";
      _map["selectAllLayers"] = "SelectAllLayersEvent";
      _map["set"] = "SetEvent";
      _map["sharpen"] = "SharpenEvent";
      _map["sharpenMore"] = "SharpenMoreEvent";
      _map["show"] = "ShowEvent";
      _map["smartBlur"] = "SmartBlurEvent";
      _map["smoothness"] = "SmoothnessEvent";
      _map["solarize"] = "SolarizeEvent";
      _map["spherize"] = "SpherizeEvent";
      _map["subtractFrom"] = "SubtractFromEvent";
      _map["surfaceBlur"] = "SurfaceBlurEvent";
      _map["stop"] = "StopEvent";
      _map["stroke"] = "StrokeEvent";
      _map["stainedGlass"] = _map["StnG"] = "StainedGlassEvent";
      _map["thresholdClassEvent"] = _map["Thrs"] = "ThresholdClassEvent";
      _map["tiles"] = "TilesEvent";
      _map["transform"] = _map["Trnf"] = "TransformEvent";
      _map["twirl"] = "TwirlEvent";
      _map["unlink"] = "UnlinkEvent";
      _map["unsharpMask"] = _map["UnsM"] = "UnsharpMaskEvent";
      _map["waterPaper"] = "WaterPaperEvent";
      _map["wave"] = "WaveEvent";
      _map["wind"] = _map["Wnd"] = "WindEvent";

      // Pre-6 events
      _map["AdNs"] = "AddNoiseEvent";
      _map["BrgC"] = "BrightnessEvent";
      _map["Clds"] = "CloudsEvent";
      _map["CpTL"] = "CopyToLayerEvent";
      _map["Crvs"] = "CurvesEvent";
      _map["Dlt"] = "DeleteEvent";
      _map["Dplc"] = "DuplicateEvent";
      _map["Embs"] = "EmbossEvent";
      _map["Exch"] = "ExchangeEvent";
      _map["Fl"] = "FillEvent";
      _map["FndE"] = "FindEdgesEvent";
      _map["GsnB"] = "GaussianBlurEvent";
      // _map["HStr"] = "HueSaturationEvent";
      _map["Invs"] = "InverseEvent";
      _map["MrgL"] = "MergeLayersEvent";
      _map["Msc"] = "MosaicEvent";
      _map["Mztn"] = "MezzotintEvent";
      _map["Rset"] = "ResetEvent";
      _map["setd"] = "SetEvent";
      _map["Shrp"] = "SharpenEvent";
      _map["slct"] = "SelectEvent";
      _map["Sphr"] = "SpherizeEvent";
      _map["Stop"] = "StopEvent";
      _map["Strk"] = "StrokeEvent";
    }

    public ActionEvent Lookup(string eventName)
    {
      ActionEvent myEvent = null;
      string eventType;

      if (_map.TryGetValue(eventName, out eventType))
	{
	  eventType = "Gimp.PhotoshopActions." + eventType;
	  Type type = Assembly.GetEntryAssembly().GetType(eventType);

	  try
	    {
	      myEvent = (ActionEvent) Activator.CreateInstance(type);
	    }
	  catch (Exception e)
	    {
	      Console.WriteLine("Event {0} problem", eventName);
	      Console.WriteLine(e.StackTrace);
	    }
	}
      else
	{
	  Console.WriteLine("Event {0} unsupported", eventName);

	  myEvent = new UnimplementedEvent();

	  int amount;
	  if (!_statistics.TryGetValue(eventName, out amount))
	    {
	      amount = 0;
	    }
	  _statistics[eventName] = ++amount;
	}
      return myEvent;
    }

    public void DumpStatistics()
    {
      foreach (KeyValuePair<string, int> kvp in _statistics)
	{
	  Console.WriteLine("{0} : {1}", kvp.Key, kvp.Value);
	}
    }
  }
}
