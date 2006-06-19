// The AverageBlur plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// AverageBlur.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;

namespace Gimp.AverageBlur
{
  public class AverageBlur : Plugin
  {
    [STAThread]
    static void Main(string[] args)
    {
      new AverageBlur(args);
    }

    public AverageBlur(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();

      Procedure procedure = new Procedure("plug_in_average_blur",
					  "Average blur",
					  "Average blur",
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2006",
					  "Average",
					  "RGB*, GRAY*",
					  in_params);
      procedure.MenuPath = "<Image>/Filters/Blur";
      // procedure.IconFile = "AverageBlur.png";

      set.Add(procedure);

      return set;
    }
    override protected void Render(Drawable drawable)
    {
      /*
      RgnIterator iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.Progress = new Progress("AVERAGEBLUR");
      iter.IterateDest(new RgnIterator.IterFuncDestFull(DoAVERAGEBLUR));
			
      Display.DisplaysFlush();
      */
    }
  }
}
