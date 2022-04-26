using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetickiAlgoritam
{
    public static class Funkcije
    {
        private static readonly double gd = -3;
        private static readonly double gg = 3;
        private static readonly int n = 10;

        //broj se dobije dijeljenjem dužine duži GdX sa širinom manjeg intervala
        public static int Kodiraj(double x)
        {
            return (int)Math.Floor(((x - gd) / (gg - gd)) * (Math.Pow(2, n) - 1)); 
        }

        //na donju granicu dodajemo bd sirina intervala
        public static double Dekodiraj(int bd)
        {
            return gd + ((gg - gd) / Math.Pow(2, n) * bd); 
        }

        //analiticki izraz funkcije 
        public static double Z(double x, double y)
        {
            return 3 * Math.Pow((1 - x), 2) * 
                Math.Pow(Math.E, -(Math.Pow(x, 2) + Math.Pow(y + 1, 2))) - 10 * 
                (x / 5 - Math.Pow(x, 3) - Math.Pow(y, 5)) * 
                Math.Pow(Math.E, -(Math.Pow(x, 2) + Math.Pow(y, 2))) - (1 / 3) * 
                Math.Pow(Math.E, -(Math.Pow(x + 1, 2) + Math.Pow(y, 2)));
        }

        public static double[] FitnesFunkcijaMaksimum(Hromozom[] populacija, int velicinaPopulacije)
        {
            double[] tmpz = new double[velicinaPopulacije];
            double[] tmpff = new double[velicinaPopulacije];
            
            //racunanje vrijednosti funkcije za populaciju
            for (int i = 0; i < velicinaPopulacije; i++)
            {
                tmpz[i] = Z(Dekodiraj(populacija[i].x), Dekodiraj(populacija[i].y));
            }

            //racunanje fitnes funkcije
            for (int i = 0; i < velicinaPopulacije; i++)
            {
                tmpff[i] = tmpz[i] - tmpz.Min();
            }
            return tmpff;
        }

        public static double[] FitnesFunkcijaMinimum(Hromozom[] populacija, int velicinaPopulacije)
        {
            double[] tmpz = new double[velicinaPopulacije];
            double[] tmpff = new double[velicinaPopulacije];

            //racunanje vrijednosti funkcije za populaciju
            for (int i = 0; i < velicinaPopulacije; i++)
            {
                tmpz[i] = Z(Dekodiraj(populacija[i].x), Dekodiraj(populacija[i].y));
            }

            //racunanje fitnes funkcije
            for (int i = 0; i < velicinaPopulacije; i++)
            {
                tmpff[i] = tmpz.Max() - tmpz[i];
            }
            return tmpff;
        }
    }
}
