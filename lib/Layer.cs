using System;
using System.Runtime.InteropServices;

namespace Gimp
{
  public class Layer
  {
    Int32 _layerID;
    
    public Layer(Image image, string name, int width, int height, 
		 ImageType type, double opacity, LayerModeEffects mode)
    {
      _layerID = gimp_layer_new(image.ID, name, width, height, type, 
				opacity, mode);
    }
  
    public Layer(Layer layer)
    {
      _layerID = gimp_layer_copy(layer.ID);
    }

    public Layer(Drawable drawable, Image dest_image)
    {
      _layerID = gimp_layer_new_from_drawable(drawable.ID, dest_image.ID);
    }

    public Layer(Int32 layerID)
    {
      _layerID = layerID;
    }

    public double Opacity
    {
      get {return gimp_layer_get_opacity (_layerID);}
      set {gimp_layer_set_opacity (_layerID, value);}
    }

    public bool Translate(int offx, int offy)
    {
      return gimp_layer_translate (_layerID, offx, offy);
    }
  
    public Int32 ID
    {
      get {return _layerID;}
    }

    [DllImport("libgimp-2.0.so")]
    static extern Int32 gimp_layer_new (Int32 image_ID,
					string name,
					int width,
					int height,
					ImageType type,
					double opacity,
					LayerModeEffects mode);
    [DllImport("libgimp-2.0.so")]
    static extern Int32 gimp_layer_copy (Int32 layer_ID);
    [DllImport("libgimp-2.0.so")]
    static extern double gimp_layer_get_opacity (Int32 layer_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_layer_set_opacity (Int32 layer_ID,
					       double opacity);
    [DllImport("libgimp-2.0.so")]
    static extern Int32 gimp_layer_new_from_drawable (Int32 drawable_ID,
						      Int32 dest_image_ID);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_layer_translate (Int32 layer_ID, int offx, 
					     int offy);
  }
  }
