using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
  public class Context
    {
    static public void Push()
      {
      if (!gimp_context_push())
        {
        throw new Exception();
        }
      }

    static public RGB Foreground
      {
      get
          {
          GimpRGB rgb = new GimpRGB();
          if (!gimp_context_get_foreground(out rgb))
            {
            throw new Exception();
            }
          return new RGB(rgb);
          }
      set
          {
          if (!gimp_context_set_foreground(value.GimpRGB))
            {
            throw new Exception();
            }
          }
      }

    static public RGB Background
      {
      get
          {
          GimpRGB rgb = new GimpRGB();
          if (!gimp_context_get_background(out rgb))
            {
            throw new Exception();
            }
          return new RGB(rgb);
          }
      set
          {
          if (!gimp_context_set_background(value.GimpRGB))
            {
            throw new Exception();
            }
          }
      }

    static public void SetDefaultColors()
      {
      if (!gimp_context_set_default_colors())
        {
        throw new Exception();
        }
      }

    static public void SwapColors()
      {
      if (!gimp_context_swap_colors())
        {
        throw new Exception();
        }
      }

    public static double Opacity
      {
      get {return gimp_context_get_opacity();}
      set
          {
          if (!gimp_context_set_opacity(value))
            {
            throw new Exception();
            }
          }
      }

    public static LayerModeEffects PaintMode
      {
      get {return gimp_context_get_paint_mode();}
      set
          {
          if (!gimp_context_set_paint_mode(value))
            {
            throw new Exception();
            }
          }
      }

    public static string Brush
      {
      get {return gimp_context_get_brush();}
      set
          {
          if (!gimp_context_set_brush(value))
            {
            throw new Exception();
            }
          }
      }

    public static string Pattern
      {
      get {return gimp_context_get_pattern();}
      set
          {
          if (!gimp_context_set_pattern(value))
            {
            throw new Exception();
            }
          }
      }

    public static string Gradient
      {
      get {return gimp_context_get_gradient();}
      set
          {
          if (!gimp_context_set_gradient(value))
            {
            throw new Exception();
            }
          }
      }

    public static string Palette
      {
      get {return gimp_context_get_palette();}
      set
          {
          if (!gimp_context_set_palette(value))
            {
            throw new Exception();
            }
          }
      }

    public static string Font
      {
      get {return gimp_context_get_font();}
      set
          {
          if (!gimp_context_set_font(value))
            {
            throw new Exception();
            }
          }
      }

    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_push();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_pop();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_get_foreground(out GimpRGB foreground);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_set_foreground(GimpRGB foreground);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_get_background(out GimpRGB background);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_set_background(GimpRGB background);
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_set_default_colors();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_swap_colors();
    [DllImport("libgimp-2.0.so")]
    static extern double gimp_context_get_opacity();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_set_opacity(double opacity);
    [DllImport("libgimp-2.0.so")]
    static extern LayerModeEffects gimp_context_get_paint_mode();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_set_paint_mode(
      LayerModeEffects paint_mode);
    [DllImport("libgimp-2.0.so")]
    static extern string gimp_context_get_brush();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_set_brush(string name);
    [DllImport("libgimp-2.0.so")]
    static extern string gimp_context_get_pattern();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_set_pattern(string name);
    [DllImport("libgimp-2.0.so")]
    static extern string gimp_context_get_gradient();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_set_gradient(string name);
    [DllImport("libgimp-2.0.so")]
    static extern string gimp_context_get_palette();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_set_palette(string name);
    [DllImport("libgimp-2.0.so")]
    static extern string gimp_context_get_font();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_context_set_font(string name);
    }
  }
