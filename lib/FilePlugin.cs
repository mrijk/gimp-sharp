// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
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
using System.IO;

namespace Gimp
{
  public abstract class FilePlugin : Plugin
  {
    Procedure _loadProcedure;
    Procedure _saveProcedure;

    protected string Filename {get; set;}
    protected BinaryReader Reader {get; set;}

    public FilePlugin(string[] args, string package) : base(args, package)
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
	  LoadFile(inParam, outParam);
	}
      else if (_saveProcedure != null && _saveProcedure.Name == name)
	{
	  SaveFile(inParam, outParam);
	}
    }

    void LoadFile(ParamDefList inParam, ParamDefList outParam)
    {
      Filename = (string) inParam[1].Value;

      if (File.Exists(Filename))
	{
	  Reader = new BinaryReader(File.Open(Filename, FileMode.Open));
	  var image = Load();
	  if (image == null)
	    {
	      outParam[0].Value = PDBStatusType.ExecutionError;
	    }
	  else
	    {
	      outParam.Add(new ParamDef(image, typeof(Image)));
	    }
	  Reader.Close();
	}
      else
	{
	  outParam[0].Value = PDBStatusType.ExecutionError;
	}
    }

    virtual protected Image Load()
    {
      return null;
    }

    protected byte[] ReadBytes(int count)
    {
      return Reader.ReadBytes(count);
    }

    protected byte ReadByte()
    {
      return Reader.ReadByte();
    }

    void SaveFile(ParamDefList inParam, ParamDefList outParam)
    {
      var image = (Image) inParam[1].Value;
      var drawable = (Drawable) inParam[2].Value;
      string filename = (string) inParam[3].Value;
      
      if (!Save(image, drawable, filename))
	{
	  outParam[0].Value = PDBStatusType.ExecutionError;
	}
    }

    protected Procedure FileLoadProcedure(string name, string blurb, 
					  string help, string author, 
					  string copyright, string date, 
					  string menu_path)
    {
      var inParams = new ParamDefList(true) {
	new ParamDef("run_mode", typeof(Int32),
		     "Interactive, non-interactive"),
	new ParamDef("filename", typeof(FileName), 
		     "The name of the file to load"),
	new ParamDef("raw_filename", typeof(FileName), 
		     "The name entered")};

      var outParams = new ParamDefList(true) {
	new ParamDef("image", typeof(Image), "Output image")};

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
      var inParams = new ParamDefList(true) {
	new ParamDef("run_mode", typeof(Int32), 
		     "Interactive, non-interactive"),
	new ParamDef("image", typeof(Image), 
		     "Input image"),
	new ParamDef("drawable", typeof(Drawable), 
		     "Drawable to save"),
	new ParamDef("filename", typeof(FileName),
		     "The name of the file to save the image in"),
	new ParamDef("raw_filename", typeof(FileName),
		     "The name of the file to save the image in")};

      _saveProcedure = new Procedure(name, blurb, help, author, copyright, 
				     date, menu_path, image_types, inParams);

      return _saveProcedure;
    }

    virtual protected bool Save(Image image, Drawable drawable, 
				string filename)
    {
      return false;
    }

    protected Image NewImage(int width, int height, ImageBaseType baseType,
			     ImageType type, string filename)
    {
      var image = new Image(width, height, baseType) {Filename = filename};
      var layer = new Layer(image, "Background", type);
      image.AddLayer(layer, 0);
      
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
