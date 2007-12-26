// The FSharpSample plug-in
// Copyright (C) 2007 Maurits Rijk
//
// FSharpSample.fs
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

#light

// namespace Gimp.FSharpSample

open System
open System.Collections.Generic

open Gimp

type FSharpSample = class
  inherit Plugin as base

  new(args: string[]) as FSharpSample =
    {
      inherit Plugin(args, "FSharpSample")
    }

  override x.ListProcedures() : seq<Procedure> =
    let procedure = new Procedure("plug_in_fsharp_sample", "Sample F# plug-in: takes the average of all colors", "Sample F# plug-in: takes the average of all colors", "Maurits Rijk", "(C) Maurits Rijk", "2007", "FSharpSample", "RGB*, GRAY*")
    procedure.MenuPath <- "<Image>/Filters/Generic"
    procedure.IconFile <- "FSharpSample.png"
    [procedure] :> seq<Procedure>

  override x.Render(drawable : Drawable) = 
    let iter = new RgnIterator(drawable, RunMode.Interactive)
    iter.Progress <- new Progress("Average")

    let (average : Pixel) = drawable.CreatePixel()
    iter.IterateSrc(fun pixel -> average.Add(pixel) |> ignore)
    average.Divide(iter.Count) |> ignore

    iter.IterateDest(fun () -> average)

end

let main() =
  let len = Array.length Sys.argv - 1
  let argv = Array.sub Sys.argv 1 len
  new FSharpSample(argv) |> ignore

main()
