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

      virtual protected Image Load(string filename)
      {
	return null;
      }
    }
  }
