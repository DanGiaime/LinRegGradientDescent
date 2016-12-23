using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinRegGradientDescent
{
    class Program
    {
        //y intercept of function
        public static double b;

        //slope of function
        public static double m;

        //learning constant
        public static double alpha;

        static void Main(string[] args)
        {
            //Initialize guesses to 0, alpha to very low number
            b = 0;
            m = 0;
            alpha = .0000001;
            Random randy = new Random();

            //Ask for size and actual slope
            Console.WriteLine("Enter desired number of data points:");
            int size = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter desired slope for points:");
            double slope = double.Parse(Console.ReadLine());

            int iterations = 0;

            //Set default values. 
            //prevError should be greater than actual error on first iteration
            //So we default to an arbitrary large value
            double error = 1;
            double prevError = double.MaxValue;

            double[,] data = CreateDataSet(size, slope, randy);
            double temp0 = 0;
            double temp1 = 0;
            

            //Print actual data for reference
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(data[i,0]+" "+data[i,1]);
            }

            Console.ReadLine();


            while (prevError/size > .0001)
            {
                //Iterate through values, determine partial error for each var
                for (int j = 0; j < size; j++)
                {
                    temp0 += CostDerivate(data[j, 0], data[j, 1]);
                    temp1 += CostDerivate(data[j, 0], data[j, 1], true);
                }

                //Alter variables by appropriate amount
                b -= temp0 / size;
                m -= temp1 / size;

                //Calculate error after alteration
                for (int j = 0; j < size; j++)
                {
                    error += Cost(data[j, 0], data[j, 1]);
                }

                //Print true if we moved closer to answer. 
                //Otherwise, adjust learning constant.
                if (prevError > error)
                {
                    Console.WriteLine(true);
                }
                else {
                    Console.WriteLine("Not gettin any better");
                    break;
                }

                //Print data
                Console.WriteLine("Error: " + Math.Sqrt(2*error) / size);
                Console.WriteLine("y = " + Math.Round(m,5) + "x + " + Math.Round(b, 5));
                Console.WriteLine("Slope error: " + Math.Round((slope-m)/slope*100,4)+"%");

                //Prepare for next iteration
                prevError = error;
                error = 0;
                iterations++;
            }
            Console.WriteLine("---------------------------");
            Console.WriteLine("Finished in {0} iterations!",iterations);
            Console.ReadLine();
        }

        /// <summary>
        /// Creates a random set of data
        /// </summary>
        /// <param name="size">Number of data points</param>
        /// <returns>int[,] filled with data points</returns>
        public static double[,] CreateDataSet(int size, double actualSlope, Random randy)
        {
            double[,] data = new double[size, 2];
            double temp = 0;
            for (int i = 0; i < size; i++)
            {
                temp = randy.NextDouble() * 100;
                data[i, 0] = temp;
                data[i, 1] = temp * actualSlope;
            }

            return data;
        }

        /// <summary>
        /// Mean Square error cost function
        /// </summary>
        /// <param name="x">x value of point</param>
        /// <param name="y">y value of point</param>
        /// <returns>half the square of the difference of expected and actual</returns>
        public static double Cost(double x, double y)
        {
            return .5 * Math.Pow(m * x + b - y, 2);
        }


        /// <summary>
        /// Derivative of cost function
        /// </summary>
        /// <param name="x">X value of current point</param>
        /// <param name="y">Y value of current point</param>
        /// <param name="constant">Whether or not partial derivative 
        /// is in terms of the constant b</param>
        /// <returns>Derivative of the cost function at that point 
        /// with respect to given variable</returns>
        public static double CostDerivate(double x, double y, bool constant = false)
        {
            return alpha * (m * x + b - y) * (constant ? x : 1);
        }


    }
}
