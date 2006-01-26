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
using System.Runtime.InteropServices;

namespace Gimp
  {
    public abstract class FilePlugin : Plugin
    {
      public FilePlugin(string[] args) : base(args)
      {
      }

      override protected void Run(string name, GimpParam[] inParam,
				  ref GimpParam[] outParam)
      {
	outParam = new GimpParam[2];
	outParam[0].type = PDBArgType.STATUS;
	outParam[0].data.d_status = PDBStatusType.SUCCESS;
	outParam[1].type = PDBArgType.IMAGE;

	string filename = Marshal.PtrToStringAuto(inParam[1].data.d_string);

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

      protected void InstallFileProcedure(string name, string blurb, 
					  string help, string author, 
					  string copyright, string date, 
					  string menu_path)
      {
        GimpParamDef[] load_args = new GimpParamDef[3];
	load_args[0].type = PDBArgType.INT32;
	load_args[0].name = "run_mode";
	load_args[0].description = "Interactive, non-interactive";
	load_args[1].type = PDBArgType.STRING;
	load_args[1].name = "filename";
	load_args[1].description = "The name of the file to load";
	load_args[2].type = PDBArgType.STRING;
	load_args[2].name = "raw_filename";
	load_args[2].description = "The name entered";
	
	GimpParamDef[] load_return_vals = new GimpParamDef[1];
	load_return_vals[0].type = PDBArgType.IMAGE;
	load_return_vals[0].name = "image";
	load_return_vals[0].description = "output image";

	InstallProcedure(name, blurb, help, author, copyright, date,
			 menu_path, null, load_args, load_return_vals);
      } 


      virtual protected Image Load(string filename)
      {
	return null;
      }
    }
  }
