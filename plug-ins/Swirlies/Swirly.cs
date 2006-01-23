// The Swirlies plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// Swirly.cs
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

namespace Gimp.Swirlies
  {
  public class Swirly
    {
    static Random random = new Random();
    
    const double ANGLE_MULTIPLIER_MAX = 3.0;
    
    const double LINEAR_PHASE_SHIFT_MAX = 0.3;
    
    const double SECONDARY_COEFFICIENT_MAX = Math.PI;
    
    const double MIXED_COEFFICIENT_MAX = SECONDARY_COEFFICIENT_MAX / 400.0;
    
    double _x, _y;

    double r, g, b;
    
    double C, S, // other coefficients ...
	  
      Pc, Ac, Dc, Cc, Sc, Kc, Zc, Ps, As, Ds, Cs, Ss, Ks, Zs,
      
      Pcc, Acc, Dcc, Ccc, Scc, Kcc, Zcc, Pcs, Acs, Dcs, Ccs, Scs, Kcs,
      Zcs, Pck, Ack, Dck, Cck, Sck, Kck, Zck, Pcz, Acz, Dcz, Ccz, Scz,
      Kcz, Zcz, Psc, Asc, Dsc, Csc, Ssc, Ksc, Zsc, Pss, Ass, Dss, Css,
      Sss, Kss, Zss, Psk, Ask, Dsk, Csk, Ssk, Ksk, Zsk, Psz, Asz, Dsz,
      Csz, Ssz, Ksz, Zsz,
      
      Pccc, Accc, Dccc, Pccs, Accs, Dccs, Pcck, Acck, Dcck, Pccz, Accz,
      Dccz, Pcsc, Acsc, Dcsc, Pcss, Acss, Dcss, Pcsk, Acsk, Dcsk, Pcsz,
      Acsz, Dcsz, Pckc, Ackc, Dckc, Pcks, Acks, Dcks, Pckk, Ackk, Dckk,
      Pckz, Ackz, Dckz, Pczc, Aczc, Dczc, Pczs, Aczs, Dczs, Pczk, Aczk,
      Dczk, Pczz, Aczz, Dczz, Pscc, Ascc, Dscc, Pscs, Ascs, Dscs, Psck,
      Asck, Dsck, Pscz, Ascz, Dscz, Pssc, Assc, Dssc, Psss, Asss, Dsss,
      Pssk, Assk, Dssk, Pssz, Assz, Dssz, Pskc, Askc, Dskc, Psks, Asks,
      Dsks, Pskk, Askk, Dskk, Pskz, Askz, Dskz, Pszc, Aszc, Dszc, Pszs,
      Aszs, Dszs, Pszk, Aszk, Dszk, Pszz, Aszz, Dszz;

    static public Swirly CreateRandom() 
      {
      Swirly swirly = new Swirly();
      
      swirly._x = random.NextDouble();
      swirly._y = random.NextDouble();

      swirly.r = drand48s();
      swirly.g = drand48s();
      swirly.b = drand48s();

      swirly.C = random.NextDouble();
      swirly.S = random.NextDouble();

      swirly.Pc = rand_phase_shift();
      swirly.Ac = rand_angle_multiplier(1, 0);
      swirly.Dc = rand_linear_phase_shift(1, 0);
      swirly.Cc = rand_secondary_coefficient(1, 0);
      swirly.Sc = rand_secondary_coefficient(1, 0);
      swirly.Kc = rand_mixed_coefficient(1, 0);
      swirly.Zc = rand_mixed_coefficient(1, 0);
      swirly.Ps = rand_phase_shift();
      swirly.As = rand_angle_multiplier(1, 0);
      swirly.Ds = rand_linear_phase_shift(1, 0);
      swirly.Cs = rand_secondary_coefficient(1, 0);
      swirly.Ss = rand_secondary_coefficient(1, 0);
      swirly.Ks = rand_mixed_coefficient(1, 0);
      swirly.Zs = rand_mixed_coefficient(1, 0);

      swirly.Pcc = rand_phase_shift();
      swirly.Acc = rand_angle_multiplier(2, 0);
      swirly.Dcc = rand_linear_phase_shift(2, 0);
      swirly.Ccc = rand_secondary_coefficient(2, 0);
      swirly.Scc = rand_secondary_coefficient(2, 0);
      swirly.Kcc = rand_mixed_coefficient(2, 0);
      swirly.Zcc = rand_mixed_coefficient(2, 0);
      swirly.Pcs = rand_phase_shift();
      swirly.Acs = rand_angle_multiplier(2, 0);
      swirly.Dcs = rand_linear_phase_shift(2, 0);
      swirly.Ccs = rand_secondary_coefficient(2, 0);
      swirly.Scs = rand_secondary_coefficient(2, 0);
      swirly.Kcs = rand_mixed_coefficient(2, 0);
      swirly.Zcs = rand_mixed_coefficient(2, 0);
      swirly.Pck = rand_phase_shift();
      swirly.Ack = rand_angle_multiplier(1, 1);
      swirly.Dck = rand_linear_phase_shift(1, 1);
      swirly.Cck = rand_secondary_coefficient(1, 1);
      swirly.Sck = rand_secondary_coefficient(1, 1);
      swirly.Kck = rand_mixed_coefficient(1, 1);
      swirly.Zck = rand_mixed_coefficient(1, 1);
      swirly.Pcz = rand_phase_shift();
      swirly.Acz = rand_angle_multiplier(1, 1);
      swirly.Dcz = rand_linear_phase_shift(1, 1);
      swirly.Ccz = rand_secondary_coefficient(1, 1);
      swirly.Scz = rand_secondary_coefficient(1, 1);
      swirly.Kcz = rand_mixed_coefficient(1, 1);
      swirly.Zcz = rand_mixed_coefficient(1, 1);
      swirly.Psc = rand_phase_shift();
      swirly.Asc = rand_angle_multiplier(2, 0);
      swirly.Dsc = rand_linear_phase_shift(2, 0);
      swirly.Csc = rand_secondary_coefficient(2, 0);
      swirly.Ssc = rand_secondary_coefficient(2, 0);
      swirly.Ksc = rand_mixed_coefficient(2, 0);
      swirly.Zsc = rand_mixed_coefficient(2, 0);
      swirly.Pss = rand_phase_shift();
      swirly.Ass = rand_angle_multiplier(2, 0);
      swirly.Dss = rand_linear_phase_shift(2, 0);
      swirly.Css = rand_secondary_coefficient(2, 0);
      swirly.Sss = rand_secondary_coefficient(2, 0);
      swirly.Kss = rand_mixed_coefficient(2, 0);
      swirly.Zss = rand_mixed_coefficient(2, 0);
      swirly.Psk = rand_phase_shift();
      swirly.Ask = rand_angle_multiplier(1, 1);
      swirly.Dsk = rand_linear_phase_shift(1, 1);
      swirly.Csk = rand_secondary_coefficient(1, 1);
      swirly.Ssk = rand_secondary_coefficient(1, 1);
      swirly.Ksk = rand_mixed_coefficient(1, 1);
      swirly.Zsk = rand_mixed_coefficient(1, 1);
      swirly.Psz = rand_phase_shift();
      swirly.Asz = rand_angle_multiplier(1, 1);
      swirly.Dsz = rand_linear_phase_shift(1, 1);
      swirly.Csz = rand_secondary_coefficient(1, 1);
      swirly.Ssz = rand_secondary_coefficient(1, 1);
      swirly.Ksz = rand_mixed_coefficient(1, 1);
      swirly.Zsz = rand_mixed_coefficient(1, 1);

      swirly.Pccc = rand_phase_shift();
      swirly.Accc = rand_angle_multiplier(3, 0);
      swirly.Dccc = rand_linear_phase_shift(3, 0);
      swirly.Pccs = rand_phase_shift();
      swirly.Accs = rand_angle_multiplier(3, 0);
      swirly.Dccs = rand_linear_phase_shift(3, 0);
      swirly.Pcck = rand_phase_shift();
      swirly.Acck = rand_angle_multiplier(2, 1);
      swirly.Dcck = rand_linear_phase_shift(2, 1);
      swirly.Pccz = rand_phase_shift();
      swirly.Accz = rand_angle_multiplier(2, 1);
      swirly.Dccz = rand_linear_phase_shift(2, 1);
      swirly.Pcsc = rand_phase_shift();
      swirly.Acsc = rand_angle_multiplier(3, 0);
      swirly.Dcsc = rand_linear_phase_shift(3, 0);
      swirly.Pcss = rand_phase_shift();
      swirly.Acss = rand_angle_multiplier(3, 0);
      swirly.Dcss = rand_linear_phase_shift(3, 0);
      swirly.Pcsk = rand_phase_shift();
      swirly.Acsk = rand_angle_multiplier(2, 1);
      swirly.Dcsk = rand_linear_phase_shift(2, 1);
      swirly.Pcsz = rand_phase_shift();
      swirly.Acsz = rand_angle_multiplier(2, 1);
      swirly.Dcsz = rand_linear_phase_shift(2, 1);
      swirly.Pckc = rand_phase_shift();
      swirly.Ackc = rand_angle_multiplier(2, 1);
      swirly.Dckc = rand_linear_phase_shift(2, 1);
      swirly.Pcks = rand_phase_shift();
      swirly.Acks = rand_angle_multiplier(2, 1);
      swirly.Dcks = rand_linear_phase_shift(2, 1);
      swirly.Pckk = rand_phase_shift();
      swirly.Ackk = rand_angle_multiplier(1, 2);
      swirly.Dckk = rand_linear_phase_shift(1, 2);
      swirly.Pckz = rand_phase_shift();
      swirly.Ackz = rand_angle_multiplier(1, 2);
      swirly.Dckz = rand_linear_phase_shift(1, 2);
      swirly.Pczc = rand_phase_shift();
      swirly.Aczc = rand_angle_multiplier(2, 1);
      swirly.Dczc = rand_linear_phase_shift(2, 1);
      swirly.Pczs = rand_phase_shift();
      swirly.Aczs = rand_angle_multiplier(2, 1);
      swirly.Dczs = rand_linear_phase_shift(2, 1);
      swirly.Pczk = rand_phase_shift();
      swirly.Aczk = rand_angle_multiplier(1, 2);
      swirly.Dczk = rand_linear_phase_shift(1, 2);
      swirly.Pczz = rand_phase_shift();
      swirly.Aczz = rand_angle_multiplier(1, 2);
      swirly.Dczz = rand_linear_phase_shift(1, 2);
      swirly.Pscc = rand_phase_shift();
      swirly.Ascc = rand_angle_multiplier(3, 0);
      swirly.Dscc = rand_linear_phase_shift(3, 0);
      swirly.Pscs = rand_phase_shift();
      swirly.Ascs = rand_angle_multiplier(3, 0);
      swirly.Dscs = rand_linear_phase_shift(3, 0);
      swirly.Psck = rand_phase_shift();
      swirly.Asck = rand_angle_multiplier(2, 1);
      swirly.Dsck = rand_linear_phase_shift(2, 1);
      swirly.Pscz = rand_phase_shift();
      swirly.Ascz = rand_angle_multiplier(2, 1);
      swirly.Dscz = rand_linear_phase_shift(2, 1);
      swirly.Pssc = rand_phase_shift();
      swirly.Assc = rand_angle_multiplier(3, 0);
      swirly.Dssc = rand_linear_phase_shift(3, 0);
      swirly.Psss = rand_phase_shift();
      swirly.Asss = rand_angle_multiplier(3, 0);
      swirly.Dsss = rand_linear_phase_shift(3, 0);
      swirly.Pssk = rand_phase_shift();
      swirly.Assk = rand_angle_multiplier(2, 1);
      swirly.Dssk = rand_linear_phase_shift(2, 1);
      swirly.Pssz = rand_phase_shift();
      swirly.Assz = rand_angle_multiplier(2, 1);
      swirly.Dssz = rand_linear_phase_shift(2, 1);
      swirly.Pskc = rand_phase_shift();
      swirly.Askc = rand_angle_multiplier(2, 1);
      swirly.Dskc = rand_linear_phase_shift(2, 1);
      swirly.Psks = rand_phase_shift();
      swirly.Asks = rand_angle_multiplier(2, 1);
      swirly.Dsks = rand_linear_phase_shift(2, 1);
      swirly.Pskk = rand_phase_shift();
      swirly.Askk = rand_angle_multiplier(1, 2);
      swirly.Dskk = rand_linear_phase_shift(1, 2);
      swirly.Pskz = rand_phase_shift();
      swirly.Askz = rand_angle_multiplier(1, 2);
      swirly.Dskz = rand_linear_phase_shift(1, 2);
      swirly.Pszc = rand_phase_shift();
      swirly.Aszc = rand_angle_multiplier(2, 1);
      swirly.Dszc = rand_linear_phase_shift(2, 1);
      swirly.Pszs = rand_phase_shift();
      swirly.Aszs = rand_angle_multiplier(2, 1);
      swirly.Dszs = rand_linear_phase_shift(2, 1);
      swirly.Pszk = rand_phase_shift();
      swirly.Aszk = rand_angle_multiplier(1, 2);
      swirly.Dszk = rand_linear_phase_shift(1, 2);
      swirly.Pszz = rand_phase_shift();
      swirly.Aszz = rand_angle_multiplier(1, 2);
      swirly.Dszz = rand_linear_phase_shift(1, 2);

      return swirly;
      }

    public void CalculateOnePoint(int terms, int width, int height, 
				  double zoom, double x, double y, 
				  ref double Fr, ref double Fg, ref double Fb) 
      {

      for (int tx = -terms; tx <= terms; ++tx) 
	{
	double dx = zoom * (x - width * ((double) tx + _x));

	for (int ty = -terms; ty <= terms; ++ty) 
	  {
	  double dy = zoom * (y - height * ((double) ty + _y));
	  double d2 = dx * dx + dy * dy;
	  double angle = Math.Atan2(dy, dx);
	  double d = Math.Sqrt(d2);


	  double Eccc = Pccc + Accc * angle + Dccc * d;
	  double Eccs = Pccs + Accs * angle + Dccs * d;
	  double Ecck = Pcck + Acck * angle + Dcck * d;
	  double Eccz = Pccz + Accz * angle + Dccz * d;
	  double Ecsc = Pcsc + Acsc * angle + Dcsc * d;
	  double Ecss = Pcss + Acss * angle + Dcss * d;
	  double Ecsk = Pcsk + Acsk * angle + Dcsk * d;
	  double Ecsz = Pcsz + Acsz * angle + Dcsz * d;
	  double Eckc = Pckc + Ackc * angle + Dckc * d;
	  double Ecks = Pcks + Acks * angle + Dcks * d;
	  double Eckk = Pckk + Ackk * angle + Dckk * d;
	  double Eckz = Pckz + Ackz * angle + Dckz * d;
	  double Eczc = Pczc + Aczc * angle + Dczc * d;
	  double Eczs = Pczs + Aczs * angle + Dczs * d;
	  double Eczk = Pczk + Aczk * angle + Dczk * d;
	  double Eczz = Pczz + Aczz * angle + Dczz * d;
	  double Escc = Pscc + Ascc * angle + Dscc * d;
	  double Escs = Pscs + Ascs * angle + Dscs * d;
	  double Esck = Psck + Asck * angle + Dsck * d;
	  double Escz = Pscz + Ascz * angle + Dscz * d;
	  double Essc = Pssc + Assc * angle + Dssc * d;
	  double Esss = Psss + Asss * angle + Dsss * d;
	  double Essk = Pssk + Assk * angle + Dssk * d;
	  double Essz = Pssz + Assz * angle + Dssz * d;
	  double Eskc = Pskc + Askc * angle + Dskc * d;
	  double Esks = Psks + Asks * angle + Dsks * d;
	  double Eskk = Pskk + Askk * angle + Dskk * d;
	  double Eskz = Pskz + Askz * angle + Dskz * d;
	  double Eszc = Pszc + Aszc * angle + Dszc * d;
	  double Eszs = Pszs + Aszs * angle + Dszs * d;
	  double Eszk = Pszk + Aszk * angle + Dszk * d;
	  double Eszz = Pszz + Aszz * angle + Dszz * d;

	  double Ecc = Pcc + Acc * angle + Dcc * d + Ccc * Math.Cos(Eccc)
	    + Scc * Math.Sin(Eccs) + Kcc * d * Math.Cos(Ecck) + Zcc
	    * d * Math.Sin(Eccz);
	  double Ecs = Pcs + Acs * angle + Dcs * d + Ccs * Math.Cos(Ecsc)
	    + Scs * Math.Sin(Ecss) + Kcs * d * Math.Cos(Ecsk) + Zcs
	    * d * Math.Sin(Ecsz);
	  double Eck = Pck + Ack * angle + Dck * d + Cck * Math.Cos(Eckc)
	    + Sck * Math.Sin(Ecks) + Kck * d * Math.Cos(Eckk) + Zck
	    * d * Math.Sin(Eckz);
	  double Ecz = Pcz + Acz * angle + Dcz * d + Ccz * Math.Cos(Eczc)
	    + Scz * Math.Sin(Eczs) + Kcz * d * Math.Cos(Eczk) + Zcz
	    * d * Math.Sin(Eczz);
	  double Esc = Psc + Asc * angle + Dsc * d + Csc * Math.Cos(Escc)
	    + Ssc * Math.Sin(Escs) + Ksc * d * Math.Cos(Esck) + Zsc
	    * d * Math.Sin(Escz);
	  double Ess = Pss + Ass * angle + Dss * d + Css * Math.Cos(Essc)
	    + Sss * Math.Sin(Esss) + Kss * d * Math.Cos(Essk) + Zss
	    * d * Math.Sin(Essz);
	  double Esk = Psk + Ask * angle + Dsk * d + Csk * Math.Cos(Eskc)
	    + Ssk * Math.Sin(Esks) + Ksk * d * Math.Cos(Eskk) + Zsk
	    * d * Math.Sin(Eskz);
	  double Esz = Psz + Asz * angle + Dsz * d + Csz * Math.Cos(Eszc)
	    + Ssz * Math.Sin(Eszs) + Ksz * d * Math.Cos(Eszk) + Zsz
	    * d * Math.Sin(Eszz);

	  double Ec = Pc + Ac * angle + Dc * d + Cc * Math.Cos(Ecc) + Sc
	    * Math.Sin(Ecs) + Kc * d * Math.Cos(Eck) + Zc * d
	    * Math.Sin(Ecz);
	  double Es = Ps + As * angle + Ds * d + Cs * Math.Cos(Esc) + Ss
	    * Math.Sin(Ess) + Ks * d * Math.Cos(Esk) + Zs * d
	    * Math.Sin(Esz);

	  double F = (C * Math.Cos(Ec) + S * Math.Sin(Es)) / d2;

	  Fr += F * r;
	  Fg += F * g;
	  Fb += F * b;
	  }
	}
      }

    private static double drand48s() 
      {
      return 1.0 - 2.0 * random.NextDouble();
      }
    
    private static double rand_phase_shift() 
      {
      return 2.0 * Math.PI * random.NextDouble();
      }
    
    private static double rand_angle_multiplier(double m, double n) 
      {
      return n - m + 1.0
	+ Math.Floor(ANGLE_MULTIPLIER_MAX * random.NextDouble());
      }
    
    private static double rand_linear_phase_shift(double m, double n) 
      {
      return LINEAR_PHASE_SHIFT_MAX * drand48s() / (m + n);
      }
    
    private static double rand_secondary_coefficient(double m, double n) 
      {
      return (m + 2.0 * n) * SECONDARY_COEFFICIENT_MAX * drand48s();
      }
    
    private static double rand_mixed_coefficient(double m, double n) 
      {
      return (2.0 * m + n) * MIXED_COEFFICIENT_MAX * drand48s();
      }
    }
  }
