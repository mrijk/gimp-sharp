// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// FilePlugin.cs
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

namespace Gimp
{
  public abstract class FilePlugin : Plugin
  {
    Procedure _loadProcedure;
    Procedure _saveProcedure;

    public FilePlugin(string[] args) : base(args)
    {
    }

    override protected void Run(string name, ParamDefList inParam,
				out ParamDefList outParam)
    {
      outParam = new ParamDefList(true);
      outParam.Add(new ParamDef(PDBStatusType.Success, 
				typeof(PDBStatusType)));

      if (_loadProcedure != null && _loadProcedure.Name == name)
	{
	  string filename = (string) inParam[1].Value;

	  Image image = Load(filename);
	  if (image == null)
	    {
	      outParam[0].Value = PDBStatusType.ExecutionError;
	    }
	  else
	    {
	      outParam.Add(new ParamDef(image, typeof(Image)));
	    }
	}
      else if (_saveProcedure != null && _saveProcedure.Name == name)
	{
	  Image image = (Image) inParam[1].Value;
	  Drawable drawable = (Drawable) inParam[2].Value;
	  string filename = (string) inParam[3].Value;

	  if (!Save(image, drawable, filename))
	    {
	      outParam[0].Value = PDBStatusType.ExecutionError;
	    }
	}
    }

    protected Procedure FileLoadProcedure(string name, string blurb, 
					  string help, string author, 
					  string copyright, string date, 
					  string menu_path)
    {
      ParamDefList inParams = new ParamDefList(true);
      inParams.Add(new ParamDef("run_mode", typeof(Int32), 
				"Interactive, non-interactive"));
      inParams.Add(new ParamDef("filename", typeof(FileName), 
				"The name of the file to load"));
      inParams.Add(new ParamDef("raw_filename", typeof(FileName), 
				"The name entered"));

      ParamDefList outParams = new ParamDefList(true);
      outParams.Add(new ParamDef("image", typeof(Image), 
				 "Output image"));

      _loadProcedure = new Procedure(name, blurb, help, author, copyright, 
				     date, menu_path, null, 
				     inParams, outParams);

      return _loadProcedure;
    } 

    protected Procedure FileSaveProcedure(string name, string blurb, 
					  string help, string author, 
					  string copyright, string date, 
					  string menu_path,
					  string image_types)
    {
      ParamDefList inParams = new ParamDefList(true);
      inParams.Add(new ParamDef("run_mode", typeof(Int32), 
				"Interactive, non-interactive"));
      inParams.Add(new ParamDef("image", typeof(Image), 
				"Input image"));
      inParams.Add(new ParamDef("drawable", typeof(Drawable), 
				"Drawable to save"));
      inParams.Add(new ParamDef("filename", typeof(FileName),
				"The name of the file to save the image in"));
      inParams.Add(new ParamDef("raw_filename", typeof(FileName),
				"The name of the file to save the image in"));

      _saveProcedure = new Procedure(name, blurb, help, author, copyright, 
				     date, menu_path, image_types, inParams);

      return _saveProcedure;
    }

    virtual protected Image Load(string filename)
    {
      return null;
    }

    virtual protected bool Save(Image image, Drawable drawable, 
				string filename)
    {
      return false;
    }

    protected Image NewImage(int width, int height, ImageBaseType baseType,
			     ImageType type, string filename)
    {
      Image image = new Image(width, height, baseType);
      Layer layer = new Layer(image, "Background", width, height, type,
			      100, LayerModeEffects.Normal);
      image.AddLayer(layer, 0);
      image.Filename = filename;

      return image;
    }

    protected void RegisterLoadHandler(string extensions, string prefixes)
    {
      Gimp.RegisterLoadHandler(_loadProcedure.Name, extensions, prefixes);
    }

    protected void RegisterSaveHandler(string extensions, string prefixes)
    {
      Gimp.RegisterSaveHandler(_saveProcedure.Name, extensions, prefixes);
    }

    protected void RegisterFileHandlerMime(string procedural_name,
					   string mime_type)
    {
      Gimp.RegisterFileHandlerMime(procedural_name, mime_type);
    }
  }
}
