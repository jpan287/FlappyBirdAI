using System;
using System.Collections.Generic;
using System.Text;

namespace MachineLearning
{
    internal static class Extensions
    {
        public static double NextDouble(this Random rand, double min, double max)
        {
            return rand.NextDouble() * (max - min) + min;
        }

        public static int RandomSign(this Random rand)
        {
            return rand.Next(2) * 2 - 1;
        }
    }
}
