using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Functions
    {
        private static double g = 9.80665;
        private static double minRobotDistance = 6.75;
        private static double maxRobotDistance = 18;

        //unipolarna sigmoidalna funkcija
        public static double activationFunction(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x));
        }

        //izvod unipolarne sigmuidalne funkcije
        public static double activationFunctionDerivative(double x)
        {
            return activationFunction(x) * (1 - activationFunction(x));
        }

        /*
        public static double activationFunction(double x)
        {
            double y = ((1 - Math.Pow(Math.E, -x)) / (1 + Math.Pow(Math.E, -x)));
            return y;
        }

        public static double activationFunctionDerivative(double x)
        {
            double y = ((2 * Math.Pow(Math.E, -x)) / ((1 + Math.Pow(Math.E, -x)) * (1 + Math.Pow(Math.E, -x))));
            return y;
        }
        */
        internal static double arcsin(double y31)
        {
            return Math.Asin(y31);
        }

        internal static double velocityCorrection(double x)
        {
            return 10 * x + 5;
        }

        internal static double mapVelocityToSigmoid(double x)
        {
            return (x - 5) / 10;
        }

        internal static double NormalizeDistance(double value, double min, double max)
        {
            var x = (value - min) / (max - min);
            //return 2 * x - 1;
            return x;
        }

        internal static double DenormalizeDistance(double normalized, double min, double max)
        {
            return (normalized * (max - min) + min);
        }

        public static string simulateShot(double distanceRobot, double distanceDefender, double angle, double velocity)
        {
            //check if shot was blocked
            double minAngle = Math.Asin(0.4 / Math.Sqrt(0.16 + Math.Pow(distanceRobot, 2)));
            if (angle <= minAngle)
            {
                return "BLOCKED";
            }

            //Check if maximum height is at least ball size over basket (34 cm + 305 cm)
            bool maxHeightOver = (MaxHeight(velocity, angle) > 3.38);
            if (!maxHeightOver)
            {
                Console.WriteLine("max height not reached");
                return "MISS";
            }

            //check if ball is moving in downward direction 1m before basket (if not, it will hit the backboard)
            if (!BallFallingWhenEnteringBasket(velocity, angle, distanceRobot))
            {
                Console.WriteLine("ball wasnt falling");
                return "MISS";
            }

            if (!BallHitsTheBasket(velocity, angle, distanceRobot))
            {
                Console.WriteLine("ball missed the basket");
                return "MISS";
            }

            return "SCORE";
        }

        public static double MaxHeight(double velocity, double angle)
        {
            return ((Math.Pow(velocity, 2) * Math.Pow(Math.Sin(angle), 2)) / g) + 2.5;
        }

        public static bool BallFallingWhenEnteringBasket(double velocity, double angle, double distanceRobot)
        {
            //check the vertical height of the ball 1m and 0.9m before basket to see whether it's falling
            double t1 = (distanceRobot - 1) / (velocity * Math.Cos(angle));
            double y1 = Height(t1, angle, velocity);

            double t2 = (distanceRobot - 0.9) / (velocity * Math.Cos(angle));
            double y2 = Height(t2, angle, velocity);

            if (y1 < y2)
                return true;
            else
                return false;
        }

        public static bool BallHitsTheBasket(double velocity, double angle, double distanceRobot)
        {
            //ball hits the basket if within boundaries of the basket at the given distance 
            //(21.72cm leeway meaning center of the ball can be 10.86 cm off in either direction)
            double t = TimeAtBasketHeight(velocity, angle).Item2;
            double distance = velocity * Math.Cos(angle) * t;
            Console.WriteLine("t = " + t);
            Console.WriteLine("distance = " + distance);
            return (distance > distanceRobot - 0.1086) && (distance < distanceRobot + 0.1086);
        }

        //returns t1,t2 at height of 3.05m
        public static Tuple<double, double> TimeAtBasketHeight(double velocity, double angle)
        {
            double v0sintheta = velocity * Math.Sin(angle);
            double t1 = (-v0sintheta + Math.Sqrt(Math.Pow(v0sintheta, 2) - 1.1 * g)) / (-g);
            double t2 = (-v0sintheta - Math.Sqrt(Math.Pow(v0sintheta, 2) - 1.1 * g)) / (-g);
            return new Tuple<double, double>(t1, t2);
        }

        public static double Height(double t, double angle, double velocity)
        {
            return 0.5 * 0.91 * Math.Pow(t, 2) + velocity * Math.Sin(angle) * t + 2.5;
        }

        public static double Velocity(double distanceRobot, double distanceDefender, double angle)
        {
            //double angleRadians = ConvertToRadians(angle);
            double angleRadians = angle;
            return Math.Sqrt((-g * Math.Pow(distanceRobot, 2)) / ((1 + Math.Cos(2 * angleRadians)) * (0.55 - Math.Tan(angleRadians) * distanceRobot)));
        }

        public static double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        public static double ConvertRadiansToDegrees(double radians)
        {
            return (180 / Math.PI) / radians;
        }

        public static Tuple<double[,], double[,]> GenerateTrainingSet()
        {
            var pairs = new List<Tuple<double, double>>();
            var solutions = new List<Tuple<double, double>>();
            Random rnd = new Random();

            for (int i = 0; i < 16; i++)
            {
                double randomD = rnd.NextDouble();
                double robotDistance = randomD * (maxRobotDistance - minRobotDistance) + minRobotDistance;

                int angle = rnd.Next(34, 68);

                double randomDef = rnd.NextDouble();
                double defenderDistance = randomDef * 2 + 1;

                double velocity = Velocity(robotDistance, defenderDistance, ConvertToRadians(angle));
                pairs.Add(new Tuple<double, double>(robotDistance, defenderDistance));
                solutions.Add(new Tuple<double, double>(angle, velocity));
            }

            var outputPair = new double[pairs.Count, 2];
            var outputSolutions = new double[solutions.Count, 2];
            var index = 0;
            foreach (var pair in pairs)
            {
                outputPair[index, 0] = pair.Item1;
                outputPair[index, 1] = pair.Item2;
                index++;
            }
            index = 0;
            foreach (var solution in solutions)
            {
                outputSolutions[index, 0] = solution.Item1;
                outputSolutions[index, 1] = solution.Item2;
                index++;
            }
            return new Tuple<double[,], double[,]>(outputPair, outputSolutions);
        }
    }
}
