using System;

namespace ALS_RECOMMENDATION_ALGORITHM
{
    internal class Randomizer
    {
        private Random random;

        public Randomizer()
        {
            this.random = new Random();
        }

        //returns new matrix as row amount n and columns amount m with randomized values between 0 , 1)
        public double[,] Randomize(int n, int m)
        {
            double[,] matrix = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = random.NextDouble();
                }
            }
            return matrix;
        }
    }
}