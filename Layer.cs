using System;
using System.Runtime.InteropServices;

namespace Gimp
{
	public enum ImageType
	{
		RGB_IMAGE,
		RGBA_IMAGE,
		GRAY_IMAGE,
		GRAYA_IMAGE,
		INDEXED_IMAGE,
		INDEXEDA_IMAGE
	}

	public enum LayerModeEffects
	{
		NORMAL_MODE,
		DISSOLVE_MODE,
		BEHIND_MODE,
		MULTIPLY_MODE,
		SCREEN_MODE,
		OVERLAY_MODE,
		DIFFERENCE_MODE,
		ADDITION_MODE,
		SUBTRACT_MODE,
		DARKEN_ONLY_MODE,
		LIGHTEN_ONLY_MODE,
		HUE_MODE,
		SATURATION_MODE,
		COLOR_MODE,
		VALUE_MODE,
		DIVIDE_MODE,
		DODGE_MODE,
		BURN_MODE,
		HARDLIGHT_MODE,
		SOFTLIGHT_MODE,
		GRAIN_EXTRACT_MODE,
		GRAIN_MERGE_MODE,
		COLOR_ERASE_MODE
	}

	public class Layer
	{
		[DllImport("libgimp-2.0.so")]
		static extern Int32 gimp_layer_new (Int32 image_ID,
			string name,
			int width,
			int height,
			ImageType type,
			double opacity,
			LayerModeEffects mode);

		Int32 _layerID;

		public Layer(Image image, string name, int width, int height, ImageType type,
			double opacity, LayerModeEffects mode)
		{
			_layerID = gimp_layer_new(image.ID, name, width, height, type, opacity, mode);
		}

		public Int32 ID
		{
			get {return _layerID;}
		}
	}
}
