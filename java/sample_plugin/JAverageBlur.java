/* The JAverageBlur plugin
 * Copyright (C) 2004-2006 Massimo Perga
 * Porting of original AverageBlur C# plugin written by Maurits Rijk
 *
 * JAverageBlur.java
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
 *
 * Created on July 26, 2006, 10:51 AM
 * Massimo Perga (C) 2006
 *
 * To change this template, choose Tools | Template Manager
 * and open the template in the editor.
 */

package Gimp.JAverageBlur;

import cli.Gimp.Display;
import cli.Gimp.Drawable;
import cli.Gimp.Plugin;
import cli.Gimp.Procedure;
import cli.Gimp.ProcedureSet;
import cli.Gimp.Progress;
import cli.Gimp.RgnIterator;
import cli.Gimp.RunMode;
import cli.Gimp.Pixel;

/**
 *
 * @author Massimo Perga (massimo.perga@gmail.com)
 *
 */
public class JAverageBlur extends Plugin  {
    
    public static int bpp;
    public static long[] sum;
    public static byte[] average;
    public static int count;
    
    /** Creates a new instance of JAverageBlur */
    
    public JAverageBlur(String[] args, String catalog) {
        super(args, catalog);
    }
    
    public static void main(String[] args) {
        new JAverageBlur(args, "JAverageBlur");
    }
    
    protected ProcedureSet GetProcedureSet() {
        System.out.println("JAverageBlur: GetProcedureSet called");
        ProcedureSet set = new ProcedureSet();
        
        Procedure procedure = new Procedure("plug_in_javerage_blur",
                "JAverage blur",
                "JAverage blur",
                "Massimo Perga",
                "(C) Massimo Perga",
                "2006",
                "JAverageBlur",
                "RGB*, GRAY*"
                );
        
        procedure.set_MenuPath("<Image>/Filters/Blur");
        
        set.Add(procedure);
        
        System.out.println("Using all native methods");
        
        return set;
    }
    
    protected void Render(Drawable drawable) {
        System.out.println("JAverageBlur: Render called");
        
        bpp = drawable.get_Bpp();
        sum = new long[bpp];
        count = 0;
        average = new byte[bpp];
        
        RgnIterator iter = new RgnIterator (drawable, 
                RunMode.wrap(RunMode.Interactive));                
        iter.set_Progress(new Progress("JAverage"));
        for(int i = 0; i < bpp; i++) sum[i] = 0;
        
        iter.IterateSrc(new cli.Gimp.RgnIterator.IterFuncSrc(
                new cli.Gimp.RgnIterator.IterFuncSrc.Method() {
            public void Invoke(cli.Gimp.Pixel pixel) {                
                byte []src = pixel.get_Bytes();
                for(int i = 0; i < bpp; i++) {
                    // Keep in mind this trick to convert byte to int
                    sum[i] += (int)(src[i] & 0xFF);
                }
                count++;
            }
        }));
        for(int i = 0; i < bpp; i++) {            
            average[i] = (byte)(sum[i] / count);
        }

        iter.IterateDest(new cli.Gimp.RgnIterator.IterFuncDest(
                new cli.Gimp.RgnIterator.IterFuncDest.Method() {
            public cli.Gimp.Pixel Invoke() {
                Pixel pixel = new Pixel(bpp);
                pixel.set_Bytes(average);
                return pixel;                                
            }
        }));
               
        Display.DisplaysFlush();
        
    }
    
}
