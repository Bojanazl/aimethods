using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            var trainingSet = Functions.GenerateTrainingSet();
            double net11, net12, net21, net22, net23, net24, net31, net32; //ukupan izlaz i-tog neurona q-tog sloja
            double y11, y12, y21, y22, y23, y24, y31, y32; //izlaz i-tog neurona q-tog sloja, 1 unutrasnji sloj sa 4 neurona
            double gama = 0.394;  //koeficijent obucavanja
            double e, ep; //kumulativna i privremena greska
            double emax = 0.001; //maksimalna dozvoljena greska
            int k; //indeks para koji se trenutno koristi za obucavanje
            
            int ic, icmax = 100000; //broj iteracija i maksimalni dozvoljeni broj iteracija
            double w211 = 0.13, w212 = -0.129, w221 = 0.12, w222 = -0.25,
                   w231 = 0.1, w232 = -0.1, w241 = -0.12, w242 = 0.1, w311 = 0.19, w312 = -0.18, w313 = 0.17,
                   w314 = -0.16, w321 = 0.15, w322 = -0.14, w323 = -0.3, w324 = 0.12; //tezinski faktori
                                                                                      //double[,] parovi = trainingSet.Item1;
                                                                                      //double[,] rjesenjeParova = trainingSet.Item2;


            //parovi za obucavanje
            double[,] parovi = { {10.52, 1.75 }, {8.45, 2.54 }, {11.12, 2.95 },
                                 {15.44, 1.59 }, {13.45, 2.15 }, {16.55, 2.78 }, {7.00, 1.51 }
                                 , {14.77, 1.12 }, {12.36, 2.22 }, {6.91, 2.64 },
                                 {16.89, 1.16 }, {17.65, 2.00 }, {15.15, 2.30 },
                                  {8.56, 2.10 }, { 10.58, 2.11}, {18.00, 2.08 },
                                 {9.05, 2.82 },  {11.50, 1.10 }, {6.90, 1.23 },
                                 {13.69, 2.98 }, {14.63, 2.09 },
                                 {6.75, 2.00 }, {13.41, 2.55 }, {9.25,1.95 }, {15.22, 1.78 },
                                 {17.01, 2.54 }, {17.09, 2.07 }, {6.99, 2.10 }, {7.77, 2.22 },
                                 {11.59,2.15 }, {16.54, 2.57 }, {7.25, 1.65 }, {8.65, 2.99 },
                                 {9.32, 2.89 }};

            //double[] rjesenjeParova = { 1, -1, 0, 0, 1, -1, 0, 0, 1, -1, 1, 1, -1, 0, 1, 1, -1, -1, -1, 0, 1, 1, -1, -1, 0 };
            double[,] rjesenjeParova = { {35.28, 10.871 }, {45.75, 9.409 }, {62.00, 11.625 },
                                {55.00, 12.857 }, {47.00, 11.726 }, {36.87, 13.303 }, {46.21, 8.621 }
                                , {70.00, 15.117 }, {41.00, 11.360 }, {36.36, 8.922 },
                                {35.92, 13.512 }, {60.00, 14.269 }, {52.12, 12.561 },
                                 {35.00, 9.919 }, {56.97, 10.841 }, {55.00, 13.857 },
                                {62.00, 10.520 }, {58.09, 11.383}, {59.00, 8.973 },
                                {38.95, 12.022}, {44.44, 12.218 },
                                {55.63, 8.674 }, {46.00, 11.707 }, {39.78, 9.968 }, {56.57, 12.897 },
                                {39.08, 13.325 }, {50.09, 13.231 }, {55.00, 8.788 }, {36.66, 9.377 },
                                {65.00, 12.320 }, {55.69, 13.3526 }, {51.26, 8.808 }, {69.00, 11.401 },
                                {56.69, 10.180 } };
            /*
            //parovi za obucavanje
            double[,] parovi = { {10.52, 1.75 }, {8.45, 2.54 }, {11.12, 2.95 }, {6.75, 1.00 },
                                 {15.44, 1.59 }, {13.45, 2.15 }, {16.55, 2.78 }, {7.00, 1.51 },
                                 {10.50, 3.00 }, {14.77, 1.12 }, {12.36, 2.22 }, {6.91, 2.64 },
                                 {16.89, 1.16 }, {17.65, 2.00 }, {15.15, 2.30 }, {6.95, 3.00 },
                                 {18.00, 2.45 }, {8.56, 2.10 }, { 10.58, 2.11}, {18.00, 2.08 },
                                 {9.05, 2.82 }, {13.00, 3.00 }, {11.50, 1.10 }, {6.90, 1.23 },
                                 {13.69, 2.98 }, {10.75, 1.00 }, {10.09, 3.00 }, {14.63, 2.09 },
                                 {6.75, 2.00 }, {13.41, 2.55 }, {9.25,1.95 }, {15.22, 1.78 },
                                 {17.01, 2.54 }, {17.09, 2.07 }, {6.99, 2.10 }, {7.77, 2.22 },
                                 {11.59,2.15 }, {16.54, 2.57 }, {7.25, 1.65 }, {8.65, 2.99 },
                                 {9.32, 2.89 }, {18.00, 3.00 }, {18.00, 1.00 }, {6.75, 1.00 }, {6.75, 3.00 } };

            //double[] rjesenjeParova = { 1, -1, 0, 0, 1, -1, 0, 0, 1, -1, 1, 1, -1, 0, 1, 1, -1, -1, -1, 0, 1, 1, -1, -1, 0 };
           double[,] rjesenjeParova = { {35.28, 10.871 }, {45.75, 9.409 }, {62.00, 11.625 }, {45.00, 8.491 },
                                {55.00, 12.857 }, {47.00, 11.726 }, {36.87, 13.303 }, {46.21, 8.621 },
                                {65.78, 11.874 }, {70.00, 15.117 }, {41.00, 11.360 }, {36.36, 8.922 },
                                {35.92, 13.512 }, {60.00, 14.269 }, {52.12, 12.561 }, {44.41, 8.613 },
                                {46.00, 13.492 }, {35.00, 9.919 }, {56.97, 10.841 }, {55.00, 13.857 },
                                {62.00, 10.520 }, {40.00, 11.678 }, {58.09, 11.383}, {59.00, 8.973 },
                                {38.95, 12.022}, {70.00, 12.930 }, {39.41, 10.396 }, {44.44, 12.218 },
                                {55.63, 8.674 }, {46.00, 11.707 }, {39.78, 9.968 }, {56.57, 12.897 },
                                {39.08, 13.325 }, {50.09, 13.231 }, {55.00, 8.788 }, {36.66, 9.377 },
                                {65.00, 12.320 }, {55.69, 13.3526 }, {51.26, 8.808 }, {69.00, 11.401 },
                                {56.69, 10.180 }, {32.62, 14.290 }, {32.62, 14.290 }, {32.62, 9.141 }, {32.62, 9.141 } };*/

            double[,] paroviTestiranje = {{6.75,1}, {7,1},{8,1.8},{9.2,2.4},{10.11,2.7},
                                        {11, 2.8},{11.98,2},{12,2.1},{12.1,3},{12.56,3},
                                        {12.98, 1},{13, 1.9},{13.4, 1.2},{13.8, 1.8},{14, 2.2},
                                        {14.2, 2.4},{14.5, 2.9},{14.7, 1.7},{14.9, 2.7},{15, 1.85},
                                        {15.5, 1.27},{16.78, 2.83},{17.29, 2.94},{17.98, 3},{18, 1.92},
                                        {7, 1.7}, {7.5, 2}, {8, 2.1}, {8.5, 3}, {16, 1},
                                        {16.5, 1.8}, {17, 2}, {17.5, 3}, {18, 3}};


            //double[,] parovi = { { 6.75, 1 }, { 10.52, 1.75 }, { 8.45, 2.54 }, { 11.12, 1.00 }, { 6.75, 1.00 }, { 15.44, 1.59 }, { 13.45, 2.15 }, { 16.55, 2.78 }, { 7.00, 1.51 } };
            //double[,] rjesenjeParova = { { 60, 8.957 }, { 35.28, 10.871 }, { 45.75, 9.409 }, { 62.00, 11.625 }, { 45.00, 8.491 }, { 55.00, 12.857 }, { 47.00, 11.726 }, { 36.87, 13.303 }, { 46.21, 8.621 } };

            //obucavanje mreze

            for (ic = 0; ; ic++)
            {
                e = 0;
                var trainingSize = parovi.GetLength(0);

                for (k = 0; k < trainingSize; k++)//prolazimo parove za obucavanje
                {
                    net11 = Functions.NormalizeDistance(parovi[k, 0], 6.75, 18);
                    net12 = Functions.NormalizeDistance(parovi[k, 1], 1, 3);

                    y11 = net11;
                    y12 = net12;

                    net21 = w211 * y11 + w212 * y12;
                    net22 = w221 * y11 + w222 * y12;
                    net23 = w231 * y11 + w232 * y12;
                    net24 = w241 * y11 + w242 * y12;

                    y21 = Functions.activationFunction(net21);
                    y22 = Functions.activationFunction(net22);
                    y23 = Functions.activationFunction(net23);
                    y24 = Functions.activationFunction(net24);

                    net31 = w311 * y21 + w312 * y22 + w313 * y23 + w314 * y24;
                    net32 = w321 * y21 + w322 * y22 + w323 * y23 + w324 * y24;

                    y31 = Functions.activationFunction(net31);
                    y32 = Functions.activationFunction(net32);

                    ep = 0.5 * Math.Pow((Functions.NormalizeDistance(rjesenjeParova[k, 0], 30, 70) - y31), 2) +
                        0.5 * Math.Pow((Functions.NormalizeDistance(rjesenjeParova[k, 1], 5, 15) - y32), 2);
                    e += ep;
                    //Console.WriteLine("Temp error: " + ep);
                    if (ep >= emax) //ako je greska veca od dozvoljene, propagiraj gresku unazad
                    {
                        double delta31, delta32, delta21, delta22, delta23, delta24;
                        double deltaw211, deltaw212, deltaw221, deltaw222, deltaw231,
                               deltaw232, deltaw241, deltaw242, deltaw311, deltaw312,
                               deltaw313, deltaw314, deltaw321, deltaw322, deltaw323, deltaw324;

                        //delta faktori izlaznog sloja i korekcije tezina
                        var diff1 = Functions.NormalizeDistance(rjesenjeParova[k, 0], 30, 70) - y31;
                        var deriv1 = Functions.activationFunctionDerivative(net31);
                        delta31 = (diff1 * deriv1);

                        deltaw314 = gama * delta31 * y24;
                        w314 += deltaw314;

                        deltaw313 = gama * delta31 * y23;
                        w313 += deltaw313;

                        deltaw312 = gama * delta31 * y22;
                        w312 += deltaw312;

                        deltaw311 = gama * delta31 * y21;
                        w311 += deltaw311;

                        delta32 = (Functions.NormalizeDistance(rjesenjeParova[k, 1], 5, 15) - y32) * Functions.activationFunctionDerivative(net32);

                        deltaw324 = gama * delta32 * y24;
                        w324 += deltaw324;

                        deltaw323 = gama * delta32 * y23;
                        w323 += deltaw323;

                        deltaw322 = gama * delta32 * y22;
                        w322 += deltaw322;

                        deltaw321 = gama * delta32 * y21;
                        w321 += deltaw321;

                        //delta faktori skrivenog sloja i korekcije tezina
                        delta24 = Functions.activationFunctionDerivative(net24) * (w314 * delta31 + w324 * delta32);

                        deltaw242 = gama * delta24 * y12;
                        w242 += deltaw242;

                        deltaw241 = gama * delta24 * y11;
                        w241 += deltaw241;

                        delta23 = Functions.activationFunctionDerivative(net23) * (w313 * delta31 + w323 * delta32);

                        deltaw232 = gama * delta23 * y12;
                        w232 += deltaw232;

                        deltaw231 = gama * delta23 * y11;
                        w231 += deltaw231;

                        delta22 = Functions.activationFunctionDerivative(net22) * (w312 * delta31 + w322 * delta32);

                        deltaw222 = gama * delta22 * y12;
                        w222 += deltaw222;

                        deltaw221 = gama * delta22 * y11;
                        w221 += deltaw221;

                        delta21 = Functions.activationFunctionDerivative(net21) * (w311 * delta31 + w321 * delta32);

                        deltaw212 = gama * delta21 * y12;
                        w212 += deltaw212;

                        deltaw211 = gama * delta21 * y11;
                        w211 += deltaw211;
                    }
                }

                Console.WriteLine("Iteracija: " + ic + ", greska = " + e / trainingSize);
                //System.Threading.Thread.Sleep(10);
                if ((e / trainingSize <= emax) || (ic >= icmax))
                    break;
            }

            Console.WriteLine("Calculating success rate of network...");
            int counterShotsMade = 0;
            int numberOfShots = paroviTestiranje.GetLength(0);
            for (int i = 0; i < numberOfShots; i++)
            {
                net11 = Functions.NormalizeDistance(paroviTestiranje[i, 0], 6.75, 18);
                net12 = Functions.NormalizeDistance(paroviTestiranje[i, 1], 1, 3);

                y11 = net11;
                y12 = net12;

                net21 = w211 * y11 + w212 * y12;
                net22 = w221 * y11 + w222 * y12;
                net23 = w231 * y11 + w232 * y12;
                net24 = w241 * y11 + w242 * y12;

                y21 = Functions.activationFunction(net21);
                y22 = Functions.activationFunction(net22);
                y23 = Functions.activationFunction(net23);
                y24 = Functions.activationFunction(net24);

                net31 = w311 * y21 + w312 * y22 + w313 * y23 + w314 * y24;
                net32 = w321 * y21 + w322 * y22 + w323 * y23 + w324 * y24;

                y31 = Functions.activationFunction(net31);
                y32 = Functions.activationFunction(net32);

                Console.WriteLine("Shot #" + (i + 1));
                Console.WriteLine("The angle of shot: " + Functions.DenormalizeDistance(y31, 30, 70) + " degrees");
                Console.WriteLine("Throw velocity: " + Functions.DenormalizeDistance(y32, 5, 15));

                var shotOutcome = Functions.simulateShot(paroviTestiranje[i, 0], paroviTestiranje[i, 1], Functions.ConvertToRadians(Functions.DenormalizeDistance(y31, 30, 70)), Functions.DenormalizeDistance(y32, 5, 15));
                Console.WriteLine("Basket? " + shotOutcome);

                if (shotOutcome == "SCORE")
                {
                    counterShotsMade++;
                }
            }

            Console.WriteLine("--------");
            Console.WriteLine("Success rate : " + (counterShotsMade * 100 / numberOfShots) + "%");

            Console.WriteLine("--------");
            Console.WriteLine("User testing with free input");
            Console.WriteLine("--------");

            for (; ; )
            {
                Console.WriteLine("Distance between robot and basket:");
                Console.Write("D1 = ");
                string stmpa, stmpb;
                double tmpa, tmpb;
                stmpa = Console.ReadLine();
                tmpa = double.Parse(stmpa);
                Console.WriteLine("Distance between player and the robot:");
                Console.Write("D2 = ");
                stmpb = Console.ReadLine();
                tmpb = double.Parse(stmpb);

                net11 = Functions.NormalizeDistance(tmpa, 6.75, 18);
                net12 = Functions.NormalizeDistance(tmpb, 1, 3);

                y11 = net11;
                y12 = net12;

                net21 = w211 * y11 + w212 * y12;
                net22 = w221 * y11 + w222 * y12;
                net23 = w231 * y11 + w232 * y12;
                net24 = w241 * y11 + w242 * y12;

                y21 = Functions.activationFunction(net21);
                y22 = Functions.activationFunction(net22);
                y23 = Functions.activationFunction(net23);
                y24 = Functions.activationFunction(net24);

                net31 = w311 * y21 + w312 * y22 + w313 * y23 + w314 * y24;
                net32 = w321 * y21 + w322 * y22 + w323 * y23 + w324 * y24;

                y31 = Functions.activationFunction(net31);
                y32 = Functions.activationFunction(net32);

                Console.WriteLine("The angle of shot: " + Functions.DenormalizeDistance(y31, 30, 70) + " degrees");
                Console.WriteLine("Throw velocity: " + Functions.DenormalizeDistance(y32, 5, 15));

                Console.WriteLine("Basket? " + Functions.simulateShot(tmpa, tmpb, Functions.ConvertToRadians(Functions.DenormalizeDistance(y31, 30, 70)), Functions.DenormalizeDistance(y32, 5, 15)));
            }
        }
    }
}
