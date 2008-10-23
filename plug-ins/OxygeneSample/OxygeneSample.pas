{
// The OxygenSample plug-in
// Copyright (C) 2008 Maurits Rijk
//
// OxygenSample.pas
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
}

namespace Gimp.OxygeneSample;

interface

uses
  System.Collections.Generic,
  System.Collections,
  Gimp;

type
   OxygeneSample = public class(Plugin)
   public
     class method Main(args: array of string);

     constructor(args: array of string);
     method ListProcedures : IEnumerable<Gimp.Procedure>; override;
     method Render(drawable: Drawable); override;
   end;                     

implementation

class method OxygeneSample.Main(args: array of string);
begin
   new OxygeneSample(args);
end;

constructor OxygeneSample(args: array of string);
begin
   inherited constructor(args, "OxygeneSample");
end;

method OxygeneSample.ListProcedures : IEnumerable<Gimp.Procedure>;
begin
   var proc: Gimp.Procedure := new Gimp.Procedure("plug_in_oxygene_sample",
                                  "Sample Oxygene plug-in : takes the average of all colors",
                                  "Sample Oxygene plug-in : takes the average of all colors",
                                  "Maurits Rijk",
                                  "(C) Maurits Rijk",
                                  "2006-2008",
                                  _("OxygeneSample"),
                                  "RGB*, GRAY*");
   var list       : List<Gimp.Procedure> := new List<Gimp.Procedure>();
   list.Add(proc);
   proc.MenuPath := "<Image>/Filters/Generic";
   exit(list);
end;

method OxygeneSample.Render(drawable: Drawable);
begin
   var iter : RgnIterator := new RgnIterator(drawable, RunMode.Interactive);

   iter.Progress := new Progress("Average");

   var average : Pixel := drawable.CreatePixel();
   iter.IterateSrc(method(pixel : Pixel);
                   begin
                     average.Add(pixel);
                   end);

   average.Divide(iter.Count);

   iter.IterateDest(method : Pixel;
                    begin
                      exit(average);
                    end);
   end;
   
end.