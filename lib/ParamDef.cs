using System;
using System.Diagnostics;

namespace Gimp
  {
    public class ParamDef
    {
      GimpParamDef _paramDef = new GimpParamDef();

      public ParamDef(PDBArgType type, string name, string description)
      {
	_paramDef.type = type;
	_paramDef.name = name;
	_paramDef.description = description;
      }

      public GimpParamDef GimpParamDef
      {
	get {return _paramDef;}
      }
    }
  }
