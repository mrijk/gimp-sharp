// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2016 Maurits Rijk
//
// GimpUnit.cs
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
  public sealed class GimpUnit
  {
    Unit _unit;

    public double Factor => gimp_unit_get_factor(_unit);
    public int Digits => gimp_unit_get_digits(_unit);
    public string Identifier => gimp_unit_get_identifier(_unit);
    public string Symbol => gimp_unit_get_symbol(_unit);
    public string Abbreviation => gimp_unit_get_abbreviation(_unit);
    public string Singular => gimp_unit_get_singular(_unit);
    public string Plural => gimp_unit_get_plural(_unit);

    static public int NumberOfUnits => gimp_unit_get_number_of_units();
    static public int NumberOfBuiltInUnits => 
      gimp_unit_get_number_of_built_in_units();

    public GimpUnit(string identifier, double factor, int digits, 
		    string symbol, string abbreviation, string singular,
		    string plural)
    {
      _unit = gimp_unit_new(identifier, factor, digits, symbol, abbreviation,
			    singular, plural);
    }

    public bool DeletionFlag
    {
      get {return gimp_unit_get_deletion_flag(_unit);}
      set {gimp_unit_set_deletion_flag(_unit, value);}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_unit_get_number_of_units();
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_unit_get_number_of_built_in_units();
    [DllImport("libgimp-2.0-0.dll")]
    static extern Unit gimp_unit_new(string identifier, double factor, 
				     int digits, string symbol, 
				     string abbreviation, string singular,
				     string plural);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_unit_get_deletion_flag(Unit unit);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_unit_set_deletion_flag(Unit unit,
						   bool deletion_flag);
    [DllImport("libgimp-2.0-0.dll")]
    static extern double gimp_unit_get_factor(Unit unit);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_unit_get_digits(Unit unit);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_unit_get_identifier(Unit unit);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_unit_get_symbol(Unit unit);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_unit_get_abbreviation(Unit unit);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_unit_get_singular(Unit unit);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_unit_get_plural(Unit unit);
  }
}
