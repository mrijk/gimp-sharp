// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
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
      _map["accentedEdges"] = _map["AccE"] = "AccentedEdgesEvent";
      _map["adaptCorrect"] = "AdaptCorrectEvent";
      _map["add"] = _map["Add"] = "AddEvent";
      _map["addNoise"] = _map["AdNs"] = "AddNoiseEvent";
      _map["addTo"] = _map["AddT"] = "AddToEvent";
      _map["Alien Skin Eye Candy 4000 Shadowlab"] = 
	_map["Alien Skin Eye Candy 4000 Bevel Boss"] = 
	_map["Alien Skin Eye Candy 4000 Chrome"] = 
	_map["Alien Skin Eye Candy 4000 Drip"] = 
	_map["Alien Skin Eye Candy 4000 Fire"] = 
	_map["Alien Skin Eye Candy 4000 Gradient Glow"] =
	_map["Alien Skin Eye Candy 4000 Jiggle"] = 
	_map["Alien Skin Eye Candy 4000 Melt"] = 
	_map["Alien Skin Eye Candy 4000 Smoke"] = 
	_map["Alien Skin Eye Candy 4000 Water Drops"] = 
	_map["Alien Skin Eye Candy 4000 Wood"] = 
	_map["Alien Skin Eye Candy 4000 Glass"] = 
	_map["Alien Skin Splat Resurface"] = "ExternalPluginEvent";
      _map["align"] = _map["Algn"] = "AlignEvent";
      _map["angledStrokes"] = _map["AngS"] = "AngledStrokesEvent";
      _map["applyImageEvent"] = "ApplyImageEvent";
      _map["applyStyle"] = "ApplyStyleEvent";
      _map["Avrg"] = "AverageEvent";
      _map["basRelief"] = _map["BsRl"] = "BasReliefEvent";
      _map["blurEvent"] = _map["blur"] = _map["Blr"] = "BlurEvent";
      _map["blurMethod"] = _map["BlrM"] = "BlurMethodEvent";
      _map["border"] = _map["Brdr"] = "BorderEvent";
      _map["brightnessEvent"] = _map["BrgC"] = "BrightnessEvent";
      _map["canvasSize"] = _map["CnvS"] = "CanvasSizeEvent";
      _map["channelMixer"] = _map["ChnM"] = "ChannelMixerEvent";
      _map["chalkCharcoal"] = _map["ChlC"] = "ChalkCharcoalEvent";
      _map["chrome"] = _map["Chrm"] = "ChromeEvent";
      _map["clearEvent"] = _map["Cler"] = "ClearEvent";
      _map["close"] = _map["Cls"] = "CloseEvent";
      _map["clouds"] = _map["Clds"] = "CloudsEvent";
      _map["colorBalance"] = _map["ClrB"] = "ColorBalanceEvent";
      _map["colorHalftone"] = _map["ClrH"] = "ColorHalftoneEvent";
      _map["colorPalette"] = "ColorPaletteEvent";
      _map["colorRange"] = _map["ClrR"] = "ColorRangeEvent";
      _map["contract"] = _map["Cntc"] = "ContractEvent";
      _map["convertMode"] = _map["CnvM"] = "ConvertModeEvent";
      _map["copyEffects"] = _map["CpFX"] = "CopyEffectsEvent";
      _map["copyEvent"] = _map["copy"] = "CopyEvent";
      _map["copyMerged"] = _map["CpyM"] = "CopyMergedEvent";
      _map["copyToLayer"] = _map["CpTL"] = "CopyToLayerEvent";
      _map["craquelure"] = _map["Crql"] = "CraquelureEvent";
      _map["crop"] = _map["Crop"] = "CropEvent";
      _map["crosshatch"] = _map["Crsh"] = "CrosshatchEvent";
      _map["crystallize"] = _map["Crst"] = "CrystallizeEvent";
      _map["curves"] = _map["Crvs"] = "CurvesEvent";
      _map["cut"] = "CutEvent";
      _map["cutout"] = _map["Ct"] = "CutoutEvent";
      _map["cutToLayer"] = _map["CtTL"] = "CutToLayerEvent";
      _map["darkStrokes"] = _map["DrkS"] = "DarkStrokesEvent";
      _map["definePattern"] = _map["DfnP"] = "DefinePatternEvent";
      _map["defringe"] = "DefringeEvent";
      _map["deInterlace"] = "DeInterlaceEvent";
      _map["delete"] = _map["Dlt"] = "DeleteEvent";
      _map["desaturate"] = _map["Dstt"] = "DesaturateEvent";
      _map["deselect"] = "DeselectEvent";
      _map["despeckle"] = _map["Dspc"] = "DespeckleEvent";
      _map["differenceClouds"] = _map["DfrC"] = "DifferenceCloudsEvent";
      _map["diffuseGlow"] = _map["DfsG"] = "DiffuseGlowEvent";
      _map["diffuse"] = _map["Dfs"] = "DiffuseEvent";
      _map["displace"] = "DisplaceEvent";
      _map["dlfx"] = "ClearEffectsEvent";
      _map["draw"] = _map["Draw"] = "DrawEvent";
      _map["dryBrush"] = _map["DryB"] = "DryBrushEvent";
      _map["duplicate"] = _map["Dplc"] = "DuplicateEvent";
      _map["dustAndScratches"] = _map["DstS"] = "DustAndScratchesEvent";
      _map["eccA"] = _map["eccH"] = _map["ecdS"] = _map["ecfI"] = 
	_map["ecgL"] = _map["ecgO"] = _map["eciN"] = _map["ecjI"] = 
	_map["ecmT"] = _map["ecoU"] = _map["ecpS"] = _map["ecwD"] = 
	_map["ecwE"] = "ExternalPluginEvent";
      _map["editInImageReady"] = "ExternalPluginEvent";
      _map["emboss"] = _map["Embs"] = "EmbossEvent";
      _map["equalize"] = _map["Eqlz"] = "EqualizeEvent";
      _map["exchange"] = _map["Exch"] = "ExchangeEvent";
      _map["expand"] = _map["Expn"] = "ExpandEvent";
      _map["extrude"] = _map["Extr"] = "ExtrudeEvent";
      _map["facet"] = _map["Fct"] = "FacetEvent";
      _map["fade"] = _map["Fade"] = "FadeEvent";
      _map["feather"] = _map["Fthr"] = "FeatherEvent";
      _map["fill"] = _map["Fl"] = "FillEvent";
      _map["filmGrain"] = _map["FlmG"] = "FilmGrainEvent";
      _map["filter"] = _map["Fltr"] = "FilterEvent";
      _map["findEdges"] = _map["FndE"] =  "FindEdgesEvent";
      _map["flattenImage"] = _map["FltI"] = "FlattenImageEvent";
      _map["flip"] = _map["Flip"] = "FlipEvent";
      _map["fragment"] = _map["Frgm"] = "FragmentEvent";
      _map["fresco"] = "FrescoEvent";
      _map["GEfc"] = "FilterGalleryEvent";
      _map["gaussianBlur"] = _map["GsnB"] = _map["GblR"] = "GaussianBlurEvent";
      _map["glass"] = _map["Gls"] = "GlassEvent";
      _map["glowingEdges"] = _map["GlwE"] = "GlowingEdgesEvent";
      _map["gradientClassEvent"] = _map["Grdn"] = "GradientClassEvent";
      _map["gradientMapEvent"] = "GradientMapEvent";
      _map["grain"] = _map["Grn"] = "GrainEvent";
      _map["graphicPen"] = "GraphicPenEvent";
      _map["groupEvent"] = "GroupEvent";
      _map["grow"] = _map["Grow"] = "GrowEvent";
      _map["halftoneScreen"] = _map["HlfS"] = "HalftoneScreenEvent";
      _map["hide"] = _map["Hd"] = "HideEvent";
      _map["highPass"] = _map["HghP"] = "HighPassEvent";
      _map["hueSaturation"] = _map["HStr"] = "HueSaturationEvent";
      _map["imageSize"] = _map["ImgS"] = "ImageSizeEvent";
      _map["inkOutlines"] = _map["InkO"] = "InkOutlinesEvent";
      _map["interfaceWhite"] = "IntersectWithEvent";
      _map["inverse"] = _map["Invs"] = "InverseEvent";
      _map["invert"] = _map["Invr"] = "InvertEvent";
      _map["lensFlare"] = _map["LnsF"] = "LensFlareEvent";
      _map["levels"] = _map["Lvls"] = "LevelsEvent";
      _map["lightingEffects"] = _map["LghE"] = "LightingEffectsEvent";
      _map["link"] = _map["Lnk"] = "LinkEvent";
      _map["linkSelectedLayers"] = "LinkSelectedLayersEvent";
      _map["make"] = _map["Mk"] = "MakeEvent";
      _map["maximum"] = _map["Mxm"] = "MaximumEvent";
      _map["median"] = _map["Mdn"] = "MedianEvent";
      _map["mergeLayers"] = _map["MrgL"] = "MergeLayersEvent";
      _map["mergeLayersNew"] = "MergeLayersNewEvent";
      _map["mergeVisible"] = _map["MrgV"] = "MergeVisibleEvent";
      _map["MetaCreations KPT Materializer"] = 
	_map["MetaCreations KPT5 ShapeShifter"] = 
	_map["MetaCreations KPT5   Blurrrr"] = 
	_map["MetaCreations KPT Gel"] = 
	_map["MetaCreations KPT5  FraxPlorer"] = 
	_map["MetaCreations KPT LensFlare"] = "ExternalPluginEvent";
      _map["mezzotint"] = _map["Mztn"] = "MezzotintEvent";
      _map["minimum"] = _map["Mnm"] = "MinimumEvent";
      _map["mosaic"] = _map["Msc"] = "MosaicEvent";
      _map["mosaicPlugin"] = "MosaicPluginEvent";
      _map["move"] = "MoveEvent";
      _map["motionBlur"] = _map["MtnB"] = "MotionBlurEvent";
      _map["MscT"] = "MosaicTilesEvent";
      _map["ncbE"] = _map["nccL"] = _map["nccR"] = _map["ncdO"] = 
	_map["ncdI"] = _map["nceL"] = _map["ncfL"] = _map["nclI"] = 
	_map["ncsM"] = _map["ncsT"] = _map["nctV"] = "ExternalPluginEvent";
      _map["NGlw"] = "NeonGlowEvent";
      _map["notePaper"] = _map["NtPr"] = "NotePaperEvent";
      _map["NTSC"] = "NTSCColorsEvent";
      _map["oceanRipple"] = _map["OcnR"] = "OceanRippleEvent";
      _map["offset"] = _map["Ofst"] = "OffsetEvent";
      _map["open"] = _map["Opn"] = "OpenEvent";
      _map["paintDaubs"] = _map["PntD"] = "PaintDaubsEvent";
      _map["paletteKnife"] = "PaletteKnifeEvent";
      _map["paste"] = _map["past"] = "PasteEvent";
      _map["pasteEffects"] = _map["PaFX"] = "PasteEffectsEvent";
      _map["PstI"] = "PasteIntoEvent";
      _map["patchwork"] = _map["Ptch"] = "PatchworkEvent";
      _map["photoFilter"] = "PhotoFilterEvent";
      _map["photocopy"] = _map["Phtc"] = "PhotocopyEvent";
      _map["pinch"] = _map["Pnch"] = "PinchEvent";
      _map["plaster"] = _map["Plst"] = "PlasterEvent";
      _map["plasticWrap"] = _map["PlsW"] = "PlasticWrapEvent";
      _map["play"] = _map["Ply"] = "PlayEvent";
      _map["pointillize"] = "PointillizeEvent";
      _map["polar"] = _map["Plr"] = "PolarEvent";
      _map["posterEdges"] = _map["PstE"] = "PosterEdgesEvent";
      _map["posterization"] = "PosterizationEvent";
      _map["purge"] = _map["Prge"] = "PurgeEvent";
      _map["radialBlur"] = _map["RdlB"] = "RadialBlurEvent";
      _map["rasterizeAll"] = "RasterizeAllEvent"; 
      _map["rasterizeLayer"] = "RasterizeLayerEvent"; 
      _map["rasterizeTypeLayer"] = _map["RstT"] = "RasterizeTypeLayerEvent"; 
      _map["removeBlackMatte"] = "RemoveBlackMatteEvent";
      _map["removeWhiteMatte"] = "RemoveWhiteMatteEvent";
      _map["replaceColor"] = _map["RplC"] = "ReplaceColorEvent";
      _map["reset"] = _map["Rset"] = "ResetEvent";
      _map["reticulation"] = _map["Rtcl"] = "ReticulationEvent";
      _map["revealAll"] = "RevealAllEvent";
      _map["revert"] = "RevertEvent";
      _map["ripple"] = _map["Rple"] = "RippleEvent";
      _map["rotateEventEnum"] = _map["Rtte"] = "RotateEvent";
      _map["roughPastels"] = _map["RghP"] = "RoughPastelsEvent";
      _map["save"] = "SaveEvent";
      _map["scaleEffectsEvent"] = "ScaleEffectsEvent";
      _map["select"] = _map["slct"] = "SelectEvent";
      _map["selectAllLayers"] = "SelectAllLayersEvent";
      _map["selectiveColor"] = _map["SlcC"] = "SelectiveColorEvent";
      _map["separationSetup"] = "SeparationSetupEvent";
      _map["set"] = _map["setd"] = "SetEvent";
      _map["sharpen"] = _map["Shrp"] = "SharpenEvent";
      _map["sharpenEdges"] = _map["ShrE"] = "SharpenEdgesEvent";
      _map["sharpenMore"] = _map["ShrM"] = "SharpenMoreEvent";
      _map["shear"] = _map["Shr"] = "ShearEvent";
      _map["show"] = _map["Shw"] = "ShowEvent";
      _map["similar"] = _map["Smlr"] = "SimilarEvent";
      _map["smartBlur"] = _map["SmrB"] = "SmartBlurEvent";
      _map["smoothness"] = _map["Smth"] = "SmoothnessEvent";
      _map["solarize"] = _map["Slrz"] = "SolarizeEvent";
      _map["spatter"] = _map["Spt"] = "SpatterEvent";
      _map["spherize"] = _map["Sphr"] = "SpherizeEvent";
      _map["sponge"] = _map["Spng"] = "SpongeEvent";
      _map["SprS"] = "SprayedStrokesEvent";
      _map["subtract"] = _map["Sbtr"] = "SubtractEvent";
      _map["subtractFrom"] = _map["SbtF"] = "SubtractFromEvent";
      _map["sumie"] = _map["Smie"] = "SumieEvent";
      _map["surfaceBlur"] = "SurfaceBlurEvent";
      _map["stamp"] = _map["Stmp"] = "StampEvent";
      _map["stop"] = _map["Stop"] = "StopEvent";
      _map["stroke"] = _map["Strk"] = "StrokeEvent";
      _map["stainedGlass"] = _map["StnG"] = "StainedGlassEvent";
      _map["texturizer"] = _map["Txtz"] = "TexturizerEvent";
      _map["thresholdClassEvent"] = _map["Thrs"] = "ThresholdClassEvent";
      _map["tiles"] = _map["Tls"] = "TilesEvent";
      _map["tornEdges"] = "TornEdgesEvent";
      _map["traceContour"] = _map["TrcC"] = "TraceContourEvent";
      _map["transform"] = _map["Trnf"] = "TransformEvent";
      _map["trim"] = "TrimEvent";
      _map["twirl"] = _map["Twrl"] = "TwirlEvent";
      _map["underpainting"] = "UnderpaintingEvent";
      _map["ungroup"] = _map["Ungr"] = "UngroupEvent";
      _map["unlink"] = _map["Unlk"] = "UnlinkEvent";
      _map["unsharpMask"] = _map["UnsM"] = "UnsharpMaskEvent";
      _map["variations"] = _map["Vrtn"] = "VariationsEvent";
      _map["watercolor"] = _map["Wtrc"] = "WatercolorEvent";
      _map["waterPaper"] = _map["WtrP"] = "WaterPaperEvent";
      _map["wave"] = _map["Wave"] = "WaveEvent";
      _map["wind"] = _map["Wnd"] = "WindEvent";
      _map["zigZag"] = _map["ZgZg"] = "ZigZagEvent";
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
