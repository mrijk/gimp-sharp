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
#if false
      override protected void Run(string name, GimpParam[] inParam,
				  ref GimpParam[] outParam)
      {
        if (name == _load_procedure_name)
	{
          string filename = Marshal.PtrToStringAuto(inParam[1].data.d_string);

	  outParam = new GimpParam[2];
	  outParam[0].type = PDBArgType.STATUS;
	  outParam[0].data.d_status = PDBStatusType.SUCCESS;
	  outParam[1].type = PDBArgType.IMAGE;
	  
	  Image image = Load(filename);
	  if (image == null)
	    {
	    outParam[0].data.d_status = PDBStatusType.EXECUTION_ERROR;
	    }
	  else
	    { 
	    outParam[1].data.d_image = image.ID;
	    }
	}
	else
	{
          string filename = Marshal.PtrToStringAuto(inParam[3].data.d_string);

	  outParam = new GimpParam[1];
	  outParam[0].type = PDBArgType.STATUS;
	  outParam[0].data.d_status = PDBStatusType.SUCCESS;
	  Save(filename);
	}
      }
#else
      override protected void Run(string name, ParamDefList inParam,
				  out ParamDefList outParam)
      {
	outParam = new ParamDefList();
	outParam.Add(new ParamDef(PDBStatusType.SUCCESS, 
				  typeof(PDBStatusType)));

	if (_loadProcedure != null && _loadProcedure.Name == name)
	  {
	  }
	else
	  {
	  }
      }
#endif
      protected Procedure FileLoadProcedure(string name, string blurb, 
					    string help, string author, 
					    string copyright, string date, 
					    string menu_path)
      {
	ParamDefList inParams = new ParamDefList(true);
	inParams.Add(new ParamDef("run_mode", typeof(Int32), 
				  "Interactive, non-interactive"));
	inParams.Add(new ParamDef("filename", typeof(string), 
				  "The name of the file to load"));
	inParams.Add(new ParamDef("raw_filename", typeof(string), 
				  "The name entered"));

	ParamDefList outParams = new ParamDefList(true);
	outParams.Add(new ParamDef("image", typeof(Image), 
				   "Output image"));

	_loadProcedure = new Procedure(name, blurb, help, author, copyright, 
				       date, menu_path, null, 
				       inParams, outParams);
	// _loadProcedure.Install(false, false);

	return _loadProcedure;
      } 

      protected Procedure FileSaveProcedure(string name, string blurb, 
					    string help, string author, 
					    string copyright, string date, 
					    string menu_path,
					    string image_types)
      {
        ParamDefList inParams = new ParamDefList();
	inParams.Add(new ParamDef("filename", null, typeof(string),
				  "The name of the file to save"));
	inParams.Add(new ParamDef("raw_filename", null, typeof(string),
				  "The name entered"));

	_saveProcedure = new Procedure(name, blurb, help, author, copyright, 
				       date, menu_path, null, inParams);

	return _saveProcedure;
      }

      virtual protected Image Load(string filename)
      {
	return null;
      }

      virtual protected bool Save(string filename)
      {
        return false;
      }
    }
  }
