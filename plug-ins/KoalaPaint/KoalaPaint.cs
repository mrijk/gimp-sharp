using System;

using Gimp;

namespace Gimp.KoalaPaint
{	
class KoalaPaint : Plugin
{
  [STAThread]
  static void Main(string[] args)
  {
    new KoalaPaint(args);
  }

  public KoalaPaint(string[] args) : base(args)
  {
  }

  override protected void Query()
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

    InstallProcedure("file_koala_paint_load",
		     "loads images of the Koala Paint file format",
		     "This plug-in loads images of the Koala Paint file format.",
		     "Maurits Rijk",
		     "(C) Maurits Rijk",
		     "1999 - 2004",
		     "KoalaPaint Image",
		     null,
		     load_args,
		     load_return_vals);
  }
}
  }
