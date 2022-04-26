using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetickiAlgoritam
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Izracunavanje minimuma: ");
            GenetickiAlgoritam(Funkcije.FitnesFunkcijaMinimum);
            Console.WriteLine("Izracunavanje maksimuma: ");
            GenetickiAlgoritam(Funkcije.FitnesFunkcijaMaksimum);
            Console.WriteLine("<(OO)>");
            Console.ReadLine();
        }
        static void GenetickiAlgoritam(Func<Hromozom[], int, double[]> fitnesFunkcija)
        {
            int velicinaPopulacije = 80;
            double vjerovatnocaRekombinacije = 0.80;
            double vjerovatnocaMutacije = 0.15;
            int brojIteracija = 1000;
            int brojRokada = 55;
            int brojMutiranihBitova = 1;


            Hromozom[] populacija = new Hromozom[velicinaPopulacije];
            double[] fitnes = new double[velicinaPopulacije];
            double ukupanFitnes = 0;
            double[] vjerovatnoce = new double[velicinaPopulacije];
            double[] kumulativneVjerovatnoce = new double[velicinaPopulacije];

            //Generisanje pocetne populacije
            Random rng = new Random();
            for (int i = 0; i < velicinaPopulacije; i++)
            {
                double tmp1 = rng.NextDouble() * 6 - 3;
                double tmp2 = rng.NextDouble() * 6 - 3;
                populacija[i] = new Hromozom(Funkcije.Kodiraj(tmp1), Funkcije.Kodiraj(tmp2));
                //Console.WriteLine(populacija[i].toString()+", "+tmp1+", "+tmp2);
            }

            for (int iteracija = 0; iteracija < brojIteracija; iteracija++)
            {
                fitnes = fitnesFunkcija(populacija, velicinaPopulacije);

                foreach (var f in fitnes)
                {
                    ukupanFitnes += f;
                }

                //racunanje vjerovatnoca izbora jedinke i kumulativne vjerovatnoce
                for (int i = 0; i < velicinaPopulacije; i++)
                {
                    vjerovatnoce[i] = fitnes[i] / ukupanFitnes;
                }
                for (int i = 0; i < velicinaPopulacije; i++)
                {
                    if (i == 0)
                    {
                        kumulativneVjerovatnoce[i] = vjerovatnoce[i];
                    }
                    else
                    {
                        kumulativneVjerovatnoce[i] = kumulativneVjerovatnoce[i - 1] + vjerovatnoce[i];
                    }
                }

                //ruletska selekcija
                int[] rulet = new int[velicinaPopulacije];
                for (int i = 0; i < velicinaPopulacije; i++)
                {
                    double ruletRandom = rng.NextDouble();
                    if (ruletRandom < kumulativneVjerovatnoce[0])
                    {
                        rulet[i] = 0;
                    }
                    else
                    {
                        for (int j = 0; j < velicinaPopulacije - 1; j++)
                        {
                            if ((ruletRandom >= kumulativneVjerovatnoce[j]) && (ruletRandom < kumulativneVjerovatnoce[j + 1]))
                            {
                                rulet[i] = j + 1;
                            }
                        }
                    }
                }

                //kreiranje medjugeneracije
                Hromozom[] medjugeneracija = new Hromozom[velicinaPopulacije];
                for (int i = 0; i < velicinaPopulacije; i++)
                {
                    medjugeneracija[i] = populacija[rulet[i]];
                }

                //rokada
                for (int i = 0; i < brojRokada; i++) //odluka o parovima koji ce zamijeniti mjesta
                {
                    int tmp1 = (int)Math.Floor(rng.NextDouble() * velicinaPopulacije);
                    int tmp2 = (int)Math.Floor(rng.NextDouble() * velicinaPopulacije);
                    Hromozom tmpHromozom = medjugeneracija[tmp1];
                    medjugeneracija[tmp1] = medjugeneracija[tmp2];
                    medjugeneracija[tmp2] = tmpHromozom;
                }

                //odluka o ukrstanju
                bool[] ukrstanje = new bool[velicinaPopulacije / 2];
                for (int i = 0; i < velicinaPopulacije / 2; i++)
                {
                    double tmp = rng.NextDouble();
                    if (tmp < vjerovatnocaRekombinacije)
                    {
                        ukrstanje[i] = true;
                    }
                    else
                        ukrstanje[i] = false;
                }

                Hromozom[] medjugeneracija2 = new Hromozom[velicinaPopulacije];

                //ukrstanje/rekombinacija
                for (int i = 0; i < velicinaPopulacije / 2; i++)
                {
                    if (ukrstanje[i])
                    {
                        int tmp1 = medjugeneracija[i * 2].x;
                        int tmp2 = medjugeneracija[i * 2].y;
                        int tmp3 = medjugeneracija[i * 2 + 1].x;
                        int tmp4 = medjugeneracija[i * 2 + 1].y;
                        medjugeneracija2[i * 2] = new Hromozom(tmp1, tmp4);
                        medjugeneracija2[i * 2 + 1] = new Hromozom(tmp3, tmp2);
                    }
                    else
                    {
                        medjugeneracija2[2 * i] = medjugeneracija[2 * i];
                        medjugeneracija2[2 * i + 1] = medjugeneracija[2 * i + 1];
                    }

                }

                //odluka o mutaciji
                bool[] mutacija = new bool[velicinaPopulacije];
                for (int i = 0; i < velicinaPopulacije; i++)
                {
                    double tmp = rng.NextDouble();
                    if (tmp <= vjerovatnocaMutacije)
                        mutacija[i] = true;
                    else
                        mutacija[i] = false;
                }

                //mutacija
                Hromozom[] medjugeneracija3 = new Hromozom[velicinaPopulacije];
                for (int i = 0; i < velicinaPopulacije; i++)
                {
                    if (mutacija[i])
                    {
                        //odluka da li ce mutirati x ili y
                        double tmp = rng.NextDouble();
                        if (tmp <= 0.5) //mutira x
                        {
                            for (int st = 0; st < brojMutiranihBitova; st++)
                            {
                                double tmp2 = rng.NextDouble();
                                int tmp3 = (int)Math.Floor(rng.NextDouble() * 8);//tacka mutacije
                                int tmp4 = 1 << tmp3; //2 na tacku mutacije
                                int tmp5 = medjugeneracija2[i].x ^ tmp4;  //xor
                                medjugeneracija3[i] = new Hromozom(tmp5, medjugeneracija2[i].y);
                            }
                        }
                        else //mutira y
                        {
                            for (int st = 0; st < brojMutiranihBitova; st++)
                            {
                                double tmp2 = rng.NextDouble();
                                int tmp3 = (int)Math.Floor(rng.NextDouble() * 8);//tacka mutacije
                                int tmp4 = 1 << tmp3; //2 na tacku mutacije
                                int tmp5 = medjugeneracija2[i].y ^ tmp4;  //xor
                                medjugeneracija3[i] = new Hromozom(medjugeneracija2[i].x, tmp5);
                            }
                        }
                    }
                    else
                    {
                        medjugeneracija3[i] = medjugeneracija2[i];
                    }
                }

                for (int i = 0; i < velicinaPopulacije; i++)
                {
                    populacija[i] = medjugeneracija3[i];
                }
            }

            fitnes = fitnesFunkcija(populacija, velicinaPopulacije);
            double fitnesmax = 0;
            int fitnesbroj = 0;
            for (int i = 0; i < velicinaPopulacije; i++)
            {
                if (fitnes[i] > fitnesmax)
                {
                    fitnesmax = fitnes[i];
                    fitnesbroj = i;
                }
            }
            Console.WriteLine(populacija[fitnesbroj].toString());
            Console.WriteLine("f(x) = " + Funkcije.Z(Funkcije.Dekodiraj(populacija[fitnesbroj].x), Funkcije.Dekodiraj(populacija[fitnesbroj].y)));
        }
    }
}
