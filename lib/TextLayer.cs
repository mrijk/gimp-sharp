// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// TextLayer.cs
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
  public class TextLayer : Layer
  {    
    public TextLayer(Image image, double x, double y, string text, int border,
		     bool antialias, double size, SizeType sizeType,
		     string fontname) :
      base(gimp_text_fontname(image.ID, -1, x, y, text, border, antialias,
			      size, sizeType, fontname))
    {    
    }

    public TextLayer(Image image, string text, string fontName, FontSize size) :
      base(gimp_text_layer_new(image.ID, text, fontName, size.Size, size.Unit))
    {
    }

    public string Text
    {
      get {return gimp_text_layer_get_text(ID);}
      set
      {
	if (!gimp_text_layer_set_text(ID, value))
	{
	  throw new GimpSharpException();
	}
      }
    }

    public string Font
    {
      get {return gimp_text_layer_get_font(ID);}
      set
      {
	if (!gimp_text_layer_set_font(ID, value))
	{
	  throw new GimpSharpException();
	}
      }
    }

    public FontSize FontSize
    {
      get
      {
	Unit unit;
	double size = gimp_text_layer_get_font_size(ID, out unit); 
	return new FontSize(size, unit);
      }
      set
      {
	if (!gimp_text_layer_set_font_size(ID, value.Size, value.Unit))
	{
	  throw new GimpSharpException();
	}
      }
    }

    public FontHinting Hinting
    {
      get
      {
	bool autohint;
	bool hinting = gimp_text_layer_get_hinting(ID, out autohint);
	return new FontHinting(hinting, autohint);
      }
      set
      {
	if (!gimp_text_layer_set_hinting(ID, value.Hinting, value.Autohint))
	{
	  throw new GimpSharpException();
	}
      }
    }

    public bool Antialias
    {
      get {return gimp_text_layer_get_antialias(ID);}
      set
      {
	if (!gimp_text_layer_set_antialias(ID, value))
	{
	  throw new GimpSharpException();
	}
      }
    }

    public bool Kerning
    {
      get {return gimp_text_layer_get_kerning(ID);}
      set
      {
	if (!gimp_text_layer_set_kerning(ID, value))
	{
	  throw new GimpSharpException();
	}
      }
    }

    public string Language
    {
      get {return gimp_text_layer_get_language(ID);}
      set
      {
	if (!gimp_text_layer_set_language(ID, value))
	{
	  throw new GimpSharpException();
	}
      }
    }

    public TextDirection BaseDirection
    {
      get {return gimp_text_layer_get_base_direction(ID);}
      set
      {
	if (!gimp_text_layer_set_base_direction(ID, value))
	{
	  throw new GimpSharpException();
	}
      }
    }

    public TextJustification Justification
    {
      get {return gimp_text_layer_get_justification(ID);}
      set
      {
	if (!gimp_text_layer_set_justification(ID, value))
	{
	  throw new GimpSharpException();
	}
      }
    }

    public RGB Color
    {
      get 
	{
          var rgb = new GimpRGB();
          if (!gimp_text_layer_get_color(_ID, ref rgb))
	    {
	      throw new GimpSharpException();
	    }
	  return new RGB(rgb);
	}
      set 
	{
          var rgb = value.GimpRGB;
          if (!gimp_text_layer_set_color(_ID, ref rgb))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public double Indent
    {
      get {return gimp_text_layer_get_indent(ID);}
      set
      {
	if (!gimp_text_layer_set_indent(ID, value))
	{
	  throw new GimpSharpException();
	}
      }
    }

    public double LineSpacing
    {
      get {return gimp_text_layer_get_line_spacing(ID);}
      set
      {
	if (!gimp_text_layer_set_line_spacing(ID, value))
	{
	  throw new GimpSharpException();
	}
      }
    }

    public double LetterSpacing
    {
      get {return gimp_text_layer_get_letter_spacing(ID);}
      set
      {
	if (!gimp_text_layer_set_letter_spacing(ID, value))
	{
	  throw new GimpSharpException();
	}
      }
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_text_fontname(Int32 image_ID,
					   Int32 drawable_ID,
					   double x,
					   double y,
					   string text,
					   int border,
					   bool antialias,
					   double size,
					   SizeType size_type,
					   string fontname);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_text_layer_new(Int32 image_ID,
					    string text,
					    string fontname,
					    double size,
					    Unit unit);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_text_layer_get_text(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_text(Int32 layer_ID, string text);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_text_layer_get_font(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_font(Int32 layer_ID, string font);
    [DllImport("libgimp-2.0-0.dll")]
    static extern double gimp_text_layer_get_font_size(Int32 layer_ID,
						       out Unit unit);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_font_size(Int32 layer_ID, 
						     double font_size,
						     Unit unit);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_get_hinting(Int32 layer_ID,
						   out bool autohint);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_hinting(Int32 layer_ID,
						   bool hinting,
						   bool autohint);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_get_antialias(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_antialias(Int32 layer_ID, 
						     bool antialias);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_get_kerning(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_kerning(Int32 layer_ID, 
						   bool kerning);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_text_layer_get_language(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_language(Int32 layer_ID, 
						    string language);
    [DllImport("libgimp-2.0-0.dll")]
    static extern TextDirection gimp_text_layer_get_base_direction(
       Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_base_direction(Int32 layer_ID, 
					     TextDirection direction);
    [DllImport("libgimp-2.0-0.dll")]
    static extern TextJustification gimp_text_layer_get_justification(
       Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_justification(Int32 layer_ID, 
					     TextJustification justify);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_get_color(Int32 channel_ID,
						 ref GimpRGB color);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_color(Int32 channel_ID,
						 ref GimpRGB color);
    [DllImport("libgimp-2.0-0.dll")]
    static extern double gimp_text_layer_get_indent(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_indent(Int32 layer_ID, 
						  double indent);
    [DllImport("libgimp-2.0-0.dll")]
    static extern double gimp_text_layer_get_line_spacing(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_line_spacing(Int32 layer_ID, 
							double line_spacing);
    [DllImport("libgimp-2.0-0.dll")]
    static extern double gimp_text_layer_get_letter_spacing(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_text_layer_set_letter_spacing(Int32 layer_ID, 
							  double letter_spacing);
  }
}
