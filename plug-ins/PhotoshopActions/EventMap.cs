// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
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
      _map["accentedEdges"] = "AccentedEdgesEvent";
      _map["adaptCorrect"] = "AdaptCorrectEvent";
      _map["add"] = "AddEvent";
      _map["addNoise"] = _map["AdNs"] = "AddNoiseEvent";
      _map["addTo"] = "AddToEvent";
      _map["align"] = _map["Algn"] = "AlignEvent";
      _map["angledStrokes"] = "AngledStrokesEvent";
      _map["applyImageEvent"] = "ApplyImageEvent";
      _map["Avrg"] = "AverageEvent";
      _map["blur"] = _map["Blr"] = "BlurEvent";
      _map["blurMethod"] = _map["BlrM"] = "BlurMethodEvent";
      _map["border"] = "BorderEvent";
      _map["brightnessEvent"] = _map["BrgC"] = "BrightnessEvent";
      _map["canvasSize"] = _map["CnvS"] = "CanvasSizeEvent";
      _map["channelMixer"] = _map["ChnM"] = "ChannelMixerEvent";
      _map["chalkCharcoal"] = "ChalkCharcoalEvent";
      _map["chrome"] = _map["Chrm"] = "ChromeEvent";
      _map["clearEvent"] = "ClearEvent";
      _map["close"] = "CloseEvent";
      _map["clouds"] = _map["Clds"] = "CloudsEvent";
      _map["colorBalance"] = _map["ClrB"] = "ColorBalanceEvent";
      _map["colorHalftone"] = "ColorHalftoneEvent";
      _map["colorPalette"] = "ColorPaletteEvent";
      _map["colorRange"] = _map["ClrR"] = "ColorRangeEvent";
      _map["contract"] = _map["Cntc"] = "ContractEvent";
      _map["convertMode"] = _map["CnvM"] = "ConvertModeEvent";
      _map["copyEffects"] = "CopyEffectsEvent";
      _map["copyEvent"] = _map["copy"] = "CopyEvent";
      _map["copyMerged"] = "CopyMergedEvent";
      _map["copyToLayer"] = _map["CpTL"] = "CopyToLayerEvent";
      _map["craquelure"] = _map["Crql"] = "CraquelureEvent";
      _map["crop"] = "CropEvent";
      _map["crosshatch"] = "CrosshatchEvent";
      _map["crystallize"] = "CrystallizeEvent";
      _map["curves"] = _map["Crvs"] = "CurvesEvent";
      _map["cut"] = "CutEvent";
      _map["cutout"] = "CutoutEvent";
      _map["cutToLayer"] = _map["CtTL"] = "CutToLayerEvent";
      _map["darkStrokes"] = "DarkStrokesEvent";
      _map["definePattern"] = _map["DfnP"] = "DefinePatternEvent";
      _map["delete"] = _map["Dlt"] = "DeleteEvent";
      _map["desaturate"] = _map["Dstt"] = "DesaturateEvent";
      _map["despeckle"] = "DespeckleEvent";
      _map["differenceClouds"] = _map["DfrC"] = "DifferenceCloudsEvent";
      _map["diffuseGlow"] = "DiffuseGlowEvent";
      _map["diffuse"] = "DiffuseEvent";
      _map["displace"] = "DisplaceEvent";
      _map["draw"] = "DrawEvent";
      _map["dryBrush"] = "DryBrushEvent";
      _map["duplicate"] = _map["Dplc"] = "DuplicateEvent";
      _map["dustAndScratches"] = "DustAndScratchesEvent";
      _map["emboss"] = _map["Embs"] = "EmbossEvent";
      _map["equalize"] = "EqualizeEvent";
      _map["exchange"] = _map["Exch"] = "ExchangeEvent";
      _map["expand"] = _map["Expn"] = "ExpandEvent";
      _map["extrude"] = "ExtrudeEvent";
      _map["facet"] = "FacetEvent";
      _map["fade"] = _map["Fade"] = "FadeEvent";
      _map["feather"] = _map["Fthr"] = "FeatherEvent";
      _map["fill"] = _map["Fl"] = "FillEvent";
      _map["filmGrain"] = "FilmGrainEvent";
      _map["findEdges"] = _map["FndE"] =  "FindEdgesEvent";
      _map["flattenImage"] = _map["FltI"] = "FlattenImageEvent";
      _map["flip"] = _map["Flip"] = "FlipEvent";
      _map["fragment"] = "FragmentEvent";
      _map["GEfc"] = "FilterGalleryEvent";
      _map["gaussianBlur"] = _map["GsnB"] = "GaussianBlurEvent";
      _map["glass"] = _map["Gls"] = "GlassEvent";
      _map["glowingEdges"] = "GlowingEdgesEvent";
      _map["gradientClassEvent"] = _map["Grdn"] = "GradientClassEvent";
      _map["gradientMapEvent"] = "GradientMapEvent";
      _map["grain"] = "GrainEvent";
      _map["graphicPen"] = "GraphicPenEvent";
      _map["groupEvent"] = "GroupEvent";
      _map["grow"] = "GrowEvent";
      _map["halftoneScreen"] = "HalftoneScreenEvent";
      _map["hide"] = "HideEvent";
      _map["highPass"] = _map["HghP"] = "HighPassEvent";
      _map["hueSaturation"] = _map["HStr"] = "HueSaturationEvent";
      _map["imageSize"] = _map["ImgS"] = "ImageSizeEvent";
      _map["inkOutlines"] = "InkOutlinesEvent";
      _map["interfaceWhite"] = "IntersectWithEvent";
      _map["inverse"] = _map["Invs"] = "InverseEvent";
      _map["invert"] = _map["Invr"] = "InvertEvent";
      _map["lensFlare"] = "LensFlareEvent";
      _map["levels"] = _map["Lvls"] = "LevelsEvent";
      _map["lightingEffects"] = _map["LghE"] = "LightingEffectsEvent";
      _map["link"] = _map["Lnk"] = "LinkEvent";
      _map["linkSelectedLayers"] = "LinkSelectedLayersEvent";
      _map["make"] = _map["Mk"] = "MakeEvent";
      _map["maximum"] = "MaximumEvent";
      _map["median"] = "MedianEvent";
      _map["mergeLayers"] = _map["MrgL"] = "MergeLayersEvent";
      _map["mergeLayersNew"] = "MergeLayersNewEvent";
      _map["mergeVisible"] = "MergeVisibleEvent";
      _map["mezzotint"] = _map["Mztn"] = "MezzotintEvent";
      _map["minimum"] = "MinimumEvent";
      _map["mosaic"] = _map["Msc"] = "MosaicEvent";
      _map["mosaicPlugin"] = "MosaicPluginEvent";
      _map["move"] = "MoveEvent";
      _map["motionBlur"] = _map["MtnB"] = "MotionBlurEvent";
      _map["oceanRipple"] = "OceanRippleEvent";
      _map["offset"] = "OffsetEvent";
      _map["open"] = "OpenEvent";
      _map["paintDaubs"] = "PaintDaubsEvent";
      _map["paletteKnife"] = "PaletteKnifeEvent";
      _map["paste"] = _map["past"] = "PasteEvent";
      _map["pasteEffects"] = "PasteEffectsEvent";
      _map["patchwork"] = "PatchworkEvent";
      _map["photoFilter"] = "PhotoFilterEvent";
      _map["photocopy"] = "PhotocopyEvent";
      _map["pinch"] = "PinchEvent";
      _map["plaster"] = "PlasterEvent";
      _map["PlsW"] = "PlasticWrapEvent";
      _map["play"] = "PlayEvent";
      _map["pointillize"] = "PointillizeEvent";
      _map["polar"] = _map["Plr"] = "PolarEvent";
      _map["posterEdges"] = "PosterEdgesEvent";
      _map["posterization"] = "PosterizationEvent";
      _map["purge"] = _map["Prge"] = "PurgeEvent";
      _map["radialBlur"] = _map["RdlB"] = "RadialBlurEvent";
      _map["rasterizeLayer"] = "RasterizeLayerEvent"; 
      _map["rasterizeTypeLayer"] = _map["RstT"] = "RasterizeTypeLayerEvent"; 
      _map["removeWhiteMatte"] = "RemoveWhiteMatteEvent";
      _map["replaceColor"] = "ReplaceColorEvent";
      _map["reset"] = _map["Rset"] = "ResetEvent";
      _map["reticulation"] = "ReticulationEvent";
      _map["revealAll"] = "RevealAllEvent";
      _map["revert"] = "RevertEvent";
      _map["ripple"] = "RippleEvent";
      _map["rotateEventEnum"] = _map["Rtte"] = "RotateEvent";
      _map["save"] = "SaveEvent";
      _map["scaleEffectsEvent"] = "ScaleEffectsEvent";
      _map["select"] = _map["slct"] = "SelectEvent";
      _map["selectAllLayers"] = "SelectAllLayersEvent";
      _map["selectiveColr"] = "SelectiveColorEvent";
      _map["separationSetup"] = "SeparationSetupEvent";
      _map["set"] = _map["setd"] = "SetEvent";
      _map["sharpen"] = _map["Shrp"] = "SharpenEvent";
      _map["ShrE"] = "SharpenEdgesEvent";
      _map["sharpenMore"] = "SharpenMoreEvent";
      _map["shear"] = "ShearEvent";
      _map["show"] = "ShowEvent";
      _map["smartBlur"] = _map["SmrB"] = "SmartBlurEvent";
      _map["smoothness"] = "SmoothnessEvent";
      _map["solarize"] = "SolarizeEvent";
      _map["spatter"] = "SpatterEvent";
      _map["spherize"] = _map["Sphr"] = "SpherizeEvent";
      _map["subtract"] = "SubtractEvent";
      _map["subtractFrom"] = _map["Sbtr"] = "SubtractFromEvent";
      _map["sumie"] = "SumieEvent";
      _map["surfaceBlur"] = "SurfaceBlurEvent";
      _map["stamp"] = "StampEvent";
      _map["traceContour"] = "TraceContourEvent";
      _map["stop"] = _map["Stop"] = "StopEvent";
      _map["stroke"] = _map["Strk"] = "StrokeEvent";
      _map["stainedGlass"] = _map["StnG"] = "StainedGlassEvent";
      _map["texturizer"] = "TexturizerEvent";
      _map["thresholdClassEvent"] = _map["Thrs"] = "ThresholdClassEvent";
      _map["tiles"] = "TilesEvent";
      _map["tornEdges"] = "TornEdgesEvent";
      _map["transform"] = _map["Trnf"] = "TransformEvent";
      _map["trim"] = "TrimEvent";
      _map["twirl"] = "TwirlEvent";
      _map["ungroup"] = "UngroupEvent";
      _map["unlink"] = "UnlinkEvent";
      _map["unsharpMask"] = _map["UnsM"] = "UnsharpMaskEvent";
      _map["variations"] = "VariationsEvent";
      _map["watercolor"] = "WatercolorEvent";
      _map["waterPaper"] = "WaterPaperEvent";
      _map["wave"] = "WaveEvent";
      _map["wind"] = _map["Wnd"] = "WindEvent";
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
